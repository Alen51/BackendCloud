using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Identity.Client;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using MongoDB.Bson;
using MongoDB.Driver;
using PredmetService.Interfaces;
using PredmetService.Models;

namespace PredmetService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class PredmetService : StatefulService, IPredmetService
    {
        private DbStart DbStart;
        public PredmetService(StatefulServiceContext context)
            : base(context)
        { 
        DbStart = new DbStart();
        }

        public Task<Tuple<bool, string>> AddStudentToPredmet(string token, string nazivPredmeta)
        {
            string email = Jwt.GetUsernameFromToken(token);
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(nazivPredmeta))
                return Task.FromResult(new Tuple<bool, string>(false, ""));

            var filter = Builders<Student>.Filter.And(
               Builders<Student>.Filter.Eq(student => student.Email, email));

            var usr = DbStart.StudentCollection.Find(filter).FirstOrDefault();

            var filter2 = Builders<Predmet>.Filter.Eq(u => u.ImePredmeta, nazivPredmeta);
            var pred = DbStart.PredmetiCollection.Find(filter2).FirstOrDefault();
            pred.StudentList.Add(email, usr);
            pred.OcenaList.Add(email, 0);
            //usr.PredmetList = new List<Predmet>();

            
            /*
            var update = Builders<Student>.Update
                .Set(u => u, usr);

            var update2 = Builders<Predmet>.Update
                .Set(u => u, pred);

            DbStart.StudentCollection.UpdateOneAsync(filter,update);
            DbStart.PredmetiCollection.UpdateOneAsync(filter2,update2);
            */
            

            Predmet newP = new Predmet()
            {
                ImePredmeta=pred.ImePredmeta,
                emailProfesora= pred.emailProfesora,
                StudentList=pred.StudentList,
                OcenaList=pred.OcenaList,
            };
            
            DbStart.PredmetiCollection.DeleteOne(filter2);
            DbStart.PredmetiCollection.InsertOne(newP);


            return Task.FromResult(new Tuple<bool, string>(true, "Succesfully updated!"));
        }

        public Task<List<PredmetDto>> GetAllPredmets(string token)
        {
            string email = Jwt.GetUsernameFromToken(token);
            var filter = Builders<Predmet>.Filter.Empty;
            List<Predmet> predmeti = DbStart.PredmetiCollection.Find(filter).ToList();
            List<PredmetDto> predmetList =new List<PredmetDto>();
            foreach(var p in predmeti)
            {
                if(p.StudentList.ContainsKey(email))
                {
                    predmetList.Add(new PredmetDto() { Predmet = p, Upisan = true });
                }
                else
                {
                    predmetList.Add(new PredmetDto() { Predmet = p, Upisan = false });
                }
            }
            return Task.FromResult(predmetList);
        }

        public Task<List<Predmet>> GetAllProfesorPredmets(string email)
        {
            var filter = Builders<Predmet>.Filter.Eq(s => s.emailProfesora, email);
            var predmeti = DbStart.PredmetiCollection.Find(filter).ToList();
            return Task.FromResult(predmeti);
        }

        public Task<Tuple<bool, string>> RemoveStudentFromPredmet(string token, string nazivPredmeta)
        {
            string email = Jwt.GetUsernameFromToken(token);
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(nazivPredmeta))
                return Task.FromResult(new Tuple<bool, string>(false, ""));

            var filter = Builders<Student>.Filter.And(
               Builders<Student>.Filter.Eq(student => student.Email, email));

            var usr = DbStart.StudentCollection.Find(filter).FirstOrDefault();

            var filter2 = Builders<Predmet>.Filter.Eq(u => u.ImePredmeta, nazivPredmeta);
            var pred = DbStart.PredmetiCollection.Find(filter2).FirstOrDefault();
            pred.StudentList.Remove(email);
            pred.OcenaList.Remove(email);
            
            /*
            var update = Builders<Student>.Update
                .Set(u => u.PredmetList, usr.PredmetList);

            var update2 = Builders<Predmet>.Update
                .Set(u => u.StudentList, pred.StudentList);

            DbStart.StudentCollection.UpdateOneAsync(filter, update);
            DbStart.PredmetiCollection.UpdateOneAsync(filter2, update2);
            */
            

            Predmet newP = new Predmet()
            {
                ImePredmeta = pred.ImePredmeta,
                emailProfesora = pred.emailProfesora,
                StudentList = pred.StudentList,
                OcenaList = pred.OcenaList,
            };
            
            DbStart.PredmetiCollection.DeleteOne(filter2);
            DbStart.PredmetiCollection.InsertOne(newP);


            return Task.FromResult(new Tuple<bool, string>(true, "Succesfully updated!"));
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
