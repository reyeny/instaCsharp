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
        return View("Log/Register");
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

            var user = await _userManager.FindByNameAsync(model.Name);
            if (user is not null)
            {                
                ViewBag.NameReturn = "Введите другой логин";
                return View("Log/Register");
            }
            
            var checkOnEmail = await _userManager.FindByEmailAsync(model.Email);
            if (checkOnEmail is not null)
            {
                ViewBag.EmailReturn = "Эта почта уже занята";
                return View("Log/Register");
            }
            
            user = new User
            {
                Email = model.Email,
                UserName = model.Name,
                PathToFile = model.PathToImage,
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

        return View("Log/Register", model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        if (!User.Identity.IsAuthenticated) 
            return View("Log/Login");
        return RedirectToAction("Index");
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
        {
            user = (await _userManager.FindByNameAsync(model.Name))!;
            if (user is null)
                user = (await _userManager.FindByEmailAsync(model.Email))!;
        }
        
        if (user is not null)
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
        ViewBag.LogIn = "Некорректные логин или пароль";
        return View("Log/Login");
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

    [HttpGet]
    public IActionResult About()
    {
        var user = _userManager.GetUserAsync(User).Result;
        return View(user);
    }
    
    [HttpGet]
    public IActionResult FullInfo()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> FullInfo(FullInfoViewModel model)
    {
        var user = new User();
        if (ModelState.IsValid)
        {
            user = _userManager.GetUserAsync(User).Result;
            
            user!.RealName = model.Name;
            user.PhoneNumber = model.UserNumber;
            user.UserTitle = model.UserAbout;
            user.Gender = model.Gender;
        }

        var result = await _userManager.UpdateAsync(user);
        return RedirectToAction("About");
    }

}
