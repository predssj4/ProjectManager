using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManager.Entities
{
    public class Message
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
        public DateTime SentDate { get; set; }
    }
}