using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.Linq;
using System.Text;


namespace HRMS.DataLayer
{
    public class DALSecurityActiveUsers
    {
        private object docLock = new object();
        private DALSecurityActiveUsers instance = null;
        private SecurityActiveUsers ObjCurrent;
        private string strErrorMessage = "";

        private string strError;

        public string getError
        {
            get { return strError; }
        }
        

        public SecurityActiveUsers objCurrent
        {
            get { return ObjCurrent; }
            set { ObjCurrent = value; }
        }
        
        public class SecurityActiveUsers
        {
			public int LoginInstance =  0;
			public int SegmentID =  0;
			public DateTime LoginDate =  DateTime.Now.Date;
			public string LoginTime =  "";
			public DateTime LogOutDate =  DateTime.Now.Date;
			public string LogOutTime =  "";
			public bool? ConnectionAlive =  true;
			public int UserID =  0;
			public string WorkStationName =  "";
			public string ComputerIP =  "";
            public int SystemUserTypeID = -1;

        }

        public DALSecurityActiveUsers()
        {
            ObjCurrent = new SecurityActiveUsers();
        }

        public DataTable getList()
        {
            try
            {
                object[] paraVals =  new object[] 
				{
					ObjCurrent.LoginInstance.ToString()
,					ObjCurrent.SegmentID.ToString()
,					ObjCurrent.LoginDate.ToString()
,					ObjCurrent.LoginTime.ToString()
,					ObjCurrent.LogOutDate.ToString()
,					ObjCurrent.LogOutTime.ToString()
,					ObjCurrent.ConnectionAlive.ToString()
,					ObjCurrent.UserID.ToString()
,					ObjCurrent.WorkStationName.ToString()
,					ObjCurrent.ComputerIP.ToString()
,					ObjCurrent.SystemUserTypeID.ToString()

				};

                paraVals= CommonFunctions.CheckPerameterArray(paraVals);

                DataSet ds = new DataSet();
                SqlHelper.FillDataset(DBProvider.GetPerfectConnStr, "SP_SecurityActiveUsers_Select", ds, new string[] { "SecurityActiveUsers" }, paraVals);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SecurityActiveUsers getFiltedList()
        {
            try
            {
                DataSet ds =  getList().DataSet;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //-------------------------------------------------
                    //LoginInstance   int
                    //SegmentID   int
                    //LoginDate   DateTime
                    //LoginTime   string
                    //LogOutDate   DateTime
                    //LogOutTime   string
                    //ConnectionAlive   bool?
                    //UserID   int
                    //WorkStationName   string
                    //ComputerIP   string

                    //------------------------------------------------


						ObjCurrent.LoginInstance = (int) ds.Tables[0].Rows[0]["LoginInstance"];
						ObjCurrent.SegmentID = (int) ds.Tables[0].Rows[0]["SegmentID"];
						ObjCurrent.LoginDate = (DateTime) ds.Tables[0].Rows[0]["LoginDate"];
						ObjCurrent.LoginTime = (string) ds.Tables[0].Rows[0]["LoginTime"];
						ObjCurrent.LogOutDate = (DateTime) ds.Tables[0].Rows[0]["LogOutDate"];
						ObjCurrent.LogOutTime = (string) ds.Tables[0].Rows[0]["LogOutTime"];
						ObjCurrent.ConnectionAlive = (bool?) ds.Tables[0].Rows[0]["ConnectionAlive"];
						ObjCurrent.UserID = (int) ds.Tables[0].Rows[0]["UserID"];
						ObjCurrent.WorkStationName = (string) ds.Tables[0].Rows[0]["WorkStationName"];
						ObjCurrent.ComputerIP = (string) ds.Tables[0].Rows[0]["ComputerIP"];
                        ObjCurrent.SystemUserTypeID = (int)ds.Tables[0].Rows[0]["SystemUserTypeID"];
                     
 
                
				
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
                //LoginInstance   int(int)
                //SegmentID   int(int)
                //LoginDate   DateTime(int)
                //LoginTime   string(int)
                //LogOutDate   DateTime(int)
                //LogOutTime   string(int)
                //ConnectionAlive   bool?(int)
                //UserID   int(int)
                //WorkStationName   string(int)
                //ComputerIP   string(int)

            //-------------------------------------------------
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[13];

               parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;
					  
                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;
					  
                parm[2] = new SqlParameter("@LoginInstance", SqlDbType.Int);
                parm[3] = new SqlParameter("@SegmentID", SqlDbType.Int);
                parm[4] = new SqlParameter("@LoginDate", SqlDbType.DateTime);
                parm[5] = new SqlParameter("@LoginTime", SqlDbType.VarChar);
                parm[6] = new SqlParameter("@LogOutDate", SqlDbType.DateTime);
                parm[7] = new SqlParameter("@LogOutTime", SqlDbType.VarChar);
                parm[8] = new SqlParameter("@ConnectionAlive", SqlDbType.Bit);
                parm[9] = new SqlParameter("@UserID", SqlDbType.Int);
                parm[10] = new SqlParameter("@WorkStationName", SqlDbType.VarChar);
                parm[11] = new SqlParameter("@ComputerIP", SqlDbType.VarChar);
                parm[12] = new SqlParameter("@SystemUserTypeID", SqlDbType.Int);
 

                parm[2].Value = ObjCurrent.LoginInstance;
                parm[3].Value = ObjCurrent.SegmentID;
                parm[4].Value = ObjCurrent.LoginDate;
                parm[5].Value = ObjCurrent.LoginTime;
                parm[6].Value = ObjCurrent.LogOutDate;
                parm[7].Value = ObjCurrent.LogOutTime;
                parm[8].Value = ObjCurrent.ConnectionAlive;
                parm[9].Value = ObjCurrent.UserID;
                parm[10].Value = ObjCurrent.WorkStationName;
                parm[11].Value = ObjCurrent.ComputerIP;
                parm[12].Value = ObjCurrent.SystemUserTypeID;
 


                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SP_SecurityActiveUsers_INSERTUPDATE", parm);

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
                //LoginInstance   int(int)
                //SegmentID   int(int)
                //LoginDate   DateTime(int)
                //LoginTime   string(int)
                //LogOutDate   DateTime(int)
                //LogOutTime   string(int)
                //ConnectionAlive   bool?(int)
                //UserID   int(int)
                //WorkStationName   string(int)
                //ComputerIP   string(int)
 
            //-------------------------------------------------
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[12];

                parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;
					  
                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;

                parm[2] = new SqlParameter("@LoginInstance", SqlDbType.BigInt);

 
                parm[2].Value = ObjCurrent.LoginInstance;
 

                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SP_SecurityActiveUsers_Delete", parm);

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





