using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.API.Controllers;

//[ApiController]
//[Route("api/[controller]")]
public class TransferController : BaseCommandController
{
    //[HttpPost]
    public async Task/*<ActionResult*/<TransferResponse>/*>*/ Transfer(
        /*[FromServices]*/ ICommandHandler<TransferRequest, TransferResponse> command,
        /*[FromBody]*/ TransferRequest request,
        CancellationToken ct)
    {
        return /*remove await*/await ExecuteCommand(command, request, ct);
    }
}
