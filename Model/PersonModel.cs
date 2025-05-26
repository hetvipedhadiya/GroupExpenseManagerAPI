namespace ExpenseManagerAPI.Model
{
    public class PersonModel
    {
        public int UserID { get; set; } // Primary Key
        public string UserName { get; set; }
        public int? EventID { get; set; }
        public DateTime? Created { get; set; }
        public int HostId {  get; set; }
        //public string UserImage { get; set; }
        public DateTime? Modified { get; set; } // Nullable
    }

    public class UserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
