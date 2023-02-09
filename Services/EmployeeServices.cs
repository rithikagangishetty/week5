using week5.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
namespace week5.Services
{
    public class EmployeeServices
    {
       
        private readonly IMongoCollection<EmployeeDetails> Employees;

        public EmployeeServices(
            IOptions<EmployeeDetailsSettings> employeeDetailsSettings)
        {
            var mongoClient = new MongoClient(
                employeeDetailsSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                employeeDetailsSettings.Value.DatabaseName);

            Employees = mongoDatabase.GetCollection<EmployeeDetails>(
                employeeDetailsSettings.Value.EmployeeCollectionName);
        }

        public async Task<List<EmployeeDetails>> GetAsync() =>
            await Employees.Find(_ => true).ToListAsync();

        public async Task<EmployeeDetails?> GetAsync(ObjectId id) =>
            await Employees.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(EmployeeDetails newEmployeeDetails) =>
            await Employees.InsertOneAsync(newEmployeeDetails);

        public async Task UpdateAsync(ObjectId id, EmployeeDetails updatedEmployeeDetails) =>
            await Employees.ReplaceOneAsync(x => x.Id ==  id, updatedEmployeeDetails);

        public async Task RemoveAsync(ObjectId id) =>
            await Employees.DeleteOneAsync(x => x.Id == id);
    }
}

