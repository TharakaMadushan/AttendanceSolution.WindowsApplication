using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.Linq;
using System.Text;


namespace HRMS.DataLayer
{
    public class DALSystemSettingServerInfo
    {
        private object docLock = new object();
        private DALSystemSettingServerInfo instance = null;
        private SystemSettingServerInfo ObjCurrent;
        private string strErrorMessage = "";

        private string strError;

        public string getError
        {
            get { return strError; }
        }
        

        public SystemSettingServerInfo objCurrent
        {
            get { return ObjCurrent; }
            set { ObjCurrent = value; }
        }
        
        public class SystemSettingServerInfo
        {
			public int LicenceingID =  0;
			public int VersionID =  0;
			public int MaxUserCount =  0;
			public string RegisteredCompanyName =  "";
			public string ServerID =  "";

        }

        public DALSystemSettingServerInfo()
        {
            ObjCurrent = new SystemSettingServerInfo();
        }

        public DataTable getList()
        {
            try
            {
                object[] paraVals =  new object[] 
				{
					ObjCurrent.LicenceingID.ToString()
,					ObjCurrent.VersionID.ToString()
,					ObjCurrent.MaxUserCount.ToString()
,					ObjCurrent.RegisteredCompanyName.ToString()
,					ObjCurrent.ServerID.ToString()

				};

                paraVals= CommonFunctions.CheckPerameterArray(paraVals);

                DataSet ds = new DataSet();
                SqlHelper.FillDataset(DBProvider.GetPerfectConnStr, "SP_SystemSettingServerInfo_Select", ds, new string[] { "SystemSettingServerInfo" }, paraVals);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SystemSettingServerInfo getFiltedList()
        {
            try
            {
                DataSet ds =  getList().DataSet;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //-------------------------------------------------
                    //LicenceingID   int
                    //VersionID   int
                    //MaxUserCount   int
                    //RegisteredCompanyName   string
                    //ServerID   string

                    //------------------------------------------------


						ObjCurrent.LicenceingID = (int) ds.Tables[0].Rows[0]["LicenceingID"];
						ObjCurrent.VersionID = (int) ds.Tables[0].Rows[0]["VersionID"];
						ObjCurrent.MaxUserCount = (int) ds.Tables[0].Rows[0]["MaxUserCount"];
						ObjCurrent.RegisteredCompanyName = (string) ds.Tables[0].Rows[0]["RegisteredCompanyName"];
						ObjCurrent.ServerID = (string) ds.Tables[0].Rows[0]["ServerID"];
 
                
				
				}
                else
                {
                    ObjCurrent = null;
                }

                return ObjCurrent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public QueryResult UpdateEntry()
        {
            //-------------------------------------------------
                //LicenceingID   int(int)
                //VersionID   int(int)
                //MaxUserCount   int(int)
                //RegisteredCompanyName   string(int)
                //ServerID   string(int)

            //-------------------------------------------------
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[7];

               parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;
					  
                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;
					  
                parm[2] = new SqlParameter("@LicenceingID", SqlDbType.Int);
                parm[3] = new SqlParameter("@VersionID", SqlDbType.Int);
                parm[4] = new SqlParameter("@MaxUserCount", SqlDbType.Int);
                parm[5] = new SqlParameter("@RegisteredCompanyName", SqlDbType.VarChar);
                parm[6] = new SqlParameter("@ServerID", SqlDbType.VarChar);
 

                parm[2].Value = ObjCurrent.LicenceingID;
                parm[3].Value = ObjCurrent.VersionID;
                parm[4].Value = ObjCurrent.MaxUserCount;
                parm[5].Value = ObjCurrent.RegisteredCompanyName;
                parm[6].Value = ObjCurrent.ServerID;
 


                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SP_SystemSettingServerInfo_INSERTUPDATE", parm);

                qryRes.RetText = parm[0].Value.ToString();
                qryRes.RetID = int.Parse(parm[1].Value.ToString());  

               
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
            }
            return qryRes;
        }

        public QueryResult RemoveEntry()
        {
            //-------------------------------------------------
                //LicenceingID   int(int)
                //VersionID   int(int)
                //MaxUserCount   int(int)
                //RegisteredCompanyName   string(int)
                //ServerID   string(int)
 
            //-------------------------------------------------
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[7];

                parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;
					  
                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;

                parm[2] = new SqlParameter("@LicenceingID", SqlDbType.Int);

 
                parm[2].Value = ObjCurrent.LicenceingID;
 

                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SP_SystemSettingServerInfo_Delete", parm);

                qryRes.RetText = parm[0].Value.ToString();


            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
            }
            return qryRes;
        }




    }
}





