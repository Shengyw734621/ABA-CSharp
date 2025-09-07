using Dapper;
using MySql.Data.MySqlClient;
using MyApp.Models;

public class CustomerService
{
    private readonly IConfiguration _config;

    public CustomerService(IConfiguration config)
    {
        _config = config;
    }

    public IEnumerable<CustomerSummary> GetSummaries(string? name, string? type, string? county)
    {
        using var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection"));

        var sql = @"SELECT 
                        customer_id AS Id,
                        公司名稱 AS CompanyName,
                        產業類別 AS IndustryType,
                        區域 AS Region,
                        縣市 AS County
                    FROM customer
                    WHERE 1=1";

        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(name))
        {
            sql += " AND 公司名稱 LIKE @Name";
            parameters.Add("Name", $"%{name}%");
        }
        if (!string.IsNullOrEmpty(type))
        {
            sql += " AND 產業類別 = @Type";
            parameters.Add("Type", type);
        }
        if (!string.IsNullOrEmpty(county))
        {
            sql += " AND 縣市 = @County";
            parameters.Add("County", county);
        }

        return connection.Query<CustomerSummary>(sql, parameters);
    }

    public IEnumerable<CustomerSummary> GetAllSummaries()
    {
        using var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
        return connection.Query<CustomerSummary>(
            @"SELECT 
            customer_id AS Id,
            `公司名稱` AS CompanyName,
            `縣市` AS County,
            `區域` AS Region,
            `產業類別` AS IndustryType
            FROM customer"
        );
    }

    public CustomerSelect GetById(int id)
    {
        using var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection"));

        return connection.QueryFirstOrDefault<CustomerSelect>(
            @"SELECT 
            customer_id AS Id,
            `公司名稱` AS CompanyName,
            `產業類別` AS IndustryType,
            `郵遞區號` AS PostalCode,
            `地址` AS Address,
            `區域` AS Region,
            `縣市` AS County,
            `國家` AS Country,
            `公司統編` AS TaxId,
            Email AS Email,
            `電話號碼` AS Phone,
            `傳真號碼` AS Fax,
            `業務窗口` AS SalesContact,
            `技術窗口` AS TechnicalContact
            FROM customer
            WHERE customer_id = @Id",
            new { Id = id }
        );
    }

    public int Insert(Customer customer)
    {
        using var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
        string sql = @"INSERT INTO customer 
        (產業類別, 公司名稱, 郵遞區號, 地址, 區域, 
        縣市, 國家, 公司統編, Email, 電話號碼, 傳真號碼, 業務窗口, 技術窗口)
        VALUES 
        (@CustomerType, @CustomerName, @CustomerPostalCode, @CustomerAddress, @CustomerRegion, 
        @CustomerCounty, @CustomerCountry, @CustomerTaxID, @CustomerEmail, @CustomerPhone, @CustomerFax, @CustomerSales, @CustomerTechnicant);
        SELECT LAST_INSERT_ID();";
        
        // 使用 ExecuteScalar 拿到新插入資料的 ID
        int newId = connection.ExecuteScalar<int>(sql, customer);
        return newId;
    }
    public void Update(Customer customer)
    {
        using var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
        string sql = @"UPDATE customer SET 
                        產業類別 = @CustomerType,
                        公司名稱 = @CustomerName,
                        郵遞區號 = @CustomerPostalCode,
                        地址 = @CustomerAddress,
                        區域 = @CustomerRegion,
                        縣市 = @CustomerCounty,
                        國家 = @CustomerCountry,
                        公司統編 = @CustomerTaxID,
                        Email = @CustomerEmail,
                        電話號碼 = @CustomerPhone,
                        傳真號碼 = @CustomerFax,
                        業務窗口 = @CustomerSales,
                        技術窗口 = @CustomerTechnicant
                    WHERE customer_id = @Id";  // ← 用主鍵條件！

        connection.Execute(sql, customer);
    }
    public void Delete(int customerId)
    {
        using var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
        
        string sql = "DELETE FROM customer WHERE customer_id = @customer_id";
        
        int affectedRows = connection.Execute(sql, new { customer_id = customerId });

        if (affectedRows == 0)
        {
            throw new Exception($"刪除失敗，找不到 customer_id = {customerId} 的紀錄");
        }
    }
}