using AvaBank.MobileApi.Models;
using AvaBank.MobileApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AvaBank.MobileApi.Controllers;

[ApiController]
[Route("api/mobile")]
public class MobileController : ControllerBase
{
    private readonly MobileBankService _service;

    public MobileController(MobileBankService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<LoginResponse> Login(LoginRequest request)
    {
        return await _service.LoginAsync(request);
    }

    [HttpGet("clients/{clientId:int}/accounts")]
    public async Task<List<AccountDto>> GetAccounts(int clientId)
    {
        return await _service.GetAccountsAsync(clientId);
    }

    [HttpGet("clients/{clientId:int}/transactions")]
    public async Task<List<TransactionDto>> GetTransactions(int clientId)
    {
        return await _service.GetTransactionsAsync(clientId);
    }

    [HttpPost("transfer/own")]
    public async Task<ApiResult> TransferOwn(TransferRequest request)
    {
        return await _service.TransferOwnAsync(request);
    }

    [HttpPost("transfer/phone")]
    public async Task<ApiResult> TransferByPhone(PhoneTransferRequest request)
    {
        return await _service.TransferByPhoneAsync(request);
    }
}