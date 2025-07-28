using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.API.Controllers;

//[ApiController]
//[Route("api/[controller]")]
public class WithdrawalController : BaseCommandController
{
    //[HttpPost]
    public async Task/*<ActionResult*/<WithdrawalResponse>/*>*/ Withdraw(
        /*[FromServices]*/ ICommandHandler<WithdrawalRequest, WithdrawalResponse> command,
        /*[FromBody]*/ WithdrawalRequest request,
        CancellationToken ct)
    {
        return /*remove await*/await ExecuteCommand(command, request, ct);
    }
}
