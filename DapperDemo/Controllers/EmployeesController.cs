using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DapperDemo.Data;
using DapperDemo.Models;
using DapperDemo.Repository;
using Microsoft.CodeAnalysis.Operations;

namespace DapperDemo.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IBonusRepository _bonusRepository;

        [BindProperty]
        public Employee Employee { get; set; }

        public EmployeesController(IEmployeeRepository employeeRepository, ICompanyRepository companyRepository, IBonusRepository bonusRepository)
        {
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _bonusRepository = bonusRepository;
        }

        // GET: Companies
        public async Task<IActionResult> Index(int companyId = 0)
        {
            List<Employee> employees = _bonusRepository.GetEmployeesWithCompany(companyId);
            return View(employees);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> CompanyList = _companyRepository.GetAll()
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.CompanyId.ToString()
                });

            ViewBag.CompanyList = CompanyList;
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePOST()
        {
            if (ModelState.IsValid)
            {
                _employeeRepository.Add(Employee);
                return RedirectToAction(nameof(Index));
            }
            return View(Employee);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IEnumerable<SelectListItem> CompanyList = _companyRepository.GetAll()
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.CompanyId.ToString()
                });

            ViewBag.CompanyList = CompanyList;

            var employee = _employeeRepository.Find(id.GetValueOrDefault());
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (id != Employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _employeeRepository.Update(Employee);
                return RedirectToAction(nameof(Index));
            }
            return View(Employee);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _employeeRepository.Remove(id.GetValueOrDefault());
            return RedirectToAction(nameof(Index));
        }
    }
}
