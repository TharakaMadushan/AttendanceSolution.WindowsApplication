using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.Linq;
using System.Text;

namespace HRMS.DataLayer
{
  public  class DALCommonSqlDataset
    {

      int LastInsert = -1;
        public DataSet getList(String Sql)
        {
            DataSet ds=null;
            try
            {
                //DataSet ds = new DataSet();
                 ds  = SqlHelper.ExecuteDataset(DBProvider.GetPerfectConnStr, CommandType.Text, Sql);
                //return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet getList(String Sql, string _server, string _database, string _userID, string _password)
        {
            DataSet ds = null;
            try
            {
                //DataSet ds = new DataSet();
                string _ConnectionSting = string.Format("Server={0};Database={1};User Id={2};Password={3};", _server, _database, _userID, _password);
                ds = SqlHelper.ExecuteDataset(_ConnectionSting, CommandType.Text, Sql);
                //return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public void  ExecuteNonSqlQuery(String Sql)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.Text, Sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Object ExecuteScalar(String Sql)
        {
            try
            {
                return SqlHelper.ExecuteScalar(DBProvider.GetPerfectConnStr, CommandType.Text, Sql);
            }
            catch (Exception ex)
            {
                throw ex;
                return -1;
            }
        }




    }
}
