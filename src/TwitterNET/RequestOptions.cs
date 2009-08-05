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
				switch(key)
				{
					case RequestOptionNames.ID:
						sb.AppendFormat("&id={0}", this[key]);
						break;
					case RequestOptionNames.SinceID:
						sb.AppendFormat("&since_id={0}", this[key]);
						break;
					case RequestOptionNames.MaxID:
						sb.AppendFormat("&max_id={0}", this[key]);
						break;
					case RequestOptionNames.Count:
						sb.AppendFormat("&count={0}", this[key]);
						break;
					case RequestOptionNames.Page:
						sb.AppendFormat("&page={0}", this[key]);
						break;
					case RequestOptionNames.ScreenName:
						if(!CheckApproporiateUse(key, ApiUrl))
							throw new TwitterNetException("Username request option is only available for certain kinds of requests");
					
						sb.AppendFormat("&screen_name={0}", this[key]);
						break;
					case RequestOptionNames.UserID:
                        if (!CheckApproporiateUse(key, ApiUrl))
                            throw new TwitterNetException("UserID request option is only available for certain kinds of requests");
					
						sb.AppendFormat("&user_id={0}", this[key]);
						break;
					default:
						break;
				}
			}
			
			//Return the string but remove the first "&" from the string builder string
			return String.Format("{0}?{1}", ApiUrl, sb.ToString().Substring(1));
		}

        private bool CheckApproporiateUse(RequestOptionNames requestOptionName, string ApiUrl)
        {
            bool Output = false;

            switch(requestOptionName)
            {
                case RequestOptionNames.ScreenName:
                    if (ApiUrl.Contains("user_timeline") || 
                        ApiUrl.Contains("friends/ids") || 
                        ApiUrl.Contains("followers/ids"))
                        Output = true;
                    break;
                case RequestOptionNames.UserID:
                    if (ApiUrl.Contains("user_timeline") || 
                        ApiUrl.Contains("friends/ids") || 
                        ApiUrl.Contains("followers/ids"))
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
