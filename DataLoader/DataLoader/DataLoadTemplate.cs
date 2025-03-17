using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Mailer;
using ErrorLogger;

namespace DataLoader
{
    /// <summary>
    /// Reading xml template data
    /// </summary>
    public class DataLoadTemplate
    {
        #region Private Members
        private string strInputFilesPath;
        private string strInputFilePattern = string.Empty;
        private string strColumnDelimiter = string.Empty;
        private string strTableName = string.Empty;
        private bool bIsTableParam = false;
        private string strSPName = string.Empty;
        private string strAsOfDate;
        private string[] strInputFiles;
        private string strFileArchivePath;
        private string strUrl = string.Empty;
        private List<string> lstSheetNames = new List<string>();
        private bool bFileNameParam = false;
        private bool bReInsert = false;
        private string strAlertMailId = string.Empty;
        private string strAlertMailSubject = string.Empty;
        private string strDBName = string.Empty;

        private DateTime dInputFileDate;
        private string strEnclosingRequired = string.Empty;
        private string strSource = string.Empty;
        private int iRecordsCount = 0;
        private int iversion = 0;
        private Enumerations.InputType eFileType;
        private bool bIsEnabled = false;
        private bool bIsHeader = false;
        private bool bIsTrailer = false;
        private int iStartLine = 0;
        private int iEndLine = 0;
        private List<Column> objColumn = new List<Column>();
        private string strVersionFilePath = string.Empty;
        private bool enableFileVersioning = false;
        private string strDbType;
        #endregion


        #region public properties
        public string InputFilePattern
        {
            get { return this.strInputFilePattern; }
            set { this.strInputFilePattern = value; }
        }

        public string ColumnDelimiter
        {
            get
            {
                if (this.strColumnDelimiter == "tab")
                    return "\t";
                else
                    return this.strColumnDelimiter.Trim();
            }
            set { this.strColumnDelimiter = value.Trim(); }
        }

        public string TableName
        {
            get { return this.strTableName; }
            set { this.strTableName = value.Trim(); }
        }

        public bool TableParam
        {
            get { return this.bIsTableParam; }
            set { this.bIsTableParam = value; }
        }

        public string SPName
        {
            get { return this.strSPName; }
            set { this.strSPName = value.Trim(); }
        }

        public string AsOfDate
        {
            get { return this.strAsOfDate; }
            set { this.strAsOfDate = value.Trim(); }
        }

        public string[] InputFiles
        {
            get { return this.strInputFiles; }
        }

        public string FileArchivePath
        {
            get { return this.strFileArchivePath; }
            set { this.strFileArchivePath = value.Trim(); }
        }

        public string Url
        {
            get { return this.strUrl; }
            set { this.strUrl = value.Trim(); }
        }

        public List<string> SheetNames
        {
            get { return this.lstSheetNames; }
            set { this.lstSheetNames = value; }
        }

        public bool FileNameParam
        {
            get { return this.bFileNameParam; }
            set { this.bFileNameParam = value; }
        }

        public bool ReInsert
        {
            get { return this.bReInsert; }
            set { this.bReInsert = value; }
        }

        public string AlertMailId
        {
            get { return this.strAlertMailId; }
            set { this.strAlertMailId = value.Trim(); }
        }

        public string AlertMailSubject
        {
            get { return this.strAlertMailSubject; }
            set { this.strAlertMailSubject = value.Trim(); }
        }

        public string DBName
        {
            get { return this.strDBName; }
            set { this.strDBName = value.Trim(); }
        }

        public DateTime InputFileDate
        {
            get { return this.dInputFileDate; }
            set { this.dInputFileDate = value; }
        }

        public string EnclosingRequired
        {
            get { return this.strEnclosingRequired; }
            set { this.strEnclosingRequired = value.Trim(); }
        }

        public string Source
        {
            get { return this.strSource; }
            set { this.strSource = value.Trim(); }
        }

        public int RecordsCount
        {
            get { return this.iRecordsCount; }
            set { this.iRecordsCount = value; }
        }

        public int FileVersionNo
        {
            get { return this.iversion; }
            set { this.iversion = value; }
        }

        public Enumerations.InputType FileType
        {
            get { return this.eFileType; }
            set { this.eFileType = value; }
        }

        public bool IsEnabled
        {
            get { return this.bIsEnabled; }
            set { this.bIsEnabled = value; }
        }

        public bool IsHeader
        {
            get { return this.bIsHeader; }
            set { this.bIsHeader = value; }
        }

        public bool IsTrailer
        {
            get { return this.bIsTrailer; }
            set { this.bIsTrailer = value; }
        }

        public int StartLine
        {
            get { return this.iStartLine; }
            set { this.iStartLine = value; }
        }

        public int EndLine
        {
            get { return this.iEndLine; }
            set { this.iEndLine = value; }
        }

        public List<Column> Columns
        {
            get { return this.objColumn; }
            set { this.objColumn = value; }
        }

        public bool EnableFileVersioning
        {
            get { return this.enableFileVersioning; }
            set { this.enableFileVersioning = value; }
        }
        #endregion


        #region constructor
        public DataLoadTemplate(string strDataLoadtemplatePath)
        {
            ErrorLog.WriteErrorLog(MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Loading template values to the class properties.", ErrorLog.Loglevel.INFO.ToString());
            try
            {
                //Here it's reading xml data present in template
                string xmlText = File.ReadAllText(strDataLoadtemplatePath);
                XmlDocument objXmlDocument = new XmlDocument();
                objXmlDocument.LoadXml(xmlText);
                var content = (Enumerations.InputType)Enum.Parse(typeof(Enumerations.InputType), (String.IsNullOrEmpty(objXmlDocument.GetElementsByTagName("INPUT_TYPE").Item(0).InnerText) ? "0" : objXmlDocument.GetElementsByTagName("INPUT_TYPE").Item(0).InnerText));
                this.eFileType = content;
                this.strSPName = objXmlDocument.GetElementsByTagName("SP_NAME").Item(0).InnerText;
                this.strAlertMailSubject = objXmlDocument.GetElementsByTagName("ALERT_MAIL_SUBJECT").Item(0).InnerText;
                this.strAlertMailId = objXmlDocument.GetElementsByTagName("ALERT_MAIL_ID").Item(0).InnerText;
                this.bReInsert = Convert.ToBoolean(String.IsNullOrEmpty(objXmlDocument.GetElementsByTagName("REINSERT").Item(0).InnerText) ? "false" : objXmlDocument.GetElementsByTagName("REINSERT").Item(0).InnerText);
                this.strDBName = objXmlDocument.GetElementsByTagName("DB_NAME").Item(0).InnerText;

                if (this.eFileType != Enumerations.InputType.DB)
                {
                    this.strInputFilePattern = objXmlDocument.GetElementsByTagName("INPUT_FILE_PATTERN").Item(0).InnerText;
                    this.strInputFilesPath = objXmlDocument.GetElementsByTagName("INPUT_FILE_PATH").Item(0).InnerText;
                    this.strFileArchivePath = String.IsNullOrEmpty(objXmlDocument.GetElementsByTagName("FILE_ARCHIVE_PATH").Item(0).InnerText) ? "" : objXmlDocument.GetElementsByTagName("FILE_ARCHIVE_PATH").Item(0).InnerText;
                    this.dInputFileDate = DateTime.Today;
                    this.strColumnDelimiter = objXmlDocument.GetElementsByTagName("COLUMN_DELIMITER").Item(0).InnerText;
                    this.strTableName = objXmlDocument.GetElementsByTagName("TABLE_NAME").Item(0).InnerText;
                    this.bIsTableParam = Convert.ToBoolean(String.IsNullOrEmpty(objXmlDocument.GetElementsByTagName("TABLE_PARAM").Item(0).InnerText) ? "false" : objXmlDocument.GetElementsByTagName("TABLE_PARAM").Item(0).InnerText);
                    this.lstSheetNames = objXmlDocument.GetElementsByTagName("SHEET_NAMES").Item(0).InnerText.Split(',').ToList<String>();
                    this.bFileNameParam = Convert.ToBoolean(String.IsNullOrEmpty(objXmlDocument.GetElementsByTagName("FILENAME_SP_PARAM").Item(0).InnerText) ? "false" : objXmlDocument.GetElementsByTagName("FILENAME_SP_PARAM").Item(0).InnerText);
                    //this.bIsHeader = Convert.ToBoolean(String.IsNullOrEmpty(objXmlDocument.GetElementsByTagName("ISHEADER").Item(0).InnerText) ? "false" : objXmlDocument.GetElementsByTagName("ISHEADER").Item(0).InnerText);
                    //this.bIsTrailer = Convert.ToBoolean(String.IsNullOrEmpty(objXmlDocument.GetElementsByTagName("ISTRAILER").Item(0).InnerText) ? "false" : objXmlDocument.GetElementsByTagName("ISTRAILER").Item(0).InnerText);
                    //this.iStartLine = Convert.ToInt32(String.IsNullOrEmpty(objXmlDocument.GetElementsByTagName("STARTLINE").Item(0).InnerText) ? "0" : objXmlDocument.GetElementsByTagName("STARTLINE").Item(0).InnerText);
                    //this.iEndLine = Convert.ToInt32(String.IsNullOrEmpty(objXmlDocument.GetElementsByTagName("ENDLINE").Item(0).InnerText) ? "0" : objXmlDocument.GetElementsByTagName("ENDLINE").Item(0).InnerText);
                    this.strUrl = objXmlDocument.GetElementsByTagName("URL").Item(0).InnerText.Contains("{") && objXmlDocument.GetElementsByTagName("URL").Item(0).InnerText.Contains("}") ? ProcessUrl(objXmlDocument.GetElementsByTagName("URL").Item(0).InnerText) : objXmlDocument.GetElementsByTagName("URL").Item(0).InnerText;
                    this.strInputFiles = Directory.GetFiles(objXmlDocument.GetElementsByTagName("INPUT_FILE_PATH").Item(0).InnerText, strInputFilePattern);
                    XmlNodeList colNode = objXmlDocument.GetElementsByTagName("COLUMN");
                    foreach (XmlNode nd in colNode)
                    {
                        Column objCol = new Column();
                        objCol.SpParamName = nd.Attributes["PARAM_NAME"].Value;
                        objCol.IncomingColumnName = nd.Attributes["NAME"].Value;
                        objCol.StrDataType = nd.Attributes["SQLDATATYPE"].Value;
                        this.objColumn.Add(objCol);
                    }
                }
                else
                {
                    this.AsOfDate = String.IsNullOrEmpty(objXmlDocument.GetElementsByTagName("AS_OF_DATE").Item(0).InnerText) ? "" : objXmlDocument.GetElementsByTagName("AS_OF_DATE").Item(0).InnerText;
                }
                ErrorLog.WriteErrorLog(MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Template values are loaded into the class properties.", ErrorLog.Loglevel.INFO.ToString());
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message + ex.StackTrace + ex.TargetSite);
                ErrorLog.WriteErrorLog(MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Error Occured: " + ex.Message + ex.InnerException + ex.StackTrace + ex.TargetSite, ErrorLog.Loglevel.INFO.ToString());
                throw ex;
            }
        }
        #endregion





        private string ProcessUrl(string url)
        {
            ErrorLog.WriteErrorLog(MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Processing the given URl: " + url, ErrorLog.Loglevel.INFO.ToString());
            try
            {
                DateTime dtValue;
                string retVal;
                string[] arrFormat = (Regex.Match(url, @"\{([^)]*)\}").Groups[1].Value).Split('-');
                if (!String.IsNullOrEmpty(arrFormat[1]))
                {
                    ErrorLog.WriteErrorLog(MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Gettign previous business date from DB.", ErrorLog.Loglevel.INFO.ToString());
                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TREASURY_RECON"].ConnectionString);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select Treasury_Recon..dbo.fn_get_previous_bus_dt_gvndt ('" + DateTime.Today.ToString() + "')", conn);
                    dtValue = Convert.ToDateTime(cmd.ExecuteScalar());
                    conn.Close();
                    ErrorLog.WriteErrorLog(MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Previous business date value: " + dtValue, ErrorLog.Loglevel.INFO.ToString());
                }
                else
                {
                    dtValue = DateTime.Now;
                    ErrorLog.WriteErrorLog(MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Date value: " + dtValue, ErrorLog.Loglevel.INFO.ToString());
                }
                retVal = dtValue.ToString(arrFormat[0].Trim(new char[] { ':', '0' }));
                retVal = string.Format(url, retVal);
                string strTempDir = this.strInputFilesPath.Trim('\\') + @"\temp";
                if (!Directory.Exists(strTempDir))
                    Directory.CreateDirectory(strTempDir);
                if (!Directory.Exists(strInputFilesPath))
                    Directory.CreateDirectory(strInputFilesPath);
                System.IO.DirectoryInfo di = new DirectoryInfo(strTempDir);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }

                using (var client = new WebClient())
                {
                    var uri = new Uri(retVal);
                    ErrorLog.WriteErrorLog(MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "File download started: " + Path.GetFileName(uri.LocalPath), ErrorLog.Loglevel.INFO.ToString());
                    client.DownloadFile(retVal, strTempDir + Path.GetFileName(uri.LocalPath));
                    FileInfo finfo = new FileInfo(strTempDir + Path.GetFileName(uri.LocalPath));
                    finfo.CopyTo(strInputFilesPath + finfo.Name, true);
                    Directory.Delete(strTempDir, true);
                    ErrorLog.WriteErrorLog(MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "File downloaded successfully.", ErrorLog.Loglevel.INFO.ToString());
                }
                return retVal;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog(MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Error Occured: " + ex.Message + ex.InnerException + ex.StackTrace + ex.TargetSite, ErrorLog.Loglevel.INFO.ToString());
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }






        ///<summary>
        ///Contains information regarding each columns in the appropriate template
        ///</summary>
        public class Column
        {
            private string strSpParamName = string.Empty;
            private string strIncommingColumnName = string.Empty;
            private string strdefaultValue = string.Empty;
            private SqlDbType sqlDbDataType = SqlDbType.Float;
            private string strSqlDbDataType = "VarChar";

            public string SpParamName
            {
                get { return this.strSpParamName; }
                set { this.strSpParamName = value.Trim(); }
            }

            public string IncomingColumnName
            {
                get { return this.strIncommingColumnName; }
                set { this.strIncommingColumnName = value.Trim(); }
            }

            public string DefaultValue
            {
                get { return this.strdefaultValue; }
                set { this.strdefaultValue = value.Trim(); }
            }

            public string StrDataType
            {
                get { return this.strSqlDbDataType; }
                set { this.strSqlDbDataType = value.Trim(); }
            }

            public SqlDbType SqlDbDataType
            {
                get
                {
                    try
                    {
                        return (SqlDbType)Enum.Parse(typeof(SqlDbType), this.strSqlDbDataType);
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.WriteErrorLog(MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Error Occured: " + ex.Message + ex.InnerException + ex.StackTrace + ex.TargetSite, ErrorLog.Loglevel.INFO.ToString());
                        Console.Write(ex.Message + ex.StackTrace + ex.TargetSite);
                        Console.ReadKey();
                        return sqlDbDataType;
                    }
                }
            }
        }
    }
}
