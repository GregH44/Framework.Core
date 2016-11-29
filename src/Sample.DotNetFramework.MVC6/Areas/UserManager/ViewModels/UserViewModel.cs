using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sample.DotNetFramework.MVC6.Areas.UserManager.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Prénom")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Nom")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Compte")]
        [StringLength(10)]
        public string Account { get; set; }
    }
}
