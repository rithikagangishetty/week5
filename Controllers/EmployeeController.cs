using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using week5.Models;

using week5.Services;

namespace week5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase

{

    private readonly EmployeeServices employeeDetailsService;
    private readonly IMongoCollection<EmployeeDetails> Employees;
    public EmployeeController(EmployeeServices EmployeeService, IOptions<EmployeeDetailsSettings> employeeDetailsSettings)
    {
        employeeDetailsService = EmployeeService;
        var mongoClient = new MongoClient(
                employeeDetailsSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            employeeDetailsSettings.Value.DatabaseName);

        Employees = mongoDatabase.GetCollection<EmployeeDetails>(
            employeeDetailsSettings.Value.EmployeeCollectionName);

    }

    [HttpGet]
    public async Task<List<EmployeeDetails>> Get() =>
        await employeeDetailsService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<EmployeeDetails>> Get(string id)
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
    public async Task<IActionResult> Update(string id, EmployeeDetails UpdatedEmployee)
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
    public async Task<IActionResult> UpdatePatch(string id, [FromBody] JsonPatchDocument<EmployeeDetails> PartialUpdatedEmployee)
    {

        var entity = await Employees.Find(x => x.Id == id).FirstOrDefaultAsync();
        if (entity == null)
        {
            return NotFound();
        }
        PartialUpdatedEmployee.ApplyTo(entity, ModelState);

        return Ok();

    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
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
