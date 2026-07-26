using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMSProject.Models;
using PMSProject.Data;
namespace PMSProject.Controllers
{
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
                return RedirectToAction("IndexS"); 
            }
            return View(sprint);
        }
    }
}