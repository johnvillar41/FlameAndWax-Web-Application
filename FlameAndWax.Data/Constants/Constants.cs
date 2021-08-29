namespace FlameAndWax.Data.Constants
{
    public class Constants
    {       
        public enum ReviewScore
        {
            Excellent = 5,
            VeryGood = 4,
            Good = 3,
            Poor = 2,
            VeryPoor = 1
        }
        public enum ModeOfPayment
        {
            Cash,
            Cheque
        }
        public enum Courier
        {
            FoodPanda,
            JNT,
            NinjaVan,
            GogoExpress
        }
        public enum CustomerAccountStatus
        {
            Banned,
            Active,
            Pending
        }
        public enum EmployeeAccountStatus
        {
            Deactivated,
            Activated
        }
        public enum OrderDetailStatus
        {
            Pending,
            Finished,
            Processing,            
            Shipping,
            Cancelled
        }
        public enum Category
        {
            Candle,
            Soap,
            Diffuser
        }
        public enum Roles
        {
            Customer,
            Employee,
            Administrator
        }
    }
}
