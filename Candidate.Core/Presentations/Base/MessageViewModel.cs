namespace Candidate.Core.Presentations.Base;

public class MessageViewModel
{
    public MessageViewModel()
    {
        Errors = new List<ErrorViewModel>();
    }
    public int ID { get; set; }
    public string Status { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string Value { get; set; }
    public List<ErrorViewModel> Errors { get; set; }
}