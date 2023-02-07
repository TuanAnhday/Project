﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Aggregates
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        protected Entity(Guid id)
        {
            Id = id;
        }
    }
}