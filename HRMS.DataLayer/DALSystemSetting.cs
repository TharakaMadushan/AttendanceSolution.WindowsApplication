using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.Linq;
using System.Text;


namespace HRMS.DataLayer
{
    public class DALSystemSetting
    {
        private object docLock = new object();
        private DALSystemSetting instance = null;
        private SystemSetting ObjCurrent;
        private string strErrorMessage = "";

        private string strError;

        public string getError
        {
            get { return strError; }
        }
        

        public SystemSetting objCurrent
        {
            get { return ObjCurrent; }
            set { ObjCurrent = value; }
        }
        
        public class SystemSetting
        {
			public int SystemSettingID =  0;
			public string SystemCode =  "";
			public int SegmentID =  0;
			public int UserGroupID =  0;
			public int SystemSettingUserID =  0;
			public string SystemSettingItem =  "";
			public bool? SystemSettingActive =  true;
			public string SystemSettingType =  "";
			public DateTime ActiveDateFrom =  DateTime.Now.Date;
			public DateTime ActiveDateTo =  DateTime.Now.Date;
			public DateTime LastModifyDate =  DateTime.Now.Date;
			public int LastModifyUser =  0;
			public string ExtraField1 =  "";
			public string ExtraField2 =  "";
			public string ExtraField3 =  "";

        }

        public DALSystemSetting()
        {
            ObjCurrent = new SystemSetting();
        }

        public DataTable getList()
        {
            try
            {
                object[] paraVals =  new object[] 
				{
					ObjCurrent.SystemSettingID.ToString()
,					ObjCurrent.SystemCode.ToString()
,					ObjCurrent.SegmentID.ToString()
,					ObjCurrent.UserGroupID.ToString()
,					ObjCurrent.SystemSettingUserID.ToString()
,					ObjCurrent.SystemSettingItem.ToString()
,					ObjCurrent.SystemSettingActive.ToString()
,					ObjCurrent.SystemSettingType.ToString()
,					ObjCurrent.ActiveDateFrom.ToString()
,					ObjCurrent.ActiveDateTo.ToString()
,					ObjCurrent.LastModifyDate.ToString()
,					ObjCurrent.LastModifyUser.ToString()
,					ObjCurrent.ExtraField1.ToString()
,					ObjCurrent.ExtraField2.ToString()
,					ObjCurrent.ExtraField3.ToString()

				};

                paraVals= CommonFunctions.CheckPerameterArray(paraVals);

                DataSet ds = new DataSet();
                SqlHelper.FillDataset(DBProvider.GetPerfectConnStr, "SP_SystemSetting_Select", ds, new string[] { "SystemSetting" }, paraVals);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SystemSetting getFiltedList()
        {
            try
            {
                DataSet ds =  getList().DataSet;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //-------------------------------------------------
                    //SystemSettingID   int
                    //SystemCode   string
                    //SegmentID   int
                    //UserGroupID   int
                    //SystemSettingUserID   int
                    //SystemSettingItem   string
                    //SystemSettingActive   bool?
                    //SystemSettingType   string
                    //ActiveDateFrom   DateTime
                    //ActiveDateTo   DateTime
                    //LastModifyDate   DateTime
                    //LastModifyUser   int
                    //ExtraField1   string
                    //ExtraField2   string
                    //ExtraField3   string

                    //------------------------------------------------


						ObjCurrent.SystemSettingID = (int) ds.Tables[0].Rows[0]["SystemSettingID"];
						ObjCurrent.SystemCode = (string) ds.Tables[0].Rows[0]["SystemCode"];
						ObjCurrent.SegmentID = (int) ds.Tables[0].Rows[0]["SegmentID"];
						ObjCurrent.UserGroupID = (int) ds.Tables[0].Rows[0]["UserGroupID"];
						ObjCurrent.SystemSettingUserID = (int) ds.Tables[0].Rows[0]["SystemSettingUserID"];
						ObjCurrent.SystemSettingItem = (string) ds.Tables[0].Rows[0]["SystemSettingItem"];
						ObjCurrent.SystemSettingActive = (bool?) ds.Tables[0].Rows[0]["SystemSettingActive"];
						ObjCurrent.SystemSettingType = (string) ds.Tables[0].Rows[0]["SystemSettingType"];
						ObjCurrent.ActiveDateFrom = (DateTime) ds.Tables[0].Rows[0]["ActiveDateFrom"];
						ObjCurrent.ActiveDateTo = (DateTime) ds.Tables[0].Rows[0]["ActiveDateTo"];
						ObjCurrent.LastModifyDate = (DateTime) ds.Tables[0].Rows[0]["LastModifyDate"];
						ObjCurrent.LastModifyUser = (int) ds.Tables[0].Rows[0]["LastModifyUser"];
						ObjCurrent.ExtraField1 = (string) ds.Tables[0].Rows[0]["ExtraField1"];
						ObjCurrent.ExtraField2 = (string) ds.Tables[0].Rows[0]["ExtraField2"];
						ObjCurrent.ExtraField3 = (string) ds.Tables[0].Rows[0]["ExtraField3"];
 
                
				
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
                //SystemSettingID   int(int)
                //SystemCode   string(int)
                //SegmentID   int(int)
                //UserGroupID   int(int)
                //SystemSettingUserID   int(int)
                //SystemSettingItem   string(int)
                //SystemSettingActive   bool?(int)
                //SystemSettingType   string(int)
                //ActiveDateFrom   DateTime(int)
                //ActiveDateTo   DateTime(int)
                //LastModifyDate   DateTime(int)
                //LastModifyUser   int(int)
                //ExtraField1   string(int)
                //ExtraField2   string(int)
                //ExtraField3   string(int)

            //-------------------------------------------------
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[17];

               parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;
					  
                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;
					  
                parm[2] = new SqlParameter("@SystemSettingID", SqlDbType.Int);
                parm[3] = new SqlParameter("@SystemCode", SqlDbType.VarChar);
                parm[4] = new SqlParameter("@SegmentID", SqlDbType.Int);
                parm[5] = new SqlParameter("@UserGroupID", SqlDbType.Int);
                parm[6] = new SqlParameter("@SystemSettingUserID", SqlDbType.Int);
                parm[7] = new SqlParameter("@SystemSettingItem", SqlDbType.VarChar);
                parm[8] = new SqlParameter("@SystemSettingActive", SqlDbType.Bit);
                parm[9] = new SqlParameter("@SystemSettingType", SqlDbType.VarChar);
                parm[10] = new SqlParameter("@ActiveDateFrom", SqlDbType.DateTime);
                parm[11] = new SqlParameter("@ActiveDateTo", SqlDbType.DateTime);
                parm[12] = new SqlParameter("@LastModifyDate", SqlDbType.DateTime);
                parm[13] = new SqlParameter("@LastModifyUser", SqlDbType.Int);
                parm[14] = new SqlParameter("@ExtraField1", SqlDbType.VarChar);
                parm[15] = new SqlParameter("@ExtraField2", SqlDbType.VarChar);
                parm[16] = new SqlParameter("@ExtraField3", SqlDbType.VarChar);
 

                parm[2].Value = ObjCurrent.SystemSettingID;
                parm[3].Value = ObjCurrent.SystemCode;
                parm[4].Value = ObjCurrent.SegmentID;
                parm[5].Value = ObjCurrent.UserGroupID;
                parm[6].Value = ObjCurrent.SystemSettingUserID;
                parm[7].Value = ObjCurrent.SystemSettingItem;
                parm[8].Value = ObjCurrent.SystemSettingActive;
                parm[9].Value = ObjCurrent.SystemSettingType;
                parm[10].Value = ObjCurrent.ActiveDateFrom;
                parm[11].Value = ObjCurrent.ActiveDateTo;
                parm[12].Value = ObjCurrent.LastModifyDate;
                parm[13].Value = ObjCurrent.LastModifyUser;
                parm[14].Value = ObjCurrent.ExtraField1;
                parm[15].Value = ObjCurrent.ExtraField2;
                parm[16].Value = ObjCurrent.ExtraField3;
 


                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SP_SystemSetting_INSERTUPDATE", parm);

                qryRes.RetText = parm[0].Value.ToString();
                qryRes.RetID = int.Parse(parm[1].Value.ToString());  

               
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
            }
            return qryRes;
        }

        //Done by menushan 2019-11-06
        public QueryResult UpdateEntryBulk(DataTable dataBulk)
        {
           
            QueryResult qryRes = new QueryResult();
            try
            {
               


                SqlParameter[] parm = new SqlParameter[3];

                parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;

                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;
                parm[2] = new SqlParameter("@dtUpload", dataBulk);




                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SP_SystemSettingsBulk_InsertUpdate", parm);

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
                //SystemSettingID   int(int)
                //SystemCode   string(int)
                //SegmentID   int(int)
                //UserGroupID   int(int)
                //SystemSettingUserID   int(int)
                //SystemSettingItem   string(int)
                //SystemSettingActive   bool?(int)
                //SystemSettingType   string(int)
                //ActiveDateFrom   DateTime(int)
                //ActiveDateTo   DateTime(int)
                //LastModifyDate   DateTime(int)
                //LastModifyUser   int(int)
                //ExtraField1   string(int)
                //ExtraField2   string(int)
                //ExtraField3   string(int)
 
            //-------------------------------------------------
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[17];

                parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;
					  
                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;

                parm[2] = new SqlParameter("@SystemSettingID", SqlDbType.Int);
                parm[3] = new SqlParameter("@SegmentID", SqlDbType.Int);
                parm[4] = new SqlParameter("@UserGroupID", SqlDbType.Int);
                parm[5] = new SqlParameter("@SystemSettingUserID", SqlDbType.Int);

 
                parm[2].Value = ObjCurrent.SystemSettingID;
                parm[3].Value = ObjCurrent.SegmentID;
                parm[4].Value = ObjCurrent.UserGroupID;
                parm[5].Value = ObjCurrent.SystemSettingUserID;
 

                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SP_SystemSetting_Delete", parm);

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





