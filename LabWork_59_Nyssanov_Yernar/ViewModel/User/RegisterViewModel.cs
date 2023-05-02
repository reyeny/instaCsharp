using System.ComponentModel.DataAnnotations;

namespace LabWork_59_Nyssanov_Yernar.ViewModel.User;

public class RegisterViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Заполните Логин")]
    [Display(Name = "Укажите Логин")]
    public string Name { get; set; }
    
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Заполните Email")]
    [Display(Name = "Укажите Email адрес")]
    public string Email { get; set; }

    [DataType(DataType.Upload)]
    [Required(ErrorMessage = "Загрузите Аватарку")]
    [Display(Name = "Загрутите Аватарку")]
    public IFormFile Image { get; set; }
    public string? PathToImage { get; set; }
    
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Укажите пароль")]
    [Display(Name = "Введите пароль")]
    public string Password { get; set; }
    
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
    [Display(Name = "Повторите пароль")]
    public string ConfirmPassword { get; set; }

}