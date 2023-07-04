using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using HRMS.DefaultConstants;

namespace HRMS.BusinessLayer
{
    public class Base
    {


        protected string[] _mainIDColumns;
        protected string[] _mainDisplaycolumns;

        public string[] MainIDColumns
        {
            get { return _mainIDColumns; }
            set { _mainIDColumns = value; }
        }


        public string[] MainDisplaycolumns
        {
            get { return _mainDisplaycolumns; }
            set { _mainDisplaycolumns = value; }
        }

        public virtual string GetName()
        { return string.Empty; }
             

        public virtual DataSet GetList()
        { return new DataSet(); }

        public virtual DataSet GetListByMonth() //Added By Thus On 19Aug2019
        { return new DataSet(); }


        private string _DBConnectionStrinng = "Server=User\\SQLExpress;Database=HRIS;User Id=sa;Password=Admin@123;";
        /// <summary>
        /// get connection string 
        /// </summary>
        public string ConnectionString
        {
            get { return _DBConnectionStrinng; }
            set { _DBConnectionStrinng = value; }
        }


        private Exception _ErrorEvent;
        /// <summary>
        ///  This sets the current error object and writes a record to the app patp
        /// </summary>
        public Exception ErrorObject
        {
            get { return _ErrorEvent; }
            set
            {
                _ErrorEvent = value;
                logEvent(_ErrorEvent);
            }
        }

        string getCurrentAppPath()
        {
            string _path = System.Environment.CurrentDirectory.ToString();
            return _path;
        }

        private void logEvent(Exception ErrorEvent)
        {
            try
            {
                string _ErrorString = "error Source :" + ErrorEvent.Source + System.Environment.NewLine;
                _ErrorString += "error StackTrace:" + ErrorEvent.Message.ToString() + System.Environment.NewLine;
                _ErrorString += "error StackTrace:" + ErrorEvent.StackTrace.ToString() + System.Environment.NewLine;

                string strFileName = getCurrentAppPath() + "\\ErrorLOG\\" + " Errorlog_" + System.DateTime.Now.ToString("yyyyMMM") + ".log";
                StreamWriter _swError = new StreamWriter(strFileName,true);
                //System.IO.StreamWriter _stwErrorLogEX = new System.IO.StreamWriter(System.Environment.CurrentDirectory + "\\ErrorLOG\\" + "ErrorLog_" + System.DateTime.Now.ToString("yyyyMMM") + ".log", true);
                _swError.WriteLine("-------- " + System.DateTime.Now.ToLongDateString() + " -- " + System.DateTime.Now.ToLongTimeString() + " -------------------------------------------------------");

                
                _swError.Write(_ErrorString);
                _swError.Close();
                _swError.Dispose();
            }
            catch (Exception)
            {
            }
        }

        //////////////////////////////////////////////////////////////////////////////
        public virtual bool MapData(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return MapData(ds.Tables[0].Rows[0]);
            }
            else
            {
                return false;
            }
        }

        //////////////////////////////////////////////////////////////////////////////
        public virtual bool MapData(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                return MapData(dt.Rows[0]);
            }
            else
            {
                return false;
            }
        }

        //////////////////////////////////////////////////////////////////////////////
        public virtual bool MapData(DataRow row)
        {
            //You can put common data mapping items here (e.g. create date, modified date, etc)
            return true;
        }

        #region Get Functions

        //////////////////////////////////////////////////////////////////////////////
        protected static int GetInt(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToInt32(row[columnName]) : Constants.NullInt;
        }
        //////////////////////////////////////////////////////////////////////////////
        protected static byte[] GetByte(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? ((byte[])(row[columnName])) : Constants.NullByte;
        }
        //////////////////////////////////////////////////////////////////////////////
        protected static DateTime GetDateTime(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToDateTime(row[columnName]) : Constants.NullDateTime;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static Decimal GetDecimal(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToDecimal(row[columnName]) : Constants.NullDecimal;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static bool GetBool(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToBoolean(row[columnName]) : false;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static string GetString(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToString(row[columnName]) : Constants.NullString;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static double GetDouble(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToDouble(row[columnName]) : Constants.NullDouble;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static float GetFloat(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToSingle(row[columnName]) : Constants.NullFloat;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static Guid GetGuid(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? (Guid)(row[columnName]) : Constants.NullGuid;
        }

        //////////////////////////////////////////////////////////////////////////////
        protected static long GetLong(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? (long)(row[columnName]) : Constants.NullLong;
        }

        #endregion



    }
}
