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
    Machine thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
    return View(thisMachine);
  }

  [HttpPost]
  public ActionResult Edit(Machine machine)
  {
    _db.Machines.Update(machine);
    _db.SaveChanges();
    return RedirectToAction("Index", "Home");
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