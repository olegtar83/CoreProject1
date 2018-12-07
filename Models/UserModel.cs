using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace webapp.Models
{
    public class User:BaseEntity 
    {



        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;



        public string UserName { get; set; } = string.Empty;


        [BsonDefaultValue("User")]
        public string Role { get; set; } = "User";

        public int UserId { get; set; } = 0;

        public string Password { get; set; }

        public string Description { get; set; }

        public bool isMale { get; set; }

        public bool isMalePreference { get; set; }

        public PrivateImage HeaderImage { get; set; }

    }

    public class PrivateImage
    {
        public string Url { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public long ImageSize { get; set; } = 0;
    }
}
