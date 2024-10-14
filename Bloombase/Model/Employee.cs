using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bloombase;

[Table("Employee")]
public class Employee
{
    [Key]
    [Required]
    [MaxLength(100)]
    public string EmployeeId { get; set; }

    [MaxLength(100)]
    public string EmployeeName { get; set; }

    [MaxLength(20)]
    public string PhoneNumber { get; set; }

    [Column(TypeName = "real")]
    public float HourlySalary { get; set; }

    public int AuthorityLevel { get; set; }

    [MaxLength(50)]
    public string Role { get; set; }

    // Define foreign key property
    [ForeignKey("BotanicalGarden")]
    [MaxLength(100)]
    public string BotanicalGardenName { get; set; }
    public List<FlowerbedCare> FlowerbedCares { get; set; }

    public Employee()
    {
        BotanicalGardenName = "Enchanted Greenhouse";
        FlowerbedCares = new List<FlowerbedCare>();
    }

    public Employee(string employeeName, string phoneNumber, float hourlySalary, int authorityLevel, string role, string botanicalGardenName)
    {
        EmployeeName = employeeName;
        PhoneNumber = phoneNumber;
        HourlySalary = hourlySalary;
        AuthorityLevel = authorityLevel;
        Role = role;
        BotanicalGardenName = botanicalGardenName;
        FlowerbedCares = new List<FlowerbedCare>();
    }

    public void AddFlowerbedCare(FlowerbedCare flowerbedCare)
    {
        if (FlowerbedCares.Count < 5)
        {
            FlowerbedCares.Add(flowerbedCare);
        }
        else
        {
            throw new Exception("Employee can't have more than 5 tasks!");
        }
    }
}
