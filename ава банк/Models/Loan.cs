namespace rabota.Models;
public class Loan { 
    public int LoanId { get; set; } 
    public int ClientId { get; set; } 
    public string LoanNumber { get; set; } = ""; 
    public decimal Amount { get; set; } 
    public decimal InterestRate { get; set; } 
    public decimal RemainingAmount { get; set; } 
    public string Status { get; set; } = ""; 
    public DateTime StartDate { get; set; } 
    public DateTime? EndDate { get; set; } 
    public string ClientName { get; set; } = ""; 
}
