using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMSProject.Data;
using PMSProject.Models;
namespace PMSProject.Controllers
{

    [Authorize(Roles = "Admin,TechLead")]

    public class SprintsController : Controller
    {
        private readonly AppDbContext _context; 
        public SprintsController(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> IndexS()
        {
            var sprints = await _context.Sprints.ToListAsync();
            return View(sprints);
        }
      
        [HttpGet]
        public IActionResult CreateS()
        {
            return View();
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateS(SprintModel sprint)
        {
            if (ModelState.IsValid)
            {
                _context.Sprints.Add(sprint);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard", "Home");
            }
            return View(sprint);
        }
        [HttpPost]
        public async Task<IActionResult> EditSprint(int id,SprintModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Sprints.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard", "Home");
            }
            return RedirectToAction("Dashboard", "Home");
        }
        public IActionResult DeleteS(int id)
        {
            var sprint = _context.Sprints

                .FirstOrDefault(p => p.Id == id);
            if (sprint == null) return NotFound();


            _context.Sprints.Remove(sprint);
            _context.SaveChanges();
            return RedirectToAction("Dashboard", "Home");


        }
    }
}