using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManager.Entities
{
    public class TaskPriority
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Task> tasks { get; set; }
    }
}