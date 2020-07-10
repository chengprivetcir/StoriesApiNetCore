using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace StoriesWebApi.Controllers
{
  
    [ApiController]
    public class StoriesController : ControllerBase
    {
        
        private readonly IstoryHttpClient _storyHttpClient;
        private readonly IMemoryCache _cache;

        public StoriesController(IstoryHttpClient storyHttpClient, IMemoryCache memoryCache)
        {
            _storyHttpClient = storyHttpClient;
            _cache = memoryCache;
        }

        [HttpGet]
        [Route("api/stories")]
        public async Task<IActionResult> GetAsync()
        {
            if (_cache.TryGetValue("StoriesList", out List<NewStory> List))
            {
                return Ok(List);
            }
            var list = await _storyHttpClient.GetIds();
            if (list.Count() > 0)
            {
                _cache.Set("StoriesList", list);
                return Ok(list);
            }
            return StatusCode(400);
        }
    }
}
