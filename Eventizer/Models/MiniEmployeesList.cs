using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eventizer.Models
{
    public class MiniEmployeesList
    {
        string ids, names;

        public string Ids
        {
            get { return ids; }
            set { ids = value; }
        }

        public string Names
        {
            get { return names; }
            set { names = value; }
        }
    }
}