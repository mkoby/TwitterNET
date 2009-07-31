using System;

namespace TwitterNET
{
    [Serializable]
    class TwitterNetException : Exception
    {
        private string _message;
        private string _httperror;

        public TwitterNetException()
        {}

        public TwitterNetException(string message) 
            : base(message)
        {}

        public TwitterNetException(string message, Exception innerException)
            : base(message, innerException)
        {}

        public TwitterNetException(string message, string httpError)
        {
            _message = message;
            _httperror = httpError;
        }

        public TwitterNetException(string message, string httpError, Exception innerException)
            : base(message, innerException)
        {
            _message = message;
            _httperror = httpError;
        }

        public string Message { get { return _message; } }
        public string HTTPError { get { return _httperror; } }
    }
}
