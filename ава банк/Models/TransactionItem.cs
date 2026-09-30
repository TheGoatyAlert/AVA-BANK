namespace rabota.Models;
public class TransactionItem { 
    public int TransactionId { get; set; } 
    public int AccountId { get; set; } 
    public string AccountNumber { get; set; } = ""; 
    public string TransactionType { get; set; } = ""; 
    public decimal Amount { get; set; } 
    public string Description { get; set; } = ""; 
    public DateTime CreatedAt { get; set; } 
}
