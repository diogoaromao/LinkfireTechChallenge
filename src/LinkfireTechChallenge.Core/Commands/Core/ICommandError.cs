namespace LinkfireTechChallenge.Core.Commands.Core
{
    public interface ICommandError
    {
        int Code { get; }
        string Title { get; }
        string Message { get; set; }
        string Property { get; set; }
    }
}
