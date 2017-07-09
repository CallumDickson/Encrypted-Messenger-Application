using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChattingInterfaces
{

    public interface IClient
    {

        // server to client contract
        [OperationContract]
        void Placeholder();
        [OperationContract]
        void GetMessage(string message, string userName, bool important);
        [OperationContract]
        void GetMessage2(string message, string userName, bool important);
        [OperationContract]
        void GetUpdate(int value, string userName);

    }
}
