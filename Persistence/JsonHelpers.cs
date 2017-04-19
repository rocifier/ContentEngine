using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;

namespace ContentEngine.Persistence
{
    public static class JsonHelpers
    {
        public static bool IsValidJson(this string strInput)
        {
            if (strInput == null) return false;
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JContainer.Parse(strInput);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool IsValidContentModel(this string strInput)
        {
            if (strInput.StartsWith("{") && strInput.EndsWith("}")) {
                var obj = JObject.Parse(strInput);
                if (obj.Property("id") != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static Guid ExtractContentKey(this string strInput)
        {
            var obj = JObject.Parse(strInput);
            return Guid.Parse(obj.Property("id").Value.ToString());
        }

        public static string FirstPartOfCompositeIndex(this string strInput)
        {
            if (strInput.Contains("_"))
            {
                return strInput.Substring(0, strInput.IndexOf('_'));
            }
            return "";
        }

        public static bool IsSuccessfulHttpStatusCode(this int code)
        {
            return (code >= 200 && code <= 299);
        }

        public static bool IsEqualToStringIgnoringWhitespace(this string strInput, string target)
        {
            string src = Regex.Replace(strInput, @"\s", "");
            string tar = Regex.Replace(target, @"\s", "");
            return String.Equals(src, tar, StringComparison.OrdinalIgnoreCase);
        }
    }
}
