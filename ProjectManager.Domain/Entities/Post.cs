﻿using ProjectManager.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectManager.Domain.Entities
{
    public class Post
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public virtual User Author { get; set; }
    }
}