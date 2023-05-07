using LabWork_59_Nyssanov_Yernar.DbContext;
using LabWork_59_Nyssanov_Yernar.Models;
using LabWork_59_Nyssanov_Yernar.Models.PostUser;
using LabWork_59_Nyssanov_Yernar.ViewModel.Post;
using LabWork_59_Nyssanov_Yernar.ViewModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabWork_59_Nyssanov_Yernar.Controllers;

public class AccountsController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ProjectContext _projectContext;

    public AccountsController(
        UserManager<User> userManager,
        SignInManager<User> signInManager, 
        IHostEnvironment hostEnvironment, 
        ProjectContext projectContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _hostEnvironment = hostEnvironment;
        _projectContext = projectContext;
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

    [HttpGet]
    public IActionResult Index()
    {
        var users = _projectContext.Users.ToList();
        var user = _userManager.GetUserAsync(User).Result;
        users.Remove(user!);

        FeedViewModel model = new()
        {
            Users = users,
            meUser = user!
        };
        
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> About(string userId)
    {
        var user = (await _userManager.FindByIdAsync(userId))!;
        
        user.Posts = _projectContext.Posts
            .Include(post => post.Likes)
            .Include(post => post.Comments)
            .Where(post => post.User!.Id == user.Id)
            .ToList();
        var follow = _projectContext.Follows
            .Include(follow1 => follow1.FollowerUser)
            .Include(follow1 => follow1.FollowingUser)
            .Where(follow1 => follow1.FollowingUser.Id == user.Id)
            .ToList();
        var following = _projectContext.Follows
            .Include(follow1 => follow1.FollowerUser)
            .Include(follow1 => follow1.FollowingUser)
            .Where(follow1 => follow1.FollowerUser.Id == user.Id)
            .ToList();


        AboutViewModel model = new()
        {
            Follows = follow,
            User = user,
            Folowing = following
        };

        return View(model);
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

    [HttpGet]
    public IActionResult CreatePost()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(CreatePostViewModel model)
    {
        if (ModelState.IsValid)
        {
            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var imagesPath = Path.Combine(wwwrootPath, "postImages");
            if (!Directory.Exists(imagesPath)) Directory.CreateDirectory(imagesPath);
            var fileName = Guid.NewGuid() + Path.GetExtension(model.PostCover.FileName);
            var filePath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot/postImages", fileName);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await model.PostCover.CopyToAsync(stream);
            
            var imageUrl = Url.Content($"/postImages/{fileName}");

            var user = await _userManager.GetUserAsync(User);

            Post post = new()
            {
                User = user,
                Description = model.Title,
                CreatedDate = DateTime.Now.ToUniversalTime(),
                ImagePath = imageUrl,
                UserId = user!.Id
            };
            _projectContext.Posts.Add(post);
            await _projectContext.SaveChangesAsync();
            
            user.Posts.Add(post);
            await _userManager.UpdateAsync(user);
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult FillInfoPost(string id)
    {
        if (_projectContext.Posts.Any(post => post.Id.ToString() == id))
        {
            var post = _projectContext.Posts
                .Include(post => post.User)
                .Include(post => post.Likes)
                .Include(post => post.Comments)
                .ThenInclude(comment => comment.User)
                .FirstOrDefault(post => post.Id.ToString() == id);
            LeaveACommentViewModel postComment = new() { Post = post! };
            return View(postComment);
        }
        return NotFound();
    }

    
    [HttpPost]
    public async Task<IActionResult> LeaveAComment(LeaveACommentViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        var post = _projectContext.Posts
            .Include(post => post.User)
            .Include(post => post.Likes)
            .Include(post => post.Comments)
            .FirstOrDefault(post => post.Id == model.Post.Id);

        Comment comment = new()
        {
            Description = model.Comment,
            Post = post!,
            CreatedDate = DateTime.Now.ToUniversalTime(),
            User = user!,
            UserId = user!.Id,
            PostId = post!.Id
        };
        _projectContext.Comments.Add(comment);
        await _projectContext.SaveChangesAsync();

        return RedirectToAction("FillInfoPost", new { id = model.Post.Id});
    }

    [HttpPost]
    public async Task<ActionResult> Like(LeaveACommentViewModel model)
    {
        
        var user = await _userManager.GetUserAsync(User);
        var post = await _projectContext.Posts.FindAsync(model.Post.Id);
        if (post is null) return NotFound();

        var checkOnLike = _projectContext.Likes
            .Where(like1 => like1.UserId == user!.Id && like1.PostId == post.Id)
            .Any(like1 => like1.PostId == model.Post.Id);

        if (checkOnLike)
        {
            var likeRemove = _projectContext.Likes.FirstOrDefault(like1 => like1.PostId == model.Post.Id)!;
            _projectContext.Remove(likeRemove);
            await _projectContext.SaveChangesAsync();
            return RedirectToAction("FillInfoPost", new { id = model.Post.Id });
        }
        
        Like like = new()
        {
            UserId = user!.Id,
            User = user,
            Post = post!,
            PostId = post!.Id
        };
        
        _projectContext.Likes.Add(like);
        await _projectContext.SaveChangesAsync();
        
        return RedirectToAction("FillInfoPost", new { id = model.Post.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Follow(string followUserId)
    {
        var userToFollow = (await _projectContext.Users.FindAsync(followUserId))!;
        var userFollowing = await _userManager.GetUserAsync(User);
    
        var checkOnFollow = _projectContext.Follows
                .Where(follow => follow.FollowerUser.Id == followUserId)
                .Any(follow => follow.FollowerUser.Id == followUserId);

        if (!checkOnFollow)
        {
            Follow follow = new()
            {
                FollowerUser = userToFollow,
                FollowerUserId = userToFollow.Id,
                FollowingUser = userFollowing!,
                FolowingUserId = userToFollow.Id
            };
            _projectContext.Follows.Add(follow);
            await _projectContext.SaveChangesAsync();
            return RedirectToAction("About", new {userId = userToFollow.Id});
        }

        var followRemove = await _projectContext.Follows
            .FirstOrDefaultAsync(follow => follow.FollowerUser.Id == followUserId);
        
        _projectContext.Follows.Remove(followRemove!);
        await _projectContext.SaveChangesAsync();
        return RedirectToAction("About", new {userId = userToFollow.Id});
    }
}
