using week5.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace week5.Services;

public class EmployeeServices
{
    private readonly IMongoCollection<EmployeeDetails> Employees;

    public EmployeeServices(
        IOptions<EmployeeDetailsSettings> employeesettings)
    {
        var mongoClient = new MongoClient(
            employeesettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            employeesettings.Value.DatabaseName);

        Employees = mongoDatabase.GetCollection<EmployeeDetails>(
            employeesettings.Value.EmployeeCollectionName);
    }

    public async Task<List<EmployeeDetails>> GetAsync() =>
        await Employees.Find(_ => true).ToListAsync();

    public async Task<EmployeeDetails?> GetAsync(string id) =>
        await Employees.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(EmployeeDetails newEmployeeDetails) =>
        await Employees.InsertOneAsync(newEmployeeDetails);

    public async Task UpdateAsync(string id, EmployeeDetails updatedEmployeeDetails) =>
        await Employees.ReplaceOneAsync(x => x.Id == id, updatedEmployeeDetails);

    public async Task RemoveAsync(string id) =>
        await Employees.DeleteOneAsync(x => x.Id == id);
}