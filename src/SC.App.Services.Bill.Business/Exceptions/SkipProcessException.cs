using System;

namespace SC.App.Services.Bill.Business.Exceptions
{
    public class SkipProcessException : Exception
    {
        public SkipProcessException(string message)
            : base(message)
        {
        }
    }
}