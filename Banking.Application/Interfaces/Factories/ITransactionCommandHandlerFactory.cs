using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.Application.Interfaces.Factories;

public interface ITransactionCommandHandlerFactory
{
    ICommandHandler<TInput, TOutput> Create<TInput, TOutput>() 
        where TInput : BaseRequest
        where TOutput : BaseResponse, new();
}
