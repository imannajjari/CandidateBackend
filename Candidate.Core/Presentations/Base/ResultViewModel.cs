namespace Candidate.Core.Presentations.Base;

public class ResultViewModel<T>
{
    public MessageViewModel Message { get; set; }
    public List<T> List { get; set; }
    public T Result { get; set; }
    public int TotalCount { get; set; }
}