using Dapper;
using DapperDemo.Data;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;

namespace DapperDemo.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private IDbConnection db;

        public EmployeeRepository(IConfiguration configuration)
        {
            db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
        public Employee Add(Employee Employee)
        {
            var sql = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId);"
                        + "SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = db.Query<int>(sql, Employee).Single();

            Employee.EmployeeId = id;
            return Employee;
        }

        public Employee Find(int id)
        {
            var sql = "SELECT * FROM Employees WHERE EmployeeId = @Id";
            return db.Query<Employee>(sql, new { id }).Single();
        }

        public List<Employee> GetAll()
        {
            var sql = "SELECT * FROM Employees";
            
            return db.Query<Employee>(sql).ToList();
        }

        public void Remove(int id)
        {
            var sql = "DELETE FROM Employees WHERE EmployeeId = @Id";
            db.Execute(sql, new { id });
        }

        public Employee Update(Employee Employee)
        {
            var sql = "UPDATE Employees SET Name = @Name, Title = @Title, Email = @Email, Phone = @Phone, CompanyId = @CompanyId WHERE EmployeeId = @EmployeeId";
            db.Execute(sql, Employee);

            return Employee;
        }
    }
}
