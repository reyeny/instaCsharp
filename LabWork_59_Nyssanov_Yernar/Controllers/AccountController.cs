using LabWork_59_Nyssanov_Yernar.Models;
using LabWork_59_Nyssanov_Yernar.ViewModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LabWork_59_Nyssanov_Yernar.Controllers;

public class AccountsController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IHostEnvironment _hostEnvironment;

    public AccountsController(
        UserManager<User> userManager,
        SignInManager<User> signInManager, 
        IHostEnvironment hostEnvironment)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _hostEnvironment = hostEnvironment;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var imagesPath = Path.Combine(wwwrootPath, "images");
            if (!Directory.Exists(imagesPath)) Directory.CreateDirectory(imagesPath);
            var fileName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
            var filePath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot/images", fileName);
            await using (var stream = new FileStream(filePath, FileMode.Create)) await model.Image.CopyToAsync(stream);

            var imageUrl = Url.Content($"/images/{fileName}");
            model.PathToImage = imageUrl;

            User user = new()
            {
                Email = model.Email,
                UserName = model.Name,
                PathToFile = model.PathToImage
            };
            
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, true);
                return RedirectToAction("Index", "Accounts");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        User user = null!;
        if (model.Name is null) model.Name = model.Email;
        if (model.Email is null) model.Email = model.Name;

        if (!string.IsNullOrEmpty(model.Name))
            user = (await _userManager.FindByNameAsync(model.Name))!;
        if (!string.IsNullOrEmpty(model.Email))
            user = (await _userManager.FindByEmailAsync(model.Email))!;
        
        if (user != null)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                true,
                false
            );
            if (signInResult.Succeeded) 
                return RedirectToAction("Index", "Accounts");
        }
        ModelState.AddModelError(string.Empty, "Некорректные логин или пароль");
        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }

    public IActionResult Index()
    {
        return View();
    }

}
