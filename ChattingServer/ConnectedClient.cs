using ChattingInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingServer
{
    // the users connection to the server database data types

    class ConnectedClient
    {
        public IClient connection;
        public string UserName { get; set;}
            
    }
}
