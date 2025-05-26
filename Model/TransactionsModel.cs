namespace ExpenseManagerAPI.Model
{
    public class TransactionsModel
    {
        public int ExpenseID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int EventID { get; set; }
        public string EventName { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int HostId { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }

        public string Currency {  get; set; }
    }

    public class InsertTransaction
    {
        public int ExpenseID { get; set; }
        public int UserID { get; set; }
        public int HostId { get; set; }
        public int EventID { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
    }

    public class TransactionsReportModel
    {
        public int TotalMembers { get; set; }
        public double TotalIncome { get; set; }
        public double TotalExpense { get; set; }
        public double ExpensePerHead { get; set; }

    }

    public class MemberTransactionModel
    {
        public string Member { get; set; }
        public double Income { get; set; }
        public double Expense { get; set; }
    }
}
