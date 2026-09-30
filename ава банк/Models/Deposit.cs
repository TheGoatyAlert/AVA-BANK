namespace rabota.Models;
public class Deposit { 
    public int DepositId { get; set; } 
    public int ClientId { get; set; } 
    public string DepositNumber { get; set; } = ""; 
    public decimal Amount { get; set; } 
    public decimal InterestRate { get; set; } 
    public string Status { get; set; } = ""; 
    public DateTime StartDate { get; set; } 
    public DateTime? EndDate { get; set; } 
    public string ClientName { get; set; } = ""; 
}
