using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TalkAPIExercise.Model;

namespace TalkApiExercise.SDK
{
    public class TalkApiHandler
    {
        private static IHttpClientFactory _httpClient;
        public TalkApiHandler(IHttpClientFactory client)
        {
            _httpClient = client;
        }

        public async Task<IList<ChannelModel>> getChannels()
        {
            var httpClient = _httpClient.CreateClient("TalkApi");
            var route = "chat-channels";
            var httpResp = await httpClient.GetAsync(route);
            httpResp.EnsureSuccessStatusCode();
            return await httpResp.Content.ReadFromJsonAsync<IList<ChannelModel>>();
        }

        public async Task<IList<MessageModel>> GetMessages()
        {
            var httpClient = _httpClient.CreateClient("TalkApi");
            var route = "chat-messages";
            var httpResp = await httpClient.GetAsync(route);
            httpResp.EnsureSuccessStatusCode();
            return await httpResp.Content.ReadFromJsonAsync<IList<MessageModel>>();  
        }
    }
}