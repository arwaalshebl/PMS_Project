using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMSProject.Data;
using PMSProject.Models;
[Authorize(Roles = "Admin,TechLead")]
public class AuditLogsController : Controller
{
    private readonly AppDbContext _context;
    public AuditLogsController(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> IndexA(string? userId, DateTime? startDate, DateTime? endDate)
    {
        var logsQuery = _context.AuditLogs.AsQueryable();
        // 1. الفلترة حسب المستخدم
        if (!string.IsNullOrEmpty(userId))
        {
            logsQuery = logsQuery.Where(l => l.UserId == userId);
        }
        // 2. الفلترة حسب تاريخ البداية
        if (startDate.HasValue)
        {
            logsQuery = logsQuery.Where(l => l.Timestamp >= startDate.Value);
        }
        if (endDate.HasValue)
        {
            var adjustedEndDate = endDate.Value.AddDays(1);
            logsQuery = logsQuery.Where(l => l.Timestamp < adjustedEndDate);
        }
        var logs = await logsQuery.OrderByDescending(l => l.Timestamp).ToListAsync();
        ViewBag.Users = await _context.Users.ToListAsync();
        ViewBag.UserNames = await _context.Users.ToDictionaryAsync(u => u.Id, u => u.UserName);
        return View(logs);
    }
}