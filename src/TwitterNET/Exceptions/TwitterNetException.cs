using System;
using System.IO;
using System.Net;

namespace TwitterNET
{
    [Serializable]
    class TwitterNetException : Exception
    {

        public TwitterNetException()
        {}

        public TwitterNetException(string message) 
            : base(message)
        {}

        public TwitterNetException(string message, Exception innerException)
            : base(message, innerException)
        {}
    }
}
