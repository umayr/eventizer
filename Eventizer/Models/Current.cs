using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eventizer.Models
{
    public class Current
    {
        public Employee Employee { set; get; }
        public Event Event { set; get; }
        public Task Task { set; get; }
        public Subtask Subtask { set; get; }
    }
}