using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Factory.Models;

namespace Factory.Controllers;

public class MachinesController : Controller
{
  private readonly FactoryContext _db;

  public MachinesController(FactoryContext db)
  {
    _db = db;
  }

  public ActionResult Details(int id)
  {
    Machine machine = _db.Machines
                          .Include(machine => machine.JoinEntities)
                          .ThenInclude(join => join.Engineer)
                          .FirstOrDefault(machine => machine.MachineId == id);
    return View(machine);
  }

  public ActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public ActionResult Create(Machine machine)
  {
    machine.MachineStatus = MachineStatus.Deployed;
    _db.Machines.Add(machine);
    _db.SaveChanges();
    return RedirectToAction("Index", "Home");
  }

  public ActionResult Edit(int id)
  {
    Machine thisMachine = _db.Machines
                            .Include(machine => machine.JoinEntities)
                            .ThenInclude(join => join.Engineer)
                            .FirstOrDefault(machine => machine.MachineId == id);
    ViewBag.Engineers = _db.Engineers.ToList();
    return View(thisMachine);
  }

  [HttpPost]
  public ActionResult Edit(int machineId, string name, List<int> engineers, MachineStatus machineStatus)
  {
    Machine thisMachine = _db.Machines
                              .Include(machine => machine.JoinEntities)
                              .ThenInclude(join => join.Engineer)
                              .FirstOrDefault(machine => machine.MachineId == machineId);
    thisMachine.Name = name;
    thisMachine.MachineStatus = machineStatus;

    foreach (EngineerMachine join in thisMachine.JoinEntities)
    {
      if (!engineers.Contains(join.EngineerId))
      {
        _db.EngineerMachines.Remove(join);
      }
    }

    foreach (int engineerId in engineers)
    {
      LinkEngineer(thisMachine, engineerId);
    }

    _db.Machines.Update(thisMachine);
    _db.SaveChanges();
    return RedirectToAction("Index", "Home");
  }

  [HttpPost]
  public void LinkEngineer(Machine machine, int engineerId)
  {
#nullable enable
    EngineerMachine? joinEntity = _db.EngineerMachines.FirstOrDefault(join => (join.MachineId == machine.MachineId && join.EngineerId == engineerId));
    if (joinEntity == null && engineerId != 0)
#nullable disable
    {
      _db.EngineerMachines.Add(new EngineerMachine() { EngineerId = engineerId, MachineId = machine.MachineId });
      _db.SaveChanges();
    }
  }

  public ActionResult Delete(int id)
  {
    Machine thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
    return View(thisMachine);
  }

  [HttpPost, ActionName("Delete")]
  public ActionResult DeleteConfirmed(int id)
  {
    Machine thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
    _db.Machines.Remove(thisMachine);
    _db.SaveChanges();
    return RedirectToAction("Index", "Home");
  }
}