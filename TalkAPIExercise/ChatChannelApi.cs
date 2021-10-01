using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TalkAPIExercise.Models;

namespace TalkAPIExercise
{
    public class ChatChannelApi
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ChatChannelApi(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<ChatChannelResponse>> getAllAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("TalkApi");
            var route = "api/chat-channels";
            var result = await httpClient.GetAsync(route);
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsAsync<IList<ChatChannelResponse>>();
        }
        
        
    }
}