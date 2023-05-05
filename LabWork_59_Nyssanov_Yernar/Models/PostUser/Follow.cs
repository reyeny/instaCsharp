namespace LabWork_59_Nyssanov_Yernar.Models.PostUser;

public class Follow
{
    public int Id { get; set; }
    
    public string UserId { get; set; }
    public User User { get; set; }
    
    public string FollowUserId { get; set; }
    public User FollowUser { get; set; }
}