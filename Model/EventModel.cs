using System.ComponentModel.DataAnnotations;

namespace ExpenseManagerAPI.Model
{
    public class EventModel
    {
        [Key]
        public int EventID { get; set; }

        public string EventName { get; set; }

        public DateTime EventDate { get; set; }

        public double Amount { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int HostID { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public class InsertEvent
    {
        public int? EventID { get; set; }
        public int HostID { get; set; }
        public string EventName { get; set; }

        public DateTime EventDate { get; set; }
    }

}
