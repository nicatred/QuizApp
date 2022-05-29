using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Business.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quiz_Application.Services.Entities;
using DataAccess.Dtos.Concrete;
using Quiz_Application.Services.Dtos;
using Business.Utilities.Results.Abstract;
using Business.Utilities.Results.Concrete;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly UserManager<Candidate> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Candidate> _signInManager;
        private readonly IMapper _mapper;
        public AuthManager(SignInManager<Candidate> signInManager, UserManager<Candidate> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IDataResult<string>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (result)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                return new SuccessDataResult<string>(userRole, Messages.SuccessLogin);
            }
            return new ErrorDataResult<string>(Messages.FailLoginUser);
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> Register(RegisterDto registerDto)
        {
            var mappedEntity = _mapper.Map<RegisterDto, Candidate>(registerDto);
            mappedEntity.UserName =  registerDto.Candidate_ID;
            var result = await _userManager.CreateAsync(mappedEntity, registerDto.Password);
              
            //var resultaa =   await _roleManager.CreateAsync(new IdentityRole("candidate"));
            //var roles = await _roleManager.Roles.ToListAsync();
            await _userManager.AddToRoleAsync(mappedEntity, "candidate");
            return result.Succeeded;
        }

    }
}
