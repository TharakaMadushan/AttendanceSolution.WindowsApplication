using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using HRMS.DefaultConstants;
using HRMS.DataLayer;

namespace HRMS.BusinessLayer
{
   public class CommonSqlDataset
      
    {
       DALCommonSqlDataset objDataWorker = new DALCommonSqlDataset();
       int LastInsert = -1;
        public  DataSet GetList(String Sql)
        {
            DataSet ds = null;
            try
            {
                ds = objDataWorker.getList(Sql);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
            return ds;
        }

        public DataSet GetList(String Sql ,string _server, string _database, string _userID, string _password)
        {
            DataSet ds = null;
            try
            {
                ds = objDataWorker.getList(Sql,_server,_database,_userID,_password);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
            return ds;
        }

        public DataSet ExecuteStoredProcedure(String Sql)
        {
            DataSet ds = null;
            try
            {
                ds = objDataWorker.getList(Sql);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
            return ds;
        }

       //2021-01-13 done by c @ 2021-01-13
        public DataSet ExecuteStoredProcedureTimeLimiter(String Sql, string _server, string _database, string _userID, string _password)
        {
            DataSet ds = null;
            try
            {
                ds = objDataWorker.getList(Sql, _server, _database, _userID, _password);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
            return ds;
        }

       /// <summary>
       /// executes sql which are not queries , just commands
       /// </summary>
       /// <param name="Sql"></param>
        public void ExecuteNonSqlQuery(String Sql)
        {
     
            try
            {
                objDataWorker.ExecuteNonSqlQuery(Sql);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);

                throw ex;
            }
        }

        /// <summary>
        /// executes sql which are not queries , just commands
        /// </summary>
        /// <param name="Sql"></param>
        public Object ExecuteScalar(String Sql)
        {

            try
            {
                return objDataWorker.ExecuteScalar(Sql);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// return the sql string value for a date value in the defualt format of dmy
        /// </summary>
        /// <param name="_dteSendingDate"></param>
        /// <returns></returns>
        public String getSqlDateValue(DateTime _dteSendingDate)
        {
            string _strSqqlFormate = _dteSendingDate.ToShortDateString();
            try
            {
                _strSqqlFormate = _dteSendingDate.ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {

                // _globalUser.ErrorObject = ex;
            }
            return _strSqqlFormate;
        }

    }
}
