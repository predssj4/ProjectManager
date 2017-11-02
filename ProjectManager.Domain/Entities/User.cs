using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectManager.Entities
{
    public class User
    {
        public int ID { get; set; }

        [Required(ErrorMessage="E-Mail field is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage="It's not valid e-mail address")]
        public string Email { get; set; }

        [Required(ErrorMessage="Password field is required")]
        [MinLength(8, ErrorMessage="Password should have at least 8 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Roles { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<Message> SentMessages { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Bug> Bugs { get; set; }
    }
}