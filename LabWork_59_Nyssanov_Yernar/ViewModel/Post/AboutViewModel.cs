using LabWork_59_Nyssanov_Yernar.Models.PostUser;

namespace LabWork_59_Nyssanov_Yernar.ViewModel.Post;

public class AboutViewModel
{
    public Models.User User { get; set; }
    public List<Follow> Follows { get; set; }
    public List<Follow> Folowing { get; set; }
}