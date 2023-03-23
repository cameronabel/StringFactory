using Microsoft.AspNetCore.Mvc;

using Factory.Models;

namespace Factory.Controllers;

public class EngineersController : Controller
{
  private readonly FactoryContext _db;

  public EngineersController(FactoryContext db)
  {
    _db = db;
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
}