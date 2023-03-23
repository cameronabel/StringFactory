namespace Factory.Models;

public class Machine
{
  public int MachineId { get; set; }
  public string Name { get; set; }
  public string SerialNumber { get; set; }
  public MachineStatus MachineStatus { get; set; }
  public List<EngineerMachine> JoinEntities { get; }
}

public enum MachineStatus
{
  Deployed,
  Down,
  Reserve
}