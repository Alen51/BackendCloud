using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Student.Models;


namespace Student.Interfaces
{
    [ServiceContract]
    public interface IStudentService : IService
    {
        [OperationContract]
        public Task<Tuple<bool, string>> Register(Student s);

        [OperationContract]
        public Task<Tuple<bool, string>> LogIn(string email, string password);

        [OperationContract]
        public Task<Tuple<bool, Student>> GetProfile(string token);

        [OperationContract]
        public Task<Tuple<bool, string>> ModifyProfile(Student s, string oldPassword, string newPassword);
    }
}
