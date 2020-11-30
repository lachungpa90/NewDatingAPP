using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Entity;
using DatingApp.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    public class AccountController: BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly ITokenService _tokenSevice;

        public AccountController(DataContext datacontext, ITokenService tokenService)
        {
            _dataContext = datacontext;            
            _tokenSevice = tokenService;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>>Register(RegisterDtos registerDtos)
        {
            if(await UserExists(registerDtos.Username))
            {
                 return BadRequest("User already exist");
            }
            using var hmac=new HMACSHA512();
            var user=new AppUser
            {
                Username = registerDtos.Username.ToLower(),
                PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDtos.Password)),
                PasswordSalt=hmac.Key
            };
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();
           return new UserDto
           {
               Username=user.Username,
               Token=_tokenSevice.CreateToken(user)
           };
            
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>>Login(LoginDto loginDto)
        {
            var user = await _dataContext.Users.SingleOrDefaultAsync(x=>x.Username==loginDto.Username);
            if(user==null) return Unauthorized("User doesn't exist");
            using var hmac=new HMACSHA512(user.PasswordSalt);
            var computedHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for(int i=0;i<computedHash.Length;i++)
            {
                if(computedHash[i]!=user.PasswordHash[i])return Unauthorized("invalid Password");
            }
            return new UserDto
            {
                Username=user.Username,
                Token=_tokenSevice.CreateToken(user)
            };
        }
        private async Task<bool>UserExists(string username)
        {
            return await _dataContext.Users.AnyAsync(x=>x.Username.Equals(username.ToLower()));
        }
    }
}