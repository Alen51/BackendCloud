using Microsoft.ServiceFabric.Services.Remoting;
using StudentServiceStatefull.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StudentServiceStatefull.Interfaces
{
    [ServiceContract]
    public interface IStudentServiceStatefull : IService
    {
        [OperationContract]
        public Task<Tuple<bool, string>> Register(Student s);

        [OperationContract]
        public Task<Tuple<bool, string>> LogIn(string email, string password);

        [OperationContract]
        public Task<Tuple<bool, string>> LogInProfesor(string email, string password);

        [OperationContract]
        public Task<Tuple<bool, Student>> GetProfile(string token);

        [OperationContract]
        public Task<Tuple<bool, string>> ModifyProfile(Student s);

        [OperationContract]
        public Task<Tuple<bool, string>> Authentcate(string token);

        [OperationContract]
        public Task<Tuple<bool, Izvestaj>> Izvestaj(string email);
    }
}
