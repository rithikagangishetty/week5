namespace week5.Models
{
    public class EmployeeDetailsSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

       public string EmployeeCollectionName { get; set; } = null!;
    }
}
