using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterNET
{
	public enum RequestOptionNames
	{
		ID,
		ScreenName,
		UserID,
		SinceID,
		MaxID,
		Count,
		Page
	}
	
	
	public class RequestOptions : Dictionary<RequestOptionNames, object>
	{
		
		public string BuildRequestUri(string ApiUrl)
		{
			if(Keys.Count <=0)
				return ApiUrl;
			
			StringBuilder sb = new StringBuilder();
			
			foreach(RequestOptionNames key in Keys)
			{
			    sb.Append(GetKeyString(key, ApiUrl));
			}
			
			//Return the string but remove the first "&" from the string builder string
			return String.Format("{0}?{1}", ApiUrl, sb.ToString().Substring(1));
		}

	    private string GetKeyString(RequestOptionNames key, string ApiUrl)
	    {
	        string Output = String.Empty;

            switch (key)
            {
                case RequestOptionNames.ID:
                   Output = String.Format("&id={0}", this[key]);
                    break;
                case RequestOptionNames.SinceID:
                   Output = String.Format("&since_id={0}", this[key]);
                    break;
                case RequestOptionNames.MaxID:
                   Output = String.Format("&max_id={0}", this[key]);
                    break;
                case RequestOptionNames.Count:
                   Output = String.Format("&count={0}", this[key]);
                    break;
                case RequestOptionNames.Page:
                   Output = String.Format("&page={0}", this[key]);
                    break;
                case RequestOptionNames.ScreenName:
                    if (!CheckApproporiateUse(key, ApiUrl))
                        throw new TwitterNetException("Username request option is only available for certain kinds of requests");

                   Output = String.Format("&screen_name={0}", this[key]);
                    break;
                case RequestOptionNames.UserID:
                    if (!CheckApproporiateUse(key, ApiUrl))
                        throw new TwitterNetException("UserID request option is only available for certain kinds of requests");

                   Output = String.Format("&user_id={0}", this[key]);
                    break;
                default:
                    break;
            }

	        return Output;
	    }

	    private bool CheckApproporiateUse(RequestOptionNames requestOptionName, string ApiUrl)
        {
            bool Output = false;

            switch(requestOptionName)
            {
                case RequestOptionNames.ScreenName:
                case RequestOptionNames.UserID:
                    if (ApiUrl.Contains("user_timeline") || 
                        ApiUrl.Contains("friends/ids") || 
                        ApiUrl.Contains("followers/ids") ||
                        ApiUrl.Contains("notifications/follow") ||
                        ApiUrl.Contains("notifications/leave"))
                        Output = true;
                    break;
                default:
                    Output = false;
                    break;
            }

            return Output;
        }

	    public new void Add(RequestOptionNames Key, object Value)
		{
			switch(Key)
			{
				default:
					base.Add(Key, Value.ToString());
					break;
			}
		}
	}
}
