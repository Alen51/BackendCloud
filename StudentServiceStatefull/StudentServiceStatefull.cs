using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using MongoDB.Driver;
using StudentService;

using StudentServiceStatefull.Interfaces;
using StudentServiceStatefull.Models;

namespace StudentServiceStatefull
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class StudentServiceStatefull : StatefulService, IStudentServiceStatefull
    {
        private DbStart DbStart;
        public StudentServiceStatefull(StatefulServiceContext context)
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
               || string.IsNullOrEmpty(s.Prezime) || string.IsNullOrEmpty(s.Index))
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
               || string.IsNullOrEmpty(s.Prezime) || string.IsNullOrEmpty(s.Index))
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

        public Task<Tuple<bool, Izvestaj>> Izvestaj(string token)
        {
            string email = Jwt.GetUsernameFromToken(token);
            if (string.IsNullOrEmpty(email))
            {
                return Task.FromResult(new Tuple<bool, Izvestaj>(false, null));
            }
            var filter = Builders<Student>.Filter.And(
               Builders<Student>.Filter.Eq(student => student.Email, email));

            var usr = DbStart.StudentCollection.Find(filter).FirstOrDefault();
            var filter2 = Builders<Predmet>.Filter.Empty;
            var predmeti = DbStart.PredmetiCollection.Find(filter2).ToList();
            Izvestaj i = new Izvestaj();
            i.ocene.Add(new OcenaNaPredmetu() { ImePredmeta="predmet2", Ocena=8});

            int count = 0;
            int sum = 0;
            foreach (var p in predmeti)
            {
                if (p.StudentList.ContainsKey(email))
                {

                    i.ocene.Add(new OcenaNaPredmetu() { ImePredmeta = p.ImePredmeta, Ocena = p.OcenaList[email] });
                    if (p.OcenaList[email] > 0) { count++; sum += p.OcenaList[email]; }
                }
            }

            if (count == 0)
            {
                i.SrednjaOcena = 0;
            }
            else
            {
                i.SrednjaOcena = sum / count;
            }
            return Task.FromResult(new Tuple<bool, Izvestaj>(true, i));
        }






        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceRemotingReplicaListeners();
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var tx = this.StateManager.CreateTransaction())
                {
                    var result = await myDictionary.TryGetValueAsync(tx, "Counter");

                    ServiceEventSource.Current.ServiceMessage(this.Context, "Current Counter Value: {0}",
                        result.HasValue ? result.Value.ToString() : "Value does not exist.");

                    await myDictionary.AddOrUpdateAsync(tx, "Counter", 0, (key, value) => ++value);

                    // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
                    // discarded, and nothing is saved to the secondary replicas.
                    await tx.CommitAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

       
    }
}
