using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace TwitterNET
{
    class TwitterNETWebException : WebException
    {
        private WebExceptionStatus _status;
        private string _exceptionResponse;

        public TwitterNETWebException()
        {}

        public TwitterNETWebException(string message) 
            : base(message)
        {}

        public TwitterNETWebException(string message, WebException innerException)
            : base(message, innerException)
        {
            ParseException(innerException);
            Console.WriteLine("Status: {0}\n\nResponse Text: {1}", _status, _exceptionResponse);
        }

        public TwitterNETWebException(WebException innerException)
            : base(innerException.Message, innerException)
        {
            ParseException(innerException);
            Console.WriteLine("Status: {0}\n\nResponse Text: {1}", _status, _exceptionResponse);
        }

        private void ParseException(WebException exception)
        {
            _status = exception.Status;
            Console.WriteLine(_status);
            string responseText = String.Empty;

            using (StreamReader r = new StreamReader(exception.Response.GetResponseStream()))
            {
                responseText = r.ReadToEnd();
            }

            _exceptionResponse = responseText;
        }

        public string WebResposneText { get { return _exceptionResponse; } }
        public WebExceptionStatus HttpStatus { get { return _status; } }
    }
}
