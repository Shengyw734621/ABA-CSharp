using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using MyApp.Services;

namespace MyApp.Controllers;

public class HomeController : Controller
{
    private readonly CustomerService _customerService;
    private readonly EmployeeService _employeeService;
    private readonly ILogger<HomeController> _logger;

    private readonly ShipmentService _shipmentService;

    public IActionResult GetHandlers()
    {
        var handlers = _employeeService.GetAdminEmployees();
        return Json(handlers);
    }
    //出貨單行政部簽名欄
    [HttpGet]
    public IActionResult GetShipmentHandlers()
    {
        var handlers = _shipmentService.GetAdministrativeEmployees() ?? new List<string>();
        return Json(handlers); // 直接回傳 JSON 陣列
    }
    //出貨單業務部簽名欄
    [HttpGet]
    public IActionResult GetSalesHandlers()
    {
        // 取得業務部員工全名
        var handlers = _shipmentService.GetEmployeesByDepartment("營業部") 
                    ?? new List<string>();
        return Json(handlers);
    }

    public HomeController(
        ILogger<HomeController> logger,
        CustomerService customerService,
        EmployeeService employeeService,
        ShipmentService shipmentService)
    {
        _logger = logger;
        _customerService = customerService;
        _employeeService = employeeService;
        _shipmentService = shipmentService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult News()
    {
        return View();
    }

    public IActionResult Customer()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GetCustomerSummaries(string? name, string? type, string? county)
    {
        var summaries = _customerService.GetSummaries(name, type, county);
        return Ok(summaries); // 回傳 JSON
    }

    [HttpGet]
    public IActionResult GetCustomerDetail(int id)
    {
        var customer = _customerService.GetById(id); // 你應該有這個方法
        if (customer == null)
            return NotFound();

        return PartialView("_CustomerDetailPartial", customer);
    }

    //撈取修改資料
    [HttpGet]
    public IActionResult GetCustomerById(int id)
    {
        var customer = _customerService.GetById(id);

        Console.WriteLine("抓到的客戶 ID：" + customer.Id);

        if (customer == null)
            return NotFound();

        // 這裡手動轉成匿名物件，欄位名稱改為前端預期的
        return Json(new
        {
            id = customer.Id, // 假如你內部是 Id
            companyName = customer.CompanyName,
            industryType = customer.IndustryType,
            postalCode = customer.PostalCode,
            address = customer.Address,
            region = customer.Region,
            county = customer.County,
            country = customer.Country,
            taxId = customer.TaxId,
            email = customer.Email,
            phone = customer.Phone,
            fax = customer.Fax,
            salesContact = customer.SalesContact,
            technicalContact = customer.TechnicalContact
        });
    }

    // 新增客戶資料
    [HttpPost]
    public IActionResult InsertCustomer([FromBody] Customer customer)
    {
        // 無論如何都印出 customer 的 JSON 字串
        string customerJson = JsonSerializer.Serialize(customer);
        Console.WriteLine($"收到的 customer 資料：{customerJson}");

        if (customer == null)
        {
            return BadRequest(new { message = "客戶資料為空，請提供有效的資料。" });
        }
        try
        {
            int newId = _customerService.Insert(customer);  // 這裡回傳新ID
            return Ok(new { message = "新增成功", id = newId });  // 回傳新 ID 給前端
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "新增客戶資料時發生錯誤");
            return StatusCode(500, new { message = "新增客戶資料失敗" });
        }

    }

    // 修改客戶資料
    [HttpPost]
    public IActionResult UpdateCustomer([FromBody] Customer customer)
    {
        string customerJson = JsonSerializer.Serialize(customer);
        Console.WriteLine($"收到的 customer 資料：{customerJson}");

        if (customer == null)
        {
            return BadRequest(new { message = "客戶資料為空，請提供有效的資料。" });
        }

        try
        {
            _customerService.Update(customer); // ← 這裡改成 Update，而不是 Insert！
            return Ok(new { message = "修改客戶資料成功" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "修改客戶資料時發生錯誤");
            return StatusCode(500, new { message = "修改客戶資料失敗" });
        }
    }

    // 刪除客戶資料
    [HttpPost]
    public IActionResult DeleteCustomer([FromBody] CustomerDeleteRequest request)
    {
        string requestJSON = JsonSerializer.Serialize(request);
        Console.WriteLine($"收到的 request 資料：{requestJSON}");
        try
        {
            _customerService.Delete(request.id); // 呼叫上面寫好的 Dapper 刪除
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // 註冊員工
    [HttpPost]
    public IActionResult RegisterEmployee([FromBody] EmployeeRegister model)
    {
        // if (!ModelState.IsValid)
        //     return BadRequest("資料格式不正確");
        string modelJSON = JsonSerializer.Serialize(model);
        Console.WriteLine($"收到的 model 資料：{modelJSON}");
        // Email 重複檢查
        if (_employeeService.IsEmailDuplicate(model.EmployeeEmailAccount))
        {
            return Conflict("Email 已被註冊");
        }

        try
        {
            _employeeService.Register(model);
            return Ok(new { success = true, message = "註冊成功" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"錯誤：{ex.Message}");
        }
    }

    //員工登入驗證
    [HttpPost("Home/LoginEmployee")]
    public IActionResult LoginEmployee([FromBody] EmployeeLogin model)
    {
        string loginJSON = JsonSerializer.Serialize(model);
        Console.WriteLine($"收到的 model 資料：{loginJSON}");

        if (string.IsNullOrWhiteSpace(model.EmployeeEmailAccount) || string.IsNullOrWhiteSpace(model.EmployeePassword))
        {
            return BadRequest(new { message = "請輸入帳號和密碼" });
        }

        var employee = _employeeService.GetByEmail(model.EmployeeEmailAccount);
        if (employee == null)
        {
            return Unauthorized(new { message = "帳號或密碼錯誤" });
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.EmployeePassword, employee.EmployeePassword);
        if (!isPasswordValid)
        {
            return Unauthorized(new { message = "帳號或密碼錯誤" });
        }

        // 儲存登入狀態到 Session
        HttpContext.Session.SetString("EmployeeEmail", employee.EmployeeEmailAccount);
        HttpContext.Session.SetString("EmployeeName", employee.EmployeeZhName);

        return Ok(new { message = "登入成功" });
    }

    public IActionResult Supplier()
    {
        return View();
    }

    public IActionResult Shipment()
    {
        return View();
    }
    public IActionResult InsertShipment()
    {   
        var handlers = _shipmentService.GetAdministrativeEmployees() ?? new List<string>();
        ViewBag.Handlers = handlers;
        var count = handlers.Count();
        Console.WriteLine($"抓到 {count} 名行政部員工");

        return View();// 預設會去找 Views/Home/InsertShipment.cshtml
    }

    //出貨單產品類別
    [HttpGet]
    public IActionResult GetProductTypes()
    {
        var productTypes = _shipmentService.GetProductTypes();
        return Json(productTypes); 
    }

    public IActionResult Maintenance()
    {
        return View();
    }

    public IActionResult Project()
    {
        return View();
    }

    public IActionResult UR()
    {
        return View();
    }

    public IActionResult URPlus()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

