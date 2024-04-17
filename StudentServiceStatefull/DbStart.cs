using MongoDB.Driver;
using StudentServiceStatefull.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentServiceStatefull
{
    public class DbStart
    {
        private readonly IMongoDatabase _database;
        public IMongoCollection<Student> Studenti;

        public DbStart()
        {
            var client = new MongoClient("mongodb+srv://aleneminovic:aleneminovic5@alen.layjvej.mongodb.net/?retryWrites=true&w=majority&appName=alen");
            _database = client.GetDatabase("AlenCloud");
            /*
            try
            {
                Profesor p1 = new Profesor();
                p1.Email = "prof1@gamil.com";
                p1.Password = "password1";
                p1.ImeIPrezime = "Profesor1";

                Profesor p2 = new Profesor();
                p2.Email = "prof2@gamil.com";
                p2.Password = "password2";
                p2.ImeIPrezime = "Profesor2";
                IMongoCollection<Profesor> pc =
            _database.GetCollection<Profesor>("Profesori");
                pc.InsertOne(p1);
                pc.InsertOne(p2);
                

                Predmet pr1 = new Predmet();
                pr1.emailProfesora = p1.Email;
                pr1.ImePredmeta = "predmet1";

                Predmet pr2 = new Predmet();
                pr2.emailProfesora = p2.Email;
                pr2.ImePredmeta = "predmet2";

                Predmet pr3 = new Predmet();
                pr3.emailProfesora = p2.Email;
                pr3.ImePredmeta = "predmet3";

                Predmet pr4 = new Predmet();
                pr4.emailProfesora = p2.Email;
                pr4.ImePredmeta = "predmet4";

                IMongoCollection<Predmet> pr =
            _database.GetCollection<Predmet>("Predmeti");
                pr.InsertOne(pr1);
                pr.InsertOne(pr2);
                pr.InsertOne(pr3);
                pr.InsertOne(pr4);
            }
            catch(Exception ex) { }*/
        }

        public IMongoCollection<Student> StudentCollection =>
        _database.GetCollection<Student>("Studenti");

        public IMongoCollection<Profesor> ProfesoriCollection =>
        _database.GetCollection<Profesor>("Profesori");

        public IMongoCollection<Predmet> PredmetiCollection =>
        _database.GetCollection<Predmet>("Predmeti");

    }
}
