namespace TodoListApp.Dtos;

public class EmailMessageDto
{
    public IEnumerable<string> To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }

    public EmailMessageDto(IEnumerable<string> to, string subject, string content)
    {
        var newList = new List<string>();
        newList.AddRange(to);
        To = newList;
        Subject = subject;
        Content = content;
    }

    public EmailMessageDto() { }
}
