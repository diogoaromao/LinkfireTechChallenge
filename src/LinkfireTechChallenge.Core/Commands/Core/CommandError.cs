using System;

namespace LinkfireTechChallenge.Core.Commands.Core
{
    public class CommandError : ICommandError
    {
        public int Code { get; }
        public string Title { get; }
        public Exception Exception { get; }
        public string Message { get; set; }
        public string Property { get; set; }

        public CommandError(CommandErrorType errorCode, string message, string property)
        {
            Code = (int)errorCode;
            Title = errorCode.ToString();
            Message = message;
            Property = property;
        }

        public CommandError(CommandErrorType errorCode, Exception ex, string property)
            : this(errorCode, ex.Message, property)
        {
            this.Exception = ex;
        }
    }
}
