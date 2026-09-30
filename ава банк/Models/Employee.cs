namespace rabota.Models;
public class Employee { 
    public int EmployeeId { get; set; } 
    public string FullName { get; set; } = ""; 
    public string Position { get; set; } = ""; 
    public string Phone { get; set; } = ""; 
    public string Email { get; set; } = ""; 
    public int? BranchId { get; set; } 
    public DateTime HireDate { get; set; } 
    public string BranchName { get; set; } = ""; 
}
