﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Domain.Entities
{
    public class Review : BaseEntity
    {
        public virtual Client Client { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Grade { get; set; }
        public virtual Restaurateur Restaurateur { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}
