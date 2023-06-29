using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class IdExistException : Exception
    {
        public IdExistException() : base() { }
        public IdExistException(string message) : base(message) { }
        public IdExistException(string message, Exception inner) : base(message, inner) { }
        override public string ToString()
        { return $"Exception!  {Message}"; }
    }

    public class IdIsNotExistExeption : Exception
    {
        public IdIsNotExistExeption() : base() { }
        public IdIsNotExistExeption(string message) : base(message) { }
        public IdIsNotExistExeption(string message, Exception inner) : base(message, inner) { }
        override public string ToString()
        { return $"Exception!  {Message}"; }

    }

    public class NoBatteryToPath : Exception
    {
        public NoBatteryToPath() : base() { }
        public NoBatteryToPath(string message) : base(message) { }
        public NoBatteryToPath(string message, Exception inner) : base(message, inner) { }
        override public string ToString()
        { return $"Exception!  {Message}"; }

    }

    public class NoPackageToAssighn : Exception
    {
        public NoPackageToAssighn() : base() { }
        public NoPackageToAssighn(string message) : base(message) { }
        public NoPackageToAssighn(string message, Exception inner) : base(message, inner) { }
        override public string ToString()
        { return $"Exception!  {Message}"; }

    }
}

