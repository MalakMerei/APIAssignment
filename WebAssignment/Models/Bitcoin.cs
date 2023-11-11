namespace WebAssignment.Models
{
    public class Bitcoin
    {
        public Guid Id { get; set; }
        public string Source { get; set; }
        public decimal Price { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
