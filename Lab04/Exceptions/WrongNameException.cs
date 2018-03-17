using System;

namespace Lab04.Exceptions
{
    class WrongNameException : Exception
    {
        public WrongNameException(string message)
        : base(message)
        { }
    }
}
