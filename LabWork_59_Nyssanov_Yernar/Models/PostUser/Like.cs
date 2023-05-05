namespace LabWork_59_Nyssanov_Yernar.Models.PostUser;

public class Like
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }

    public int PostId { get; set; }
    public Post Post { get; set; }
}