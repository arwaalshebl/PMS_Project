using System.ComponentModel.DataAnnotations;

namespace PMSProject.Models
{
    public class LoginViewModel
    {
        [Display(Name ="User Name")]
        public string UserName { get; set; }
        
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
