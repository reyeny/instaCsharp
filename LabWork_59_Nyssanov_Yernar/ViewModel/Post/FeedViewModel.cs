using LabWork_59_Nyssanov_Yernar.Models.PostUser;

namespace LabWork_59_Nyssanov_Yernar.ViewModel.Post;
using LabWork_59_Nyssanov_Yernar.Models;

public class FeedViewModel
{
    public List<User> UsersAll { get; set; }
    public User meUser { get; set; }
    public List<Follow> Follows { get; set; }
    public Models.PostUser.Post Post { get; set; }
    public string Comment { get; set; }
}