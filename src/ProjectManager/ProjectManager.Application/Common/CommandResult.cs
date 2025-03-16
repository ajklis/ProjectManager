namespace ProjectManager.Application.Common
{
    public class CommandResult
    {
        public bool IsSuccess { get; set; }
        public dynamic? ReturnValue { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }

        protected CommandResult(bool success, dynamic? returnValue, string? message, int statusCode)
        {
            IsSuccess = success;
            ReturnValue = returnValue;
            Message = message;
            StatusCode = statusCode;
        }

        public static CommandResult Success(dynamic? returnValue, int statusCode = 200) 
            => new CommandResult(true, returnValue, null, statusCode);

        public static CommandResult Failed(string message, int statusCode) 
            => new CommandResult(false, null, message, statusCode);

        public static CommandResult InternalServerError(Exception e = null)
        {
#if DEBUG
            return new CommandResult(false, null, $"Internal server error: {e.ToString()}", 500);
#endif
            return new CommandResult(false, null, "Internal server error", 500);
        }

        public static CommandResult Unauthorized()
            => new CommandResult(false, null, "Unauthorized", 401);
    }
}
