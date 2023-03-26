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
}