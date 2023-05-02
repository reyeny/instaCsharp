using System.ComponentModel.DataAnnotations;

namespace LabWork_59_Nyssanov_Yernar.ViewModel.User;

public class LoginViewModel
{
    [Required(ErrorMessage = "Заполните Логин или Email")]
    [Display(Name = "Укажите Логин или Email")]
    public string Name { get; set; }
    
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Заполните Логин или Email")]
    [Display(Name = "Укажите Логин или Email адрес")]
    public string Email { get; set; }
    
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Укажите пароль")]
    [Display(Name = "Введите пароль")]
    public string Password { get; set; }
}