using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.API.Controllers;

//[ApiController]
//[Route("api/[controller]")]
public class CreateAccountController : BaseCommandController
{
    //[HttpPost]
    public async Task/*<ActionResult*/<CreateAccountRequestResponse>/*>*/ CreateAccount(
        /*[FromServices]*/ ICommandHandler<CreateAccountRequestRequest, CreateAccountRequestResponse> command,
        /*[FromBody]*/ CreateAccountRequestRequest request,
        CancellationToken ct)
    {
        var result = await ExecuteCommand(command, request, ct);
        return result;
        //return /*remove await*/await ExecuteCommand(command, request, ct);
    }
}
