using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChattingInterfaces
{
    [ServiceContract(CallbackContract = typeof(IClient))]
    public interface IChattingService
    {
        // Client to server contracts
        [OperationContract]
        int Login(string userName);
        [OperationContract]
        void SendMessageToALL(string message, string userName, bool important);
        [OperationContract]
        void Logout();
        [OperationContract]
        List<string> GetCurrentUsers();
        [OperationContract]
        void SendMessageToPrivate(string message, string userName, string theiruserName, bool important);
    }
}
