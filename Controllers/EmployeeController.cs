using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using week5.Models;
using week5.Services;

namespace week5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase

{ 

    private readonly EmployeeServices employeeDetailsService;
    IList<EmployeeDetails> Employee { get; set; }
    public EmployeeController(EmployeeServices EmployeeService)
    {
        employeeDetailsService = EmployeeService;
    }

    [HttpGet]
    public async Task<List<EmployeeDetails>> Get() =>
        await employeeDetailsService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<EmployeeDetails>> Get(ObjectId id)
    {
        var employee = await employeeDetailsService.GetAsync(id);

        if (employee is null)
        {
            return NotFound();
        }

        return employee;
    }

    [HttpPost]
    public async Task<IActionResult> Post(EmployeeDetails NewEmployee)
    {
        await employeeDetailsService.CreateAsync(NewEmployee);

        return CreatedAtAction(nameof(Get), new { id = NewEmployee.Id }, NewEmployee);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(ObjectId id, EmployeeDetails UpdatedEmployee)
    {
        var employee = await employeeDetailsService.GetAsync(id);

        if (employee is null)
        {
            return NotFound();
        }

        UpdatedEmployee.Id = employee.Id;

        await employeeDetailsService.UpdateAsync(id, UpdatedEmployee);

        return NoContent();
    }
    [HttpPatch("{id:length(24)}")]
    public  IActionResult UpdatePartial(ObjectId id,[FromBody] JsonPatchDocument<EmployeeDetails> PartialUpdatedEmployee)
    {
        var entity = Employee.FirstOrDefault(s => s.Id == id);
        if (entity == null)
           return NotFound();
        PartialUpdatedEmployee.ApplyTo(entity,ModelState);
        return Ok();

    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(ObjectId id)
    {
        var employee = await employeeDetailsService.GetAsync(id);

        if (employee is null)
        {
            return NotFound();
        }

        await employeeDetailsService.RemoveAsync(id);

        return NoContent();
    }
}

