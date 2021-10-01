using System.Text.Json.Serialization;

namespace TalkAPIExercise.Models
{
    public class ChatChannelResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedAt { get; set; }
    }
}