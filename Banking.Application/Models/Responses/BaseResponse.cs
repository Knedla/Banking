using Banking.Application.Enumerations;
using Banking.Application.Models.Common;

namespace Banking.Application.Models.Responses;

public abstract class BaseResponse
{
    public List<ResponseMessage> Messages { get; } = new();

    public bool IsValid => !Messages.Any(m => m.Type == MessageType.Error);

    public bool ContinueExecution { get; set; } = true; // TODO: change name to something more meaningful

    public void AddMessage(string content, MessageType type = MessageType.Info)
    {
        Messages.Add(new ResponseMessage(type, content));
    }

    public void AddError(string content) => AddMessage(content, MessageType.Error); // TODO: add the field to which the error is related
    public void AddWarning(string content) => AddMessage(content, MessageType.Warning);
    public void AddInfo(string content) => AddMessage(content, MessageType.Info);
    public void AddSuccess(string content) => AddMessage(content, MessageType.Success);
}
