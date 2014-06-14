using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eventizer.Models
{
    public class EventDetails
    {
        public Employee Employee { set; get; }
        public Event Event { set; get; }
        public List<Task> Tasks { set; get; }
        public List<Subtask> Subtasks { set; get; }
    }
}