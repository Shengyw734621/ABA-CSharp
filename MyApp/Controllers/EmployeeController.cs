using Microsoft.AspNetCore.Mvc;

public class EmployeeController : Controller
{
    public IActionResult Profile()
    {
        // 你可以從 Session 拿姓名、帳號等資料，傳給 View 顯示
        var name = HttpContext.Session.GetString("EmployeeName");
        ViewBag.EmployeeName = name;
        return View();
    }
}