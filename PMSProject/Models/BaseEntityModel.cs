namespace PMSProject.Models
{
    public abstract class BaseEntityModel
    {
        //abstract: It is not a real database table by itself; it is only a "parent template" designed for other models to inherit from.
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // for soft delete
        public bool IsDeleted { get; set; } = false;
    }
}
