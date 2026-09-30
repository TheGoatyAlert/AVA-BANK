namespace rabota.Models;
public class Client { 
    public int ClientId { get; set; } 
    public string FullName { get; set; } = ""; 
    public string Phone { get; set; } = ""; 
    public string Email { get; set; } = ""; 
    public string PassportData { get; set; } = ""; 
    public string Address { get; set; } = ""; 
    public DateTime? BirthDate { get; set; } 
    public DateTime CreatedAt { get; set; } 
}
