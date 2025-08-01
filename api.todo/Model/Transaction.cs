namespace api.todo.Model
{
    public class Transaction
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Category { get; set; }
    }

    public class TransactionRq
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Category { get; set; }
    }
}
