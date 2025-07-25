using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.API.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    public class CreateAccountController : BaseCommandController
    {
        //[HttpPost]
        public async Task/*<ActionResult*/<CreateAccountResponse>/*>*/ CreateAccount(
            /*[FromServices]*/ ICommand<CreateAccountRequest, CreateAccountResponse> command,
            /*[FromBody]*/ CreateAccountRequest request,
            CancellationToken ct)
        {
            return /*remove await*/await ExecuteCommand(command, request, ct);
        }
    }
}
