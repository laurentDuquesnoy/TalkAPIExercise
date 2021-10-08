using System;
using System.Text.Json.Serialization;

namespace TalkAPIExercise.Model
{
    public class ChannelModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}