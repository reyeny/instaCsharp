using System.ComponentModel.DataAnnotations;
using LabWork_59_Nyssanov_Yernar.Enum;

namespace LabWork_59_Nyssanov_Yernar.ViewModel.User;

public class FullInfoViewModel
{
    [DataType(DataType.PhoneNumber)]
    [Required(ErrorMessage = "Укажите номер телефона")]
    [Display(Name = "Укажите номер телефона")]
    public string UserNumber { get; set; }
    
    [Required(ErrorMessage = "Введите ваше имя")]
    [Display(Name = "Укажите Ваше Имя")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Введите информацию о себе")]
    [Display(Name = "Укажите Информацию о себе")]
    public string UserAbout { get; set; }
    
    public UserGender Gender { get; set; }
}