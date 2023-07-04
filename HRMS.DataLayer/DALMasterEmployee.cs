using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.Linq;
using System.Text;


namespace HRMS.DataLayer
{
    public class DALMasterEmployee
    {
        private object docLock = new object();
        private DALMasterEmployee instance = null;
        private MasterEmployee ObjCurrent;
        private string strErrorMessage = "";

        private string strError;

        public string getError
        {
            get { return strError; }
        }


        public MasterEmployee objCurrent
        {
            get { return ObjCurrent; }
            set { ObjCurrent = value; }
        }

        public class MasterEmployee
        {
            public int SegmentID = 0;
            public int DesignationID = 0;
            public int EmployeeNo = 0;
            public int LevelID = 0;
            public int EmpSegmentID = 0;
            public string DocumentEmployeeNo = "";
            public string NIC = "";
            public string BadgeNo = "";
            public string FirstName = "";
            public string MiddleName = "";
            public string LastName = "";
            public string OtherNames = "";
            public string Initials = "";
            public int GenderID = 0;
            public int OrientationID = 0;
            public int RaceID = 0;
            public DateTime DOB = DateTime.Now.Date;
            public int CountryID = 0;
            public DateTime DateJoined = DateTime.Now.Date;
            public DateTime DateLeftOrganization = DateTime.Now.Date;
            public string JobTitle = "";
            public int ElectrorateID = 0;
            public bool? IsCitizen = true;
            public string BloodType = "";
            public int MedicalCondtionsID = 0;
            public string PaySlipName = "";
            public string TaxationName = "";
            public string PassportNo = "";
            public string PaidCurrencyCode = "";
            public int PaySchemeID = 0;
            public string TaxFileNo = "";
            public string EPFNo = "";
            public string KnowMedicalCondtions = "";
            public double BasicSalary = 0;
            public int PreferedTransportMethodID = 0;
            public int ShiftID = 0;
            public int CategoryID = 0;
            public int SectionID = 0;
            public int TaxTypeID = 0;
            public string ETFNo = "";
            public int BankID = 0;
            public int PayProcessed = 0;
            public int PayCycleID = 0;
            public int AttendanceSummeryID = 0;
            public int WorkingHourScheduleID = 0;
            public int LeaveSchemeID = 0;
            public int PerformaneBonus = 0;
            public int EmployeePaymentBreakdownID = 0;
            public int ClaimSchemeID = 0;
            public bool? EmployeeRetired = true;
            public bool? EmployeeConfirmed = true;
            public string EISPassword = "";
            public int LastModifyUser = 0;
            public DateTime LastModifyDate = DateTime.Now.Date;
            public double ServiceePercentage = 0;
            public DateTime PassportExpireDate = DateTime.Now.Date;
            public int LanguageID = 0;
            public DateTime EmployeeConfirmedDate = DateTime.Now.Date;
            public int LocationID = 0;
            public string JobSpecification = "";
            public int Title = -1;
            public double Increment = 0;
            public bool? IsActive = true;
            public int WorkingCalenderSchemeID = 0;
            public int UserID = -1;
            public bool? IsIncrementApplyingAfter = true;
            public DateTime IncrementDateFrom = DateTime.Now.Date;
            public int AttendanceSchemeID = 0;
            public int ExtraInt1 = 0;
            public int ExtraInt2 = 0;
            public int ExtraInt3 = 0;
            public bool? ExtraBool1 = true;
            public bool? ExtraBool2 = true;
            public bool? ExtraBool3 = true;
            public string ExtraText1 = "";
            public string ExtraText2 = "";
            public string ExtraText3 = "";
            public bool? IsContractEmployee = true;
            public DateTime InactiveDate = DateTime.Now;
            public string LeftReason = ""; //TharakaM 2020.04.16
            public bool? EPFEntitlement = true; //TharakaM 2020.04.17
            public int CivilStatusId = 0; //TharakaM 2020.04.21
            public int ReligionID = 0; //TharakaM 2020.04.21
            public DateTime DateRetired = DateTime.Now.Date; //TharakaM 2020.05.22
            public bool? IsRetired = true; //TharakaM 2020.05.22
            public string RetiredYear = ""; //TharakaM 2020.05.26
            public int ProjectDesignationID = 0;
            public int ProjectSkillID = 0;
            public int ProjectJobTypeID = 0;
            public int GradeID = 0;
            public string NID = "";
            public int ExtraInt4 = 0;
            public int ExtraInt5 = 0;
            public int ExtraInt6 = 0;
            public bool? ExtraBool4 = true;
            public bool? ExtraBool5 = true;
            public bool? ExtraBool6 = true;
            public string ExtraText4 = "";
            public string ExtraText5 = "";
            public string ExtraText6 = "";
            public DateTime ExtraDate1 = DateTime.Now;
            public DateTime ExtraDate2 = DateTime.Now;
            public DateTime ExtraDate3 = DateTime.Now;
            public DateTime ExtraDate4 = DateTime.Now;
            public DateTime ExtraDate5 = DateTime.Now;
            public DateTime ExtraDate6 = DateTime.Now;
            public double ExtraFloat1 = -1;
            public double ExtraFloat2 = -1;
            public double ExtraFloat3 = -1;
            public double ExtraFloat4 = -1;
            public double ExtraFloat5 = -1;
            public double ExtraFloat6 = -1;
            public DateTime ContractStartDate = DateTime.Now;
            public DateTime ContractEndDate = DateTime.Now;
        }

        public DALMasterEmployee()
        {
            ObjCurrent = new MasterEmployee();
        }

        public DataTable getList()
        {
            try
            {
                object[] paraVals = new object[] 
				{
                    ObjCurrent.UserID.ToString()
,					ObjCurrent.SegmentID.ToString()
,					ObjCurrent.DesignationID.ToString()
,					ObjCurrent.EmployeeNo.ToString()
,					ObjCurrent.LevelID.ToString()
,					ObjCurrent.EmpSegmentID.ToString()
,					ObjCurrent.DocumentEmployeeNo.ToString()
,					ObjCurrent.NIC.ToString()
,					ObjCurrent.BadgeNo.ToString()
,					ObjCurrent.FirstName.ToString()
,					ObjCurrent.MiddleName.ToString()
,					ObjCurrent.LastName.ToString()
,					ObjCurrent.OtherNames.ToString()
,					ObjCurrent.Initials.ToString()
,					ObjCurrent.GenderID.ToString()
,					ObjCurrent.OrientationID.ToString()
,					ObjCurrent.RaceID.ToString()
,					ObjCurrent.DOB.ToString()
,					ObjCurrent.CountryID.ToString()
,					ObjCurrent.DateJoined.ToString()
,					ObjCurrent.DateLeftOrganization.ToString()
,					ObjCurrent.JobTitle.ToString()
,					ObjCurrent.ElectrorateID.ToString()
,					ObjCurrent.IsCitizen.ToString()
,					ObjCurrent.BloodType.ToString()
,					ObjCurrent.MedicalCondtionsID.ToString()
,					ObjCurrent.PaySlipName.ToString()
,					ObjCurrent.TaxationName.ToString()
,					ObjCurrent.PassportNo.ToString()
,					ObjCurrent.PaidCurrencyCode.ToString()
,					ObjCurrent.PaySchemeID.ToString()
,					ObjCurrent.TaxFileNo.ToString()
,					ObjCurrent.EPFNo.ToString()
,					ObjCurrent.KnowMedicalCondtions.ToString()
,					ObjCurrent.BasicSalary.ToString()
,					ObjCurrent.PreferedTransportMethodID.ToString()
,					ObjCurrent.ShiftID.ToString()
,					ObjCurrent.CategoryID.ToString()
,					ObjCurrent.SectionID.ToString()
,					ObjCurrent.TaxTypeID.ToString()
,					ObjCurrent.ETFNo.ToString()
,					ObjCurrent.BankID.ToString()
,					ObjCurrent.PayProcessed.ToString()
,					ObjCurrent.PayCycleID.ToString()
,					ObjCurrent.AttendanceSummeryID.ToString()
,					ObjCurrent.WorkingHourScheduleID.ToString()
,					ObjCurrent.LeaveSchemeID.ToString()
,					ObjCurrent.PerformaneBonus.ToString()
,					ObjCurrent.EmployeePaymentBreakdownID.ToString()
,					ObjCurrent.ClaimSchemeID.ToString()
,					ObjCurrent.EmployeeRetired.ToString()
,					ObjCurrent.EmployeeConfirmed.ToString()
,					ObjCurrent.EISPassword.ToString()
,					ObjCurrent.LastModifyUser.ToString()
,					ObjCurrent.LastModifyDate.ToString()
,					ObjCurrent.ServiceePercentage.ToString()
,					ObjCurrent.PassportExpireDate.ToString()
,					ObjCurrent.LanguageID.ToString()
,					ObjCurrent.EmployeeConfirmedDate.ToString()
,					ObjCurrent.LocationID.ToString()
,					ObjCurrent.JobSpecification.ToString()
,                   ObjCurrent.Title.ToString()
,                   ObjCurrent.Increment.ToString()
,                   ObjCurrent.IsActive.ToString()
,                   ObjCurrent.WorkingCalenderSchemeID.ToString()
,                   ObjCurrent.IsIncrementApplyingAfter.ToString()
,                   ObjCurrent.IncrementDateFrom.ToString()
,					ObjCurrent.AttendanceSchemeID.ToString()
,					ObjCurrent.ExtraInt1.ToString()
,					ObjCurrent.ExtraInt2.ToString()
,					ObjCurrent.ExtraInt3.ToString()
,					ObjCurrent.ExtraBool1.ToString()
,					ObjCurrent.ExtraBool2.ToString()
,					ObjCurrent.ExtraBool3.ToString()
,					ObjCurrent.ExtraText1.ToString()
,					ObjCurrent.ExtraText2.ToString()
,					ObjCurrent.ExtraText3.ToString()
,                   ObjCurrent.IsContractEmployee.ToString()//Tharaka 2018/09/06
,                   ObjCurrent.InactiveDate.ToString()
,                   ObjCurrent.LeftReason.ToString() //TharakaM 2020.04.16
,                   ObjCurrent.EPFEntitlement.ToString() //Tharaka 2020.04.17
,                   ObjCurrent.CivilStatusId.ToString() //TharakaM 2020.04.21
,                   ObjCurrent.ReligionID.ToString() //TharakaM 2020.04.21
,                   ObjCurrent.DateRetired.ToString() //TharakaM 2020.05.22
,                   ObjCurrent.IsRetired.ToString() //TharakaM 2020.05.22
,                   ObjCurrent.RetiredYear.ToString() //TharkaM 2020.05.26
,                   ObjCurrent.ProjectDesignationID.ToString()
,                   ObjCurrent.ProjectSkillID.ToString()
,                   ObjCurrent.ProjectJobTypeID.ToString()
,                   ObjCurrent.GradeID.ToString()
,                   ObjCurrent.NID.ToString()
,                   ObjCurrent.ExtraInt4.ToString()
,                   ObjCurrent.ExtraInt5.ToString()
,                   ObjCurrent.ExtraInt6.ToString()
,                   ObjCurrent.ExtraBool4.ToString()
,                   ObjCurrent.ExtraBool5.ToString()
,                   ObjCurrent.ExtraBool6.ToString()
,                   ObjCurrent.ExtraText4.ToString()
,                   ObjCurrent.ExtraText5.ToString()
,                   ObjCurrent.ExtraText6.ToString()
,                   ObjCurrent.ExtraDate1.ToString()
,                   ObjCurrent.ExtraDate2.ToString()
,                   ObjCurrent.ExtraDate3.ToString()
,                   ObjCurrent.ExtraDate4.ToString()
,                   ObjCurrent.ExtraDate5.ToString()
,                   ObjCurrent.ExtraDate6.ToString()
,                   ObjCurrent.ExtraFloat1.ToString()
,                   ObjCurrent.ExtraFloat2.ToString()
,                   ObjCurrent.ExtraFloat3.ToString()
,                   ObjCurrent.ExtraFloat4.ToString()
,                   ObjCurrent.ExtraFloat5.ToString()
,                   ObjCurrent.ExtraFloat6.ToString()
,                   ObjCurrent.ContractStartDate.ToString()
,                   ObjCurrent.ContractEndDate.ToString()
                };

                paraVals = CommonFunctions.CheckPerameterArray(paraVals);

                DataSet ds = new DataSet();
                SqlHelper.FillDataset(DBProvider.GetPerfectConnStr, "SP_MasterEmployee_Select", ds, new string[] { "MasterEmployee" }, paraVals);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getListApprisal()
        {
            try
            {
                object[] paraVals = new object[]
                {
                    ObjCurrent.UserID.ToString()
,                   ObjCurrent.SegmentID.ToString()
,                   ObjCurrent.DesignationID.ToString()
,                   ObjCurrent.EmployeeNo.ToString()
,                   ObjCurrent.LevelID.ToString()
,                   ObjCurrent.EmpSegmentID.ToString()
,                   ObjCurrent.DocumentEmployeeNo.ToString()
,                   ObjCurrent.NIC.ToString()
,                   ObjCurrent.BadgeNo.ToString()
,                   ObjCurrent.FirstName.ToString()
,                   ObjCurrent.MiddleName.ToString()
,                   ObjCurrent.LastName.ToString()
,                   ObjCurrent.OtherNames.ToString()
,                   ObjCurrent.Initials.ToString()
,                   ObjCurrent.GenderID.ToString()
,                   ObjCurrent.OrientationID.ToString()
,                   ObjCurrent.RaceID.ToString()
,                   ObjCurrent.DOB.ToString()
,                   ObjCurrent.CountryID.ToString()
,                   ObjCurrent.DateJoined.ToString()
,                   ObjCurrent.DateLeftOrganization.ToString()
,                   ObjCurrent.JobTitle.ToString()
,                   ObjCurrent.ElectrorateID.ToString()
,                   ObjCurrent.IsCitizen.ToString()
,                   ObjCurrent.BloodType.ToString()
,                   ObjCurrent.MedicalCondtionsID.ToString()
,                   ObjCurrent.PaySlipName.ToString()
,                   ObjCurrent.TaxationName.ToString()
,                   ObjCurrent.PassportNo.ToString()
,                   ObjCurrent.PaidCurrencyCode.ToString()
,                   ObjCurrent.PaySchemeID.ToString()
,                   ObjCurrent.TaxFileNo.ToString()
,                   ObjCurrent.EPFNo.ToString()
,                   ObjCurrent.KnowMedicalCondtions.ToString()
,                   ObjCurrent.BasicSalary.ToString()
,                   ObjCurrent.PreferedTransportMethodID.ToString()
,                   ObjCurrent.ShiftID.ToString()
,                   ObjCurrent.CategoryID.ToString()
,                   ObjCurrent.SectionID.ToString()
,                   ObjCurrent.TaxTypeID.ToString()
,                   ObjCurrent.ETFNo.ToString()
,                   ObjCurrent.BankID.ToString()
,                   ObjCurrent.PayProcessed.ToString()
,                   ObjCurrent.PayCycleID.ToString()
,                   ObjCurrent.AttendanceSummeryID.ToString()
,                   ObjCurrent.WorkingHourScheduleID.ToString()
,                   ObjCurrent.LeaveSchemeID.ToString()
,                   ObjCurrent.PerformaneBonus.ToString()
,                   ObjCurrent.EmployeePaymentBreakdownID.ToString()
,                   ObjCurrent.ClaimSchemeID.ToString()
,                   ObjCurrent.EmployeeRetired.ToString()
,                   ObjCurrent.EmployeeConfirmed.ToString()
,                   ObjCurrent.EISPassword.ToString()
,                   ObjCurrent.LastModifyUser.ToString()
,                   ObjCurrent.LastModifyDate.ToString()
,                   ObjCurrent.ServiceePercentage.ToString()
,                   ObjCurrent.PassportExpireDate.ToString()
,                   ObjCurrent.LanguageID.ToString()
,                   ObjCurrent.EmployeeConfirmedDate.ToString()
,                   ObjCurrent.LocationID.ToString()
,                   ObjCurrent.JobSpecification.ToString()
,                   ObjCurrent.Title.ToString()
,                   ObjCurrent.Increment.ToString()
,                   ObjCurrent.IsActive.ToString()
,                   ObjCurrent.WorkingCalenderSchemeID.ToString()
,                   ObjCurrent.IsIncrementApplyingAfter.ToString()
,                   ObjCurrent.IncrementDateFrom.ToString()
,                   ObjCurrent.AttendanceSchemeID.ToString()
,                   ObjCurrent.ExtraInt1.ToString()
,                   ObjCurrent.ExtraInt2.ToString()
,                   ObjCurrent.ExtraInt3.ToString()
,                   ObjCurrent.ExtraBool1.ToString()
,                   ObjCurrent.ExtraBool2.ToString()
,                   ObjCurrent.ExtraBool3.ToString()
,                   ObjCurrent.ExtraText1.ToString()
,                   ObjCurrent.ExtraText2.ToString()
,                   ObjCurrent.ExtraText3.ToString()
,                   ObjCurrent.IsContractEmployee.ToString()//Tharaka 2018/09/06
,                   ObjCurrent.InactiveDate.ToString()
,                   ObjCurrent.LeftReason.ToString() //TharakaM 2020.04.16
,                   ObjCurrent.EPFEntitlement.ToString() //Tharaka 2020.04.17
,                   ObjCurrent.CivilStatusId.ToString() //TharakaM 2020.04.21
,                   ObjCurrent.ReligionID.ToString() //TharakaM 2020.04.21
,                   ObjCurrent.DateRetired.ToString() //TharakaM 2020.05.22
,                   ObjCurrent.IsRetired.ToString() //TharakaM 2020.05.22
,                   ObjCurrent.RetiredYear.ToString() //TharkaM 2020.05.26
,                   ObjCurrent.ProjectDesignationID.ToString()
,                   ObjCurrent.ProjectSkillID.ToString()
,                   ObjCurrent.ProjectJobTypeID.ToString()
,                   ObjCurrent.GradeID.ToString()
,                   ObjCurrent.NID.ToString()
,                   ObjCurrent.ExtraInt4.ToString()
,                   ObjCurrent.ExtraInt5.ToString()
,                   ObjCurrent.ExtraInt6.ToString()
,                   ObjCurrent.ExtraBool4.ToString()
,                   ObjCurrent.ExtraBool5.ToString()
,                   ObjCurrent.ExtraBool6.ToString()
,                   ObjCurrent.ExtraText4.ToString()
,                   ObjCurrent.ExtraText5.ToString()
,                   ObjCurrent.ExtraText6.ToString()
,                   ObjCurrent.ExtraDate1.ToString()
,                   ObjCurrent.ExtraDate2.ToString()
,                   ObjCurrent.ExtraDate3.ToString()
,                   ObjCurrent.ExtraDate4.ToString()
,                   ObjCurrent.ExtraDate5.ToString()
,                   ObjCurrent.ExtraDate6.ToString()
,                   ObjCurrent.ExtraFloat1.ToString()
,                   ObjCurrent.ExtraFloat2.ToString()
,                   ObjCurrent.ExtraFloat3.ToString()
,                   ObjCurrent.ExtraFloat4.ToString()
,                   ObjCurrent.ExtraFloat5.ToString()
,                   ObjCurrent.ExtraFloat6.ToString()
,                   ObjCurrent.ContractStartDate.ToString()
,                   ObjCurrent.ContractEndDate.ToString()
                };

                paraVals = CommonFunctions.CheckPerameterArray(paraVals);

                DataSet ds = new DataSet();
                SqlHelper.FillDataset(DBProvider.GetPerfectConnStr, "SP_MasterEmployee_Select", ds, new string[] { "MasterEmployee" }, paraVals);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getListByEmployeeList(int SegmentIDP,DataTable EmplTable)
        {//Added By Thus On 05Sep2019
            try
            {
                DataSet ds = new DataSet();

                using (SqlConnection sqlCon = new SqlConnection(DBProvider.GetPerfectConnStr))
                {
                    using (SqlCommand sqlCom = new SqlCommand())
                    {
                        SqlParameter[] paraVals = new SqlParameter[2];

                        paraVals[0] = new SqlParameter("@SegmentID", SegmentIDP);
                        paraVals[1] = new SqlParameter("@EmployeeList", EmplTable);
                    
                        sqlCon.Open();

                        sqlCom.Connection = sqlCon;
                        sqlCom.CommandType = CommandType.StoredProcedure;
                        sqlCom.CommandText = "SPC_MasterEmployeeSelect_EmployeeList";
                        sqlCom.Parameters.AddRange(paraVals);

                        using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCom))
                        {                            
                            sqlAdapter.Fill(ds);
                        }
                    }                
                }

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getLimitedList()
        {
            try
            {
                object[] paraVals = new object[] 
				{
                    ObjCurrent.UserID.ToString()
,					ObjCurrent.SegmentID.ToString()
,					ObjCurrent.DesignationID.ToString()
,					ObjCurrent.EmployeeNo.ToString()
,					ObjCurrent.LevelID.ToString()
,					ObjCurrent.EmpSegmentID.ToString()
,					ObjCurrent.DocumentEmployeeNo.ToString()
,					ObjCurrent.NIC.ToString()
,					ObjCurrent.BadgeNo.ToString()
,					ObjCurrent.FirstName.ToString()
,					ObjCurrent.MiddleName.ToString()
,					ObjCurrent.LastName.ToString()
,					ObjCurrent.OtherNames.ToString()
,					ObjCurrent.Initials.ToString()
,					ObjCurrent.GenderID.ToString()
,					ObjCurrent.OrientationID.ToString()
,					ObjCurrent.RaceID.ToString()
,					ObjCurrent.DOB.ToString()
,					ObjCurrent.CountryID.ToString()
,					ObjCurrent.DateJoined.ToString()
,					ObjCurrent.DateLeftOrganization.ToString()
,					ObjCurrent.JobTitle.ToString()
,					ObjCurrent.ElectrorateID.ToString()
,					ObjCurrent.IsCitizen.ToString()
,					ObjCurrent.BloodType.ToString()
,					ObjCurrent.MedicalCondtionsID.ToString()
,					ObjCurrent.PaySlipName.ToString()
,					ObjCurrent.TaxationName.ToString()
,					ObjCurrent.PassportNo.ToString()
,					ObjCurrent.PaidCurrencyCode.ToString()
,					ObjCurrent.PaySchemeID.ToString()
,					ObjCurrent.TaxFileNo.ToString()
,					ObjCurrent.EPFNo.ToString()
,					ObjCurrent.KnowMedicalCondtions.ToString()
,					ObjCurrent.BasicSalary.ToString()
,					ObjCurrent.PreferedTransportMethodID.ToString()
,					ObjCurrent.ShiftID.ToString()
,					ObjCurrent.CategoryID.ToString()
,					ObjCurrent.SectionID.ToString()
,					ObjCurrent.TaxTypeID.ToString()
,					ObjCurrent.ETFNo.ToString()
,					ObjCurrent.BankID.ToString()
,					ObjCurrent.PayProcessed.ToString()
,					ObjCurrent.PayCycleID.ToString()
,					ObjCurrent.AttendanceSummeryID.ToString()
,					ObjCurrent.WorkingHourScheduleID.ToString()
,					ObjCurrent.LeaveSchemeID.ToString()
,					ObjCurrent.PerformaneBonus.ToString()
,					ObjCurrent.EmployeePaymentBreakdownID.ToString()
,					ObjCurrent.ClaimSchemeID.ToString()
,					ObjCurrent.EmployeeRetired.ToString()
,					ObjCurrent.EmployeeConfirmed.ToString()
,					ObjCurrent.EISPassword.ToString()
,					ObjCurrent.LastModifyUser.ToString()
,					ObjCurrent.LastModifyDate.ToString()
,					ObjCurrent.ServiceePercentage.ToString()
,					ObjCurrent.PassportExpireDate.ToString()
,					ObjCurrent.LanguageID.ToString()
,					ObjCurrent.EmployeeConfirmedDate.ToString()
,					ObjCurrent.LocationID.ToString()
,					ObjCurrent.JobSpecification.ToString()
,                   ObjCurrent.Title.ToString()
,                   ObjCurrent.Increment.ToString()
,                   ObjCurrent.IsActive.ToString()
,                   ObjCurrent.WorkingCalenderSchemeID.ToString()
,                   ObjCurrent.IsIncrementApplyingAfter.ToString()
,                   ObjCurrent.IncrementDateFrom.ToString()
,					ObjCurrent.AttendanceSchemeID.ToString()
,					ObjCurrent.ExtraInt1.ToString()
,					ObjCurrent.ExtraInt2.ToString()
,					ObjCurrent.ExtraInt3.ToString()
,					ObjCurrent.ExtraBool1.ToString()
,					ObjCurrent.ExtraBool2.ToString()
,					ObjCurrent.ExtraBool3.ToString()
,					ObjCurrent.ExtraText1.ToString()
,					ObjCurrent.ExtraText2.ToString()
,					ObjCurrent.ExtraText3.ToString()
,                   ObjCurrent.IsContractEmployee.ToString()
,                   ObjCurrent.InactiveDate.ToString()
,                   ObjCurrent.LeftReason.ToString() //TharakaM 2020.04.16
,                   ObjCurrent.EPFEntitlement.ToString() //TharakaM 2020.04.17
,                   ObjCurrent.CivilStatusId.ToString() //TharakaM 2020.04.21
,                   ObjCurrent.ReligionID.ToString() //TharakaM 2020.04.21
 , ObjCurrent.DateRetired.ToString() //TharakaM 2020.05.22
,                   ObjCurrent.IsRetired.ToString() //TharakaM 2020.05.22
,                   ObjCurrent.RetiredYear.ToString() //TharakaM 2020.05.26
,                   ObjCurrent.ProjectDesignationID.ToString()
,                   ObjCurrent.ProjectSkillID.ToString()
,                   ObjCurrent.ProjectJobTypeID.ToString()
,                   ObjCurrent.GradeID.ToString()
	,                   ObjCurrent.NID.ToString()
    ,                   ObjCurrent.ExtraInt4.ToString()
,                   ObjCurrent.ExtraInt5.ToString()
,                   ObjCurrent.ExtraInt6.ToString()
,                   ObjCurrent.ExtraBool4.ToString()
,                   ObjCurrent.ExtraBool5.ToString()
,                   ObjCurrent.ExtraBool6.ToString()
,                   ObjCurrent.ExtraText4.ToString()
,                   ObjCurrent.ExtraText5.ToString()
,                   ObjCurrent.ExtraText6.ToString()
,                   ObjCurrent.ExtraDate1.ToString()
,                   ObjCurrent.ExtraDate2.ToString()
,                   ObjCurrent.ExtraDate3.ToString()
,                   ObjCurrent.ExtraDate4.ToString()
,                   ObjCurrent.ExtraDate5.ToString()
,                   ObjCurrent.ExtraDate6.ToString()
,                   ObjCurrent.ExtraFloat1.ToString()
,                   ObjCurrent.ExtraFloat2.ToString()
,                   ObjCurrent.ExtraFloat3.ToString()
,                   ObjCurrent.ExtraFloat4.ToString()
,                   ObjCurrent.ExtraFloat5.ToString()
,                   ObjCurrent.ExtraFloat6.ToString()
,                   ObjCurrent.ContractStartDate.ToString()
,                   ObjCurrent.ContractEndDate.ToString()
                };

                paraVals = CommonFunctions.CheckPerameterArray(paraVals);

                DataSet ds = new DataSet();
                SqlHelper.FillDataset(DBProvider.GetPerfectConnStr, "SP_MasterEmployee_Limited_Select", ds, new string[] { "MasterEmployee" }, paraVals);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MasterEmployee getFiltedList()
        {
            try
            {
                DataSet ds = getList().DataSet;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //-------------------------------------------------
                    //SegmentID   int
                    //DesignationID   int
                    //EmployeeNo   int
                    //LevelID   int
                    //EmpSegmentID   int
                    //DocumentEmployeeNo   string
                    //NIC   string
                    //BadgeNo   string
                    //FirstName   string
                    //MiddleName   string
                    //LastName   string
                    //OtherNames   string
                    //Initials   string
                    //GenderID   int
                    //CiviStatusId int
                    //OrientationID   int
                    //RaceID   int
                    //DOB   DateTime
                    //CountryID   int
                    //DateJoined   DateTime
                    //DateLeftOrganization   DateTime
                    //JobTitle   string
                    //ElectrorateID   int
                    //IsCitizen   bool?
                    //BloodType   string
                    //MedicalCondtionsID   int
                    //PaySlipName   string
                    //TaxationName   string
                    //PassportNo   string
                    //PaidCurrencyCode   string
                    //PaySchemeID   int
                    //TaxFileNo   string
                    //EPFNo   string
                    //KnowMedicalCondtions   string
                    //BasicSalary   double
                    //PreferedTransportMethodID   int
                    //ShiftID   int
                    //CategoryID   int
                    //SectionID   int
                    //TaxTypeID   int
                    //ETFNo   string
                    //BankID   int
                    //PayProcessed   int
                    //PayCycleID   int
                    //AttendanceSummeryID   int
                    //WorkingHourScheduleID   int
                    //LeaveSchemeID   int
                    //PerformaneBonus   int
                    //EmployeePaymentBreakdownID   int
                    //ClaimSchemeID   int
                    //EmployeeRetired   bool?
                    //EmployeeConfirmed   bool?
                    //EISPassword   string
                    //LastModifyUser   int
                    //LastModifyDate   DateTime
                    //ServiceePercentage   double
                    //PassportExpireDate   DateTime
                    //LanguageID   int
                    //EmployeeConfirmedDate   DateTime
                    //LocationID   int
                    //JobSpecification   string
                    //AttendanceSchemeID   int
                    //ExtraInt1   int
                    //ExtraInt2   int
                    //ExtraInt3   int
                    //ExtraBool1   bool?
                    //ExtraBool2   bool?
                    //ExtraBool3   bool?
                    //ExtraText1   string
                    //ExtraText2   string
                    //ExtraText3   string
                    //LeftReason string
                    //EPFEntitlement bool?

                    //------------------------------------------------


                    ObjCurrent.SegmentID = (int)ds.Tables[0].Rows[0]["SegmentID"];
                    ObjCurrent.DesignationID = (int)ds.Tables[0].Rows[0]["DesignationID"];
                    ObjCurrent.EmployeeNo = (int)ds.Tables[0].Rows[0]["EmployeeNo"];
                    ObjCurrent.LevelID = (int)ds.Tables[0].Rows[0]["LevelID"];
                    ObjCurrent.EmpSegmentID = (int)ds.Tables[0].Rows[0]["EmpSegmentID"];
                    ObjCurrent.DocumentEmployeeNo = (string)ds.Tables[0].Rows[0]["DocumentEmployeeNo"];
                    ObjCurrent.NIC = (string)ds.Tables[0].Rows[0]["NIC"];
                    ObjCurrent.BadgeNo = (string)ds.Tables[0].Rows[0]["BadgeNo"];
                    ObjCurrent.FirstName = (string)ds.Tables[0].Rows[0]["FirstName"];
                    ObjCurrent.MiddleName = (string)ds.Tables[0].Rows[0]["MiddleName"];
                    ObjCurrent.LastName = (string)ds.Tables[0].Rows[0]["LastName"];
                    ObjCurrent.OtherNames = (string)ds.Tables[0].Rows[0]["OtherNames"];
                    ObjCurrent.Initials = (string)ds.Tables[0].Rows[0]["Initials"];
                    ObjCurrent.GenderID = (int)ds.Tables[0].Rows[0]["GenderID"];
                    ObjCurrent.OrientationID = (int)ds.Tables[0].Rows[0]["OrientationID"];
                    ObjCurrent.RaceID = (int)ds.Tables[0].Rows[0]["RaceID"];
                    ObjCurrent.DOB = (DateTime)ds.Tables[0].Rows[0]["DOB"];
                    ObjCurrent.CountryID = (int)ds.Tables[0].Rows[0]["CountryID"];
                    ObjCurrent.DateJoined = (DateTime)ds.Tables[0].Rows[0]["DateJoined"];
                    ObjCurrent.DateLeftOrganization = (DateTime)ds.Tables[0].Rows[0]["DateLeftOrganization"];
                    ObjCurrent.JobTitle = (string)ds.Tables[0].Rows[0]["JobTitle"];
                    ObjCurrent.ElectrorateID = (int)ds.Tables[0].Rows[0]["ElectrorateID"];
                    ObjCurrent.IsCitizen = (bool?)ds.Tables[0].Rows[0]["IsCitizen"];
                    ObjCurrent.BloodType = (string)ds.Tables[0].Rows[0]["BloodType"];
                    ObjCurrent.MedicalCondtionsID = (int)ds.Tables[0].Rows[0]["MedicalCondtionsID"];
                    ObjCurrent.PaySlipName = (string)ds.Tables[0].Rows[0]["PaySlipName"];
                    ObjCurrent.TaxationName = (string)ds.Tables[0].Rows[0]["TaxationName"];
                    ObjCurrent.PassportNo = (string)ds.Tables[0].Rows[0]["PassportNo"];
                    ObjCurrent.PaidCurrencyCode = (string)ds.Tables[0].Rows[0]["PaidCurrencyCode"];
                    ObjCurrent.PaySchemeID = (int)ds.Tables[0].Rows[0]["PaySchemeID"];
                    ObjCurrent.TaxFileNo = (string)ds.Tables[0].Rows[0]["TaxFileNo"];
                    ObjCurrent.EPFNo = (string)ds.Tables[0].Rows[0]["EPFNo"];
                    ObjCurrent.KnowMedicalCondtions = (string)ds.Tables[0].Rows[0]["KnowMedicalCondtions"];
                    ObjCurrent.BasicSalary = (double)ds.Tables[0].Rows[0]["BasicSalary"];
                    ObjCurrent.PreferedTransportMethodID = (int)ds.Tables[0].Rows[0]["PreferedTransportMethodID"];
                    ObjCurrent.ShiftID = (int)ds.Tables[0].Rows[0]["ShiftID"];
                    ObjCurrent.CategoryID = (int)ds.Tables[0].Rows[0]["CategoryID"];
                    ObjCurrent.SectionID = (int)ds.Tables[0].Rows[0]["SectionID"];
                    ObjCurrent.TaxTypeID = (int)ds.Tables[0].Rows[0]["TaxTypeID"];
                    ObjCurrent.ETFNo = (string)ds.Tables[0].Rows[0]["ETFNo"];
                    ObjCurrent.BankID = (int)ds.Tables[0].Rows[0]["BankID"];
                    ObjCurrent.PayProcessed = (int)ds.Tables[0].Rows[0]["PayProcessed"];
                    ObjCurrent.PayCycleID = (int)ds.Tables[0].Rows[0]["PayCycleID"];
                    ObjCurrent.AttendanceSummeryID = (int)ds.Tables[0].Rows[0]["AttendanceSummeryID"];
                    ObjCurrent.WorkingHourScheduleID = (int)ds.Tables[0].Rows[0]["WorkingHourScheduleID"];
                    ObjCurrent.LeaveSchemeID = (int)ds.Tables[0].Rows[0]["LeaveSchemeID"];
                    ObjCurrent.PerformaneBonus = (int)ds.Tables[0].Rows[0]["PerformaneBonus"];
                    ObjCurrent.EmployeePaymentBreakdownID = (int)ds.Tables[0].Rows[0]["EmployeePaymentBreakdownID"];
                    ObjCurrent.ClaimSchemeID = (int)ds.Tables[0].Rows[0]["ClaimSchemeID"];
                    ObjCurrent.EmployeeRetired = (bool?)ds.Tables[0].Rows[0]["EmployeeRetired"];
                    ObjCurrent.EmployeeConfirmed = (bool?)ds.Tables[0].Rows[0]["EmployeeConfirmed"];
                    ObjCurrent.EISPassword = (string)ds.Tables[0].Rows[0]["EISPassword"];
                    ObjCurrent.LastModifyUser = (int)ds.Tables[0].Rows[0]["LastModifyUser"];
                    ObjCurrent.LastModifyDate = (DateTime)ds.Tables[0].Rows[0]["LastModifyDate"];
                    ObjCurrent.ServiceePercentage = (double)ds.Tables[0].Rows[0]["ServiceePercentage"];
                    ObjCurrent.PassportExpireDate = (DateTime)ds.Tables[0].Rows[0]["PassportExpireDate"];
                    ObjCurrent.LanguageID = (int)ds.Tables[0].Rows[0]["LanguageID"];
                    ObjCurrent.EmployeeConfirmedDate = (DateTime)ds.Tables[0].Rows[0]["EmployeeConfirmedDate"];
                    ObjCurrent.LocationID = (int)ds.Tables[0].Rows[0]["LocationID"];
                    ObjCurrent.JobSpecification = (string)ds.Tables[0].Rows[0]["JobSpecification"];
                    ObjCurrent.Title = (int)ds.Tables[0].Rows[0]["Title"];
                    ObjCurrent.Increment = (double)ds.Tables[0].Rows[0]["Increment"];
                    ObjCurrent.IsActive = (bool?)ds.Tables[0].Rows[0]["IsActive"];
                    ObjCurrent.WorkingCalenderSchemeID = (int)ds.Tables[0].Rows[0]["WorkingCalenderSchemeID"];
                    ObjCurrent.IsIncrementApplyingAfter = (bool?)ds.Tables[0].Rows[0]["IsIncrementApplyingAfter"];
                    ObjCurrent.IncrementDateFrom = (DateTime)ds.Tables[0].Rows[0]["IncrementDateFrom"];
                    ObjCurrent.AttendanceSchemeID = (int)ds.Tables[0].Rows[0]["AttendanceSchemeID"];
                    ObjCurrent.ExtraInt1 = (int)ds.Tables[0].Rows[0]["ExtraInt1"];
                    ObjCurrent.ExtraInt2 = (int)ds.Tables[0].Rows[0]["ExtraInt2"];
                    ObjCurrent.ExtraInt3 = (int)ds.Tables[0].Rows[0]["ExtraInt3"];
                    ObjCurrent.ExtraBool1 = (bool?)ds.Tables[0].Rows[0]["ExtraBool1"];
                    ObjCurrent.ExtraBool2 = (bool?)ds.Tables[0].Rows[0]["ExtraBool2"];
                    ObjCurrent.ExtraBool3 = (bool?)ds.Tables[0].Rows[0]["ExtraBool3"];
                    ObjCurrent.ExtraText1 = (string)ds.Tables[0].Rows[0]["ExtraText1"];
                    ObjCurrent.ExtraText2 = (string)ds.Tables[0].Rows[0]["ExtraText2"];
                    ObjCurrent.ExtraText3 = (string)ds.Tables[0].Rows[0]["ExtraText3"];
                    ObjCurrent.IsContractEmployee = (bool)ds.Tables[0].Rows[0]["IsContractEmployee"];
                    ObjCurrent.InactiveDate = (DateTime)ds.Tables[0].Rows[0]["InactiveDate"];
                    ObjCurrent.LeftReason = (string)ds.Tables[0].Rows[0]["LeftReason"];//TharakaM 2020.04.16
                    ObjCurrent.EPFEntitlement = (bool?)ds.Tables[0].Rows[0]["EPFENtitlement"]; //TharakaM 2020.04.17
                    ObjCurrent.CivilStatusId = (int)ds.Tables[0].Rows[0]["CivilStatusId"]; //TharakaM 2020.04.21
                    ObjCurrent.ReligionID = (int)ds.Tables[0].Rows[0]["ReligionID"]; //TharakaM 2020.04.21
                    ObjCurrent.DateRetired = (DateTime)ds.Tables[0].Rows[0]["DateRetired"]; //TharakaM 2020.05.22
                    ObjCurrent.IsRetired = (bool?)ds.Tables[0].Rows[0]["IsRetired"]; //TharakaM 2020.05.22
                    ObjCurrent.RetiredYear = (string)ds.Tables[0].Rows[0]["RetiredYear"]; //TharakaM 2020.05.26
                    ObjCurrent.ProjectDesignationID = (int)ds.Tables[0].Rows[0]["ProjectDesignationID"];
                    ObjCurrent.ProjectSkillID = (int)ds.Tables[0].Rows[0]["ProjectSkillID"];
                    ObjCurrent.ProjectJobTypeID = (int)ds.Tables[0].Rows[0]["ProjectJobTypeID"];
                    ObjCurrent.GradeID = (int)ds.Tables[0].Rows[0]["GradeID"];
                    ObjCurrent.NID = (string)ds.Tables[0].Rows[0]["NID"];
                    ObjCurrent.ExtraInt4= (int)ds.Tables[0].Rows[0]["ExtraInt4"];
                    ObjCurrent.ExtraInt5 = (int)ds.Tables[0].Rows[0]["ExtraInt5"];
                    ObjCurrent.ExtraInt6 = (int)ds.Tables[0].Rows[0]["ExtraInt6"];
                    ObjCurrent.ExtraBool4 = (bool)ds.Tables[0].Rows[0]["ExtraBool4"];
                    ObjCurrent.ExtraBool5 = (bool)ds.Tables[0].Rows[0]["ExtraBool5"];
                    ObjCurrent.ExtraBool6 = (bool)ds.Tables[0].Rows[0]["ExtraBool6"];
                    ObjCurrent.ExtraText4= (string)ds.Tables[0].Rows[0]["ExtraText4"];
                    ObjCurrent.ExtraText5 = (string)ds.Tables[0].Rows[0]["ExtraText5"];
                    ObjCurrent.ExtraText6 = (string)ds.Tables[0].Rows[0]["ExtraText6"];
                    ObjCurrent.ExtraDate1 = (DateTime)ds.Tables[0].Rows[0]["ExtraDate1"];
                    ObjCurrent.ExtraDate2 = (DateTime)ds.Tables[0].Rows[0]["ExtraDate2"];
                    ObjCurrent.ExtraDate3 = (DateTime)ds.Tables[0].Rows[0]["ExtraDate3"];
                    ObjCurrent.ExtraDate4 = (DateTime)ds.Tables[0].Rows[0]["ExtraDate4"];
                    ObjCurrent.ExtraDate5 = (DateTime)ds.Tables[0].Rows[0]["ExtraDate5"];
                    ObjCurrent.ExtraDate6 = (DateTime)ds.Tables[0].Rows[0]["ExtraDate6"];
                    ObjCurrent.ExtraFloat1 = (double)ds.Tables[0].Rows[0]["ExtraFloat1"];
                    ObjCurrent.ExtraFloat2 = (double)ds.Tables[0].Rows[0]["ExtraFloat2"];
                    ObjCurrent.ExtraFloat3 = (double)ds.Tables[0].Rows[0]["ExtraFloat3"];
                    ObjCurrent.ExtraFloat4 = (double)ds.Tables[0].Rows[0]["ExtraFloat4"];
                    ObjCurrent.ExtraFloat5 = (double)ds.Tables[0].Rows[0]["ExtraFloat5"];
                    ObjCurrent.ExtraFloat6 = (double)ds.Tables[0].Rows[0]["ExtraFloat6"];
                    ObjCurrent.ContractStartDate = (DateTime)ds.Tables[0].Rows[0]["ContractStartDate"];
                    ObjCurrent.ContractEndDate = (DateTime)ds.Tables[0].Rows[0]["ContractEndDate"];


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

        public DataTable getListSelect(string Filter)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter("@EmployeeFilter", Filter);
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(DBProvider.GetPerfectConnStr, "SP_MasterEmployee_Select_Filtered", ds, new string[] { "MasterEmployee" }, parm);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getListExcelUpload(int SegmentID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter("@SegmentID", SegmentID);                
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(DBProvider.GetPerfectConnStr, "SPC_MasterEmployeeExcelUpload_Temp_Select", ds, new string[] { "MasterEmployee" }, parm);
                return ds.Tables[0];
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
            //DesignationID   int(int)
            //EmployeeNo   int(int)
            //LevelID   int(int)
            //EmpSegmentID   int(int)
            //DocumentEmployeeNo   string(int)
            //NIC   string(int)
            //BadgeNo   string(int)
            //FirstName   string(int)
            //MiddleName   string(int)
            //LastName   string(int)
            //OtherNames   string(int)
            //Initials   string(int)
            //GenderID   int(int)
            //CivilStatusId int(int)
            //OrientationID   int(int)
            //RaceID   int(int)
            //DOB   DateTime(int)
            //CountryID   int(int)
            //DateJoined   DateTime(int)
            //DateLeftOrganization   DateTime(int)
            //JobTitle   string(int)
            //ElectrorateID   int(int)
            //IsCitizen   bool?(int)
            //BloodType   string(int)
            //MedicalCondtionsID   int(int)
            //PaySlipName   string(int)
            //TaxationName   string(int)
            //PassportNo   string(int)
            //PaidCurrencyCode   string(int)
            //PaySchemeID   int(int)
            //TaxFileNo   string(int)
            //EPFNo   string(int)
            //KnowMedicalCondtions   string(int)
            //BasicSalary   double(int)
            //PreferedTransportMethodID   int(int)
            //ShiftID   int(int)
            //CategoryID   int(int)
            //SectionID   int(int)
            //TaxTypeID   int(int)
            //ETFNo   string(int)
            //BankID   int(int)
            //PayProcessed   int(int)
            //PayCycleID   int(int)
            //AttendanceSummeryID   int(int)
            //WorkingHourScheduleID   int(int)
            //LeaveSchemeID   int(int)
            //PerformaneBonus   int(int)
            //EmployeePaymentBreakdownID   int(int)
            //ClaimSchemeID   int(int)
            //EmployeeRetired   bool?(int)
            //EmployeeConfirmed   bool?(int)
            //EISPassword   string(int)
            //LastModifyUser   int(int)
            //LastModifyDate   DateTime(int)
            //ServiceePercentage   double(int)
            //PassportExpireDate   DateTime(int)
            //LanguageID   int(int)
            //EmployeeConfirmedDate   DateTime(int)
            //LocationID   int(int)
            //JobSpecification   string(int)
            //AttendanceSchemeID   int(int)
            //ExtraInt1   int(int)
            //ExtraInt2   int(int)
            //ExtraInt3   int(int)
            //ExtraBool1   bool?(int)
            //ExtraBool2   bool?(int)
            //ExtraBool3   bool?(int)
            //ExtraText1   string(int)
            //ExtraText2   string(int)
            //ExtraText3   string(int)
            //LeftReason string(int)
            //EPFEntitlement bool?(int)

            //-------------------------------------------------
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[115];

                parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;

                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;

                parm[2] = new SqlParameter("@SegmentID", SqlDbType.Int);
                parm[3] = new SqlParameter("@DesignationID", SqlDbType.Int);
                parm[4] = new SqlParameter("@EmployeeNo", SqlDbType.Int);
                parm[5] = new SqlParameter("@LevelID", SqlDbType.Int);
                parm[6] = new SqlParameter("@EmpSegmentID", SqlDbType.Int);
                parm[7] = new SqlParameter("@DocumentEmployeeNo", SqlDbType.VarChar);
                parm[8] = new SqlParameter("@NIC", SqlDbType.VarChar);
                parm[9] = new SqlParameter("@BadgeNo", SqlDbType.VarChar);
                parm[10] = new SqlParameter("@FirstName", SqlDbType.VarChar);
                parm[11] = new SqlParameter("@MiddleName", SqlDbType.VarChar);
                parm[12] = new SqlParameter("@LastName", SqlDbType.VarChar);
                parm[13] = new SqlParameter("@OtherNames", SqlDbType.VarChar);
                parm[14] = new SqlParameter("@Initials", SqlDbType.VarChar);
                parm[15] = new SqlParameter("@GenderID", SqlDbType.Int);
                parm[16] = new SqlParameter("@OrientationID", SqlDbType.Int);
                parm[17] = new SqlParameter("@RaceID", SqlDbType.Int);
                parm[18] = new SqlParameter("@DOB", SqlDbType.DateTime);
                parm[19] = new SqlParameter("@CountryID", SqlDbType.Int);
                parm[20] = new SqlParameter("@DateJoined", SqlDbType.DateTime);
                parm[21] = new SqlParameter("@DateLeftOrganization", SqlDbType.DateTime);
                parm[22] = new SqlParameter("@JobTitle", SqlDbType.VarChar);
                parm[23] = new SqlParameter("@ElectrorateID", SqlDbType.Int);
                parm[24] = new SqlParameter("@IsCitizen", SqlDbType.Bit);
                parm[25] = new SqlParameter("@BloodType", SqlDbType.VarChar);
                parm[26] = new SqlParameter("@MedicalCondtionsID", SqlDbType.Int);
                parm[27] = new SqlParameter("@PaySlipName", SqlDbType.VarChar);
                parm[28] = new SqlParameter("@TaxationName", SqlDbType.VarChar);
                parm[29] = new SqlParameter("@PassportNo", SqlDbType.VarChar);
                parm[30] = new SqlParameter("@PaidCurrencyCode", SqlDbType.VarChar);
                parm[31] = new SqlParameter("@PaySchemeID", SqlDbType.Int);
                parm[32] = new SqlParameter("@TaxFileNo", SqlDbType.VarChar);
                parm[33] = new SqlParameter("@EPFNo", SqlDbType.VarChar);
                parm[34] = new SqlParameter("@KnowMedicalCondtions", SqlDbType.VarChar);
                parm[35] = new SqlParameter("@BasicSalary", SqlDbType.Float);
                parm[36] = new SqlParameter("@PreferedTransportMethodID", SqlDbType.Int);
                parm[37] = new SqlParameter("@ShiftID", SqlDbType.Int);
                parm[38] = new SqlParameter("@CategoryID", SqlDbType.Int);
                parm[39] = new SqlParameter("@SectionID", SqlDbType.Int);
                parm[40] = new SqlParameter("@TaxTypeID", SqlDbType.Int);
                parm[41] = new SqlParameter("@ETFNo", SqlDbType.VarChar);
                parm[42] = new SqlParameter("@BankID", SqlDbType.Int);
                parm[43] = new SqlParameter("@PayProcessed", SqlDbType.Int);
                parm[44] = new SqlParameter("@PayCycleID", SqlDbType.Int);
                parm[45] = new SqlParameter("@AttendanceSummeryID", SqlDbType.Int);
                parm[46] = new SqlParameter("@WorkingHourScheduleID", SqlDbType.Int);
                parm[47] = new SqlParameter("@LeaveSchemeID", SqlDbType.Int);
                parm[48] = new SqlParameter("@PerformaneBonus", SqlDbType.Int);
                parm[49] = new SqlParameter("@EmployeePaymentBreakdownID", SqlDbType.Int);
                parm[50] = new SqlParameter("@ClaimSchemeID", SqlDbType.Int);
                parm[51] = new SqlParameter("@EmployeeRetired", SqlDbType.Bit);
                parm[52] = new SqlParameter("@EmployeeConfirmed", SqlDbType.Bit);
                parm[53] = new SqlParameter("@EISPassword", SqlDbType.VarChar);
                parm[54] = new SqlParameter("@LastModifyUser", SqlDbType.Int);
                parm[55] = new SqlParameter("@LastModifyDate", SqlDbType.DateTime);
                parm[56] = new SqlParameter("@ServiceePercentage", SqlDbType.Float);
                parm[57] = new SqlParameter("@PassportExpireDate", SqlDbType.DateTime);
                parm[58] = new SqlParameter("@LanguageID", SqlDbType.Int);
                parm[59] = new SqlParameter("@EmployeeConfirmedDate", SqlDbType.DateTime);
                parm[60] = new SqlParameter("@LocationID", SqlDbType.Int);
                parm[61] = new SqlParameter("@JobSpecification", SqlDbType.VarChar);
                parm[62] = new SqlParameter("@Title", SqlDbType.Int);
                parm[63] = new SqlParameter("@Increment", SqlDbType.Float);
                parm[64] = new SqlParameter("@IsActive", SqlDbType.Bit);
                parm[65] = new SqlParameter("@WorkingCalenderSchemeID", SqlDbType.Int);
                parm[66] = new SqlParameter("@IsIncrementApplyingAfter", SqlDbType.Bit);
                parm[67] = new SqlParameter("@IncrementDateFrom", SqlDbType.DateTime);
                parm[68] = new SqlParameter("@AttendanceSchemeID", SqlDbType.Int);
                parm[69] = new SqlParameter("@ExtraInt1", SqlDbType.Int);
                parm[70] = new SqlParameter("@ExtraInt2", SqlDbType.Int);
                parm[71] = new SqlParameter("@ExtraInt3", SqlDbType.Int);
                parm[72] = new SqlParameter("@ExtraBool1", SqlDbType.Bit);
                parm[73] = new SqlParameter("@ExtraBool2", SqlDbType.Bit);
                parm[74] = new SqlParameter("@ExtraBool3", SqlDbType.Bit);
                parm[75] = new SqlParameter("@ExtraText1", SqlDbType.VarChar);
                parm[76] = new SqlParameter("@ExtraText2", SqlDbType.VarChar);
                parm[77] = new SqlParameter("@ExtraText3", SqlDbType.VarChar);
                parm[78] = new SqlParameter("@IsContractEmployee", SqlDbType.Bit);
                parm[79] = new SqlParameter("@InactiveDate", SqlDbType.Date);
                parm[80] = new SqlParameter("@LeftReason", SqlDbType.VarChar); //TharakaM 2020.04.16
                parm[81] = new SqlParameter("@EPFEntitlement", SqlDbType.Bit); //TharakaM 2020.04.17
                parm[82] = new SqlParameter("@CivilStatusId", SqlDbType.Int); //TharakaM 2020.04.21
                parm[83] = new SqlParameter("@ReligionID", SqlDbType.Int); //TharakaM 2020.04.21
                parm[84] = new SqlParameter("@DateRetired", SqlDbType.DateTime); //TharakaM 2020.05.22
                parm[85] = new SqlParameter("@IsRetired", SqlDbType.Bit); //TharakaM 2020.05.22
                parm[86] = new SqlParameter("@RetiredYear", SqlDbType.VarChar); //TharakaM 2020.05.26
                parm[87] = new SqlParameter("@ProjectDesignationID", SqlDbType.Int); //TharakaM 2020.05.26
                parm[88] = new SqlParameter("@ProjectSkillID", SqlDbType.Int); //TharakaM 2020.05.26
                parm[89] = new SqlParameter("@ProjectJobTypeID", SqlDbType.Int); //TharakaM 2020.05.26
                parm[90] = new SqlParameter("@GradeID", SqlDbType.Int);
                parm[91] = new SqlParameter("@NID", SqlDbType.VarChar);
                parm[92] = new SqlParameter("@ExtraInt4", SqlDbType.Int);
                parm[93] = new SqlParameter("@ExtraInt5", SqlDbType.Int);
                parm[94] = new SqlParameter("@ExtraInt6", SqlDbType.Int);
                parm[95] = new SqlParameter("@ExtraBool4", SqlDbType.Bit);
                parm[96] = new SqlParameter("@ExtraBool5", SqlDbType.Bit);
                parm[97] = new SqlParameter("@ExtraBool6", SqlDbType.Bit);
                parm[98] = new SqlParameter("@ExtraText4", SqlDbType.VarChar);
                parm[99] = new SqlParameter("@ExtraText5", SqlDbType.VarChar);
                parm[100] = new SqlParameter("@ExtraText6", SqlDbType.VarChar);
                parm[101] = new SqlParameter("@ExtraDate1", SqlDbType.DateTime);
                parm[102] = new SqlParameter("@ExtraDate2", SqlDbType.DateTime);
                parm[103] = new SqlParameter("@ExtraDate3", SqlDbType.DateTime);
                parm[104] = new SqlParameter("@ExtraDate4", SqlDbType.DateTime);
                parm[105] = new SqlParameter("@ExtraDate5", SqlDbType.DateTime);
                parm[106] = new SqlParameter("@ExtraDate6", SqlDbType.DateTime);
                parm[107] = new SqlParameter("@ExtraFloat1", SqlDbType.Float);
                parm[108] = new SqlParameter("@ExtraFloat2", SqlDbType.Float);
                parm[109] = new SqlParameter("@ExtraFloat3", SqlDbType.Float);
                parm[110] = new SqlParameter("@ExtraFloat4", SqlDbType.Float);
                parm[111] = new SqlParameter("@ExtraFloat5", SqlDbType.Float);
                parm[112] = new SqlParameter("@ExtraFloat6", SqlDbType.Float);
                parm[113] = new SqlParameter("@ContractStartDate", SqlDbType.DateTime);
                parm[114] = new SqlParameter("@ContractEndDate", SqlDbType.DateTime);



                parm[2].Value = ObjCurrent.SegmentID;
                parm[3].Value = ObjCurrent.DesignationID;
                parm[4].Value = ObjCurrent.EmployeeNo;
                parm[5].Value = ObjCurrent.LevelID;
                parm[6].Value = ObjCurrent.EmpSegmentID;
                parm[7].Value = ObjCurrent.DocumentEmployeeNo;
                parm[8].Value = ObjCurrent.NIC;
                parm[9].Value = ObjCurrent.BadgeNo;
                parm[10].Value = ObjCurrent.FirstName;
                parm[11].Value = ObjCurrent.MiddleName;
                parm[12].Value = ObjCurrent.LastName;
                parm[13].Value = ObjCurrent.OtherNames;
                parm[14].Value = ObjCurrent.Initials;
                parm[15].Value = ObjCurrent.GenderID;
                parm[16].Value = ObjCurrent.OrientationID;
                parm[17].Value = ObjCurrent.RaceID;
                parm[18].Value = ObjCurrent.DOB;
                parm[19].Value = ObjCurrent.CountryID;
                parm[20].Value = ObjCurrent.DateJoined;
                parm[21].Value = ObjCurrent.DateLeftOrganization;
                parm[22].Value = ObjCurrent.JobTitle;
                parm[23].Value = ObjCurrent.ElectrorateID;
                parm[24].Value = ObjCurrent.IsCitizen;
                parm[25].Value = ObjCurrent.BloodType;
                parm[26].Value = ObjCurrent.MedicalCondtionsID;
                parm[27].Value = ObjCurrent.PaySlipName;
                parm[28].Value = ObjCurrent.TaxationName;
                parm[29].Value = ObjCurrent.PassportNo;
                parm[30].Value = ObjCurrent.PaidCurrencyCode;
                parm[31].Value = ObjCurrent.PaySchemeID;
                parm[32].Value = ObjCurrent.TaxFileNo;
                parm[33].Value = ObjCurrent.EPFNo;
                parm[34].Value = ObjCurrent.KnowMedicalCondtions;
                parm[35].Value = ObjCurrent.BasicSalary;
                parm[36].Value = ObjCurrent.PreferedTransportMethodID;
                parm[37].Value = ObjCurrent.ShiftID;
                parm[38].Value = ObjCurrent.CategoryID;
                parm[39].Value = ObjCurrent.SectionID;
                parm[40].Value = ObjCurrent.TaxTypeID;
                parm[41].Value = ObjCurrent.ETFNo;
                parm[42].Value = ObjCurrent.BankID;
                parm[43].Value = ObjCurrent.PayProcessed;
                parm[44].Value = ObjCurrent.PayCycleID;
                parm[45].Value = ObjCurrent.AttendanceSummeryID;
                parm[46].Value = ObjCurrent.WorkingHourScheduleID;
                parm[47].Value = ObjCurrent.LeaveSchemeID;
                parm[48].Value = ObjCurrent.PerformaneBonus;
                parm[49].Value = ObjCurrent.EmployeePaymentBreakdownID;
                parm[50].Value = ObjCurrent.ClaimSchemeID;
                parm[51].Value = ObjCurrent.EmployeeRetired;
                parm[52].Value = ObjCurrent.EmployeeConfirmed;
                parm[53].Value = ObjCurrent.EISPassword;
                parm[54].Value = ObjCurrent.LastModifyUser;
                parm[55].Value = ObjCurrent.LastModifyDate;
                parm[56].Value = ObjCurrent.ServiceePercentage;
                parm[57].Value = ObjCurrent.PassportExpireDate;
                parm[58].Value = ObjCurrent.LanguageID;
                parm[59].Value = ObjCurrent.EmployeeConfirmedDate;
                parm[60].Value = ObjCurrent.LocationID;
                parm[61].Value = ObjCurrent.JobSpecification;
                parm[62].Value = ObjCurrent.Title;
                parm[63].Value = ObjCurrent.Increment;
                parm[64].Value = ObjCurrent.IsActive;
                parm[65].Value = ObjCurrent.WorkingCalenderSchemeID;
                parm[66].Value = ObjCurrent.IsIncrementApplyingAfter;
                parm[67].Value = ObjCurrent.IncrementDateFrom;
                parm[68].Value = ObjCurrent.AttendanceSchemeID;
                parm[69].Value = ObjCurrent.ExtraInt1;
                parm[70].Value = ObjCurrent.ExtraInt2;
                parm[71].Value = ObjCurrent.ExtraInt3;
                parm[72].Value = ObjCurrent.ExtraBool1;
                parm[73].Value = ObjCurrent.ExtraBool2;
                parm[74].Value = ObjCurrent.ExtraBool3;
                parm[75].Value = ObjCurrent.ExtraText1;
                parm[76].Value = ObjCurrent.ExtraText2;
                parm[77].Value = ObjCurrent.ExtraText3;
                parm[78].Value = ObjCurrent.IsContractEmployee;

                if (Convert.ToString(ObjCurrent.InactiveDate.Date) == "01/01/0001 00:00:00")
                {
                    parm[79].Value = DBNull.Value;
                }
                else
                {
                    parm[79].Value = ObjCurrent.InactiveDate.Date;
                }

                parm[80].Value = ObjCurrent.LeftReason;//TharakaM 2020.04.16
                parm[81].Value = ObjCurrent.EPFEntitlement; //TharakaM 2020.04.17
                parm[82].Value = ObjCurrent.CivilStatusId; //TharakaM 2020.04.21
                parm[83].Value = ObjCurrent.ReligionID; //TharakaM 2020.04.21
                parm[84].Value = ObjCurrent.DateRetired; //TharakaM 2020.05.22
                parm[85].Value = ObjCurrent.IsRetired; //TharakaM 2020.05.22
                parm[86].Value = ObjCurrent.RetiredYear; //TharakaM 2020.05.26
                parm[87].Value = ObjCurrent.ProjectDesignationID; //TharakaM 2020.05.26
                parm[88].Value = ObjCurrent.ProjectSkillID; //TharakaM 2020.05.26
                parm[89].Value = ObjCurrent.ProjectJobTypeID; //TharakaM 2020.05.26
                parm[90].Value = ObjCurrent.GradeID;
                parm[91].Value = ObjCurrent.NID;
                parm[92].Value = ObjCurrent.ExtraInt4;
                parm[93].Value = ObjCurrent.ExtraInt5;
                parm[94].Value = ObjCurrent.ExtraInt6;
                parm[95].Value = ObjCurrent.ExtraBool4;
                parm[96].Value = ObjCurrent.ExtraBool5;
                parm[97].Value = ObjCurrent.ExtraBool6;
                parm[98].Value = ObjCurrent.ExtraText4;
                parm[99].Value = ObjCurrent.ExtraText5;
                parm[100].Value = ObjCurrent.ExtraText6;
                parm[101].Value = ObjCurrent.ExtraDate1;
                parm[102].Value = ObjCurrent.ExtraDate2;
                parm[103].Value = ObjCurrent.ExtraDate3;
                parm[104].Value = ObjCurrent.ExtraDate4;
                parm[105].Value = ObjCurrent.ExtraDate5;
                parm[106].Value = ObjCurrent.ExtraDate6;
                parm[107].Value = ObjCurrent.ExtraFloat1;
                parm[108].Value = ObjCurrent.ExtraFloat2;
                parm[109].Value = ObjCurrent.ExtraFloat3;
                parm[110].Value = ObjCurrent.ExtraFloat4;
                parm[111].Value = ObjCurrent.ExtraFloat5;
                parm[112].Value = ObjCurrent.ExtraFloat6;
                parm[113].Value = ObjCurrent.ContractStartDate;
                parm[114].Value = ObjCurrent.ContractEndDate;

                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SP_MasterEmployee_INSERTUPDATE", parm);

                qryRes.RetText = parm[0].Value.ToString();
                qryRes.RetID = int.Parse(parm[1].Value.ToString());


            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
            }
            return qryRes;
        }

        public QueryResult SaveExcelUpload(int SegmentID, int LastModifyUser)
        {
            //-------------------------------------------------
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[4];

                parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;

                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;

                parm[2] = new SqlParameter("@SegmentID", SegmentID);
                parm[3] = new SqlParameter("@LastModifyUser", LastModifyUser);



                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SP_MasterEmployee_ExcelUplod_Confirm", parm);

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
            //DesignationID   int(int)
            //EmployeeNo   int(int)
            //LevelID   int(int)
            //EmpSegmentID   int(int)
            //DocumentEmployeeNo   string(int)
            //NIC   string(int)
            //BadgeNo   string(int)
            //FirstName   string(int)
            //MiddleName   string(int)
            //LastName   string(int)
            //OtherNames   string(int)
            //Initials   string(int)
            //GenderID   int(int)
            //OrientationID   int(int)
            //RaceID   int(int)
            //DOB   DateTime(int)
            //CountryID   int(int)
            //DateJoined   DateTime(int)
            //DateLeftOrganization   DateTime(int)
            //JobTitle   string(int)
            //ElectrorateID   int(int)
            //IsCitizen   bool?(int)
            //BloodType   string(int)
            //MedicalCondtionsID   int(int)
            //PaySlipName   string(int)
            //TaxationName   string(int)
            //PassportNo   string(int)
            //PaidCurrencyCode   string(int)
            //PaySchemeID   int(int)
            //TaxFileNo   string(int)
            //EPFNo   string(int)
            //KnowMedicalCondtions   string(int)
            //BasicSalary   double(int)
            //PreferedTransportMethodID   int(int)
            //ShiftID   int(int)
            //CategoryID   int(int)
            //SectionID   int(int)
            //TaxTypeID   int(int)
            //ETFNo   string(int)
            //BankID   int(int)
            //PayProcessed   int(int)
            //PayCycleID   int(int)
            //AttendanceSummeryID   int(int)
            //WorkingHourScheduleID   int(int)
            //LeaveSchemeID   int(int)
            //PerformaneBonus   int(int)
            //EmployeePaymentBreakdownID   int(int)
            //ClaimSchemeID   int(int)
            //EmployeeRetired   bool?(int)
            //EmployeeConfirmed   bool?(int)
            //EISPassword   string(int)
            //LastModifyUser   int(int)
            //LastModifyDate   DateTime(int)
            //ServiceePercentage   double(int)
            //PassportExpireDate   DateTime(int)
            //LanguageID   int(int)
            //EmployeeConfirmedDate   DateTime(int)
            //LocationID   int(int)
            //JobSpecification   string(int)
            //AttendanceSchemeID   int(int)
            //ExtraInt1   int(int)
            //ExtraInt2   int(int)
            //ExtraInt3   int(int)
            //ExtraBool1   bool?(int)
            //ExtraBool2   bool?(int)
            //ExtraBool3   bool?(int)
            //ExtraText1   string(int)
            //ExtraText2   string(int)
            //ExtraText3   string(int)
            //LeftReason string(int)

            //-------------------------------------------------
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[62];

                parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;

                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;

                parm[2] = new SqlParameter("@SegmentID", SqlDbType.Int);
                parm[3] = new SqlParameter("@EmployeeNo", SqlDbType.Int);


                parm[2].Value = ObjCurrent.SegmentID;
                parm[3].Value = ObjCurrent.EmployeeNo;


                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SP_MasterEmployee_Delete", parm);

                qryRes.RetText = parm[0].Value.ToString();

            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
            }
            return qryRes;
        }

        public DataTable getListBadgeNo()
        {
            try
            {
                object[] paraVals = new object[] 
                           {

                           ObjCurrent.SegmentID.ToString() 
,                                 ObjCurrent.BadgeNo.ToString() 


                           };

                paraVals = CommonFunctions.CheckPerameterArray(paraVals);

                DataSet ds = new DataSet();
                SqlHelper.FillDataset(DBProvider.GetPerfectConnStr, "SP_MasterEmployee_Select_BadgeNo", ds, new string[] { "MasterEmployee" }, paraVals);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QueryResult RemoveEntryExcelUpload(int SegmentID)
        {            

            //-------------------------------------------------
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[3];

                parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;

                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;

                parm[2] = new SqlParameter("@SegmentID", SegmentID);
                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SPC_MasterEmployeeExcelUpload_Temp_Delete", parm);

                qryRes.RetText = parm[0].Value.ToString();

            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
            }
            return qryRes;
        }

        public DataTable getDocList()
        {
            try
            {
                object[] paraVals = new object[]
                {
                    ObjCurrent.SegmentID.ToString()
,                   ObjCurrent.EmployeeNo.ToString()
,                   ObjCurrent.DocumentEmployeeNo.ToString()
,                   ObjCurrent.OtherNames.ToString()
,                   ObjCurrent.IsActive.ToString()
                };

                paraVals = CommonFunctions.CheckPerameterArray(paraVals);

                DataSet ds = new DataSet();
                SqlHelper.FillDataset(DBProvider.GetPerfectConnStr, "SP_MasterEmployee_DocumentEmpNo_Select", ds, new string[] { "MasterEmployee" }, paraVals);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QueryResult UpdateDocEntry()
        {
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[92];

                parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;

                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;

                parm[2] = new SqlParameter("@SegmentID", SqlDbType.Int);
                parm[3] = new SqlParameter("@EmployeeNo", SqlDbType.Int);
                parm[4] = new SqlParameter("@DocumentEmployeeNo", SqlDbType.VarChar);

                parm[2].Value = ObjCurrent.SegmentID;
                parm[3].Value = ObjCurrent.EmployeeNo;
                parm[4].Value = ObjCurrent.DocumentEmployeeNo;


                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SP_MasterEmployeeDocNo_INSERTUPDATE", parm);

                qryRes.RetText = parm[0].Value.ToString();
                qryRes.RetID = int.Parse(parm[1].Value.ToString());


            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
            }
            return qryRes;
        }

        public DataTable getList_SAP_Posting()
        {
            try
            {
                object[] paraVals = new object[] 
				{
                    ObjCurrent.UserID.ToString()
,					ObjCurrent.SegmentID.ToString()
,					ObjCurrent.DesignationID.ToString()
,					ObjCurrent.EmployeeNo.ToString()
,					ObjCurrent.LevelID.ToString()
,					ObjCurrent.EmpSegmentID.ToString()
,					ObjCurrent.DocumentEmployeeNo.ToString()
,					ObjCurrent.NIC.ToString()
,					ObjCurrent.BadgeNo.ToString()
,					ObjCurrent.FirstName.ToString()
,					ObjCurrent.MiddleName.ToString()
,					ObjCurrent.LastName.ToString()
,					ObjCurrent.OtherNames.ToString()
,					ObjCurrent.Initials.ToString()
,					ObjCurrent.GenderID.ToString()
,					ObjCurrent.OrientationID.ToString()
,					ObjCurrent.RaceID.ToString()
,					ObjCurrent.DOB.ToString()
,					ObjCurrent.CountryID.ToString()
,					ObjCurrent.DateJoined.ToString()
,					ObjCurrent.DateLeftOrganization.ToString()
,					ObjCurrent.JobTitle.ToString()
,					ObjCurrent.ElectrorateID.ToString()
,					ObjCurrent.IsCitizen.ToString()
,					ObjCurrent.BloodType.ToString()
,					ObjCurrent.MedicalCondtionsID.ToString()
,					ObjCurrent.PaySlipName.ToString()
,					ObjCurrent.TaxationName.ToString()
,					ObjCurrent.PassportNo.ToString()
,					ObjCurrent.PaidCurrencyCode.ToString()
,					ObjCurrent.PaySchemeID.ToString()
,					ObjCurrent.TaxFileNo.ToString()
,					ObjCurrent.EPFNo.ToString()
,					ObjCurrent.KnowMedicalCondtions.ToString()
,					ObjCurrent.BasicSalary.ToString()
,					ObjCurrent.PreferedTransportMethodID.ToString()
,					ObjCurrent.ShiftID.ToString()
,					ObjCurrent.CategoryID.ToString()
,					ObjCurrent.SectionID.ToString()
,					ObjCurrent.TaxTypeID.ToString()
,					ObjCurrent.ETFNo.ToString()
,					ObjCurrent.BankID.ToString()
,					ObjCurrent.PayProcessed.ToString()
,					ObjCurrent.PayCycleID.ToString()
,					ObjCurrent.AttendanceSummeryID.ToString()
,					ObjCurrent.WorkingHourScheduleID.ToString()
,					ObjCurrent.LeaveSchemeID.ToString()
,					ObjCurrent.PerformaneBonus.ToString()
,					ObjCurrent.EmployeePaymentBreakdownID.ToString()
,					ObjCurrent.ClaimSchemeID.ToString()
,					ObjCurrent.EmployeeRetired.ToString()
,					ObjCurrent.EmployeeConfirmed.ToString()
,					ObjCurrent.EISPassword.ToString()
,					ObjCurrent.LastModifyUser.ToString()
,					ObjCurrent.LastModifyDate.ToString()
,					ObjCurrent.ServiceePercentage.ToString()
,					ObjCurrent.PassportExpireDate.ToString()
,					ObjCurrent.LanguageID.ToString()
,					ObjCurrent.EmployeeConfirmedDate.ToString()
,					ObjCurrent.LocationID.ToString()
,					ObjCurrent.JobSpecification.ToString()
,                   ObjCurrent.Title.ToString()
,                   ObjCurrent.Increment.ToString()
,                   ObjCurrent.IsActive.ToString()
,                   ObjCurrent.WorkingCalenderSchemeID.ToString()
,                   ObjCurrent.IsIncrementApplyingAfter.ToString()
,                   ObjCurrent.IncrementDateFrom.ToString()
,					ObjCurrent.AttendanceSchemeID.ToString()
,					ObjCurrent.ExtraInt1.ToString()
,					ObjCurrent.ExtraInt2.ToString()
,					ObjCurrent.ExtraInt3.ToString()
,					ObjCurrent.ExtraBool1.ToString()
,					ObjCurrent.ExtraBool2.ToString()
,					ObjCurrent.ExtraBool3.ToString()
,					ObjCurrent.ExtraText1.ToString()
,					ObjCurrent.ExtraText2.ToString()
,					ObjCurrent.ExtraText3.ToString()
,                   ObjCurrent.IsContractEmployee.ToString()//Tharaka 2018/09/06
,                   ObjCurrent.InactiveDate.ToString()
,                   ObjCurrent.LeftReason.ToString() //TharakaM 2020.04.16
,                   ObjCurrent.EPFEntitlement.ToString() //Tharaka 2020.04.17
,                   ObjCurrent.CivilStatusId.ToString() //TharakaM 2020.04.21
,                   ObjCurrent.ReligionID.ToString() //TharakaM 2020.04.21
,                   ObjCurrent.DateRetired.ToString() //TharakaM 2020.05.22
,                   ObjCurrent.IsRetired.ToString() //TharakaM 2020.05.22
,                   ObjCurrent.RetiredYear.ToString() //TharkaM 2020.05.26
,                   ObjCurrent.ProjectDesignationID.ToString()
,                   ObjCurrent.ProjectSkillID.ToString()
,                   ObjCurrent.ProjectJobTypeID.ToString()
,                   ObjCurrent.GradeID.ToString()
,                   ObjCurrent.NID.ToString()
,                   ObjCurrent.ExtraInt4.ToString()
,                   ObjCurrent.ExtraInt5.ToString()
,                   ObjCurrent.ExtraInt6.ToString()
,                   ObjCurrent.ExtraBool4.ToString()
,                   ObjCurrent.ExtraBool5.ToString()
,                   ObjCurrent.ExtraBool6.ToString()
,                   ObjCurrent.ExtraText4.ToString()
,                   ObjCurrent.ExtraText5.ToString()
,                   ObjCurrent.ExtraText6.ToString()
,                   ObjCurrent.ExtraDate1.ToString()
,                   ObjCurrent.ExtraDate2.ToString()
,                   ObjCurrent.ExtraDate3.ToString()
,                   ObjCurrent.ExtraDate4.ToString()
,                   ObjCurrent.ExtraDate5.ToString()
,                   ObjCurrent.ExtraDate6.ToString()
,                   ObjCurrent.ExtraFloat1.ToString()
,                   ObjCurrent.ExtraFloat2.ToString()
,                   ObjCurrent.ExtraFloat3.ToString()
,                   ObjCurrent.ExtraFloat4.ToString()
,                   ObjCurrent.ExtraFloat5.ToString()
,                   ObjCurrent.ExtraFloat6.ToString()
,                   ObjCurrent.ContractStartDate.ToString()
,                   ObjCurrent.ContractEndDate.ToString()

                };
                paraVals = CommonFunctions.CheckPerameterArray(paraVals);
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(DBProvider.GetPerfectConnStr, "SP_MasterEmployee_Select_SAP_Posting", ds, new string[] { "MasterEmployee" }, paraVals);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QueryResult SaveExcelUpload_WithApprovals(int SegmentID, int LastModifyUser)
        {
            //-------------------------------------------------
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[5];

                parm[0] = new SqlParameter("@RetText", SqlDbType.VarChar, 255);
                parm[0].Direction = ParameterDirection.Output;

                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;

                parm[2] = new SqlParameter("@SegmentID", SegmentID);
                parm[3] = new SqlParameter("@LastModifyUser", LastModifyUser);
                //parm[4] = new SqlParameter("@ApprovalID", ApprovalID);


                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SP_MasterEmployee_ExcelUplod_Confirm_withApprovals", parm);

                qryRes.RetText = parm[0].Value.ToString();
                qryRes.RetID = int.Parse(parm[1].Value.ToString());


            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
            }
            return qryRes;
        }

        public DataTable getListExcelUpload_With_Approvals(int SegmentID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter("@SegmentID", SegmentID);
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(DBProvider.GetPerfectConnStr, "SPC_MasterEmployeeExcelUpload_Temp_Approvals_Select", ds, new string[] { "MasterEmployee" }, parm);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QueryResult SaveExcelUpload_Temp(int SegmentID, int LastModifyUser, DataTable dtEmpTemp)
        {
            //-------------------------------------------------
            QueryResult qryRes = new QueryResult();
            try
            {
                SqlParameter[] parm = new SqlParameter[5];

                parm[0] = new SqlParameter("@RetText", SqlDbType.NVarChar, 4000);
                parm[0].Direction = ParameterDirection.Output;

                parm[1] = new SqlParameter("@RetID", SqlDbType.Int);
                parm[1].Direction = ParameterDirection.Output;

                parm[2] = new SqlParameter("@SegmentID", SegmentID);
                parm[3] = new SqlParameter("@LastModifyUser", LastModifyUser);
                parm[4] = new SqlParameter("@MasteEmployee", dtEmpTemp);


                SqlHelper.ExecuteNonQuery(DBProvider.GetPerfectConnStr, CommandType.StoredProcedure, "SPC_MasterEmployeeExcelUpload_Validate", parm);

                qryRes.RetText = parm[0].Value.ToString();
                qryRes.RetID = int.Parse(parm[1].Value.ToString());


            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
            }
            return qryRes;
        }


    }
}





