using Microsoft.ServiceFabric.Services.Remoting;
using MongoDB.Bson;
using PredmetService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PredmetService.Interfaces
{
    [ServiceContract]
    public  interface IPredmetService : IService
    {
        [OperationContract]
        public Task<List<PredmetDto>> GetAllPredmets(string token);

        [OperationContract]
        public Task<List<Predmet>> GetAllProfesorPredmets(string email);

        [OperationContract]
        public Task<Tuple<bool, string>> AddStudentToPredmet(string email, string nazivPredmeta);

        [OperationContract]
        public Task<Tuple<bool, string>> RemoveStudentFromPredmet(string email, string nazivPredmeta);
    }
}
