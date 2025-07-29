using Banking.Application.Enumerations;

namespace Banking.Application.Models.Common;

public class ResponseMessage
{
    public MessageType Type { get; set; }
    public string Content { get; set; }

    public ResponseMessage(MessageType type, string content)
    {
        Type = type;
        Content = content;
    }
}
