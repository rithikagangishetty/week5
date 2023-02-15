using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace week5.Models;

public class EmployeeDetails
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    [BsonElement("Name")]
    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    public string? Country { get; set; }

    public double AnnualIncome { get; set; }

    public string? EmailIdsList { get; set; }
}