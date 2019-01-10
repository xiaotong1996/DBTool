using DB.Properties;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DB.ParsingTool
{
    class MappingTool
    {


        /// <summary>
        /// 取所有Oracle类型
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string,string> GetAllTypesOracle()
        {
            XElement root = XElement.Parse(Resources.ResourceManager.GetObject("XMLTypeOracle") as string);
            IEnumerable<XElement> tests =
                from el in root.Elements("JavaType")
                select el;

            Dictionary<string, string> typeList = new Dictionary<string, string>();

            foreach (XElement el2 in tests.Elements("DBType"))
            {
                string dbType = el2.Value;
                string dbTypeLength = (el2.Attribute("length") == null) ? "" : el2.Attribute("length").Value;

                typeList.Add(dbType, dbTypeLength);
            }

            return typeList;
        }

        /// <summary>
        /// 取所有SQLServer类型
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetAllTypesSQL()
        {
            XElement root = XElement.Parse(Resources.ResourceManager.GetObject("XMLTypeSQL") as string);
            IEnumerable<XElement> tests =
                from el in root.Elements("JavaType")
                select el;

            Dictionary<string, string> typeList = new Dictionary<string, string>();

            foreach (XElement el2 in tests.Elements("DBType"))
            {
                string dbType = el2.Value;
                string dbTypeLength = (el2.Attribute("length") == null) ? "" : el2.Attribute("length").Value;

                typeList.Add(dbType, dbTypeLength);
            }

            return typeList;
        }


        public static Dictionary<string,string> JavaToSQL(string javatype)
        {
            if(javatype!=null&&javatype.Length!=0)
            {
                return ParserInfoTypeSQL(javatype);
            }
            else
            {
                return new Dictionary<string, string>() { };
            }
        }


        public static Dictionary<string, string> JavaToOracle(string javatype)
        {

            if (javatype != null && javatype.Length != 0)
            {
                return ParserInfoTypeOracle(javatype);
            }
            else
            {
                return new Dictionary<string, string>() { };
            }
        }

        /// <summary>
        /// 解析保存类型映射的配置文件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ParserInfoTypeSQL(string type)
        {
            XElement root = XElement.Parse(Resources.ResourceManager.GetObject("XMLTypeSQL") as string);
            IEnumerable<XElement> tests =
                from el in root.Elements("JavaType")
                where (string)el.Attribute("type") == type
                select el;

            Dictionary<string, string> typeList = new Dictionary<string, string>();

            foreach (XElement el2 in tests.Elements("DBType"))
            {
                string dbType = el2.Value;
                string dbTypeLength = (el2.Attribute("length") == null) ? "":el2.Attribute("length").Value;
                
                typeList.Add(dbType,dbTypeLength);
            }

            return typeList;
        }


        /// <summary>
        /// 解析保存类型映射的配置文件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ParserInfoTypeOracle(string type)
        {

            XElement root = XElement.Parse(Resources.ResourceManager.GetObject("XMLTypeOracle") as string);
            //XElement.Load(Resources.ResourceManager.GetObject("XMLTypeOracle") as string);
            IEnumerable<XElement> tests =
                from el in root.Elements("JavaType")
                where (string)el.Attribute("type") == type
                select el;

            Dictionary<string, string> typeList = new Dictionary<string, string>();

            foreach (XElement el2 in tests.Elements("DBType"))
            {
                string dbType = el2.Value;
                string dbTypeLength = (el2.Attribute("length") == null) ? "" : el2.Attribute("length").Value;

                typeList.Add(dbType, dbTypeLength);
            }

            return typeList;
        }


        /// <summary>
        /// 根据DBType的值，查找length的值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string SQltypeToLength(string type)
        {
            XElement root = XElement.Parse(Resources.ResourceManager.GetObject("XMLTypeSQL") as string);
            IEnumerable<XElement> tests =
                from el in root.Elements("JavaType").Elements("DBType")
                where (string)el.Value == type
                select el;
            if (tests.Any())
            {
                XElement el2 = tests.First();
                string dbTypeLength = ((string)el2.Attribute("length").Value == "") ? "" : el2.Attribute("length").Value;
                return dbTypeLength;
            }
            else
            {
                return "";
            }
            
        }

        /// <summary>
        /// 根据DBType的值，查找length的值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string OracletypeToLength(string type)
        {
            XElement root = XElement.Parse(Resources.ResourceManager.GetObject("XMLTypeOracle") as string);
            IEnumerable<XElement> tests =
                from el in root.Elements("JavaType").Elements("DBType")
                where (string)el.Value == type
                select el;
            if (tests.Any())
            {
                XElement el2 = tests.First();
                string dbTypeLength = ((string)el2.Attribute("length").Value == "") ? "" : el2.Attribute("length").Value;
                return dbTypeLength;
            }
            else
            {
                return "";
            }
               
        }


    }

}

