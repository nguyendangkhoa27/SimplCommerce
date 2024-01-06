using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NDK.SimplCommerce.Core.Areas.Core.ViewModels;
using NDK.SimplCommerce.Core.Models;

namespace NDK.SimplCommerce.Core.Areas.Core.Controllers;

[Area("Core")]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    public AccountController(ILogger<AccountController> logger, UserManager<User> userManager,SignInManager<User> signInManager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    [HttpGet("login")]
    public IActionResult Login(string returnUrl)
    {
        if(User.Identity.IsAuthenticated){
            return Redirect("/");
        }
        returnUrl ??= "/";
        ViewData["ReturnUrl"] = returnUrl;
        LoginVm model = new LoginVm();
        return View();
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginVm model,string returnUrl)
    { 
        returnUrl ??= "/";
        model??= new LoginVm();
        ViewData["ReturnUrl"] = returnUrl;
        var user = await _userManager.FindByEmailAsync(model.Email);
        if(user != null){
            var result = await _signInManager.PasswordSignInAsync(user,model.Password,true,lockoutOnFailure:false);
            if(result.Succeeded){
               return Redirect("/");
            }
        }
        return View(model);
    }
    [HttpGet("register")]
    public IActionResult Register(string returnUrl)
    {
         returnUrl ??= "/";
        ViewData["ReturnUrl"] = returnUrl;
        RegisterVm model = new RegisterVm();
        return View(model);
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterVm model,string returnUrl)
    {
         returnUrl ??= "/";
         model??= new RegisterVm();
        ViewData["ReturnUrl"] = returnUrl;
        if(model != null){
            var user = new User{
                UserName = model.Email,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user,model.Password);
            if(result.Succeeded){
               await _signInManager.SignInAsync(user,true);
               return Redirect("/");
            }
        }
        return View(model);
    }
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Redirect("/");
    }
}
