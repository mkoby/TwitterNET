using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterNET
{
    public enum AccountRequestOptionNames
    {
        ProfileBackgroundColor,
        ProfileTextColor,
        ProfileLinkColor,
        ProfileSidebarFillColor,
        ProfileSidebarBorderColor,
        Name,
        Website,
        Location,
        Description
    }

    public class AccountRequestOptions : Dictionary<AccountRequestOptionNames, object>
    {
        public string BuildRequestUri(string ApiUrl)
        {
            if (Keys.Count <= 0)
                return ApiUrl;

            StringBuilder sb = new StringBuilder();

            foreach (AccountRequestOptionNames key in Keys)
            {
                sb.Append(GetKeyString(key, ApiUrl));
            }

            //Return the string but remove the first "&" from the string builder string
            return String.Format("{0}?{1}", ApiUrl, sb.ToString().Substring(1));
        }

        private string GetKeyString(AccountRequestOptionNames key, string ApiUrl)
        {
            string Output = String.Empty;

            switch (key)
            {
                case AccountRequestOptionNames.Name:
                    Output = String.Format("&name={0}", this[key]);
                    break;
                case AccountRequestOptionNames.Website:
                    Output = String.Format("&url={0}", this[key]);
                    break;
                case AccountRequestOptionNames.Location:
                    Output = String.Format("&location={0}", this[key]);
                    break;
                case AccountRequestOptionNames.Description:
                    Output = String.Format("&description={0}", this[key]);
                    break;
                case AccountRequestOptionNames.ProfileBackgroundColor:
                    Output = String.Format("&profile_background_color={0}", this[key]);
                    break;
                case AccountRequestOptionNames.ProfileLinkColor:
                    Output = String.Format("&profile_link_color={0}", this[key]);
                    break;
                case AccountRequestOptionNames.ProfileSidebarBorderColor:
                    Output = String.Format("&profile_sidebar_border_color={0}", this[key]);
                    break;
                case AccountRequestOptionNames.ProfileSidebarFillColor:
                    Output = String.Format("&profile_sidebar_fill_color={0}", this[key]);
                    break;
                case AccountRequestOptionNames.ProfileTextColor:
                    Output = String.Format("&profile_text_color={0}", this[key]);
                    break;
                default:
                    break;
            }

            return Output;
        }

        public new void Add(AccountRequestOptionNames Key, object Value)
        {
            switch (Key)
            {
                default:
                    base.Add(Key, Value.ToString());
                    break;
            }
        }
    }
}
