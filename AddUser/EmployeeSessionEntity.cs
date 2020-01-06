using Microsoft.WindowsAzure.Storage.Table;

namespace AddUser
{
    public class EmployeeSessionEntity : TableEntity
    {
        public EmployeeSessionEntity()
        {
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public double Salaray { get; set; }
    }
}
