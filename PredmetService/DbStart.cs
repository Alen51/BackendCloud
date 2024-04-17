using MongoDB.Driver;
using PredmetService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredmetService
{
    public class DbStart
    {
        private readonly IMongoDatabase _database;
        public IMongoCollection<Student> Studenti;

        public DbStart()
        {
            var client = new MongoClient("mongodb+srv://aleneminovic:aleneminovic5@alen.layjvej.mongodb.net/?retryWrites=true&w=majority");
            _database = client.GetDatabase("AlenCloud");
            

        }

        public IMongoCollection<Predmet> PredmetiCollection =>
        _database.GetCollection<Predmet>("Predmeti");

        public IMongoCollection<Profesor> ProfesoriCollection =>
        _database.GetCollection<Profesor>("Profesori");

        public IMongoCollection<Student> StudentCollection =>
       _database.GetCollection<Student>("Studenti");
    }
}
