using Microsoft.AspNetCore.Mvc;
using PMSProject.Data; 
using Microsoft.EntityFrameworkCore;
using PMSProject.Models;
namespace PMSProject.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly AppDbContext _context;
        
        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }
        


        public async Task<IActionResult> IndexP()
        {
            var projects = await _context.Projects.ToListAsync();
            return View(projects);
        }

        
        public IActionResult CreateP()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateP(ProjectModel project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); 
            }
            return View(project); 
        }
    }
}