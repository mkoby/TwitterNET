using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace TwitterNET
{
    class TwitterNETWebException : WebException
    {
        private WebExceptionStatus _status;
        private string _exceptionResponse;
        private string _responseErrorText;

        public TwitterNETWebException()
        {}

        public TwitterNETWebException(string message) 
            : base(message)
        {}

        public TwitterNETWebException(string message, WebException innerException)
            : base(message, innerException)
        {
            ParseException(innerException);
#if DEBUG
            Console.WriteLine("Status: {0}\n\nResponseErrorText: {1}\n\nResponse Full Text: {2}", _status, _responseErrorText, _exceptionResponse);
#endif
        }

        public TwitterNETWebException(WebException innerException)
            : base(innerException.Message, innerException)
        {
            ParseException(innerException);
#if DEBUG
            Console.WriteLine("Status: {0}\n\nResponseErrorText: {1}\n\nResponse Full Text: {2}", _status, _responseErrorText, _exceptionResponse);
#endif
        }

        private void ParseException(WebException exception)
        {
            _status = exception.Status;
            string responseText = String.Empty;

            using (StreamReader r = new StreamReader(exception.Response.GetResponseStream()))
            {
                responseText = r.ReadToEnd();
            }

            _exceptionResponse = responseText;
            XElement element = XElement.Parse(responseText);

            if (element != null)
                foreach (var s in element.DescendantsAndSelf("error"))
                {
                    _responseErrorText = s.Value;
                }

            element = null;
        }

        /// <summary>
        /// The full response from the request
        /// </summary>
        public string WebResposneText { get { return _exceptionResponse; } }

        /// <summary>
        /// This is just the output of the <error/> part of the response XML from Twitter
        /// </summary>
        public string ResponseErrorText { get { return _responseErrorText; } }

        /// <summary>
        /// The HTTP status of the error
        /// </summary>
        public WebExceptionStatus HttpStatus { get { return _status; } }
    }
}
