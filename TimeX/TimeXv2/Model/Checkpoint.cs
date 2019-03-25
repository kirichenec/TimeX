using System;

namespace TimeXv2.Model
{
    class Checkpoint
    {
        public DateTime? CheckedDate { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime EndTime { get { return StartTime + Duration; } }

        public bool IsOrderNeeded { get; set; }

        public string Name { get; set; }

        public DateTime StartTime { get; set; }
    }
}
