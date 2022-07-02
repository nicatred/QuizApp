using Business.Abstract;
using DataAccess.Dtos.Concrete;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Quiz_Application.Services.Dtos;
using Quiz_Application.Services.Entities;
using Quiz_Application.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Quiz_Application.Web.Controllers
{
    public class AuthController : Controller
    {

        private readonly IAuthService _authService;
        private readonly UserManager<Candidate> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Candidate> _signInManager;

        public AuthController(IAuthService authService, UserManager<Candidate> userManager, RoleManager<IdentityRole> roleManager, SignInManager<Candidate> signInManager)
        {
            _authService = authService;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _authService.Register(registerDto);
            if (result)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {

            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            var result = await _userManager.CheckPasswordAsync(user, viewModel.Password);
            if (result)
            {
                var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                await _signInManager.UserManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, user.Id));
                await _signInManager.UserManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, user.Name));
                await _signInManager.UserManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
                await _signInManager.SignInAsync(user, isPersistent: true);
                var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();


                var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Role, userRole)
                };
                //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity
                var principal = new ClaimsPrincipal(identity);
                //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                {
                    IsPersistent = true
                });
                if (userRole == "candidate")
                {
                    return RedirectToAction("Index", "Home", new { Area = "candidate" });
                }
                else if (userRole == "admin")
                {
                    return RedirectToAction("Index", "Home", new { Area = "admin" });
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { Area = "admin" });
                }
            }
           
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _authService.LogOut();

            return RedirectToAction("Login", "Auth");
        }
    }
}
