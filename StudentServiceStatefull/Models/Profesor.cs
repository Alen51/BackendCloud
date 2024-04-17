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
    public class Profesor
    {
        [BsonId]
        public ObjectId Id { get; set; }


        public string ImeIPrezime { get; set; }

        [EmailAddress]
        public string Email { get; set; }


        public string Password { get; set; }

        public List<Predmet> PredmetList { get; set; } = new List<Predmet>();
    }
}
