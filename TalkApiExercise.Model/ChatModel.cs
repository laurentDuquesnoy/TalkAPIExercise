using System.Collections.Generic;

namespace TalkAPIExercise.Model
{
    public class ChatModel
    {
        public string SelectedChannel { get; set; }
        public IList<MessageModel> Messages { get; set; }
        public IList<ChannelModel> Channels { get; set; }
        public ChatModel()
        {
            Messages = new List<MessageModel>();
            Channels = new List<ChannelModel>();
        }
        
    }
}