using Business.Abstract;
using DataAccess.Dtos.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Quiz_Application.Services.Dtos;
using Quiz_Application.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz_Application.Web.Controllers
{
    public class AuthController : Controller
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
            LoginDto dto = new LoginDto
            {
                Email = viewModel.Email,
                Password = viewModel.Password
            };
            var result = await _authService.Login(dto);
            if (result.Data=="candidate")
            {
                return RedirectToAction("Index", "Home", new { Area = "candidate" });
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
