using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Diagnostics

namespace BlogProject.Models
{
    public class Publisher
    {
        //The following fields define a publisher
        public int Id { get; set; }
        public string PublisherName { get; set; }
        public string Founder { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        //Server-side validation logic
        public bool IsValid()
        {
            bool valid = true;

            if (Country == null || City == null || PublisherName == null || Founder == null || Id == null)
            {
                //check to see if important fields are filled.
                valid = false;
            }

            else
            {
                //validate fields to make sure they meet server constraints

                //game title cannot be too short or long
                if (PublisherName.Length < 2 || PublisherName.Length > 255) valid = false;

                //publisher name can't be too short or long
                if (Founder.Length < 2 || Founder.Length > 255) valid = false;

            }
            Debug.WriteLine("The model validity is: " + valid);
            return valid;
        }

        public Publisher() { }

    }
}
