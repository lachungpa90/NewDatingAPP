using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly DataContext _dataContext;
        public UserController(DataContext dataContext)
        {

            _dataContext=dataContext;
        }

        [HttpGet]
        public async Task< ActionResult <IEnumerable<AppUser>>> GetUsers()
        {
            return await _dataContext.Users.ToListAsync();


        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _dataContext.Users.FindAsync(id);

        }
        
    }
}