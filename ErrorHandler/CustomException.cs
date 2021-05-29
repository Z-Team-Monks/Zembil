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
}