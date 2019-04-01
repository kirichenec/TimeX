using System;
using System.Collections.Generic;

namespace TimeXv2.Model
{
    public class Action
    {
        public List<Checkpoint> Checkpoints { get; set; }

        public string Name { get; set; }

        public DateTime StartTime { get; set; }
    }
}