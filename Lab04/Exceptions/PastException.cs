using System;

namespace Lab04.Exceptions
{
    class PastException : Exception
    {
        public PastException(string message)
        : base(message)
        { }
    }
}
