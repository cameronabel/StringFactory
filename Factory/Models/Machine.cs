using System.ComponentModel.DataAnnotations;

namespace Factory.Models;

public class Machine
{
  public int MachineId { get; set; }
  [Required(ErrorMessage = "The Machine's name can't be empty!")]
  public string Name { get; set; }
  public MachineStatus MachineStatus { get; set; }
  public List<EngineerMachine> JoinEntities { get; }
}

public enum MachineStatus
{
  Deployed,
  Down,
  Reserve
}