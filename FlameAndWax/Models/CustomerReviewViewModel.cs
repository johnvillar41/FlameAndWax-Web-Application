using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Models
{
    public class CustomerReviewViewModel
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }        
        public string ReviewDetail { get; set; }
        public ReviewScore ReviewScore { get; set; }
        public CustomerViewModel Customer { get; set; }
    }
}
