using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Services;

namespace Sat.Recruitment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
     
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/UsersJson
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            List<User> userList = _userService.GetUsers();
            if (userList.Count()==0)
            {
                return NotFound();
            }
            return userList;
        }


        // GET: api/UsersJson/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUser(int userId)
        {
            User user = _userService.GetUser(userId);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/UsersJson/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<ActionResult<Result>> PutUser(User user)
        {
            Result rta = _userService.PutUser(user);

            if (rta.IsSuccess)
                return Ok("Ok");
            else
                return BadRequest(rta.Errors);


        }

        // POST: api/UsersJson
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Result>> PostUser(User user)
        {



            Result rta = _userService.PostUser(user);

            if (rta.IsSuccess)
                return Ok("Ok");
            else
                return BadRequest(rta.Errors);
        }

        // DELETE: api/UsersJson/5
        [HttpDelete("{UserId}")]
        public async Task<ActionResult<Result>> DeleteUser(int UserId)
        {
            Result rta = _userService.DeleteUser(UserId);

            if (rta.IsSuccess)
                return Ok("Ok");
            else
                return BadRequest(rta.Errors);
        }



    

    }
}
