using ExpenseManagerAPI.Model;
using ExpenseManagerAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository
            ;
        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;

        }
        [HttpGet]
        public ActionResult getAllUser()
        {
            var userList = _userRepository.getAllUser();
            return Ok(userList);
        }

        [HttpGet("{UserID}")]
        public ActionResult GetUserID(int userID)
        {
            var userList = _userRepository.GetUserByID(userID);
            if (userList == null)
            {
                return NotFound();
            }
            return Ok(userList);
        }
        [HttpGet("by-host/{hostId}")] // lowercase "hostId"
        public ActionResult GetUserByHost(int hostId)
        {
            var userList = _userRepository.GetUserByHost(hostId);

            if (userList == null || !userList.Any())
            {
                return NotFound($"No users found for HostId: {hostId}");
            }

            return Ok(userList);
        }



        [HttpDelete("{UserID}")]
        public ActionResult DeleteUser(int UserID)
        {
            try
            {
                var success = _userRepository.deleteUser(UserID);
                if (success)
                {
                    return NoContent(); // Return 204 if deletion is successful
                }

                return NotFound("User not found."); // Return 404 if no rows were affected
            }
            catch (InvalidOperationException ex)
            {
                // Handle foreign key conflict
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPost]
        public IActionResult InsertUser([FromBody] PersonModel userModel)
        {
            if (userModel == null)
            {
                return BadRequest("User data cannot be null.");
            }

            if (!ModelState.IsValid) // This will check FluentValidation rules
            {
                return BadRequest(ModelState);
            }

            bool insertUser = _userRepository.insertUser(userModel);
            if (insertUser)
            {
                return Ok(new { Message = "User inserted successfully" });
            }

            return StatusCode(500, "An error occurred while inserting the user.");
        }


        [HttpPut("{UserID}")]
        public IActionResult UpdateUser(int UserID, [FromBody] PersonModel userModel)
        {
            if (userModel == null || UserID != userModel.UserID)
            {
                return BadRequest();
            }
            bool updateUser = _userRepository.updateUser(userModel);
            if (updateUser)
            {
                // Return the updated city data
                var updatedUser = _userRepository.GetUserByID(UserID);
                return Ok(updatedUser);
            }
            return NotFound();
        }

        [HttpGet("dropdown/{EventID}")]
        public IActionResult GetUserDropDown(int EventID)
        {
            var user = _userRepository.GetUserDropDown(EventID);
            if (!user.Any())
                return NotFound("No countries found.");

            return Ok(user);
        }
    }
}
