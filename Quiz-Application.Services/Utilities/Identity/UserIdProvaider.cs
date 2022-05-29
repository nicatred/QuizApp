using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using Business.Utilities.IoC;

namespace Business.Constants
{
    public static class UserIdProvaider
    {
        public static string GetUserId()
        {
            //var _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>(); ;
            //var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            //var handler = new JwtSecurityTokenHandler();
            //var jwtSecurityToken = handler.ReadJwtToken(accessToken);
            //var result = Convert.ToInt32(jwtSecurityToken.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            //return result;
            var identity = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext.User.Identity.Name;
            return identity;
        }
    }
}
