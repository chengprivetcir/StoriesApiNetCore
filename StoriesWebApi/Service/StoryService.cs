using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace StoriesWebApi
{
    public interface IstoryHttpClient
    {
        Task<List<NewStory>> GetIds();
    }

    public class storyHttpClient : IstoryHttpClient
    {
        private readonly IHttpClientFactory _clientFactory;

        public storyHttpClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<List<NewStory>> GetIds()
        {
            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync("https://hacker-news.firebaseio.com/v0/newstories.json?print=pretty");

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var storyIdList = await JsonSerializer.DeserializeAsync<List<int>>(responseStream);

                var storiesList = new List<NewStory>();
                foreach (var id in storyIdList)
                {
                    var responseDetails = await client.GetAsync(string.Format("https://hacker-news.firebaseio.com/v0/item/{0}.json?print=pretty", id));
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStreamDetails = await responseDetails.Content.ReadAsStreamAsync();
                        var story = await JsonSerializer.DeserializeAsync<NewStory>(responseStreamDetails);
                        storiesList.Add(story);
                    }
                    else
                    {
                        return null;
                    }
                }
                return storiesList;
            }

            return null;
        }
    }

    public class storyFakeHttpClient : IstoryHttpClient
    {
        public async Task<List<NewStory>> GetIds()
        {

            var storiesList = new List<NewStory>();
            for (int i = 1; i <= 3; i++)
            {
                var test = new NewStory();
                test.id = i;
                storiesList.Add(test);               
            }
            return storiesList;
        }

    }
}

