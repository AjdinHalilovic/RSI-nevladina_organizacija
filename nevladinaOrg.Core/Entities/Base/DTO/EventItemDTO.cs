using System;
using System.Collections.Generic;

namespace Core.Entities.Base.DTO
{
    public class EventItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string ConferenceRoom { get; set; }
        public string EventItemTypeName { get; set; }
        public string AboutLecture { get; set; }
        public string Lecturers { get; set; }

        public string StartTimeString { get { return StartTime.ToString(); } }
        public string EndTimeString { get { return EndTime.ToString(); } }
    }
}
