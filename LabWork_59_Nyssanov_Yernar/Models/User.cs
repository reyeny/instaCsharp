using LabWork_59_Nyssanov_Yernar.Enum;
using Microsoft.AspNetCore.Identity;

namespace LabWork_59_Nyssanov_Yernar.Models;

public class User : IdentityUser
{
    public string PathToFile { get; set; }
    public string RealName { get; set; }
    public string UserTitle { get; set; }
    public UserGender Gender { get; set; }
}