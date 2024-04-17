using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentServiceStatefull.Models
{
    public class Predmet
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [Required]
        public string ImePredmeta { get; set; }

        public string emailProfesora { get; set; }

        public Dictionary<string, Student> StudentList { get; set; } = new Dictionary<string, Student>();

        public Dictionary<string, int> OcenaList { get; set; } = new Dictionary<string, int>();

    }
}
