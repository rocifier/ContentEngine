using Newtonsoft.Json.Linq;

namespace ContentEngine.Persistence
{
    public static class JsonHelpers
    {
        public static bool IsValidJson(this string strInput)
        {
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

        public static string FirstPartOfCompositeIndex(this string strInput)
        {
            if (strInput.Contains("_"))
            {
                return strInput.Substring(0, strInput.IndexOf('_'));
            }
            return "";
        }
    }
}
