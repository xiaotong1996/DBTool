using DB.DatabaseConnectionSQLServer;
using System;
using System.Collections.Generic;
using System.Xml;
using DB.ParsingTool;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Forms;

namespace DB
{
    class XMLParsing
    {

        /*

        linq to xml方式解析xml,有问题
        public static void ParserInfoSQL(IEnumerable<XElement> elements)
        {
            List<ParsedInfo> infoLists = new List<ParsedInfo>();
            foreach(var ele in elements)
            {
                ParsedInfo info = new ParsedInfo();
                info.Name = ele.Attribute("name").Value;
                info.Type = ele.Attribute("type").Value;
                info.Column = ele.Attribute("column").Value;

                infoLists.Add(info);
            }

            Console.Write(infoLists.ToString());
        }*/

        /// <summary>
        /// DOM方式解析xml
        /// </summary>
        /// <param name="path"></param>
        /// <param name="infoLists"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool ParserInfoSQL(string path,ref List<ParsedInfo> infoLists,ref string tableName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;//this忽略 DTD
                doc.Load(path);

                XmlNode xn = doc.SelectSingleNode("hibernate-mapping");
                XmlNode xn1 = xn.ChildNodes[0];
                XmlElement xe1 = (XmlElement)xn1;
                tableName = xe1.GetAttribute("table").ToString();


                XmlNodeList xnlist = xn1.ChildNodes;

                foreach (XmlNode xn2 in xnlist)
                {
                    ParsedInfo info = new ParsedInfo();
                    XmlElement xe = (XmlElement)xn2;
                    info.Name = xe.GetAttribute("name").ToString();
                    string type= xe.GetAttribute("type").ToString().Split(new char[] { '.' })[2];
                    info.TypeInfo = MappingTool.JavaToSQL(type);
                    info.Column = xe.GetAttribute("column").ToString();
                    infoLists.Add(info);
                }

                if (infoLists == null || tableName == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "XML解析失败,请重新尝试！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }



        public static bool ParserInfoOracle(string path, ref List<ParsedInfo> infoLists, ref string tableName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;//this忽略 DTD
                doc.Load(path);

                XmlNode xn = doc.SelectSingleNode("hibernate-mapping");
                XmlNode xn1 = xn.ChildNodes[0];
                XmlElement xe1 = (XmlElement)xn1;
                tableName = xe1.GetAttribute("table").ToString().ToUpper();


                XmlNodeList xnlist = xn1.ChildNodes;

                foreach (XmlNode xn2 in xnlist)
                {
                    ParsedInfo info = new ParsedInfo();
                    XmlElement xe = (XmlElement)xn2;
                    info.Name = xe.GetAttribute("name").ToString();
                    string type = xe.GetAttribute("type").ToString().Split(new char[] { '.' })[2];
                    info.TypeInfo = MappingTool.JavaToOracle(type);
                    info.Column = xe.GetAttribute("column").ToString();
                    infoLists.Add(info);
                }

                if (infoLists == null || tableName == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "XML解析失败,请重新尝试！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }


    }

}
