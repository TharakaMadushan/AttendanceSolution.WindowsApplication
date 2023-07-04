using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.Linq;
using System.Text;


namespace HRMS.DataLayer
{
    public class DALSecurityAuditTrailOptions
    {
        private object docLock = new object();
        private DALSecurityAuditTrailOptions instance = null;
        private SecurityAuditTrailOptions ObjCurrent;
        private string strErrorMessage = "";

        private string strError;

        public string getError
        {
            get { return strError; }
        }
        

        public SecurityAuditTrailOptions objCurrent
        {
            get { return ObjCurrent; }
            set { ObjCurrent = value; }
        }
        
        public class SecurityAuditTrailOptions
        {
			public int AduitLoginID =  0;
			public int UserID =  0;
			public int OptionID =  0;
			public int OptionFacilityID =  0;
			public DateTime UseDateTime =  DateTime.Now.Date;

        }

        public DALSecurityAuditTrailOptions()
        {
            ObjCurrent = new SecurityAuditTrailOptions();
        }

        public DataTable getList()
        {
            try
            {
                object[] paraVals =  new object[] 
				{
					ObjCurrent.AduitLoginID.ToString()
,					ObjCurrent.UserID.ToString()
,					ObjCurrent.OptionID.ToString()
,					ObjCurrent.OptionFacilityID.ToString()
,					ObjCurrent.UseDateTime.ToString()

				};

                paraVals= CommonFunctions.CheckPerameterArray(paraVals);

                DataSet ds = new DataSet();
                SqlHelper.FillDataset(DBProvider.GetPerfectConnStr, "SP_SecurityAuditTrailOptions_Select", ds, new string[] { "SecurityAuditTrailOptions" }, paraVals);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SecurityAuditTrailOptions getFiltedList()
        {
            try
            {
                DataSet ds =  getList().DataSet;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //-------------------------------------------------
                    //AduitLoginID   int
                    //UserID   int
                    //OptionID   int
                    //OptionFacilityID   int
                    //UseDateTime   DateTime

                    //------------------------------------------------


						ObjCurrent.AduitLoginID = (int) ds.Tables[0].Rows[0]["AduitLoginID"];
						ObjCurrent.UserID = (int) ds.Tables[0].Rows[0]["UserID"];
						ObjCurrent.OptionID = (int) ds.Tables[0].Rows[0]["OptionID"];
						ObjCurrent.OptionFacilityID = (int) ds.Tables[0].Rows[0]["OptionFacilityID"];
						ObjCurrent.UseDateTime = (DateTime) ds.Tables[0].Rows[0]["UseDateTime"];
 
                
				
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
                //AduitLoginID   int(int)
                //UserID   int(int)
                //OptionID   int(int)
                //OptionFacilityID   int(int)
                //UseDateTime   DateTime(int)

            //-------------------------------------------------
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[7];

               parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;
					  
                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;
					  
                parm[2] = new SqlParameter("@AduitLoginID", SqlDbType.Int);
                parm[3] = new SqlParameter("@UserID", SqlDbType.Int);
                parm[4] = new SqlParameter("@OptionID", SqlDbType.Int);
                parm[5] = new SqlParameter("@OptionFacilityID", SqlDbType.Int);
                parm[6] = new SqlParameter("@UseDateTime", SqlDbType.DateTime);
 

                parm[2].Value = ObjCurrent.AduitLoginID;
                parm[3].Value = ObjCurrent.UserID;
                parm[4].Value = ObjCurrent.OptionID;
                parm[5].Value = ObjCurrent.OptionFacilityID;
                parm[6].Value = ObjCurrent.UseDateTime;
 


                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SP_SecurityAuditTrailOptions_INSERTUPDATE", parm);

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
                //AduitLoginID   int(int)
                //UserID   int(int)
                //OptionID   int(int)
                //OptionFacilityID   int(int)
                //UseDateTime   DateTime(int)
 
            //-------------------------------------------------
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[7];

                parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;
					  
                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;

                parm[2] = new SqlParameter("@AduitLoginID", SqlDbType.BigInt);

 
                parm[2].Value = ObjCurrent.AduitLoginID;
 

                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SP_SecurityAuditTrailOptions_Delete", parm);

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





