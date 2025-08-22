namespace App.Common.Exceptions
{
    public class InvalidNullInputException : Exception
    {
        public InvalidNullInputException()
        {
        }

        public InvalidNullInputException(string message) : base(message)
        {
        }

        public InvalidNullInputException(string message , Exception innerException) : base(message , innerException)
        {
        }

        public InvalidNullInputException(string name , object key) : base($"Input \"{name}\" ({key}) is not valid.")
        {
        }
    }
}