namespace NatechAssignment.Models
{
    public class Error
    {
        public int ErrorCode { get; }
        public string ErrorMessage { get; }

        public Error(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }

}
