using System;
namespace Lab04.Exceptions
{
    class FutureException : Exception
    {
        public FutureException(string message)
        : base(message)
        { }
    }
}
