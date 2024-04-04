namespace NatechAssignment.Models
{
    public class Result<T>
    {
        public bool Success { get; private set; }
        public T Data { get; private set; }
        public Error Error { get; private set; }

        private Result(bool success, T data, Error errorMessage)
        {
            Success = success;
            Data = data;
            Error = errorMessage;
        }

        public static Result<T> CreateSuccess(T data)
        {
            return new Result<T>(true, data, null);
        }

        public static Result<T> CreateFailure(Error errorMessage)
        {
            return new Result<T>(false, default(T), errorMessage);
        }
    }

}
