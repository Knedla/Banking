using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.Application.Commands.Common;

public interface IInitializationTransactionCommandHandler<TInput, TOutput> : ITransactionCommandHandler<TInput, TOutput>
    where TInput : BaseRequest
    where TOutput : BaseResponse, new()
{ }
