using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartDividendTracker.Models
{
    public class ChatSession
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = "Новий чат";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<ChatMessageEntity> Messages { get; set; } = new List<ChatMessageEntity>();
    }

    public class ChatMessageEntity
    {
        [Key]
        public int Id { get; set; }
        public int ChatSessionId { get; set; }
        public virtual ChatSession ChatSession { get; set; } = null!;

        public string Sender { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}