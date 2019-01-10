namespace DB.DatabaseConnectionSQLServer
{
    class DBField
    {
        private string name;
        private string type;
        private string length;
        public bool IsIndex
        {
            get;set;
        }

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

        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public string Length
        {
            get
            {
                return length;
            }

            set
            {
                length = value;
            }
        }
    }
}
