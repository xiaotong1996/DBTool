using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;


namespace DB
{
    [Serializable]
    class ConfigerManger
    {

        private string userId;
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private string pwd;
        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }

        private string server;
        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        private string serverType;
        public string ServerType
        {
            get { return serverType;}
            set { serverType = value; }
        }

        private string dbName;
        public string DbName
        {
            get
            {
                return dbName;
            }

            set
            {
                dbName = value;
            }
        }



    }
}

