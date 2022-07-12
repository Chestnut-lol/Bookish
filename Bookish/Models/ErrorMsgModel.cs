namespace Bookish.Models;

public class ErrorMsgModel
{
    public string Msg { get; set; }

    public ErrorMsgModel(string msg)
    {
        Msg = msg;
    }
}