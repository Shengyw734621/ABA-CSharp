using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Dapper;

namespace MyApp.Services
{
    public class ShipmentService
    {
        private readonly string _connectionString;

        public ShipmentService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<string> GetAdministrativeEmployees()
        {
            using var conn = new MySqlConnection(_connectionString);
            string sql = "SELECT First_Name FROM aba.employee WHERE department='行政部'";
            var result = conn.Query<string>(sql).AsList();
            var rows = conn.Query(sql);
            foreach (var row in rows)
            {
                Console.WriteLine(row.First_Name);
            }
            return result;

        }
        public List<string> GetEmployeesByDepartment(string department)
        {
            using var conn = new MySqlConnection(_connectionString);
            string sql = "SELECT First_Name, Last_Name FROM aba.employee WHERE department=@Dept";
            var list = conn.Query(sql, new { Dept = department })
                        .Select(e => $"{e.First_Name} {e.Last_Name}")
                        .ToList();
            return list;
        }
    }
}