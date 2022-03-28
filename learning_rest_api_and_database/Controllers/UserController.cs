using learning_rest_api_and_database.Data;
using learning_rest_api_and_database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learning_rest_api_and_database.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private DemoDbContext _dbContext;

        public UserController(DemoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("GetUsers")]
        public IActionResult Get()
        {
            try
            {
                var users = _dbContext.tblUsers.ToList();

                if (users.Count == 0)
                {
                    return StatusCode(404, "No user found");
                }
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error has occurred");
            }
        }

        [HttpPost("CreateUser")]
        public IActionResult Create([FromBody] UserRequest request)
        {
            tblUser user = new tblUser();
            user.UserName = request.UserName;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.City = request.City;
            user.Country = request.Country;
            user.State = request.State;

            try
            {
                _dbContext.tblUsers.Add(user);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error has occurred");
            }

            var users = _dbContext.tblUsers.ToList();
            return Ok(users);
        }

        [HttpPut("UpdateUser")]
        public IActionResult Update([FromBody] UserRequest request)
        {
            try
            {
                var user = _dbContext.tblUsers.FirstOrDefault(x => x.Id == request.Id);
            
                if (user == null)
                {
                    return StatusCode(404, "No user found");
                }

                user.UserName = request.UserName;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.City = request.City;
                user.Country = request.Country;
                user.State = request.State;

                _dbContext.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error has occurred");
            }

            var users = _dbContext.tblUsers.ToList();
            return Ok(users);
        }

        [HttpDelete("DeleteUser/{Id}")]
        public IActionResult Delete([FromRoute] int Id)
        {
            try
            {
                var user = _dbContext.tblUsers.FirstOrDefault(x => x.Id == Id);

                if (user == null)
                {
                    return StatusCode(404, "No user found");
                }

                _dbContext.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error has occurred");
            }

            var users = _dbContext.tblUsers.ToList();
            return Ok(users);
        }
    }
}
