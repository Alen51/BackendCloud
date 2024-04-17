using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Student.Models
{
    public class Student
    {
        //[BsonId]
        public ObjectId Id { get; set; }

        //[Required]
        //[EmailAddress]
        public string Email { get; set; }

        //[Required]
        public string Password { get; set; }

        

        //[Required]
        public string Ime { get; set; }

        //[Required]
        public string Prezime { get; set; }

        //[Required]
        public string Index { get; set; }
    }
}
