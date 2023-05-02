using Microsoft.AspNetCore.Identity;

namespace LabWork_59_Nyssanov_Yernar.Models;

public class User : IdentityUser
{
    public string PathToFile { get; set; }
}