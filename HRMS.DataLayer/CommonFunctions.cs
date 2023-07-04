using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace HRMS.DataLayer
{
    public static class CommonFunctions
    {
        public static object[] CheckPerameterArray(object[] paraVals, bool DismissZero = false)
        {
            for (int i = 0; i < paraVals.Count<object>(); i++)
            {
                if (paraVals[i] != null)
                {
                    if (paraVals[i].GetType() == typeof(int))
                    {
                        if (((int)paraVals[i]) == -1)
                        {
                            paraVals[i] = DBNull.Value;
                        }
                    }

                    if (DismissZero == false)
                    {
                        if (paraVals[i].GetType() == typeof(string))
                        {
                            if (((string)paraVals[i]) == "" || ((string)paraVals[i]) == "-1" || ((string)paraVals[i]) == "0" || ((string)paraVals[i]) == "0.0" || ((string)paraVals[i]) == "1/1/1900 12:00:00 AM" || ((string)paraVals[i]) == "01-Jan-00 12:00:00 AM" || ((string)paraVals[i]) == "1 - Jun - 00 12:00:00 AM" || ((string)paraVals[i]) == "01/01/1900 12:00:00 AM" || ((string)paraVals[i]) == "01/01/1900 00:00:00" || ((string)paraVals[i]) == "01-01-1900 00:00:00" || ((string)paraVals[i]) == "01/01/1900 00:00:00")
                            {
                                paraVals[i] = DBNull.Value;
                            }
                        }
                    }
                    else
                    {
                        if (paraVals[i].GetType() == typeof(string))
                        {
                            if (((string)paraVals[i]) == "" || ((string)paraVals[i]) == "-1" || ((string)paraVals[i]) == "0.0" || ((string)paraVals[i]) == "1/1/1900 12:00:00 AM" || ((string)paraVals[i]) == "01-Jan-00 12:00:00 AM" || ((string)paraVals[i]) == "1 - Jun - 00 12:00:00 AM" || ((string)paraVals[i]) == "01/01/1900 12:00:00 AM" || ((string)paraVals[i]) == "01/01/1900 00:00:00" || ((string)paraVals[i]) == "01-01-1900 00:00:00" || ((string)paraVals[i]) == "01/01/1900 00:00:00")
                            {
                                paraVals[i] = DBNull.Value;
                            }
                        }
                    }

                }

            }
            return paraVals;
        }

        public static object[] CheckPerameterArrayMaltiSelect(object[] paraVals)
        {
            for (int i = 0; i < paraVals.Count<object>(); i++)
            {
                if (paraVals[i] != null)
                {
                    if (paraVals[i].GetType() == typeof(int))
                    {
                        if (((int)paraVals[i]) == -1)
                        {
                            paraVals[i] = DBNull.Value;
                        }
                    }

                    if (paraVals[i].GetType() == typeof(string))
                    {
                        if (((string)paraVals[i]) == "" || ((string)paraVals[i]) == "-1")
                        {
                            paraVals[i] = DBNull.Value;
                        }

                    }
                }

            }
            return paraVals;
        }

        /// <summary>
        /// gets a dataset from a sql command strinng
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public static DataSet getSqlDataset(string Sql)
        {
            DataSet _ds = null;
            try
            {
                DALCommonSqlDataset objDataWorker = new DALCommonSqlDataset();
                _ds = objDataWorker.getList(Sql);
            }
            catch (Exception ex)
            {

                WriteErrorLog(ex);
            }
            return _ds;
        }


        /// <summary>
        /// Check the existancce of a key value of a key field in the specified table to check existance in record removal 
        /// </summary>
        /// <param name="tableName">table which contains the id field</param>
        /// <param name="field">ID field </param>
        /// <param name="ID">ID value</param>
        /// <param name="whereCondition"> if additiona where condition exist ( Default is empty)</param>
        /// <returns>true - if ID is present / false if ID is not present </returns>
        public static bool CheckExistancePrimaryKeyInTable(string tableName, string field, string ID, string whereCondition )
        {

            bool _blnRecordsExist = false;
            try
            {
                string _sql = string.Format("Select {0} from {1} where {2} = {3}", field, tableName, field, ID, whereCondition == null ? string.Empty : whereCondition);
                DataSet _ds = CommonFunctions.getSqlDataset(_sql);

                if (CheckExistanceOfRecords(_ds))/// checks if records exist
                {
                    _blnRecordsExist = true;
                }

            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                _blnRecordsExist = false;
            }
            return _blnRecordsExist;

        }

        /// <summary>
        /// check the existance of records for the provided recordset 
        /// true if present 
        /// false if null or not present
        /// </summary>
        /// <param name="_dsRecords"></param>
        /// <returns></returns>
        public static bool CheckExistanceOfRecords(DataSet _dsRecords)
        {
            bool _blnRecordsExist = true;
            try
            {
                if (_dsRecords == null)
                { _blnRecordsExist = false; }
                else
                {
                    if (_dsRecords.Tables.Count <= 0)
                    {
                        _blnRecordsExist = false;
                    }
                    else
                    {
                        if (_dsRecords.Tables[0].Rows.Count <= 0)
                        {
                            _blnRecordsExist = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog( ex);
                _blnRecordsExist = false;
            }
            return _blnRecordsExist;
        }

        /// <summary>
        /// returns the dataformat for sql - set dateformt dmy
        /// </summary>
        /// <returns></returns>
        public static String getSqlDateFormate()
        {
            string _strSqqlFormate = "Set Dateformat dmy ";
            try
            {
                //System.Environment

            }
            catch (Exception ex)
            {
                throw ex;
                // _globalUser.ErrorObject = ex;
            }
            return _strSqqlFormate;
        }

        /// <summary>
        /// Returns the formatted date value for a sql statemen in dd/MM/yyyy format
        /// </summary>
        /// <param name="_dteSendingDate"></param>
        /// <returns></returns>
        public static String getSqlDateValue(DateTime _dteSendingDate)
        {
            string _strSqqlFormate = _dteSendingDate.ToShortDateString();
            try
            {
                _strSqqlFormate = _dteSendingDate.ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {
                throw ex;
                //_globalUser.ErrorObject = ex;
            }
            return _strSqqlFormate;
        }


        public static void WriteErrorLog(Exception ex)
        {
            try
            {
                // Edited by DDEV for maintain a seperate file for a month on 08 SEP 2017
                //System.IO.StreamWriter _stwErrorLog = new System.IO.StreamWriter(System.Environment.CurrentDirectory + "\\" + "ErrorDataLayerlog_" + System.DateTime.Now.ToString("yyyyMMddmmHHss") + ".log");
                System.IO.StreamWriter _stwErrorLog = new System.IO.StreamWriter(System.Environment.CurrentDirectory + "\\ErrorLOG\\" + "ErrorLog_" + System.DateTime.Now.ToString("yyyyMMM") + ".log",true);
                _stwErrorLog.WriteLine("-------- " + System.DateTime.Now.ToLongDateString() + " -- " + System.DateTime.Now.ToLongTimeString() + " -------------------------------------------------------");
                _stwErrorLog.Write("Error : " + ex.Message.ToString() + System.Environment.NewLine + "Stack Trace:" + ex.StackTrace.ToString());
                
                _stwErrorLog.Close();
                _stwErrorLog.Dispose();

            }
            catch (Exception exEx)
            {
                System.IO.StreamWriter _stwErrorLogEX = new System.IO.StreamWriter(System.Environment.CurrentDirectory + "\\ErrorLOG\\" + "ErrorLog_" + System.DateTime.Now.ToString("yyyyMMM") + ".log", true);
                _stwErrorLogEX.WriteLine("-------- " + System.DateTime.Now.ToLongDateString() + " -- " + System.DateTime.Now.ToLongTimeString() + " -------------------------------------------------------");
                _stwErrorLogEX.Write("Exception Error : " + exEx.Message.ToString() + System.Environment.NewLine + "Stack Trace:" + exEx.StackTrace.ToString());

                _stwErrorLogEX.Close();
                _stwErrorLogEX.Dispose();
                //throw;
            }
        }




    }


}
