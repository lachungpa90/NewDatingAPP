using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Entity;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class AccountController: BaseApiController
    {
        private readonly DataContext _dataContext;

        public AccountController(DataContext datacontext)
        {
            _dataContext=datacontext;            
        }
        
        public async Task<ActionResult<AppUser>>Register(string username, string password)
        {
            using var hmac=new HMACSHA512();
            var user=new AppUser
            {
                Username=username,
                PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt=hmac.Key
            };
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();
            return user;
        }
    }
}