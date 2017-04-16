using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Generic;

namespace ContentEngine.Persistence
{
    public class JsonDataUpdater : IDataUpdater
    {
        public string Update(string denormalizedData, Guid contentId, string contentData)
        {
            if (denormalizedData == null) return contentData;
            if (contentData == null) return denormalizedData;
            if (!denormalizedData.IsValidJson()) throw new Exception("Target denormalized data to merge content into is not valid JSON");
            if (!contentData.IsValidJson()) throw new Exception("Source content data to update is not valid JSON");

            JToken contentToken = JToken.Parse(contentData);
            JToken denormalizedToken = JToken.Parse(denormalizedData);
            var matches = FindTokens(denormalizedToken, contentId.ToString());
            foreach (var match in matches)
            {
                match.Parent.Parent.Replace(contentToken);
            }
            return JsonConvert.SerializeObject(denormalizedToken);
        }

        private static List<JToken> FindTokens(JToken containerToken, string contentId)
        {
            List<JToken> matches = new List<JToken>();
            FindTokens(containerToken, contentId, matches);
            return matches;
        }

        private static void FindTokens(JToken containerToken, string contentId, List<JToken> matches)
        {
            if (containerToken.Type == JTokenType.Object)
            {
                foreach (JProperty child in containerToken.Children<JProperty>())
                {
                    if (child.Name == "id" && child.Value.Value<string>() == contentId)
                    {
                        matches.Add(child.Value);
                    }
                    FindTokens(child.Value, contentId, matches);
                }
            }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (JToken child in containerToken.Children())
                {
                    FindTokens(child, contentId, matches);
                }
            }
        }

        /*
        private JToken FindContentIdNode(JToken parent, Guid contentIdToFind) {

            Stack<JToken> S = new Stack<JToken>();
            JToken current = parent;
            S.Push(current);

            // r
            if (!parent.HasValues) return null;
            
            foreach (KeyValuePair<string, JToken> child in parent)
            {
                if (child.Key.ToLower() == "id")
                {
                    if (child.Value.Type == JTokenType.Guid || child.Value.Type == JTokenType.String)
                    {
                        if (child.Value.Value<string>() == contentIdToFind.ToString())
                        {
                            return child.Value;
                        }
                    }
                }
            }

            return foundToken;
        }
        */

    }
}
