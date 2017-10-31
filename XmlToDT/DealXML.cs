using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Xml;
using System.Data.OleDb;
using DevExpress.Xpf.Grid;
using System.Windows.Forms;

namespace XmlToDT
{
    public class DealXML
    {
        /// <summary>
        /// 将xml对象内容转换为dataset
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        private static DataSet ConvertXMLToDataSet(string xmlData)
        {
            using (StringReader stream = new StringReader(xmlData))
            {
                using (XmlTextReader reader = new XmlTextReader(stream))
                {
                    try
                    {
                        DataSet xmlDS = new DataSet();
                        xmlDS.ReadXml(reader);
                        return xmlDS;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 将DataSet转换为xml对象字符串
        /// </summary>
        /// <param name="xmlDS"></param>
        /// <returns></returns>

        public static string ConvertDataSetToXML(DataSet xmlDS)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                //从stream装载到XmlTextReader 
                using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.Default))
                {

                    try
                    {                       
                        //用WriteXml方法写入文件.  
                        xmlDS.WriteXml(writer);
                        int count = (int)stream.Length;
                        byte[] arr = new byte[count];
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.Read(arr, 0, count);

                        return Encoding.Default.GetString(arr).Trim();
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (writer != null) writer.Close();
                    }
                }
            }
        }  

        /// <summary>
        /// 将DataSet转换为xml文件
        /// </summary>
        /// <param name="xmlDS"></param>
        /// <param name="xmlFile"></param>
 
        public static void ConvertDataSetToXMLFile(DataSet xmlDS,string xmlFile)  
        {
            using (MemoryStream stream = new MemoryStream())
            {
                //从stream装载到XmlTextReader 
                using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.Default))
                {

                    try
                    {                        
                        //用WriteXml方法写入文件.  
                        xmlDS.WriteXml(writer);
                        int count = (int)stream.Length;
                        byte[] arr = new byte[count];
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.Read(arr, 0, count);

                        //返回Encoding.Default编码的文本  
                        using (StreamWriter sw = new StreamWriter(xmlFile))
                        {
                            sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                            string ss = System.Text.Encoding.Default.GetString(arr).Trim();
                            sw.WriteLine(ss);
                            sw.Flush();
                            sw.Close();
                        }
                        //排版生成的xml文档
                        XmlDocument doc = new XmlDocument();
                        doc.Load(xmlFile);
                        doc.Save(xmlFile);
                        doc = null;

                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (writer != null) writer.Close();
                    }
                }
            }
        }


        // Xml结构的文件读到DataSet中
        public static DataSet XmlToDataTableByFile(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            string xmlString = doc.InnerXml;
            DataSet XmlDS = ConvertXMLToDataSet(xmlString);            

            return XmlDS;
        }

        public static string GetComment(string name)
        {
            switch (name)
            {
                case "basicConfig":
                    return "基本设定";
                case "teleMeterNode":
                    return "遥测定义";
                case "teleControl":
                    return "遥控定义";
                case "teleSignalGB":
                    return "遥信定义_GB";
                case "teleSignal":
                    return "遥信定义_Bit&历史告警定义";
                case "modeText":
                    return "遥信定义_Mode";
                case "int_para":
                    return "整形参数定义&校准参数定义";
                case "bit_para":
                    return "位参数定义";
                case "teleMeter":
                    return "历史记录";
                default:
                    return "其他";
            }
        }

        public static string GetDetailComment(string name)
        {
            switch (name)
            {
                case "basicConfig":
                    return Models.basicConfig_dc;
                case "teleMeterNode":
                    return Models.teleMeterNode_dc;
                case "teleControl":
                    return Models.teleControl_dc;
                case "teleSignalGB":
                    return Models.teleSignalGB_dc;
                case "teleSignal":
                    return Models.teleSignal_dc;
                case "modeText":
                    return Models.modeText_dc;
                case "int_para":
                    return Models.int_para_dc;
                case "bit_para":
                    return Models.bit_para_dc;
                case "teleMeter":
                    return Models.teleMeter_dc;
                default:
                    return "其他";
            }
        }

        public static string GetName(string comment)
        {
            switch (comment)
            {
                case "基本设定":
                    return "basicConfig";
                case "遥测定义":
                    return "teleMeterNode";
                case "遥控定义":
                    return "teleControl";
                case "遥信定义_GB":
                    return "teleSignalGB";
                case "遥信定义_Bit&历史告警定义":
                    return "teleSignal";
                case "遥信定义_Mode":
                    return "modeText";
                case "整形参数定义&校准参数定义":
                    return "int_para";
                case "位参数定义":
                    return "bit_para";
                case "历史记录":
                    return "teleMeter";
                default:
                    return "other";
            }
        }
    }

    public class DealXLS
    {
        public static DataSet ExcelToDS(string Path)
        {
            if (Path.Length <= 0)
            {
                return null;
            }
            string strConn = "";
            string tableName = "";

            //需要安装下载新的驱动引擎http://download.microsoft.com/download/7/0/3/703ffbcb-dc0c-4e19-b0da-1463960fdcdb/AccessDatabaseEngine.exe
            strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 12.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                if (Path.Substring(Path.Length - 1, 1) == "s")
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
                else if (Path.Substring(Path.Length - 1, 1) == "x")
                {
                    throw new Exception("仅支持.xls版本导入");
                }
                conn.Open();
            }
            string strExcel = "";
            OleDbDataAdapter myCommand = null;

            DataSet ds = new DataSet(); ;
            DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
            for (int i = 0; i < schemaTable.Rows.Count; i++)
            {
                tableName = schemaTable.Rows[i][2].ToString().Trim();
                if (!tableName.Contains("FilterDatabase") && tableName.Substring(tableName.Length - 1, 1) != "_")
                {
                    ds.Tables.Add(tableName);
                    strExcel = string.Format("select * from [{0}]", tableName);
                    myCommand = new OleDbDataAdapter(strExcel, strConn);
                    myCommand.Fill(ds, tableName);
                }
            }
            conn.Close();
            return ds;
        }
    }

}
