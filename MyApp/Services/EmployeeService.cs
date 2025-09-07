using Dapper;
using MySql.Data.MySqlClient;
using MyApp.Models;
using BCrypt.Net;

namespace MyApp.Services
{
    // 介面：定義要提供的功能
    public interface IEmployeeService
    {
        List<Employee> GetAdminEmployees();
        void Register(EmployeeRegister employee);
        bool IsEmailDuplicate(string email);
        EmployeeRegister GetByEmail(string email);
    }

    // 服務實作
    public class EmployeeService : IEmployeeService
    {
        private readonly string _connectionString;

        public EmployeeService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Employee> GetAdminEmployees()
        {
            var employees = new List<Employee>();

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var sql = "SELECT First_Name FROM aba.employee WHERE department = '行政部'";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            First_Name = reader["First_Name"].ToString()
                        });
                    }
                }
            }

            return employees;
        }

        public void Register(EmployeeRegister employee)
        {
            using var connection = new MySqlConnection(_connectionString);

            // 密碼加密
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(employee.EmployeePassword);

            string sql = @"
                INSERT INTO employee 
                (department, 姓名, First_name, Last_name, job_title, email_account, password)
                VALUES 
                (@Department, @EmployeeZhName, @EmployeeFirstName, @EmployeeLastName, @JobTitle, @EmployeeEmailAccount, @EmployeePassword)";

            connection.Execute(sql, new
            {
                employee.Department,
                employee.EmployeeZhName,
                employee.EmployeeFirstName,
                employee.EmployeeLastName,
                employee.JobTitle,
                employee.EmployeeEmailAccount,
                EmployeePassword = hashedPassword
            });
        }

        public bool IsEmailDuplicate(string email)
        {
            using var connection = new MySqlConnection(_connectionString);
            string sql = "SELECT COUNT(*) FROM employee WHERE email_account = @EmployeeEmailAccount";
            int count = connection.ExecuteScalar<int>(sql, new { EmployeeEmailAccount = email });
            return count > 0;
        }

        public EmployeeRegister GetByEmail(string email)
        {
            using var connection = new MySqlConnection(_connectionString);
            string sql = "SELECT email_account AS EmployeeEmailAccount, password AS EmployeePassword, 姓名 AS EmployeeZhName FROM employee WHERE email_account = @EmployeeEmailAccount";
            return connection.QueryFirstOrDefault<EmployeeRegister>(sql, new { EmployeeEmailAccount = email });
        }
    }
}