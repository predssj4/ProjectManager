using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManager.Entities
{
    public class Task
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public virtual User User { get; set; }
        public virtual TaskStatus Status { get; set; }
        public virtual TaskPriority Priority { get; set; }
    }
}