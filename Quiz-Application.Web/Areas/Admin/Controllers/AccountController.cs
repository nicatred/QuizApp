using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quiz_Application.Services.Dtos;
using Quiz_Application.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz_Application.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles ="admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<Candidate> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Candidate> _signInManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<Candidate> userManager, RoleManager<IdentityRole> roleManager, SignInManager<Candidate> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            UserRegisterDto userRegisterDto = new UserRegisterDto();
            return View(userRegisterDto);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            var mapped = _mapper.Map<UserRegisterDto, Candidate>(userRegisterDto);
            mapped.UserName = userRegisterDto.Candidate_ID;
            var result = await _userManager.CreateAsync(mapped,userRegisterDto.Password);
            if (result.Succeeded)
            {
                var result2 = await _userManager.AddToRoleAsync(mapped,userRegisterDto.Role);
                if (result2.Succeeded)
                {
                   return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }
            }
            return View(userRegisterDto);
        }
    }
}
