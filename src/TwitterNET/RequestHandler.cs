using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace TwitterNET
{
    internal class RequestHandler
    {
		private string _Login = String.Empty;
        private string _Password = String.Empty;
        private bool _HasLogin = false;

        /// <summary>
        /// Create a new RequestHandler object.  To handle anonymous request, pass empty/null values.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        public RequestHandler(string login, string password)
        {
            if(!String.IsNullOrEmpty(login))
                _Login = login;
            
            if(!String.IsNullOrEmpty(password))
                _Password = password;

            if( !String.IsNullOrEmpty(_Login) && !String.IsNullOrEmpty(_Password))
                _HasLogin = true;
        }
        
        /// <summary>
        /// Password for logging into Twitter account
        /// </summary>
        public string Password
        {
            get { return _Password; }
        }

        /// <summary>
        /// Username for logging into Twitter account
        /// </summary>
        public string Login
        {
            get { return _Login; }
        }

        /// <summary>
        /// Has the RequestHandler has been supplied a Login/Password combination
        /// </summary>
        public bool HasLogin
        {
            get { return _HasLogin; }
        }

		/// <summary>
        /// Creates a new Web Request to the Twitter API using the API Url specified.
        /// When passing options, you'll need to pass the ? for starting the parameters list.
        /// </summary>
        /// <param name="strURL">The Twitter API Url</param>
        /// <returns>Web request used to get data from the Twitter API</returns>
        private WebRequest CreateNewTwitterRequest(string strURL)
        {
            if(String.IsNullOrEmpty(strURL))
                throw new ArgumentException("API URL not passed correctly");

            string methodType = "GET";

			//TODO: Better way to do this?
            if (strURL.Contains("statuses/update") || 
			    strURL.Contains("statuses/destroy"))
                methodType = "POST";

            WebRequest Output = WebRequest.Create(strURL.ToString());
            Output.Method = methodType;

            if(_HasLogin)
            {
                string usernamePassword = String.Format("{0}:{1}", _Login, _Password);
                string loginBytes = Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword));
                CredentialCache credCache = new CredentialCache
                                                {{new Uri(strURL), 
                                                     "Basic", 
                                                     new NetworkCredential(_Login, _Password)}};
                Output.Credentials = credCache;
                Output.Headers.Add("Authorization", String.Format("Basic {0}", loginBytes)); 
            }

            return Output;
        }

        private string GetTwitterResponse(WebRequest twitterRequest)
        {
            if (twitterRequest == null)
                throw new ArgumentException("TwitterRequest is either NULL or empty");

            StringBuilder Output = new StringBuilder(String.Empty);

            //Handle response
            WebResponse twitterResponse = null;
			StreamReader r = null;
			
			try
			{
				twitterResponse = twitterRequest.GetResponse();
			}
			catch(WebException webex)
			{
				throw new TwitterNetException(webex.Message, webex);
			}
			
			if(twitterResponse != null)
            	r = new StreamReader(twitterResponse.GetResponseStream(), Encoding.UTF8);
            
			if(r != null)
			{
	            try
	            {
	                Output.Append(r.ReadToEnd());
	            }
	            catch(IOException ioException)
	            {
	                throw new TwitterNetException("Error reading response from Twitter", ioException);
	            }
	            catch(Exception ex)
	            {
	                throw new TwitterNetException("Error reading response from Twitter", ex);
	            }
			}

            return Output.ToString();
        }
		
		public string MakeAPIRequest(RequestHandler requestHandler, string strAPIUrl)
		{
			string Output = String.Empty;
			Output = GetTwitterResponse( CreateNewTwitterRequest(strAPIUrl) ); 
			
			return Output;
		}
    }
}
