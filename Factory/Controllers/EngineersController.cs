using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Factory.Models;

namespace Factory.Controllers;

public class EngineersController : Controller
{
  private readonly FactoryContext _db;

  public EngineersController(FactoryContext db)
  {
    _db = db;
  }

  public ActionResult Details(int id)
  {
    Engineer engineer = _db.Engineers
                          .Include(engineer => engineer.JoinEntities)
                          .ThenInclude(join => join.Machine)
                          .FirstOrDefault(engineer => engineer.EngineerId == id);
    return View(engineer);
  }

  public ActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public ActionResult Create(Engineer engineer)
  {
    engineer.Status = EmployeeStatus.Active;
    _db.Engineers.Add(engineer);
    _db.SaveChanges();
    return RedirectToAction("Index", "Home");
  }

  public ActionResult Edit(int id)
  {
    Engineer thisEngineer = _db.Engineers
                            .Include(engineer => engineer.JoinEntities)
                            .ThenInclude(join => join.Machine)
                            .FirstOrDefault(engineer => engineer.EngineerId == id);
    ViewBag.Machines = _db.Machines.ToList();
    return View(thisEngineer);
  }

  [HttpPost]
  public ActionResult Edit(int engineerId, string name, List<int> machines)
  {
    Engineer thisEngineer = _db.Engineers
                              .Include(engineer => engineer.JoinEntities)
                              .ThenInclude(join => join.Machine)
                              .FirstOrDefault(engineer => engineer.EngineerId == engineerId);
    thisEngineer.Name = name;
    // IEnumerable<EngineerMachine> joins = _db.EngineerMachines.Include(join => join.EngineerId == engineerId);

    foreach (EngineerMachine join in thisEngineer.JoinEntities)
    {
      if (!machines.Contains(join.MachineId))
      {
        _db.EngineerMachines.Remove(join);
      }
    }
    foreach (int machineId in machines)
    {
      LinkMachine(thisEngineer, machineId);
    }

    _db.Engineers.Update(thisEngineer);
    _db.SaveChanges();
    return RedirectToAction("Index", "Home");
  }

  [HttpPost]
  public void LinkMachine(Engineer engineer, int machineId)
  {
#nullable enable
    EngineerMachine? joinEntity = _db.EngineerMachines.FirstOrDefault(join => (join.MachineId == machineId && join.EngineerId == engineer.EngineerId));
    if (joinEntity == null && machineId != 0)
#nullable disable
    {
      _db.EngineerMachines.Add(new EngineerMachine() { EngineerId = engineer.EngineerId, MachineId = machineId });
      _db.SaveChanges();
    }
  }
  public ActionResult Delete(int id)
  {
    Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
    return View(thisEngineer);
  }

  [HttpPost, ActionName("Delete")]
  public ActionResult DeleteConfirmed(int id)
  {
    Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
    _db.Engineers.Remove(thisEngineer);
    _db.SaveChanges();
    return RedirectToAction("Index", "Home");
  }
}