using System;

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
