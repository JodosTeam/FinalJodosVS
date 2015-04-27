namespace JodosServer.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserModel
    {
        [Required(ErrorMessage = "יש להזין שם משתמש.")]
        [Display(Name = "שם משתמש")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "יש להזין סיסמא.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "אישור סיסמא")]
        [Compare("Password", ErrorMessage = "סיסמאות לא תואמות.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "יש להזין שם פרטי.")]
        [Display(Name = "שם פרטי")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "יש להזין שם משפחה.")]
        [Display(Name = "שם משפחה ")]
        public string lastName { get; set; }
    }
}