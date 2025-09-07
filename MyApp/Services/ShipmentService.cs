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
    }
}