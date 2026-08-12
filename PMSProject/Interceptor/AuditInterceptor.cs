using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PMSProject.Models;
using System.Security.Claims;
using System.Text.Json;
namespace PMSProject.Data.Interceptors
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuditInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateAuditableEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateAuditableEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        private void UpdateAuditableEntities(DbContext? context)
        {
            if (context == null) return;
            // جلب الـ UserId الحالي
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            // تتبع التغييرات
            context.ChangeTracker.DetectChanges();
            var auditLogs = new List<AuditLog>();
            foreach (var entry in context.ChangeTracker.Entries())
            {
                // نتجاهل جدول الـ AuditLog نفسه أو الكائنات غير المتغيرة
                if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditLog = new AuditLog
                {
                    TableName = entry.Metadata.GetTableName() ?? entry.Entity.GetType().Name,
                    UserId = userId,
                    Timestamp = DateTime.Now,
                    RecordTitle = GetRecordTitle(entry)
                };
                // التعامل مع Soft Delete إذا كان الكائن يرث من BaseEntityModel
                if (entry.State == EntityState.Deleted && entry.Entity is BaseEntityModel baseEntity)
                {
                    entry.State = EntityState.Modified;
                    baseEntity.IsDeleted = true;
                    auditLog.Action = "Deleted (Soft)";
                }
                else
                {
                    auditLog.Action = entry.State.ToString(); // Added, Modified
                }
                var oldValues = new Dictionary<string, object?>();
                var newValues = new Dictionary<string, object?>();
                foreach (var property in entry.Properties)
                {
                    // تجاهل الـ Temporary properties أثناء الإضافة
                    if (property.IsTemporary) continue;
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditLog.PrimaryKey = property.CurrentValue?.ToString() ?? "";
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            newValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Modified:
                            // نسجل فقط الحقول التي تغيرت فعلياً
                            if (property.IsModified)
                            {
                                oldValues[propertyName] = property.OriginalValue;
                                newValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
                auditLog.OldValues = oldValues.Count == 0 ? null : JsonSerializer.Serialize(oldValues);
                auditLog.NewValues = newValues.Count == 0 ? null : JsonSerializer.Serialize(newValues);
                auditLogs.Add(auditLog);
            }
            // إضافة السجلات لجدول الـ AuditLogs ليتم حفظها مع العملية الأصلية
            if (auditLogs.Count > 0)
            {
                context.Set<AuditLog>().AddRange(auditLogs);
            }
        }
        // دالة مساعدة لاستخراج اسم السجل (عنوان المهمة، اسم المشروع، إلخ)
        private string GetRecordTitle(EntityEntry entry)
        {
            var entity = entry.Entity;
            var titleProp = entity.GetType().GetProperty("TaskName")
                         ?? entity.GetType().GetProperty("ProjectName")
                         ?? entity.GetType().GetProperty("SprintName")
                         ?? entity.GetType().GetProperty("Name");
            if (titleProp != null)
            {
                return titleProp.GetValue(entity)?.ToString() ?? "Unknown";
            }
            return "N/A";
        }
    }
}
