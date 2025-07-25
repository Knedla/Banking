using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.API.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    public class DepositController : BaseCommandController
    {
        //[HttpPost]
        public async Task/*<ActionResult*/<DepositResponse>/*>*/ Deposit(
            /*[FromServices]*/ ICommand<DepositRequest, DepositResponse> command,
            /*[FromBody]*/ DepositRequest request,
            CancellationToken ct)
        {
            return /*remove await*/await ExecuteCommand(command, request, ct);
        }
    }
}
