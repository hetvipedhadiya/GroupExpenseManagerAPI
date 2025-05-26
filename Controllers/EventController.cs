using ExpenseManagerAPI.Data;
using ExpenseManagerAPI.Model;
using ExpenseManagerAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventRepository _eventRepository;
        private readonly AppDbContext _dbcontext;
        public EventController(EventRepository eventRepository, AppDbContext dbcontext)
        {
            _eventRepository = eventRepository;
            _dbcontext = dbcontext;

        }
        [HttpGet]
        public ActionResult GetAllEvent()
        {
            var eventList = _eventRepository.getAllEvent();
            return Ok(eventList);
        }

        [HttpGet("event/{EventID:int}/{hostID:int}")]
        public IActionResult GetEventByID(int EventID, int hostID)
        {
            var eventList = _eventRepository.GetEventByID(EventID, hostID);

            if (eventList == null || !eventList.Any())
            {
                return NotFound("No users found for the specified event and host.");
            }

            return Ok(eventList);
        }


        [HttpGet("host-events/{HostId}")]
        public ActionResult GetEventsByHostId(int HostId)
        {
            var events = _eventRepository.GetEventsByHostID(HostId);
            if (events == null || !events.Any())
            {
                return NotFound();
            }
            return Ok(events);
        }


        [HttpDelete("{EventID}")]
        public ActionResult DeleteEvent(int EventID)
        {
            //var isDelete = _eventRepository.deleteEvent(EventID);
            //if (!isDelete == null)
            //{
            //    return NotFound();
            //}
            //return NoContent();

            try
            {
                var success = _eventRepository.deleteEvent(EventID);
                if (success)
                {
                    return NoContent(); // Return 204 if deletion is successful
                }

                return NotFound("Event not found."); // Return 404 if no rows were affected
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
        public IActionResult InsertEvent([FromBody] InsertEvent eventModel)
        {
            if (eventModel == null)
            {
                return BadRequest();
            }
            bool insertEvent = _eventRepository.insertEvent(eventModel);
            if (insertEvent)
            {
                return Ok(new { Message = "event insert successfully" });
            }
            return StatusCode(500, "An error while inserting event");
        }

        [HttpPut("{EventID}")]
        public IActionResult UpdateCity(int EventID, [FromBody] InsertEvent eventModel)
        {
            if (eventModel == null || EventID != eventModel.EventID)
            {
                return BadRequest();
            }

            bool updateEvent = _eventRepository.updateEvent(eventModel);
            if (updateEvent)
            {
                // Get HostID from eventModel
                var updatedEvent = _eventRepository.GetEventByID(EventID, eventModel.HostID);
                return Ok(updatedEvent);
            }

            return NotFound();
        }



        //[HttpGet("GetEventsByHost/{hostId}")]
        //public IActionResult GetEventsByHost(int hostId)
        //{
        //    var events = _dbcontext.Event.Where(e => e.HostID == hostId).ToList();

        //    if (events.Any())
        //    {
        //        return Ok(events);
        //    }

        //    return NotFound(new { message = "No events found for this user." });
        //}
    }

}
