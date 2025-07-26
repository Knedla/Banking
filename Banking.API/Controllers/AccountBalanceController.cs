using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.API.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    public class AccountBalanceController : BaseCommandController
    {
        //[HttpGet("{accountId}")]
        public async Task/*<ActionResult*/<AccountBalanceResponse>/*>*/ GetBalance(
            /*[FromServices]*/ ICommandHandler<AccountBalanceRequest, AccountBalanceResponse> command,
            /*[FromBody]*/ AccountBalanceRequest request,
            CancellationToken ct)
        {
            return /*remove await*/await ExecuteCommand(command, request, ct);
        }
    }
}
