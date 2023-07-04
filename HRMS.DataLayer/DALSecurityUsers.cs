using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.Linq;
using System.Text;


namespace HRMS.DataLayer
{
    public class DALSecurityUsers
    {
        private object docLock = new object();
        private DALSecurityUsers instance = null;
        private SecurityUsers ObjCurrent;
        private string strErrorMessage = "";

        private string strError;

        public string getError
        {
            get { return strError; }
        }
        

        public SecurityUsers objCurrent
        {
            get { return ObjCurrent; }
            set { ObjCurrent = value; }
        }
        
        public class SecurityUsers
        {
			public int UserID =  0;
			public int SegmentID =  0;
			public string UserName =  "";
			public string UserActualName =  "";
			public string Password =  "";
			public string BadgeNo =  "";
			public int EmployeeNo =  0;
			public string SegmentFilter =  "";
			public string DesignationsFilters =  "";
			public string EmployeeLevelsFilters =  "";
			public string EmployeeAntiFilter =  "";
			public string SectionFilter =  "";
			public string CategoryFilter =  "";
			public string LocationFilter =  "";
			public string OrientationFilter =  "";
			public string GenderFilter =  "";
			public string TransportMethodFilter =  "";
			public DateTime LastModifyDate =  DateTime.Now.Date;
			public int LastModifyUser =  0;
            public int LastInsertID = 0;

            //edit by dinesh
            public bool ResetPassword = false;
            public DateTime PasswordResetDate = DateTime.Now.Date;          
            public int PasswordResetBy = 0;
            public int UserGroupID = 0;
            public bool? IsDeactivate = false;
            

        }

        public DALSecurityUsers()
        {
            ObjCurrent = new SecurityUsers();
        }

        public DataTable getList()
        {
            try
            {
                object[] paraVals =  new object[] 
				{
					ObjCurrent.UserID.ToString()
,					ObjCurrent.SegmentID.ToString()
,					ObjCurrent.UserName.ToString()
,					ObjCurrent.UserActualName.ToString()
,					ObjCurrent.Password.ToString()
,					ObjCurrent.BadgeNo.ToString()
,					ObjCurrent.EmployeeNo.ToString()
,					ObjCurrent.SegmentFilter.ToString()
,					ObjCurrent.DesignationsFilters.ToString()
,					ObjCurrent.EmployeeLevelsFilters.ToString()
,					ObjCurrent.EmployeeAntiFilter.ToString()
,					ObjCurrent.SectionFilter.ToString()
,					ObjCurrent.CategoryFilter.ToString()
,					ObjCurrent.LocationFilter.ToString()
,					ObjCurrent.OrientationFilter.ToString()
,					ObjCurrent.GenderFilter.ToString()
,					ObjCurrent.TransportMethodFilter.ToString()
,					ObjCurrent.LastModifyDate.ToString()
,					ObjCurrent.LastModifyUser.ToString()              


				};

                paraVals= CommonFunctions.CheckPerameterArray(paraVals);

                DataSet ds = new DataSet();
                SqlHelper.FillDataset(DBProvider.GetPerfectConnStr, "SP_SecurityUsers_Select", ds, new string[] { "SecurityUsers" }, paraVals);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SecurityUsers getFiltedList()
        {
            try
            {
                DataSet ds =  getList().DataSet;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //-------------------------------------------------
                    //UserID   int
                    //SegmentID   int
                    //UserName   string
                    //UserActualName   string
                    //Password   string
                    //BadgeNo   string
                    //EmployeeNo   int
                    //SegmentFilter   string
                    //DesignationsFilters   string
                    //EmployeeLevelsFilters   string
                    //EmployeeAntiFilter   string
                    //SectionFilter   string
                    //CategoryFilter   string
                    //LocationFilter   string
                    //OrientationFilter   string
                    //GenderFilter   string
                    //TransportMethodFilter   string
                    //LastModifyDate   DateTime
                    //LastModifyUser   int

                    //------------------------------------------------


						ObjCurrent.UserID = (int) ds.Tables[0].Rows[0]["UserID"];
						ObjCurrent.SegmentID = (int) ds.Tables[0].Rows[0]["SegmentID"];
						ObjCurrent.UserName = (string) ds.Tables[0].Rows[0]["UserName"];
						ObjCurrent.UserActualName = (string) ds.Tables[0].Rows[0]["UserActualName"];
						ObjCurrent.Password = (string) ds.Tables[0].Rows[0]["Password"];
						ObjCurrent.BadgeNo = (string) ds.Tables[0].Rows[0]["BadgeNo"];
						ObjCurrent.EmployeeNo = (int) ds.Tables[0].Rows[0]["EmployeeNo"];
						ObjCurrent.SegmentFilter = (string) ds.Tables[0].Rows[0]["SegmentFilter"];
						ObjCurrent.DesignationsFilters = (string) ds.Tables[0].Rows[0]["DesignationsFilters"];
						ObjCurrent.EmployeeLevelsFilters = (string) ds.Tables[0].Rows[0]["EmployeeLevelsFilters"];
						ObjCurrent.EmployeeAntiFilter = (string) ds.Tables[0].Rows[0]["EmployeeAntiFilter"];
						ObjCurrent.SectionFilter = (string) ds.Tables[0].Rows[0]["SectionFilter"];
						ObjCurrent.CategoryFilter = (string) ds.Tables[0].Rows[0]["CategoryFilter"];
						ObjCurrent.LocationFilter = (string) ds.Tables[0].Rows[0]["LocationFilter"];
						ObjCurrent.OrientationFilter = (string) ds.Tables[0].Rows[0]["OrientationFilter"];
						ObjCurrent.GenderFilter = (string) ds.Tables[0].Rows[0]["GenderFilter"];
						ObjCurrent.TransportMethodFilter = (string) ds.Tables[0].Rows[0]["TransportMethodFilter"];
						ObjCurrent.LastModifyDate = (DateTime) ds.Tables[0].Rows[0]["LastModifyDate"];
						ObjCurrent.LastModifyUser = (int) ds.Tables[0].Rows[0]["LastModifyUser"];
                        ObjCurrent.LastInsertID = (int)ds.Tables[0].Rows[0]["LastInsertID"];

                    //edit By Dinesh 2017-07-04
                        ObjCurrent.ResetPassword = (bool)ds.Tables[0].Rows[0]["ResetPassword"];
                        ObjCurrent.PasswordResetDate = (DateTime)ds.Tables[0].Rows[0]["PasswordResetDate"];
                        ObjCurrent.PasswordResetBy = (int)ds.Tables[0].Rows[0]["PasswordResetBy"];
                        ObjCurrent.UserGroupID = (int)ds.Tables[0].Rows[0]["UserGroupID"];
                        ObjCurrent.IsDeactivate = (bool)ds.Tables[0].Rows[0]["IsDeactivate"];


 
                
				
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
                //UserID   int(int)
                //SegmentID   int(int)
                //UserName   string(int)
                //UserActualName   string(int)
                //Password   string(int)
                //BadgeNo   string(int)
                //EmployeeNo   int(int)
                //SegmentFilter   string(int)
                //DesignationsFilters   string(int)
                //EmployeeLevelsFilters   string(int)
                //EmployeeAntiFilter   string(int)
                //SectionFilter   string(int)
                //CategoryFilter   string(int)
                //LocationFilter   string(int)
                //OrientationFilter   string(int)
                //GenderFilter   string(int)
                //TransportMethodFilter   string(int)
                //LastModifyDate   DateTime(int)
                //LastModifyUser   int(int)

            //-------------------------------------------------
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[26];

               parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;
					  
                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;
					  
                parm[2] = new SqlParameter("@UserID", SqlDbType.Int);
                parm[3] = new SqlParameter("@SegmentID", SqlDbType.Int);
                parm[4] = new SqlParameter("@UserName", SqlDbType.VarChar);
                parm[5] = new SqlParameter("@UserActualName", SqlDbType.VarChar);
                parm[6] = new SqlParameter("@Password", SqlDbType.VarChar);
                parm[7] = new SqlParameter("@BadgeNo", SqlDbType.VarChar);
                parm[8] = new SqlParameter("@EmployeeNo", SqlDbType.Int);
                parm[9] = new SqlParameter("@SegmentFilter", SqlDbType.VarChar);
                parm[10] = new SqlParameter("@DesignationsFilters", SqlDbType.VarChar);
                parm[11] = new SqlParameter("@EmployeeLevelsFilters", SqlDbType.VarChar);
                parm[12] = new SqlParameter("@EmployeeAntiFilter", SqlDbType.VarChar);
                parm[13] = new SqlParameter("@SectionFilter", SqlDbType.VarChar);
                parm[14] = new SqlParameter("@CategoryFilter", SqlDbType.VarChar);
                parm[15] = new SqlParameter("@LocationFilter", SqlDbType.VarChar);
                parm[16] = new SqlParameter("@OrientationFilter", SqlDbType.VarChar);
                parm[17] = new SqlParameter("@GenderFilter", SqlDbType.VarChar);
                parm[18] = new SqlParameter("@TransportMethodFilter", SqlDbType.VarChar);
                parm[19] = new SqlParameter("@LastModifyDate", SqlDbType.DateTime);
                parm[20] = new SqlParameter("@LastModifyUser", SqlDbType.Int);

                //edit by dinesh 2017-07-07
                parm[21] = new SqlParameter("@ResetPassword", SqlDbType.Bit);
                parm[22] = new SqlParameter("@PasswordResetDate", SqlDbType.DateTime);
                parm[23] = new SqlParameter("@PasswordResetBy", SqlDbType.Int);
                parm[24] = new SqlParameter("@UserGroupID", SqlDbType.Int);
                parm[25] = new SqlParameter("@IsDeactivate", SqlDbType.Bit);
                
 

                parm[2].Value = ObjCurrent.UserID;
                parm[3].Value = ObjCurrent.SegmentID;
                parm[4].Value = ObjCurrent.UserName;
                parm[5].Value = ObjCurrent.UserActualName;
                parm[6].Value = ObjCurrent.Password;
                parm[7].Value = ObjCurrent.BadgeNo;
                parm[8].Value = ObjCurrent.EmployeeNo;
                parm[9].Value = ObjCurrent.SegmentFilter;
                parm[10].Value = ObjCurrent.DesignationsFilters;
                parm[11].Value = ObjCurrent.EmployeeLevelsFilters;
                parm[12].Value = ObjCurrent.EmployeeAntiFilter;
                parm[13].Value = ObjCurrent.SectionFilter;
                parm[14].Value = ObjCurrent.CategoryFilter;
                parm[15].Value = ObjCurrent.LocationFilter;
                parm[16].Value = ObjCurrent.OrientationFilter;
                parm[17].Value = ObjCurrent.GenderFilter;
                parm[18].Value = ObjCurrent.TransportMethodFilter;
                parm[19].Value = ObjCurrent.LastModifyDate;
                parm[20].Value = ObjCurrent.LastModifyUser;

                //edit by dinesh 2017-07-07
                parm[21].Value = ObjCurrent.ResetPassword;
                parm[22].Value = ObjCurrent.PasswordResetDate;
                parm[23].Value = ObjCurrent.PasswordResetBy;
                parm[24].Value = ObjCurrent.UserGroupID;
                parm[25].Value = ObjCurrent.IsDeactivate;


                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SP_SecurityUsers_INSERTUPDATE", parm);

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
                //UserID   int(int)
                //SegmentID   int(int)
                //UserName   string(int)
                //UserActualName   string(int)
                //Password   string(int)
                //BadgeNo   string(int)
                //EmployeeNo   int(int)
                //SegmentFilter   string(int)
                //DesignationsFilters   string(int)
                //EmployeeLevelsFilters   string(int)
                //EmployeeAntiFilter   string(int)
                //SectionFilter   string(int)
                //CategoryFilter   string(int)
                //LocationFilter   string(int)
                //OrientationFilter   string(int)
                //GenderFilter   string(int)
                //TransportMethodFilter   string(int)
                //LastModifyDate   DateTime(int)
                //LastModifyUser   int(int)
 
            //-------------------------------------------------
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[21];

                parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;
					  
                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;

                parm[2] = new SqlParameter("@UserID", SqlDbType.Int);
                parm[3] = new SqlParameter("@SegmentID", SqlDbType.Int);

 
                parm[2].Value = ObjCurrent.UserID;
                parm[3].Value = ObjCurrent.SegmentID;
 

                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SP_SecurityUsers_Delete", parm);

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





