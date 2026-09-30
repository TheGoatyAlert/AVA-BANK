namespace AvaBank.MobileApi.Models;

public class LoginRequest
{
    public string Phone { get; set; } = "";
}

public class LoginResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = "";
    public int ClientId { get; set; }
    public string FullName { get; set; } = "";
    public string Phone { get; set; } = "";
    public List<AccountDto> Accounts { get; set; } = new();
}

public class AccountDto
{
    public int AccountId { get; set; }
    public string AccountNumber { get; set; } = "";
    public string AccountType { get; set; } = "";
    public decimal Balance { get; set; }
    public string Status { get; set; } = "";
}

public class TransactionDto
{
    public int TransactionId { get; set; }
    public string AccountNumber { get; set; } = "";
    public string TransactionType { get; set; } = "";
    public decimal Amount { get; set; }
    public string Description { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}

public class TransferRequest
{
    public int ClientId { get; set; }
    public string SenderAccountNumber { get; set; } = "";
    public string ReceiverAccountNumber { get; set; } = "";
    public decimal Amount { get; set; }
    public string Description { get; set; } = "";
}

public class PhoneTransferRequest
{
    public int ClientId { get; set; }
    public string SenderAccountNumber { get; set; } = "";
    public string ReceiverPhone { get; set; } = "";
    public decimal Amount { get; set; }
    public string Description { get; set; } = "";
}

public class ApiResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = "";
}