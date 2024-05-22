using Agency.DTOs.Account;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Agency.Controllers
{
    public class AccountController : Controller
    {
        UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        
        public async Task<IActionResult> CreateRoles()
        {
            IdentityRole role1 = new IdentityRole("Admin");
            IdentityRole role2 = new IdentityRole("Member");
            await _roleManager.CreateAsync(role2);
            await _roleManager.CreateAsync(role1);
            return Ok("Nicee");
        }

        public async Task<IActionResult> CreateAdmin()
        {
            AppUser admin = new AppUser()
            {
                FullName = "Rashid Babazada",
                Email = "rashid@gmail.com",
                UserName = "Admin"
            };
            await _userManager.CreateAsync(admin, "Admin123@");
            await _userManager.AddToRoleAsync(admin, "Admin");
            return Ok("Admin Created");
        }






        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByNameAsync(login.Username);
            if(user == null)
            {
                ModelState.AddModelError("", "User Name or Password is not valid!");
                return View();
            }
            
            var checkResult = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            if (!checkResult.Succeeded)
            {
                ModelState.AddModelError("", "User Name or Password is not valid!");
                return View();
            }
            if (checkResult.IsLockedOut)
            {
                ModelState.AddModelError("", "You Banned! Please wait!");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user, login.Password,login.RememberMe,false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Error, Please again enter!");
                return View();
            }
            return RedirectToAction("index", "Home");

        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }
            AppUser user = new AppUser()
            {
                Email = register.Email,
                UserName = register.UserName,
                FullName = register.FullName,
            };
            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            var roleResult =  await _userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded)
            {
               await _userManager.DeleteAsync(user);
                return View("Error");
            }
            return RedirectToAction("Login");
        }
    }
}
