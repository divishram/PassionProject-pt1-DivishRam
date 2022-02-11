using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Diagnostics

namespace BlogProject.Models
{
    public class GameArticle
    {
        //The following fields define a Game Article
        public int Id { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public string Genre { get; set; }
        public float Rating { get; set; }
        public string PublisherName { get; set; }
        public string Summary { get; set; }

        //Server-side validation logic
        public bool IsValid()
        {
            bool valid = true;

            if (Title == null || Rating == null || PublisherName == null || ReleaseYear == null || Summary == null)
            {
                //check to see if important fields are filled.
                valid = false;
            }

            else
            {
                //validate fields to make sure they meet server constraints

                //game title cannot be too short or long
                if (Title.Length < 2 || Title.Length > 255) valid = false;

                //publisher name can't be too short or long
                if (PublisherName.Length < 2 || PublisherName.Length > 255) valid = false;

                //Game rating cannot be less than 1
                if (Rating < 1) valid = false;

                //The game must be relatively recent. Cannot be released prior to 2010. 
                if (ReleaseYear < 2010) valid = false;
            }
            Debug.WriteLine("The model validity is: " + valid);
            return valid;
        }
        //Parameter-less constructore function
        //necessary for ajax request to automatically bind from the [FromBody] attribute
        public GameArticle() { }

    }
}
