using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManager.Entities
{
    public class BugAttachment
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public virtual Bug Bug { get; set; }
    }
}