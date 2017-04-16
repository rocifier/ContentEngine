using System;
using Xunit;
using ContentEngine.Persistence;
using FluentAssertions;

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
            // arrange
            string normalizedData = null;
            string contentData = "{\"key\": \"value\"}";
            Guid contentId = Guid.NewGuid();

            // act
            string result = _jsonDataUpdater.Update(normalizedData, contentId, contentData);

            // assert
            Assert.Same(result, contentData);
        }

        [Fact]
        public void Update_Works_In_Denormalized_Data()
        {
            // arrange
            Guid parentId = Guid.NewGuid();
            Guid firstChildId = Guid.NewGuid();
            Guid secondChildId = Guid.NewGuid();
            string normalizedData = "{\"id\":\"" + parentId + "\",\"children\":[{\"first child\":\"old data\",\"id\":\"" + firstChildId + "\"},{\"second child\":\"old data\",\"id\":\"" + secondChildId + "\"}]}";
            string expectedData = "{\"id\":\"" + parentId + "\",\"children\":[{\"first child\":\"new data\",\"id\":\"" + firstChildId + "\"},{\"second child\":\"old data\",\"id\":\"" + secondChildId + "\"}]}";
            string contentData = "{\"first child\": \"new data\", \"id\": \"" + firstChildId + "\"}";

            // act
            string result = _jsonDataUpdater.Update(normalizedData, firstChildId, contentData);

            // assert
            Assert.Equal(result, expectedData);
        }

        [Fact]
        public void Update_ThrowsException_ForInvalid_Target()
        {
            // arrange
            string normalizedData = "{key: value}";
            string contentData = "{\"key\":\"value\"}";
            Guid contentId = Guid.NewGuid();

            // act
            _jsonDataUpdater.Invoking(u => u.Update(normalizedData, contentId, contentData)).ShouldThrow<Exception>("normalizedData is invalid json data");

        }

        [Fact]
        public void Update_ThrowsException_ForInvalid_Source()
        {
            // arrange
            string normalizedData = "{\"key\":\"value\"}";
            string contentData = "{key: value}";
            Guid contentId = Guid.NewGuid();

            // act
            _jsonDataUpdater.Invoking(u => u.Update(normalizedData, contentId, contentData)).ShouldThrow<Exception>("contentData is invalid json data");

        }

    }
}
