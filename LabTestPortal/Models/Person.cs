using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LabTestPortal.Models
{
    public class Person
    {
        public int? person_id { get; set; }
        [DisplayName("First Name")]
        public string first_name { get; set; }
        [DisplayName("Last Name")]
        public string last_name { get; set; }
        [DisplayName("State")]
        public int? state_id { get; set; }
        [DisplayName("Gender")]
        public string gender { get; set; }
        [DisplayName("DOB")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? dob { get; set; }
        public string target_id { get; set; }
    }
}