using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.DatabaseConnectionSQLServer
{
    class ParsedInfo
    {
        private string name;
        private Dictionary<string,string> typeInfo;
        private string column;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Column
        {
            get
            {
                return column;
            }

            set
            {
                column = value;
            }
        }


        public Dictionary<string, string> TypeInfo
        {
            get
            {
                return typeInfo;
            }

            set
            {
                typeInfo = value;
            }
        }
    }
}
