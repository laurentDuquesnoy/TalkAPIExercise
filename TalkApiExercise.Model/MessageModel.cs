using System;

namespace TalkAPIExercise.Model
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Message { get; set; }
        public string Channel { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}