namespace LabWork_59_Nyssanov_Yernar.Models.PostUser;

public class Post
{
    public int Id { get; set; }

    public string UserId { get; set; }
    public User? User { get; set; }
    public string ImagePath { get; set; }
    public string Description { get; set; }

    public DateTime CreatedDate { get; set; }
    
    public List<Like> Likes { get; set; }
    public List<Comment> Comments { get; set; }
}