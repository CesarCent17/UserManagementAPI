using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("user")]
    [EnableCors("AllowOrigin")]

    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public UserController(IConfiguration configuration) 
        {

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 27)));

            _dbContext = new ApplicationDbContext(optionsBuilder.Options);

        }

        [HttpGet]
        [Route("list")]
        public IEnumerable<User> GetUsers()
        {
            return _dbContext.Users.ToList();
        }


        [HttpGet]
        [Route("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        [HttpPost]
        [Route("create")]
        public IActionResult CreateUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(user).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return NoContent();
        }

    }
}
