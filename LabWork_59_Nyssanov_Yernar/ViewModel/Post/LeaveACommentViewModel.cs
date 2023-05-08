using System.ComponentModel.DataAnnotations;

namespace LabWork_59_Nyssanov_Yernar.ViewModel.Post;
using Models.PostUser;

public class LeaveACommentViewModel
{
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Введите минимум один символ, максимум 50")]
    public string Comment { get; set; }
    public Post Post { get; set; }
}