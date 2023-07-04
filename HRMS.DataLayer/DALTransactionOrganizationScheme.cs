using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.Linq;
using System.Text;


namespace HRMS.DataLayer
{
    public class DALTransactionOrganizationScheme
    {
        private object docLock = new object();
        private DALTransactionOrganizationScheme instance = null;
        private TransactionOrganizationScheme ObjCurrent;
        private string strErrorMessage = "";

        private string strError;

        public string getError
        {
            get { return strError; }
        }
        

        public TransactionOrganizationScheme objCurrent
        {
            get { return ObjCurrent; }
            set { ObjCurrent = value; }
        }
        
        public class TransactionOrganizationScheme
        {
			public int SegmentID =  0;
			public string SegmentName =  "";
			public int SegmentTypeID =  0;
			public int ParentSegmentID =  0;
			public DateTime LastModifyDate =  DateTime.Now.Date;
			public int LastModifyUser =  0;
			public double SalaryBudget =  0;
			public double RecuitmentBudget =  0;
			public double TrainingBudget =  0;
			public double DiscressionaryBudget =  0;

        }

        public DALTransactionOrganizationScheme()
        {
            ObjCurrent = new TransactionOrganizationScheme();
        }

        public DataTable getList()
        {
            try
            {
                object[] paraVals =  new object[] 
				{
					ObjCurrent.SegmentID.ToString()
,					ObjCurrent.SegmentName.ToString()
,					ObjCurrent.SegmentTypeID.ToString()
,					ObjCurrent.ParentSegmentID.ToString()
,					ObjCurrent.LastModifyDate.ToString()
,					ObjCurrent.LastModifyUser.ToString()
,					ObjCurrent.SalaryBudget.ToString()
,					ObjCurrent.RecuitmentBudget.ToString()
,					ObjCurrent.TrainingBudget.ToString()
,					ObjCurrent.DiscressionaryBudget.ToString()

				};

                paraVals= CommonFunctions.CheckPerameterArray(paraVals);

                DataSet ds = new DataSet();
                SqlHelper.FillDataset(DBProvider.GetPerfectConnStr, "SP_TransactionOrganizationScheme_Select", ds, new string[] { "TransactionOrganizationScheme" }, paraVals);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TransactionOrganizationScheme getFiltedList()
        {
            try
            {
                DataSet ds =  getList().DataSet;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //-------------------------------------------------
                    //SegmentID   int
                    //SegmentName   string
                    //SegmentTypeID   int
                    //ParentSegmentID   int
                    //LastModifyDate   DateTime
                    //LastModifyUser   int
                    //SalaryBudget   double
                    //RecuitmentBudget   double
                    //TrainingBudget   double
                    //DiscressionaryBudget   double

                    //------------------------------------------------


						ObjCurrent.SegmentID = (int) ds.Tables[0].Rows[0]["SegmentID"];
						ObjCurrent.SegmentName = (string) ds.Tables[0].Rows[0]["SegmentName"];
						ObjCurrent.SegmentTypeID = (int) ds.Tables[0].Rows[0]["SegmentTypeID"];
						ObjCurrent.ParentSegmentID = (int) ds.Tables[0].Rows[0]["ParentSegmentID"];
						ObjCurrent.LastModifyDate = (DateTime) ds.Tables[0].Rows[0]["LastModifyDate"];
						ObjCurrent.LastModifyUser = (int) ds.Tables[0].Rows[0]["LastModifyUser"];
						ObjCurrent.SalaryBudget = (double) ds.Tables[0].Rows[0]["SalaryBudget"];
						ObjCurrent.RecuitmentBudget = (double) ds.Tables[0].Rows[0]["RecuitmentBudget"];
						ObjCurrent.TrainingBudget = (double) ds.Tables[0].Rows[0]["TrainingBudget"];
						ObjCurrent.DiscressionaryBudget = (double) ds.Tables[0].Rows[0]["DiscressionaryBudget"];
 
                
				
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
                //SegmentID   int(int)
                //SegmentName   string(int)
                //SegmentTypeID   int(int)
                //ParentSegmentID   int(int)
                //LastModifyDate   DateTime(int)
                //LastModifyUser   int(int)
                //SalaryBudget   double(int)
                //RecuitmentBudget   double(int)
                //TrainingBudget   double(int)
                //DiscressionaryBudget   double(int)

            //-------------------------------------------------
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[12];

               parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;
					  
                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;
					  
                parm[2] = new SqlParameter("@SegmentID", SqlDbType.Int);
                parm[3] = new SqlParameter("@SegmentName", SqlDbType.VarChar);
                parm[4] = new SqlParameter("@SegmentTypeID", SqlDbType.Int);
                parm[5] = new SqlParameter("@ParentSegmentID", SqlDbType.Int);
                parm[6] = new SqlParameter("@LastModifyDate", SqlDbType.DateTime);
                parm[7] = new SqlParameter("@LastModifyUser", SqlDbType.Int);
                parm[8] = new SqlParameter("@SalaryBudget", SqlDbType.Float);
                parm[9] = new SqlParameter("@RecuitmentBudget", SqlDbType.Float);
                parm[10] = new SqlParameter("@TrainingBudget", SqlDbType.Float);
                parm[11] = new SqlParameter("@DiscressionaryBudget", SqlDbType.Float);
 

                parm[2].Value = ObjCurrent.SegmentID;
                parm[3].Value = ObjCurrent.SegmentName;
                parm[4].Value = ObjCurrent.SegmentTypeID;
                parm[5].Value = ObjCurrent.ParentSegmentID;
                parm[6].Value = ObjCurrent.LastModifyDate;
                parm[7].Value = ObjCurrent.LastModifyUser;
                parm[8].Value = ObjCurrent.SalaryBudget;
                parm[9].Value = ObjCurrent.RecuitmentBudget;
                parm[10].Value = ObjCurrent.TrainingBudget;
                parm[11].Value = ObjCurrent.DiscressionaryBudget;
 


                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SP_TransactionOrganizationScheme_INSERTUPDATE", parm);

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
                //SegmentID   int(int)
                //SegmentName   string(int)
                //SegmentTypeID   int(int)
                //ParentSegmentID   int(int)
                //LastModifyDate   DateTime(int)
                //LastModifyUser   int(int)
                //SalaryBudget   double(int)
                //RecuitmentBudget   double(int)
                //TrainingBudget   double(int)
                //DiscressionaryBudget   double(int)
 
            //-------------------------------------------------
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[12];

                parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;
					  
                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;

                parm[2] = new SqlParameter("@SegmentID", SqlDbType.Int);

 
                parm[2].Value = ObjCurrent.SegmentID;
 

                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SP_TransactionOrganizationScheme_Delete", parm);

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





