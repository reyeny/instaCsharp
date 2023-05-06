namespace LabWork_59_Nyssanov_Yernar.Models.PostUser;

public class Follow
{
    public int Id { get; set; }
    
    public string FolowingUserId { get; set; }
    public User FollowingUser { get; set; }
    
    public string FollowerUserId { get; set; }
    public User FollowerUser { get; set; }
}