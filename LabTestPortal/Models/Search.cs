using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabTestPortal.Models
{
    public class Search
    {
        public Person Criteria { get; set; }
        public List<Person> Results { get; set; }
        public List<State> State_List { get; set; }
    }
}