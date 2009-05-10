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
        public WebRequest CreateNewTwitterRequest(string strURL, string strOptions)
        {
            if(String.IsNullOrEmpty(strURL))
                throw new ArgumentException("API URL not passed correctly");

            string methodType = "GET";
            
            StringBuilder requestURL = new StringBuilder(String.Format("{0}{1}", strURL, strOptions));

            if (requestURL.ToString().Contains("statuses/update") || 
			    requestURL.ToString().Contains("statuses/destroy"))
                methodType = "POST";

            WebRequest Output = WebRequest.Create(requestURL.ToString());
            Output.Method = methodType;

            if(_HasLogin)
            {
                string usernamePassword = String.Format("{0}:{1}", _Login, _Password);
                string loginBytes = Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword));
                CredentialCache credCache = new CredentialCache
                                                {{new Uri(requestURL.ToString()), 
                                                     "Basic", 
                                                     new NetworkCredential(_Login, _Password)}};
                Output.Credentials = credCache;
                Output.Headers.Add("Authorization", String.Format("Basic {0}", loginBytes)); 
            }

            return Output;
        }

        public string GetTwitterResponse(WebRequest twitterRequest)
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
		
		public string MakeAPIRequest(RequestHandler requestHandler, string strAPIUrl, string strRequestOptions)
		{
			string Output = String.Empty;
			Output = this.GetTwitterResponse( CreateNewTwitterRequest(strAPIUrl, strRequestOptions) ); 
			
			return Output;
		}

        public IList<IStatus> RepsonseHandler(string responseText)
        {
            IList<IStatus> Output = null;
            bool multipleStatus = responseText.Contains("statuses type=\"array\"");
            XElement xmlElement = XElement.Load(new XmlTextReader(new StringReader(responseText)));

            if(!multipleStatus)
            {
                IStatus status = Status.ParseStatusXML(responseText);
                status.User = User.ParseUserXml(xmlElement.Element("user").ToString());
                
                if(status != null)
                {
                    Output = new List<IStatus>();
                    Output.Add(status);
                }
            }
            else
            {
                Output = new List<IStatus>();
                //Query for count of statuses to make sure we have some
                var query = from c in xmlElement.Descendants("status")
                            select c;

                if (query.Count() > 0)
                {
                    Output = new List<IStatus>();

                    foreach (var status in query)
                    {
                        IStatus currentStatus = Status.ParseStatusXML(status.ToString());
                        currentStatus.User = User.ParseUserXml(status.Element("user").ToString());
                        Output.Add(currentStatus);
                    }
                }
            }
            

            return Output;
        }
    }
}
