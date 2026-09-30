namespace rabota.Models;
public class Account { 
    public int AccountId { get; set; } 
    public int ClientId { get; set; } 
    public string AccountNumber { get; set; } = ""; 
    public string AccountType { get; set; } = ""; 
    public decimal Balance { get; set; } 
    public string Status { get; set; } = ""; 
    public DateTime OpenedAt { get; set; } 
    public string ClientName { get; set; } = ""; 
}
