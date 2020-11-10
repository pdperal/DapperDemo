using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DapperDemo.Repository
{
    public class BonusRepository : IBonusRepository
    {
        private IDbConnection db;

        public BonusRepository(IConfiguration configuration)
        {
            db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
        public List<Employee> GetEmployeesWithCompany(int id)
        {
            var sql = "SELECT E.*, C.* FROM Employees AS E INNER JOIN Companies AS C on E.CompanyId = C.CompanyId";
            if (id != 0)
            {
                sql += " WHERE E.CompanyId = @Id";
            }

            var employee = db.Query<Employee, Company, Employee>(sql, (empl, comp) => 
                {
                    empl.Company = comp;
                    return empl;
                }, new { id}, splitOn: "CompanyId");

            return employee.ToList();
        }
    }
}
