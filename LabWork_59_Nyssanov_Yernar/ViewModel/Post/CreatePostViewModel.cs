using System.ComponentModel.DataAnnotations;

namespace LabWork_59_Nyssanov_Yernar.ViewModel.Post;

public class CreatePostViewModel
{
    [DataType(DataType.Upload)]
    [Required(ErrorMessage = "Загрузите Обложку")]
    [Display(Name = "Загрутите Обложку")]
    public IFormFile PostCover { get; set; }
    
    [Required(ErrorMessage = "Заполните Инофрмацию о посте")]
    [StringLength(150, MinimumLength = 10, ErrorMessage = "Минимальное чисто символов: 10, Максимальное 150")]
    [Display(Name = "Укажите информацию о посте")]
    public string Title { get; set; }
}