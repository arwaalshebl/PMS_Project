namespace PMSProject.Models
{
    public class SprintModel : BaseEntityModel
    {
        public int Id { get; set; }
        public string SprintName { get; set; }
        public DateTime? StartON { get; set; }
        public DateTime? EndON { get; set; }


    }
}
