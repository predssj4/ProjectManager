using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectManager.Entities
{
    public class Bug
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual BugStatus Status { get; set; }
        public virtual BugType Type { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<BugAttachment> Attachments { get; set; }
    }
}