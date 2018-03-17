using System;

namespace Lab04.Exceptions
{
    class EmailException : Exception
    {
        public EmailException(string message)
        : base(message)
        { }
    }
}
