using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using MyContacts.API.Models;
using MyContacts.SqlClient.Tables;
using Microsoft.AspNetCore.Identity;
using MyContacts.Core.Services.Contacts;

namespace MyContacts.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private IConfiguration _config;
        private IContactsRepository _contactsRepository;
        public AuthController(IConfiguration config, IContactsRepository contactsRepository)
        {
            _config = config;
            _contactsRepository = contactsRepository;
        }

        [Route(""), HttpPost, AllowAnonymous]
        public async Task<ApiResult<BearerToken>> GenerateToken()
        {
            var response = Unauthorized();

            var authHeader = (string)this.Request.Headers["Authorization"];

            //Extract credentials
            string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

            int seperatorIndex = usernamePassword.IndexOf(':');

            var username = usernamePassword.Substring(0, seperatorIndex);
            var password = usernamePassword.Substring(seperatorIndex + 1);

            var login = new LoginViewModel() { Username = username, Password = password };

            try
            {
                var user = await Authenticate(login);

                if (user != null)
                {
                    var token = await BuildToken(user);
                    return new ApiResult<BearerToken>(token);
                }

                return new ApiResult<BearerToken>()
                {
                    Error = "Unauthorized to user"
                };
            }
            catch (Exception ex)
            {
                return new ApiResult<BearerToken>()
                {
                    Error = ex.Message
                };
            }
        }

        [Route("create"), HttpPost, AllowAnonymous]
        public async Task<ApiResult<string>> CreateLoginAccess([FromBody]MyContactsUser user)
        {
            try
            {
                return new ApiResult<string>(await _contactsRepository.RegisterUserAsync(user));
            }
            catch (Exception ex)
            {
                return new ApiResult<string>()
                {
                    Error = ex.Message
                };
            }
        }

        private async Task<BearerToken> BuildToken(MyContactsUser user)
        {
            // Add claims to limit access to certain resources
            //var claims = new List<Claim>()
            //{
            //    new Claim(JwtRegisteredClaimNames.Sub, user.Name),
            //    new Claim(JwtRegisteredClaimNames.Email, user.Email),
            //    new Claim(JwtRegisteredClaimNames.Birthdate, user.BirthDate.ToString("yyyy-MM-dd")),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            //};

            // Build Token using JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["JWT:Issuer"],
                _config["JWT:Audience"],
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            return await Task.FromResult(
                new BearerToken()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    ExpirationDate = token.ValidTo
                }
            );
        }

        private async Task<MyContactsUser> Authenticate(LoginViewModel login)
        {
            return await Task.FromResult(await _contactsRepository.ValidateLoginCredsAsync(login));
        }
    }
}