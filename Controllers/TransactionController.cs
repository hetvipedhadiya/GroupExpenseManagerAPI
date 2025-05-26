using ExpenseManagerAPI.Model;
using ExpenseManagerAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionRepository _transactionRepository;
        public TransactionController(TransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;

        }
        [HttpGet]
        public ActionResult GetAllTransaction()
        {
            var transactionList = _transactionRepository.getAllTransaction();
            return Ok(transactionList);
        }

        [HttpGet("by-event/{EventID:int}/{HostID:int}")]
        public ActionResult GetTransactionByEventID(int EventID,int HostID)
        {
            var transactionList = _transactionRepository.GetTransactionByEventID(EventID,HostID);
            if (transactionList == null || transactionList.Count == 0)
            {
                return NotFound();
            }
            return Ok(transactionList);
        }

        [HttpGet("by-host/{HostID}")]
        public ActionResult GetTransactionByHostID(int HostID)
        {
            var transactionList = _transactionRepository.GetTransactionByHostID(HostID);
            if (transactionList == null || transactionList.Count == 0)
            {
                return NotFound();
            }
            return Ok(transactionList);
        }


        [HttpDelete("{TransactionID}")]
        public ActionResult DeleteTransaction(int TransactionID)
        {
            var isDelete = _transactionRepository.deleteTransaction(TransactionID);
            if (!isDelete == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost]
        public IActionResult InsertTransaction([FromBody] InsertTransaction transactionModel)
        {
            if (transactionModel == null)
            {
                return BadRequest();
            }
            bool insertTransaction = _transactionRepository.insertTransaction(transactionModel);
            if (insertTransaction)
            {
                return Ok(new { Message = "Transaction insert successfully" });
            }
            return StatusCode(500, "An error while inserting Transaction");
        }

        [HttpPut("{TransactionID}")]
        public IActionResult UpdateTransaction(int TransactionID, [FromBody] InsertTransaction transactionModel)
        {
            if (transactionModel == null || TransactionID != transactionModel.ExpenseID)
            {
                return BadRequest();
            }
            bool updateTransaction = _transactionRepository.updateTransaction(transactionModel);
            if (updateTransaction)
            {
                // Return the updated city data
                var updatedTransaction = _transactionRepository.GetTransactionByID(TransactionID); // Retrieve the updated city
                return Ok(updatedTransaction); // Return the updated city in the response
            }
            return NotFound();
        }



        [HttpGet("overall/{eventId}")]
        public ActionResult<TransactionsReportModel> GetOverallReport(int eventId)
        {
            var report = _transactionRepository.GetOverallReport(eventId);
            if (report == null)
            {
                return NotFound("No report found.");
            }
            return Ok(report);
        }

        [HttpGet("members/{eventId}")]
        public ActionResult<IEnumerable<MemberTransactionModel>> GetMemberWiseReport(int eventId)
        {
            var report = _transactionRepository.GetMemberWiseReport(eventId);
            if (report == null || !report.Any())
            {
                return NotFound("No member transactions found.");
            }
            return Ok(report);
        }
    }
}
