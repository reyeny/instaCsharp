using System.ComponentModel.DataAnnotations;

namespace LabWork_59_Nyssanov_Yernar.ViewModel.User;

public class SearchViewModel
{
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Введите минимум один символ, максимум 50")]
    public string Name { get; set; }
    public List<Models.User> Users { get; set; }
}