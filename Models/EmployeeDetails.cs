using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace week5.Models
{
    public class EmployeeDetails
    {
        [BsonId]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public double AnnualIncome { get; set; }
        public List<string>? EmailIdsList { get; set; }
    }
}
