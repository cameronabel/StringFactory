using System.ComponentModel.DataAnnotations;

namespace Factory.Models;

public class Engineer
{
  public int EngineerId { get; set; }
  [Required(ErrorMessage = "The Engineer's name can't be empty!")]
  public string Name { get; set; }
  public DateTime? HireDate { get; set; }
  public EmployeeStatus Status { get; set; }
  public List<EngineerMachine> JoinEntities { get; }
}

public enum EmployeeStatus
{
  Active,
  Terminated,
  Inactive
}