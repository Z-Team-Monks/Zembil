using System;
using System.Globalization;

namespace Zembil.ErrorHandler
{
    public class CustomAppException : Exception
    {
        public ErrorDetail errorDetail { get; set; }
        public CustomAppException() : base() { }
        public CustomAppException(ErrorDetail err) : base(err.Message)
        {
            this.errorDetail = err;
        }
    }

    public class CustomAppException2 : Exception
    {
        public ErrorDetail errorDetail { get; set; }
        public CustomAppException2() : base() { }
        public CustomAppException2(string Message, Exception e) : base(Message)
        {
            Console.WriteLine($"{Message}:  {e.Message}");
        }
    }
}