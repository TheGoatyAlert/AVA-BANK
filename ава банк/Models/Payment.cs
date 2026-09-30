namespace rabota.Models;
public class Payment { 
    public int PaymentId { get; set; } 
    public int AccountId { get; set; } 
    public decimal Amount { get; set; } 
    public string Purpose { get; set; } = ""; 
    public string Status { get; set; } = ""; 
    public DateTime PaymentDate { get; set; } 
    public string AccountNumber { get; set; } = ""; 
}
