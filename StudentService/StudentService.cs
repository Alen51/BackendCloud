using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using MongoDB.Driver;
using StudentService.Interfaces;
using StudentService.Models;

namespace StudentService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class StudentService : StatelessService , IStudentService
    {
        private DbStart DbStart;
        public StudentService(StatelessServiceContext context)
            : base(context)
        {
            DbStart = new DbStart();
        }

        public Task<Tuple<bool, Student>> GetProfile(string token)
        {
            string email = Jwt.GetUsernameFromToken(token);
            var filter = Builders<Student>.Filter.Eq(s => s.Email, email);
            var student = DbStart.StudentCollection.Find(filter).FirstOrDefault();
            if (student == null || student.Email == null)
            {
                return Task.FromResult(Tuple.Create(false, new Student()));
            }

            return Task.FromResult(Tuple.Create(true, student));
        }

        public Task<Tuple<bool, string>> LogIn(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return Task.FromResult(new Tuple<bool, string>(false, ""));

            var filter = Builders<Student>.Filter.And(
                Builders<Student>.Filter.Eq(student => student.Email, email),
                Builders<Student>.Filter.Eq(student => student.Password, password)
);
            var usr = DbStart.StudentCollection.Find(filter).FirstOrDefault();

            if (usr == null || usr.Email == null)
            {
                
                    return Task.FromResult(new Tuple<bool, string>(false, ""));
               
            }




            return Task.FromResult(new Tuple<bool, string>(true, Jwt.GenerateJwtToken(usr.Email)));
        }

        public Task<Tuple<bool, string>> LogInProfesor(string email, string password)
        {

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return Task.FromResult(new Tuple<bool, string>(false, ""));

            var filter = Builders<Profesor>.Filter.And(
                Builders<Profesor>.Filter.Eq(student => student.Email, email),
                Builders<Profesor>.Filter.Eq(student => student.Password, password)
);
            var usr = DbStart.ProfesoriCollection.Find(filter).FirstOrDefault();
            if (usr == null || usr.Email == null)
            {
                return Task.FromResult(new Tuple<bool, string>(false, ""));
            }

            return Task.FromResult(new Tuple<bool, string>(true, Jwt.GenerateJwtToken(usr.Email)));
        }

        public Task<Tuple<bool, string>> ModifyProfile(Student s)
        {
            if (s == null || string.IsNullOrEmpty(s.Email) || string.IsNullOrEmpty(s.Ime)
               || string.IsNullOrEmpty(s.Prezime) || string.IsNullOrEmpty(s.Index) )
                return Task.FromResult(new Tuple<bool, string>(false, "Wrong data input"));

            var filter = Builders<Student>.Filter.Eq(u => u.Email, s.Email);
            var update = Builders<Student>.Update
                .Set(u => u.Index, s.Index)
                .Set(u => u.Ime, s.Ime)
                
                .Set(u => u.Prezime, s.Prezime);
            // Add other fields to update as needed


            


            DbStart.StudentCollection.UpdateOneAsync(filter, update);
            //logic for checking token
            return Task.FromResult(new Tuple<bool, string>(true, "Succesfully saved!"));
        }

        public Task<Tuple<bool, string>> Register(Student s)
        {
            if (s == null || string.IsNullOrEmpty(s.Email) || string.IsNullOrEmpty(s.Password) || string.IsNullOrEmpty(s.Ime)
               || string.IsNullOrEmpty(s.Prezime) || string.IsNullOrEmpty(s.Index) )
                return Task.FromResult(new Tuple<bool, string>(false, "Wrong data input"));

            var filter = Builders<Student>.Filter.Eq(user => user.Email, s.Email);
            var count = DbStart.StudentCollection.CountDocumentsAsync(filter).Result;

            if (count > 0)
                return Task.FromResult(new Tuple<bool, string>(false, "User already exist"));


            var filter2 = Builders<Student>.Filter.Eq(user => user.Index, s.Index);
            var count2 = DbStart.StudentCollection.CountDocumentsAsync(filter).Result;

            if (count2 > 0)
                return Task.FromResult(new Tuple<bool, string>(false, "User already exist"));

            DbStart.StudentCollection.InsertOneAsync(s);

            return Task.FromResult(new Tuple<bool, string>(true, "Saved Succesfully"));
        }

        public Task<Tuple<bool, string>> Authentcate(string token)
        {
            string email = Jwt.GetUsernameFromToken(token);
            var filter = Builders<Student>.Filter.Eq(user => user.Email, email);
            var user = DbStart.StudentCollection.Find(filter).FirstOrDefault();
            if (user == null || user.Email == null)
            {
                return Task.FromResult(Tuple.Create(false, ""));
            }

            return Task.FromResult(Tuple.Create(true, "student"));
        }

        public Task<Tuple<bool, Izvestaj>> Izvestaj(string email)
        {
            if (string.IsNullOrEmpty(email)){
                return Task.FromResult(new Tuple<bool, Izvestaj>(false, null));
            }
            var filter = Builders<Student>.Filter.And(
               Builders<Student>.Filter.Eq(student => student.Email, email));

            var usr = DbStart.StudentCollection.Find(filter).FirstOrDefault();
            var filter2 = Builders<Predmet>.Filter.Empty;
            var predmeti = DbStart.PredmetiCollection.Find(filter2).ToList();
            Izvestaj i = new Izvestaj();

            int count = 0;
            int sum = 0;
            foreach(var p in predmeti)
            {
                if (usr.PredmetList.Contains(p))
                {

                    i.ocene.Add(new OcenaNaPredmetu() { ImePredmeta=p.ImePredmeta, Ocena= p.OcenaList[email]});
                    if (p.OcenaList[email] > 0) { count++; sum += p.OcenaList[email]; }
                }
            }

            i.SrednjaOcena = sum / count;

            return Task.FromResult(new Tuple<bool, Izvestaj>(true, i));
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

        
    }
}
