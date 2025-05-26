using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using ExpenseManagerAPI.Model;
using ExpenseManagerAPI.Data;

namespace ExpenseManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext dbcontext;
        private readonly IPasswordHasher<UserModel> _passwordHasher;

        public AccountController(AppDbContext context, IPasswordHasher<UserModel> passwordHasher)
        {
            dbcontext = context;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("register")]
        public IActionResult Registration(RegistrationModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (model.Password != model.ConfirmPassword)
                {
                    return BadRequest("Passwords do not match");
                }

                if (dbcontext.Users.Any(u => u.Email == model.Email))
                {
                    return BadRequest("Email already exists");
                }

                var user = new UserModel
                {
                    Email = model.Email,
                    MobileNo = model.MobileNo ?? "",
                    CreateAt = DateTime.UtcNow,
                    IsActive = 1
                };

                // Corrected: use the user instance for hashing
                user.Password = _passwordHasher.HashPassword(user, model.Password);
                user.ConfirmPassword = model.ConfirmPassword;

                dbcontext.Users.Add(user);
                dbcontext.SaveChanges();

                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong: {ex.Message}");
            }
        }






        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LogInModel logInModel)
        {
            var user = dbcontext.Users.FirstOrDefault(e => e.Email == logInModel.Email);

            if (user != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(user, user.Password, logInModel.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    return Ok(new
                    {
                        message = "Login successful",
                        email = user.Email,
                        HostId=user.HostId
                        // Avoid returning password
                    });
                }
            }

            return Unauthorized(new { message = "Invalid email or password" });
        }


        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()
        {
            // Simply return a success message
            return Ok(new { message = "Logout successful. Please clear user session on client side." });
        }

        



        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        { 
            return Ok(dbcontext.Users.ToList());
        }

        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUser(int id) {
            var user = dbcontext.Users.FirstOrDefault(x => x.HostId == id);
            if(user == null)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
           
        }
    }
}
