using System;
using Xunit;
using ContentEngine.Persistence;

namespace Persistence.Tests
{
    public class JsonDataUpdaterTests
    {
        private readonly JsonDataUpdater _jsonDataUpdater;

        public JsonDataUpdaterTests() {
            _jsonDataUpdater = new JsonDataUpdater();
        }

        [Fact]
        public void Update_Replaces_Null()
        {
            string normalizedData = null;
            string content = "{\"key\": \"value\"}";
            Guid contentId = Guid.NewGuid();
            string result = _jsonDataUpdater.Update(normalizedData, contentId, contentData);
            Assert.AreEqual(result, content);
        }
    }
}
