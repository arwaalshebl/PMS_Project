using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSProject.Models
{
    public class PublishHistoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        
        //relation with project model
        public int ProjectId { get; set; }
        public ProjectModel Project { get; set; }


        public DateTime PublishDate { get; set; }
        public string Reason { get; set; }
        public string Details { get; set; }
    }


}
