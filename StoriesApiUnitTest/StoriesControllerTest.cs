using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StoriesWebApi;
using StoriesWebApi.Controllers;
using System;
using System.Collections.Generic;
using Xunit;

namespace StoriesApiUnitTest
{
    public class StoriesWebApiControllerTest
    {
        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var _service = new storyFakeHttpClient();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var _controller = new StoriesController(_service, cache);
            var okResult = _controller.GetAsync();

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var _service = new storyFakeHttpClient();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var _controller = new StoriesController(_service, cache);
            var okResult = _controller.GetAsync().Result as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<NewStory>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }
    }
}


