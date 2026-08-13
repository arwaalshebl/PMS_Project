using System;
namespace PMSProject.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string TableName { get; set; }       // اسم الجدول (مثل Tasks)
        public string Action { get; set; }          // نوع العملية (Added, Modified, Deleted)
        public string? UserId { get; set; }         // من قام بالتعديل
        public string? PrimaryKey { get; set; }      // الـ ID الخاص بالسجل
        public string? RecordTitle { get; set; }
        public string? OldValues { get; set; }      // القيم القديمة بصيغة JSON
        public string? NewValues { get; set; }      // القيم الجديدة بصيغة JSON
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}