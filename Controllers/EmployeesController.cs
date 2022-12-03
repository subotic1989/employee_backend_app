using Employee.API.Data;
using Employee.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.API.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : Controller
    {
        private readonly EmployeeDbContext _context;

        public EmployeesController(EmployeeDbContext employeeDbContext)
        {
            _context = employeeDbContext;
        }
         

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody]EmployeeModel employeeRequest) 
        { 
            employeeRequest.Id= Guid.NewGuid();
            employeeRequest.Date= DateTime.Now;

            await _context.Employees.AddAsync(employeeRequest);
            await _context.SaveChangesAsync();
            return Ok(employeeRequest);
        }


        [HttpGet("{employeeId:Guid}")]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid employeeId)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
            if (employee == null) return NotFound();
            return Ok(employee);
        }

        [HttpPut("{employeeId:Guid}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid employeeId, EmployeeModel employeeRequest)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null) return NotFound();
            employee.Name = employeeRequest.Name;
            employee.Email = employeeRequest.Email;
            employee.Phone = employeeRequest.Phone;
            employee.Department = employeeRequest.Department;

            await _context.SaveChangesAsync();
            return Ok(employee);
        }

        [HttpDelete("{employeeId:Guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null) return NotFound();
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return Ok(employee);
        }

    }
}
