using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System.Data;
using HRMS.BusinessLayer;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Reflection;
using System.ComponentModel;
using System.Configuration;


namespace AttendanceSolution
{

    public class PrintingInformation
    {
        int LastInsert = -1;
        public enum ExportingFormats
        {
            excel,
            word,
            Pdf
        }

        private string _strReportName = "";

        public string ReportName
        {
            get { return _strReportName; }
            set { _strReportName = value; }
        }

        private int _intSegmentID;

        public int SegmentID
        {
            get { return _intSegmentID; }
            set
            {
                _intSegmentID = value;
                _strSegmentName = getSegmentName();
            }
        }

        private string getSegmentName()
        {
            string _strSegmentNames = "";
            if (_intSegmentID != -1 && _strSegmentName.Length <= 0)
            {
                _strSegmentNames = CommonFunctions.FindDatasetValue((new OrganizationScheme()).GetList(), "SegmentID", "SegmentName", CommonFunctions.FilterType.NumericField, _intSegmentID)[0].ToString();
            }
            return _strSegmentNames;
        }

        private string _strSegmentName = "";
        public string SegmentName
        {
            get
            {

                return _strSegmentName;
            }
            set { _strSegmentName = value; }
        }

        private ExportingFormats _exportFormat = ExportingFormats.excel;
        public ExportingFormats ExportFormat
        {
            get { return _exportFormat; }
            set { _exportFormat = value; }
        }

        private string _strUserName = "";
        public string UserName
        {
            get { return _strUserName; }
            set { _strUserName = value; }
        }

        private string _strFooterText = "Developed by Perfect Business Soluction Service (pvt) Ltd.";
        public string FooterText
        {
            get { return _strFooterText; }
            set { _strFooterText = value; }
        }

        private string _strHeaderText = string.Format("Printed from {0} @{1}", CommonFunctions.getMachineName(), DateTime.Now.ToShortTimeString());
        public string HeaderText
        {
            get { return _strHeaderText; }
            set { _strHeaderText = value; }
        }

        Control controlToPrint;
        public Control ControlToPrint
        {
            get { return controlToPrint; }
            set { controlToPrint = value; }
        }

    }

    public struct DayInfo
    {
        public int WeekNumber;
        public string DayName;
    }

    /// <summary>
    /// 
    /// </summary>
    public class Filters
    {
        public ColumnDetails _column;
        public string _strfilter;
        public string _strCaption;
        public Filters(ColumnDetails column, string Filter, string Caption)
        {
            _column = column;
            _strfilter = Filter;
            _strCaption = Caption;
        }

    }

    public class ColumnDetails
    {
        public string _strcolumnName;
        public string _ColumnDataType;
        public string _ColumnValue;
        public ColumnDetails(string ColumnName, string ColumnDataTypeValue)
        {
            _strcolumnName = ColumnName;
            _ColumnDataType = ColumnDataTypeValue;

        }

        public ColumnDetails(string ColumnName, string ColumnDataTypeValue, string ColumnValue)
        {
            _strcolumnName = ColumnName;
            _ColumnDataType = ColumnDataTypeValue;
            _ColumnValue = ColumnValue;

        }
    }

    public class DefaultColumns
    {
        public string ColumnName;
        public string ColumnSystemName;
        public DefaultColumns(string ColName, string ColSystemName)
        {
            ColumnName = ColName;
            ColumnSystemName = ColSystemName;
        }
    }


    public enum ColumnDataType
    { NumericColum, StringColumn, DateColumn, BinaryColumn, BitColumn }

    /// <summary>
    /// Approval status of transactions
    /// </summary>
    public enum ApprovalStatus
    {
        FinalApproved,
        ParticallyApproved,
        ApprovalPending,
        Rejected,
        Error
    }

    

    public class Approval
    {
        ApprovalStatus _status = new ApprovalStatus();

        public ApprovalStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }
        int _TransactionID = -1;

        public int TransactionID
        {
            get { return _TransactionID; }
            set { _TransactionID = value; }
        }
        int _AppliedByEmployeeNo = -1;

        public int AppliedByEmployeeNo
        {
            get { return _AppliedByEmployeeNo; }
            set { _AppliedByEmployeeNo = value; }
        }
        int _ApprovalSchemeID = -1;
        DateTime _CreateTime = DateTime.Now;

        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }
        string _Message = string.Empty;

        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
        string _rejectedReason = string.Empty;

        public string RejectedReason
        {
            get { return _rejectedReason; }
            set { _rejectedReason = value; }
        }
        DateTime _rejectedDate = DateTime.Now;

        public DateTime RejectedDate
        {
            get { return _rejectedDate; }
            set { _rejectedDate = value; }
        }
        DateTime _finalApprovalDate = DateTime.Now;

        public DateTime FinalApprovalDate
        {
            get { return _finalApprovalDate; }
            set { _finalApprovalDate = value; }
        }

    }

    public static class CommonFunctions
    {
        public static String[] imageTypes = new String[] { "JPG", "PNG", "BMP", "GIF" };
        public static String[] documrntType = new String[] { "PDF", "DOC", "DOCX", "xlsx" };
        public static clsGobalUser _globalUser = new clsGobalUser();
        public static string _SelectedFormula = "";
        public static List<Employee> _SelectedEmployees;
        //public static List<EmployeeClaimApplications> _SelectedClaims;
        //public static List<EmployeeGrievanceMessages> _SelectedGrievance;
        public static CommonSqlDataset _objCommonSqlDataset = new CommonSqlDataset();
        //public static List<MasterEmployeeSalaryHoldList> _lstSalaryHoldList;

        public enum LeaveRequestType
        {
            Days = 0,
            Hours = 1,
        }

        public enum ServerType
        {
            Server2008 = 2008,
            Server2012 = 2012,
            Server2014 = 2014,
            Server2016 = 2016

        }

        public enum ReportGroupType
        {
            _HRM_REports = 0,
            _HRM_Payroll_Reports = 1,
            _Time_Attendance_Reports = 2
        }

        public enum ResourceTransactions
        {
            ResourcePurchasing = 1,
            ResourceAllocation = 2,
            ResourceRequest = 3,
            ResourceReturn = 4,
            ResourceSendForRepairs = 5,
            ResourceReturnFromRepairs = 6,
            ResourceDistroyItems = 7,
        }

        /// <summary>
        /// Form Options
        /// </summary>
        public enum FormOptions
        {
            Add = 0,
            Delete = 1,
            Print = 2,
            Save = 3,
            Visible = 4
        }

        public enum CelenderEntryType  /* Add By TharakaM 2022-07-19*/
        {
            WorkGroup = 0,
            Employee = 1,
        }

        public enum DayOfWeek /* Add By TharakaM 2022-07-19*/
        {
            Monday = 0,
            Tuesday = 1,
            Wednesday = 2,
            Thursday = 3,
            Friday = 4,
            Saturday = 5,
            Sunday = 6,
        }

        public enum LoanPortion
        {
            Loan_Interest_Portion = 1,
            Loan_Principal_Portion = 2,
            Monthly_Installment = 3,
            Loan_Capital = 4,
            Loan_Reschedule_Amount=5,
            Loan_Reschedule_Interest=6
        }

        public enum LoanRescheduleOptions
        {
            Add_to_next_month_without_interest = 1,
            Add_to_next_month_with_interest = 2,
            Add_to_last_month_without_interest = 3,
            Add_to_last_month_with_interest = 4,
            Add_to_next_month_as_new_installement_without_interest = 5,
            Add_to_next_month_as_new_installement_with_interest = 6,
            Add_as_new_installement_at_the_end_of_the_Schedule_without_interest = 7,
            Add_as_new_installement_at_the_end_of_the_Schedule_with_interest = 8,
            Add_as_equal_amounts_to_all_the_installments_to_be_collected_without_interest = 9,
            Add_as_equal_amounts_to_all_the_installments_to_be_collected_with_Interest = 10
        }

        public static int setLoanPortionID(LoanPortion MethodName)
        {
            int returnval = 0;
            if (MethodName == LoanPortion.Loan_Interest_Portion)
            {
                returnval = 1;
            }
            else if (MethodName == LoanPortion.Loan_Principal_Portion)
            {
                returnval = 2;
            }
            else if (MethodName == LoanPortion.Monthly_Installment)
            {
                returnval = 3;
            }
            else
            {
                returnval = 4;
            }
            return returnval;
        }


        /// <summary>
        /// Processing Types
        /// </summary>
        public enum ProcessingType
        {
            RAW = 1,
            RULE = 2,
            PAYROLL = 3,
            POSTTOPAYROLL = 4,
            MONTHEND = 5,

        }



        public static int SelectLoanPortion(string LoanPortionText)
        {
            int returnval = 0;
            try
            {
                switch (LoanPortionText)
                {
                    case "Loan Interest Portion":
                        returnval = setLoanPortionID(LoanPortion.Loan_Interest_Portion);
                        break;

                    case "Loan Principal Portion":
                        returnval = setLoanPortionID(LoanPortion.Loan_Principal_Portion);
                        break;

                    case "Monthly Installment":
                        returnval = setLoanPortionID(LoanPortion.Monthly_Installment);
                        break;

                    case "Loan Capital":
                        returnval = setLoanPortionID(LoanPortion.Loan_Capital);
                        break;

                    case "Loan Reschedule Amount":
                        returnval = setLoanPortionID(LoanPortion.Loan_Reschedule_Amount);
                        break;

                    case "Loan Reschedule Interest":
                        returnval = setLoanPortionID(LoanPortion.Loan_Reschedule_Interest);
                        break;
                }
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
                //_globalBaseclass.ErrorObject = ex;
            }
            return returnval;
        }


        public static string GetLoanPortionTypeName(int LoanPortionTypeID)
        {
            string _strName = string.Empty;
            try
            {
                if (LoanPortionTypeID == 1)
                {
                    _strName = "Loan Interest Portion";
                }
                if (LoanPortionTypeID == 2)
                {
                    _strName = "Loan Principal Portion";
                }
                if (LoanPortionTypeID == 3)
                {
                    _strName = "Monthly Installment";
                }
                if (LoanPortionTypeID == 4)
                {
                    _strName = "Loan Capital";
                }
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }
            return _strName;
        }

        public enum ContactBookEntryType
        {
            ResidenceNo = 9,
            Mobile = 1,
            Fax = 2,
            Email = 3,
            TelNo = 4,
            Tel_No_Hunting = 5,
            Tel_No_Extension = 6,
            FaceBook = 7,
            Tweeter = 8,
        }

        public enum GrievenceUrgenecyLevel
        {
            NormalAction = 0,
            FastAction = 1,
            Critical_Urgent = 2
        }

        public class ColumnDetails
        {
            public string _strcolumnName;
            public string _ColumnDataType;
            public object _ColumnValue;
            public ColumnDetails(string ColumnName, string ColumnDataTypeValue)
            {
                _strcolumnName = ColumnName;
                _ColumnDataType = ColumnDataTypeValue;

            }

            public ColumnDetails(string ColumnName, string ColumnDataTypeValue, object ColumnValue)
            {
                _strcolumnName = ColumnName;
                _ColumnDataType = ColumnDataTypeValue;
                _ColumnValue = ColumnValue;

            }
        }

        public class HolidaysDates
        {
            
            DateTime _dtpHolidy = DateTime.Now.Date;
            Holidaytypes _holidayType = Holidaytypes.StatutoryHoliday;

            public HolidaysDates( DateTime HolidayDate, Holidaytypes HolidayType)
            {
                this.DtpHolidy = HolidayDate;
                this.HolidayType = HolidayType;
            }


            public DateTime DtpHolidy
            {
                get { return _dtpHolidy; }
                set { _dtpHolidy = value; }
            }


            public Holidaytypes HolidayType
            {
                get { return _holidayType; }
                set { _holidayType = value; }
            }
           

        }

        public class LinknItems
        {
            private int _LinkID;

            /// <summary>
            /// this is the link for salary item link Names of Claims\Training\Loans
            /// </summary>
            public int LinkID
            {
                get { return _LinkID; }
                set { _LinkID = value; }
            }

            private string _LinkName;

            /// <summary>
            /// this is the link for salary item link Names of Claims\Training\Loans
            /// </summary>
            public string LinkName
            {
                get { return _LinkName; }
                set { _LinkName = value; }
            }

            public LinknItems(int LinkID, string LinkName)
            {
                _LinkID = LinkID;
                _LinkName = LinkName;
            }

            private int _LoanPortionType;

            public int LoanPortionType
            {
                get { return _LoanPortionType; }
                set { _LoanPortionType = value; }
            }            

            public LinknItems(int LinkID, string LinkName, int LoanPortionType)
            {
                _LinkID = LinkID;
                _LinkName = LinkName;
                _LoanPortionType = LoanPortionType;             
            }


        }

        public enum Holidaytypes
        {
            NormalWorkingDay = 0,
            StatutoryHoliday = 1,
            GovernmentHoliday = 2,
            RestDay = 3,
            OffDay = 4,
            NormalWorkingHalfDays=5


        }

        public enum SalaryItemCalculationMethod
        {
            Fixed = 0,
            Variable = 1,
            Calculated = 2
        }

        public enum SalaryItemRoundingMethod
        {
            Up = 0,
            Down = 1,
            Flat = 2
        }

        public enum EmployeePayProcessedTypes
        {
            Hourly = 1,
            Daily = 2,
            Weekly = 3,
            By_Weekly = 4,
            Monthly = 5
        }

        public enum PayProcessedTypes
        {
            MonthPerYear
            ,
            MonthlyRateOTDaysPerMonth
                ,
            MonthlyRateAbsentPerMonth
                ,
            MonthlyRateAdvancePerMonth
                ,
            MonthlyRateORPPerMonth
                ,
            MonthlyRateHoursPerDay
                ,
            MonthlyRateHoursPerYear
                ,
            DailyRateHoursPerDay
                ,
            DailyRateDaysPerMonth
                ,
            HourlyRateHoursPerDay
                , HourlyRateDaysPerMonth
        }

        public enum SalaryItemSalaryAddtionType
        {
            Addtion = 0,
            Deduction = 1
        }

        public enum SalaryItemSalaryType
        {
            BasicSalary = 0,
            Overtime = 1,
            Nopay = 2,
            Taxes = 3,
            Standard = 4,
            Claims = 5,
            Loans = 6,
            Training = 7,
            SalaryAdvance = 8,
            Cumulative = 9,
            PRC=10
        }

        public enum ContactType
        {
            SegmentInfo = 1,
            EmployeeInfo = 2
        }

        public enum SentMessageTypes
        {
            Internal = 0,
            SMS = 2,
            Email = 1
        }

        public enum FileUploadLocation
        {
            EmployeeMaster = 1,
            LoanManagement = 2,
            Claims = 3,
            ApplicationManagement_Image = 5,
            ApplicationManagement_Doc = 4
        }

        public enum FilterType
        {
            stringField = 0,
            NumericField = 1
        }

        public enum DayValueType
        {
            Days = 0,
            Hours = 1
        }

        public enum AppraisalCommentType
        {
            PlanLevels = 0
        }


        public enum Direction
        { Up, Down, Last, First }

        /// <summary>
        /// returns employee object
        /// </summary>
        /// <returns></returns>
        public static Employee getEmployeeDatils(int EmployeeNo)
        {
            Employee _objEmployee = new Employee();
            try
            {
                _objEmployee = new Employee(_globalUser.LoginUser.UserID);
                _objEmployee.SegmentID = _globalUser.LoginUser.SegmentID;
                _objEmployee.EmployeeNo = EmployeeNo;
                _objEmployee.GetByID();
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }

            return _objEmployee;
        }


        /// <summary>
        /// returns Employee contact information
        /// </summary>
        /// <returns></returns>
        //public static string getEmployeeContactDetails(string EmployeeNo, ContactBookEntryType ContactType)
        //{
        //    string _ConectValue = string.Empty;
        //    try
        //    {
        //        ContactBook _objContactBook = new ContactBook();
        //        _objContactBook.SegmentID = _globalUser.LoginUser.SegmentID;
        //        _objContactBook.IsEmergencyNo = true;
        //        _objContactBook.EntityID = int.Parse(EmployeeNo);
        //        _objContactBook.ContactType = (int)ContactType;
        //        _objContactBook.GetByID();
        //        if (_objContactBook.ContactValue != string.Empty)
        //        {
        //            _ConectValue = _objContactBook.ContactValue;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _globalUser.ErrorObject = ex;
        //    }

        //    return _ConectValue;
        //}





        /// <summary>
        /// Checks the security status of the option activeed in the form
        /// </summary>
        /// <param name="FormName"></param>
        /// <param name="ActiveOption"></param>
        /// <returns></returns>
        //public static bool CheckOptionAuthorization(string FormName, FormOptions ActiveOption, int LoginUserID, bool CheckOptionName = false)
        //{
        //    bool _blnOptionSatus = false;
        //    int _FormID = -1;
        //    SecurityVersions _objSecurityVersion = new SecurityVersions();
        //    vw_OptionTree _objOptionTree = new vw_OptionTree();
        //    DataSet _dsOption = null;
        //    try
        //    {

        //        _objSecurityVersion.Active = true;
        //        _objSecurityVersion.IsHRMS = true;
        //        _objSecurityVersion.GetByID();
        //        if (_objSecurityVersion.VersionID != -1)
        //        {
        //            _objOptionTree.VersionID = _objSecurityVersion.VersionID;
        //            if (CheckOptionName == false) // this is the default
        //            {
        //                _objOptionTree.FrmName = FormName.Trim();
        //            }
        //            else
        //            { // this is for employee master tab options
        //                _objOptionTree.OptionName = FormName.Trim();
        //            }
        //            _objOptionTree.GetByID();

        //            if (_objOptionTree.FormID != -1)
        //            {
        //                //_FormID = _objOptionTree.FormID;  
        //                Modules _objModule = new Modules();
        //                _objModule.UserID = LoginUserID;
        //                _objModule.OptionID = _objOptionTree.OptionID;
        //                _objModule.VersionID = _objSecurityVersion.VersionID;
        //                _objModule.GetByID();
        //                switch (ActiveOption)
        //                {
        //                    case FormOptions.Add:
        //                        _blnOptionSatus = _objModule.OptionAdd.Value;
        //                        break;
        //                    case FormOptions.Delete:
        //                        _blnOptionSatus = _objModule.OptionDelete.Value;
        //                        break;
        //                    case FormOptions.Print:
        //                        _blnOptionSatus = _objModule.OptionPrint.Value;
        //                        break;
        //                    case FormOptions.Save:
        //                        _blnOptionSatus = _objModule.OptionSave.Value;
        //                        break;
        //                    case FormOptions.Visible:
        //                        _blnOptionSatus = _objModule.OptionVisible.Value;
        //                        break;
        //                }
        //                //------------------------ updates the audit trail with activated option ----------------------------
        //                if (_blnOptionSatus == true)
        //                {
        //                    SecurityAuditTrailOptions _objAudittrail = new SecurityAuditTrailOptions();
        //                    _objAudittrail.UserID = _globalUser.LoginUser.UserID;
        //                    _objAudittrail.UseDateTime = DateTime.Now;
        //                    _objAudittrail.OptionID = _objOptionTree.OptionID;
        //                    _objAudittrail.OptionFacilityID = (int)ActiveOption;
        //                    _objAudittrail.Save();// save audit trail .... 
        //                }
        //                //-----------------------------------------------------------------------------------------------
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _globalUser.ErrorObject = ex;
        //    }
        //    return _blnOptionSatus;
        //}

        /// <summary>
        /// returns a dataset filterd for user security filters 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static DataSet getSecurityFilteredEmployees(clsGobalUser _global)
        {
            DataSet _ds = new DataSet();
            try
            {
                Employee _objEmployee = new Employee(_global.LoginUser.UserID);
                //_objEmployee.SegmentID = _global.LoginUser.SegmentID;
                _ds = _objEmployee.GetList();// dataset 
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }
            return _ds;
        }


        /// <summary>
        /// Filters datasets for the security employees 
        /// </summary>
        /// <param name="EmployeeDataSet">Employee dataset</param>
        /// <param name="DatasetToFilter">Filter dataset</param>
        /// <returns></returns>
        public static DataSet getSecurityfilteredDataset(DataSet EmployeeDataSet, DataSet DatasetToFilter)
        {
            DataSet _ds = DatasetToFilter.Clone();
            bool _blnIsEmployeeNoPresent = false;
            try
            {
                if (DatasetToFilter.Tables.Count > 0)
                {
                    foreach (DataColumn item in DatasetToFilter.Tables[0].Columns)
                    {
                        if (item.ColumnName.ToUpper() == "EMPLOYEENO")
                        {
                            _blnIsEmployeeNoPresent = true;
                            break;
                        }
                    }
                }

                if (_blnIsEmployeeNoPresent == true)
                {
                    //-----------------------------------------------------
                    foreach (DataRow item in EmployeeDataSet.Tables[0].Rows)
                    {
                        //-------------------------------------------------------------------------
                        DataRow[] _rows = DatasetToFilter.Tables[0].Select("employeeno = " + item["employeeno"].ToString());
                        foreach (DataRow row in _rows)
                        {
                            _ds.Tables[0].ImportRow(row);
                        }
                        //-------------------------------------------------------------------------
                    }
                    //-----------------------------------------------------
                }
                else
                {
                    _ds = DatasetToFilter;
                }
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }

            return _ds;// returns the clone of the original dataset 
        }



        /// <summary>
        /// gets the day information 
        /// </summary>
        /// <param name="CheckDate">the date to break down into week and dayofweek</param>
        /// <returns></returns>
        public static DayInfo getDayInfo(DateTime CheckDate)
        {
            DayInfo _day;
            _day.WeekNumber = Math.Abs(CheckDate.Day / 7) + 1;
            _day.DayName = CheckDate.DayOfWeek.ToString();
            return _day;
        }

        /// <summary>
        /// returns the record count of the dataset 
        /// </summary>
        /// <param name="_ds"></param>
        /// <returns></returns>
        public static double CheckDatasetRecordCount(DataSet _ds)
        {
            double _intCount = 0;
            try
            {
                if (_ds != null)
                {
                    if (_ds.Tables.Count > 0)
                    {
                        _intCount = _ds.Tables[0].Rows.Count;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return _intCount;
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
                _ds = _objCommonSqlDataset.GetList(Sql);
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
            return _ds;
        }


        public static DataSet getSqlDataset(string Sql, string _server, string _database, string _userID, string _password)
        {
            DataSet _ds = null;
            try
            {
                _ds = _objCommonSqlDataset.GetList(Sql, _server, _database, _userID, _password);
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
            return _ds;
        }

        /// <summary>
        /// gets a dataset from a sql command strinng
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public static DataSet getSqlStoredProcedureDatasetExternalDB(string StoredProdprocedure, List<CommonFunctions.ColumnDetails> ParameterDetails, int executionType, string _server, string _database, string _userID, string _password)
        {
            DataSet _ds = null;
            try
            {
                //-------------------------------------------------------------
                string _sql = string.Format("set Dateformat dmy Execute {0} ", StoredProdprocedure);
                string _paramters = "";
                //---------------------------------------------------------------------------
                foreach (CommonFunctions.ColumnDetails item in ParameterDetails)
                {
                    //-----------------------------------------------------------------------------
                    if (CommonFunctions.getDataType(item._ColumnDataType) == ColumnDataType.NumericColum || CommonFunctions.getDataType(item._ColumnDataType) == ColumnDataType.BitColumn)
                    {
                        if (_paramters.Length <= 0)
                        {
                            _paramters += item._ColumnValue.ToString();
                        }
                        else
                        {
                            _paramters += "," + item._ColumnValue.ToString();
                        }
                    }
                    //-----------------------------------------------------------------------------
                    if (CommonFunctions.getDataType(item._ColumnDataType) == ColumnDataType.DateColumn)
                    {
                        DateTime _dt = (DateTime)item._ColumnValue;
                        if (_paramters.Length <= 0)
                        {
                            _paramters += "'" + _dt.ToString("yyyy-MM-dd") + "'";
                        }
                        else
                        {
                            _paramters += ",'" + _dt.ToString("yyyy-MM-dd") + "'";
                        }
                    }
                    //-----------------------------------------------------------------------------
                    if (CommonFunctions.getDataType(item._ColumnDataType) == ColumnDataType.StringColumn)
                    {
                        //DateTime _dt = (DateTime)item._ColumnValue;
                        if (_paramters.Length <= 0)
                        {
                            _paramters += "'" + item._ColumnValue.ToString() + "'";
                        }
                        else
                        {
                            _paramters += ",'" + item._ColumnValue.ToString() + "'";
                        }
                    }
                    //-----------------------------------------------------------------------------
                }
                //---------------------------------------------------------------------------
                if (executionType == 1)
                {
                    _ds = _objCommonSqlDataset.ExecuteStoredProcedureTimeLimiter(_sql + _paramters, _server, _database, _userID, _password);
                }
                else
                {
                    _ds = _objCommonSqlDataset.ExecuteStoredProcedure(_sql + _paramters);
                }
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }
            return _ds;
        }

        /// <summary>
        /// gets a dataset from a sql command strinng
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public static DataSet getSqlStoredProcedureDataset(string StoredProdprocedure, List<CommonFunctions.ColumnDetails> ParameterDetails)
        {
            DataSet _ds = null;
            try
            {
                //-------------------------------------------------------------
                string _sql = string.Format("set Dateformat dmy Execute {0} ", StoredProdprocedure);
                string _paramters = "";
                //---------------------------------------------------------------------------
                foreach (CommonFunctions.ColumnDetails item in ParameterDetails)
                {
                    //-----------------------------------------------------------------------------
                    if (CommonFunctions.getDataType(item._ColumnDataType) == ColumnDataType.NumericColum || CommonFunctions.getDataType(item._ColumnDataType) == ColumnDataType.BitColumn)
                    {
                        if (_paramters.Length <= 0)
                        {
                            _paramters += item._ColumnValue.ToString();
                        }
                        else
                        {
                            _paramters += "," + item._ColumnValue.ToString();
                        }
                    }
                    //-----------------------------------------------------------------------------
                    if (CommonFunctions.getDataType(item._ColumnDataType) == ColumnDataType.DateColumn)
                    {
                        DateTime _dt = (DateTime)item._ColumnValue;
                        if (_paramters.Length <= 0)
                        {
                            _paramters += "'" + _dt.ToString("dd/MM/yyyy") + "'";
                        }
                        else
                        {
                            _paramters += ",'" + _dt.ToString("dd/MM/yyyy") + "'";
                        }


                    }
                    //-----------------------------------------------------------------------------
                    if (CommonFunctions.getDataType(item._ColumnDataType) == ColumnDataType.StringColumn)
                    {
                        //DateTime _dt = (DateTime)item._ColumnValue;
                        if (_paramters.Length <= 0)
                        {
                            _paramters += "'" + item._ColumnValue.ToString() + "'";
                        }
                        else
                        {
                            _paramters += ",'" + item._ColumnValue.ToString() + "'";
                        }
                    }
                    //-----------------------------------------------------------------------------
                }
                //---------------------------------------------------------------------------
                _ds = _objCommonSqlDataset.ExecuteStoredProcedure(_sql + _paramters);
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
            return _ds;
        }

        /// <summary>
        /// Get SAP Account Tree
        /// </summary>
        /// <returns></returns>        
        //public static SAPbobsCOM.Recordset getSAPAccountsDataSet(ServerType LinkedServerType, string Password, string UserName, string Server, string CompanyDB, string SAPDBUserName, string SAPDBPassword, string LicenseServer) //
        //{
        //    SAPbobsCOM.Recordset _dsACC = null;
        //    Company _objCompany = new Company();
        //    try
        //    {
        //        SAPbobsCOM.Recordset _dsRecordSet;
        //        if (LinkedServerType == ServerType.Server2008)
        //        {
        //            _objCompany.DbServerType = BoDataServerTypes.dst_MSSQL2008;
        //        }

        //        if (LinkedServerType == ServerType.Server2012)
        //        {
        //            _objCompany.DbServerType = BoDataServerTypes.dst_MSSQL2012;
        //        }

        //        if (LinkedServerType == ServerType.Server2014)
        //        {
        //            _objCompany.DbServerType = BoDataServerTypes.dst_MSSQL2014;
        //        }
        //        if (LinkedServerType == ServerType.Server2016)
        //        {
        //            _objCompany.DbServerType = BoDataServerTypes.dst_MSSQL2016;
        //        }

        //        _objCompany.Server = Server;
        //        _objCompany.LicenseServer = LicenseServer;
        //        _objCompany.UserName = UserName;
        //        _objCompany.Password = Password;
        //        _objCompany.language = SAPbobsCOM.BoSuppLangs.ln_English;
        //        _objCompany.UseTrusted = false;
        //        _objCompany.CompanyDB = CompanyDB;
        //        _objCompany.DbUserName = SAPDBUserName;
        //        _objCompany.DbPassword = SAPDBPassword;

        //        SAPbobsCOM.SBObob oObj;
        //        SAPbobsCOM.Recordset rs;

        //        int lRetCode = _objCompany.Connect();

        //        if (lRetCode != 0)
        //        {
        //            //MessageBox.Show(lRetCode.ToString());
        //            CommonFunctions.MessageInformation(_objCompany.GetLastErrorCode() + "-" + _objCompany.GetLastErrorDescription() + "-" + "Please Check the Connection For SAP");
        //        }

        //        oObj = (SAPbobsCOM.SBObob)_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge);
        //        rs = (SAPbobsCOM.Recordset)_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

        //        // Set the Customer Name and Customer Code Combo Boxes
        //        _dsACC = (SAPbobsCOM.Recordset)_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);//SAPbobsCOM.Recordset 
        //        _dsACC.DoQuery("Select * from OACT");
        //        _dsACC.MoveFirst();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return _dsACC;
        //}

        /// <summary>
        /// Get SAP branch Tree
        /// </summary>
        /// <returns></returns>        
        //public static SAPbobsCOM.Recordset getSAPBranchDataSet(ServerType LinkedServerType, string Password, string UserName, string Server, string CompanyDB, string SAPDBUserName, string SAPDBPassword, string LicenseServer) //
        //{
        //    SAPbobsCOM.Recordset _dsACC = null;
        //    Company _objCompany = new Company();
        //    try
        //    {
        //        SAPbobsCOM.Recordset _dsRecordSet;
        //        if (LinkedServerType == ServerType.Server2008)
        //        {
        //            _objCompany.DbServerType = BoDataServerTypes.dst_MSSQL2008;
        //        }

        //        if (LinkedServerType == ServerType.Server2012)
        //        {
        //            _objCompany.DbServerType = BoDataServerTypes.dst_MSSQL2012;
        //        }

        //        if (LinkedServerType == ServerType.Server2014)
        //        {
        //            _objCompany.DbServerType = BoDataServerTypes.dst_MSSQL2014;
        //        }
        //        if (LinkedServerType == ServerType.Server2016)
        //        {
        //            _objCompany.DbServerType = BoDataServerTypes.dst_MSSQL2016;
        //        }

        //        _objCompany.Server = Server;
        //        _objCompany.LicenseServer = LicenseServer;
        //        _objCompany.UserName = UserName;
        //        _objCompany.Password = Password;
        //        _objCompany.language = SAPbobsCOM.BoSuppLangs.ln_English;
        //        _objCompany.UseTrusted = false;
        //        _objCompany.CompanyDB = CompanyDB;
        //        _objCompany.DbUserName = SAPDBUserName;
        //        _objCompany.DbPassword = SAPDBPassword;

        //        SAPbobsCOM.SBObob oObj;
        //        SAPbobsCOM.Recordset rs;

        //        int lRetCode = _objCompany.Connect();

        //        if (lRetCode != 0)
        //        {
        //            CommonFunctions.MessageInformation(_objCompany.GetLastErrorCode() + "-" + _objCompany.GetLastErrorDescription() + "-" + "Please Check the Connection For SAP");
        //        }

        //        oObj = (SAPbobsCOM.SBObob)_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge);
        //        rs = (SAPbobsCOM.Recordset)_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

        //        // Set the Customer Name and Customer Code Combo Boxes
        //        _dsACC = (SAPbobsCOM.Recordset)_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);//SAPbobsCOM.Recordset 
        //        _dsACC.DoQuery("Select * from OBPL");
        //        _dsACC.MoveFirst();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return _dsACC;
        //}


        //public static string MailReport(XtraReport1 report, string ReportName, string ToMailID, string ToMailRecepient, string Subject, string MailBody)
        //{
        //    string _returnMessage = "Message Sent to " + ToMailID + " - " + ToMailRecepient;
        //    try
        //    {

        //        // Create a new memory stream and export the report into it as PDF.
        //        MemoryStream mem = new MemoryStream();
        //        report.ExportToPdf(mem);

        //        // Create a new attachment and put the PDF report into it.
        //        mem.Seek(0, System.IO.SeekOrigin.Begin);
        //        System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(mem, ReportName.Trim() + ".pdf", "application/pdf");

        //        HRMS.BusinessLayer.SystemSettingEmail _objMailSettings = new SystemSettingEmail();
        //        mem.Dispose();
        //        _objMailSettings.GetByID();

        //        using (MailMessage mail = new MailMessage())
        //        {
        //            mail.From = new MailAddress(_objMailSettings.EmailID);
        //            mail.To.Add(new MailAddress(ToMailID));
        //            mail.Body = MailBody;
        //            mail.BodyEncoding = System.Text.Encoding.UTF8;
        //            mail.Subject = Subject;
        //            mail.SubjectEncoding = System.Text.Encoding.UTF8;
        //            mail.IsBodyHtml = false;
        //            // Can set to false, if you are sending pure text.
        //            mail.Attachments.Add(att);
        //            //using (SmtpClient smtp = new SmtpClient(_objMailSettings.URL ,_objMailSettings.Port)) //  smtpAddress, portNumber))
        //            //{
                  

        //            SmtpClient client = new SmtpClient();
        //            client.Port = _objMailSettings.Port;
        //            client.Host = _objMailSettings.URL;
        //            client.EnableSsl = _objMailSettings.SSL.Value;                    
        //            client.Timeout = 50000;
        //            client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //            client.UseDefaultCredentials = false;
        //            //client.Credentials = new System.Net.NetworkCredential(_objMailSettings.EmailID, "Myp@!654");
        //            client.Credentials = new System.Net.NetworkCredential(_objMailSettings.EmailID, _objMailSettings.Password);

        //            //client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
        //            //string userstate = "Sending...";
        //            ////Send email async
        //            //client.SendAsync(mail, userstate);
                   
        //            client.Send(mail);
        //            client.Dispose();
        //            //mail.Dispose();
        //            //SmtpClient client = new SmtpClient(_objMailSettings.URL, 587);
        //            //client.Credentials = new System.Net.NetworkCredential(_objMailSettings.UserName, _objMailSettings.Password);
        //            //client.EnableSsl = true;
        //            //client.Send(mail);




        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(this, "Error sending a report.\n" + ex.ToString());
        //        _returnMessage = "Message Sending error - " + ToMailID + " - " + ToMailRecepient;
        //    }
        //    return _returnMessage;

        //}

        //public static string MailReportwithConfigSMTP(XtraReport1 report, string ReportName, string ToMailID, string ToMailRecepient, string Subject, string MailBody,bool Settings)
        //{
        //    string _returnMessage = "Message Sent to " + ToMailID + " - " + ToMailRecepient;
        //    try
        //    {
        //        string securityProtocol = ConfigurationManager.AppSettings["SecurityProtocol"].ToString();
        //        // Create a new memory stream and export the report into it as PDF.
        //        MemoryStream mem = new MemoryStream();
                
        //        report.ExportToPdf(mem);

        //        //report.ExportToPdf(System.Environment.CurrentDirectory +"\\"+ ReportName.Trim() + ".pdf");
                
        //        // Create a new attachment and put the PDF report into it.
        //        mem.Seek(0, System.IO.SeekOrigin.Begin);
        //        System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(mem,ReportName.Trim() + ".pdf", "application/pdf");
        //        //System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(@"E:\\test123.txt");
                
        //        HRMS.BusinessLayer.SystemSettingEmail _objMailSettings = new SystemSettingEmail();
        //        //mem.Dispose();
        //        _objMailSettings.GetByID();



        //        using (MailMessage mail = new MailMessage())
        //        {
        //            //mail.From = new MailAddress(_objMailSettings.EmailID);
        //            mail.To.Add(new MailAddress(ToMailID));
        //            mail.Body = MailBody;
        //            //mail.BodyEncoding = System.Text.Encoding.UTF8;
        //            mail.Subject = Subject;
        //            //mail.SubjectEncoding = System.Text.Encoding.UTF8;
        //            mail.IsBodyHtml = false;
        //            // Can set to false, if you are sending pure text.
        //            mail.Attachments.Add(att);
        //            //using (SmtpClient smtp = new SmtpClient(_objMailSettings.URL ,_objMailSettings.Port)) //  smtpAddress, portNumber))
        //            //{
        //            SmtpClient client = new SmtpClient();
        //            if (!Settings)
        //            {
        //                client.Port = _objMailSettings.Port;
        //                client.Host = _objMailSettings.URL;
        //                client.EnableSsl = _objMailSettings.SSL.Value;
                        
        //                client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //                client.UseDefaultCredentials = false;
        //                //client.Credentials = new System.Net.NetworkCredential(_objMailSettings.EmailID, "Myp@!654");
        //                client.Credentials = new System.Net.NetworkCredential(_objMailSettings.EmailID, _objMailSettings.Password);
        //            }
        //            client.Timeout = 50000;
        //            System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)Enum.Parse(typeof(System.Net.SecurityProtocolType), securityProtocol);                  
        //            client.Send(mail);
        //            client.Dispose();
        //            //mail.Dispose();
        //            //SmtpClient client = new SmtpClient(_objMailSettings.URL, 587);
        //            //client.Credentials = new System.Net.NetworkCredential(_objMailSettings.UserName, _objMailSettings.Password);
        //            //client.EnableSsl = true;
        //            //client.Send(mail);




        //            //}
        //        }

                
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error sending a report.\n" + ex.ToString());
        //        //_returnMessage = "Message Sending error - " + ToMailID + " - " + ToMailRecepient;
        //        //MessageBox.Show(this, "Error sending a report.\n" + ex.ToString());
        //    }
        //    return _returnMessage;

        //}

        //public static string MailReportNew(XtraReport1 report, string ReportName, string ToMailID, string ToMailRecepient, string Subject, string MailBody,string Path,bool systemSettingsSmTp)
        //{
        //    string _returnMessage = "Message Sent to " + ToMailID + " - " + ToMailRecepient;
        //    try
        //    {

        //        // Create a new memory stream and export the report into it as PDF.
        //        MemoryStream mem = new MemoryStream();
        //        report.ExportToPdf(mem);

        //        // Create a new attachment and put the PDF report into it.
        //        mem.Seek(0, System.IO.SeekOrigin.Begin);
        //        //System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(mem, ReportName.Trim() + ".pdf");

        //        HRMS.BusinessLayer.SystemSettingEmail _objMailSettings = new SystemSettingEmail();
        //        mem.Dispose();
        //        _objMailSettings.GetByID();

        //        MailMessage message = new MailMessage();
        //        SmtpClient smtp = new SmtpClient();
        //        message.From = new MailAddress(_objMailSettings.EmailID.ToString().Trim());
        //        message.To.Add(new MailAddress(ToMailID.ToString().Trim()));
        //        message.Subject = Subject;
        //        message.IsBodyHtml = true; //to make message body as html  
        //        message.Body = MailBody;
               



        //        smtp.Port = _objMailSettings.Port;
        //        smtp.Host = _objMailSettings.URL.ToString().Trim();
        //        smtp.EnableSsl = true;
        //        smtp.UseDefaultCredentials = false;
        //        smtp.Credentials = new NetworkCredential(_objMailSettings.EmailID.ToString().Trim(), "Myp@!654");
        //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtp.EnableSsl = true;

        //        smtp.Send(message);
        //        message.Dispose();
        //        smtp.Dispose();
                
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(this, "Error sending a report.\n" + ex.ToString());
        //        _returnMessage = "Message Sending error - " + ToMailID + " - " + ToMailRecepient;
        //    }
        //    return _returnMessage;

        //}

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.    
            String token = (string)e.UserState;
            if (e.Cancelled)
            {
                MessageBox.Show("Mail Cancelled");    
            }
            if (e.Error != null)
            {
                //Console.WriteLine("[{0}] {1}", token, e.Error.ToString());    
            }
            else
            {
                MessageBox.Show("Mail Sent Successfully");    
            }
        }  

        /// <summary>
        /// Check the existancce of a key value of a key field in the specified table to check existance in record removal 
        /// </summary>
        /// <param name="tableName">table which contains the id field</param>
        /// <param name="field">ID field </param>
        /// <param name="ID">ID value</param>
        /// <param name="whereCondition"> if additiona where condition exist ( Default is empty)</param>
        /// <returns>true - if ID is present / false if ID is not present </returns>
        public static bool CheckExistancePrimaryKeyInTable(string tableName, string field, string ID, string whereCondition)
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
                _globalUser.ErrorObject = ex;
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
                _globalUser.ErrorObject = ex;
                _blnRecordsExist = false;
            }
            return _blnRecordsExist;
        }

        /// <summary>
        /// returns the IP address of the workstation
        /// </summary>
        /// <returns></returns>
        public static string getIPAddress()
        {
            string _ipAddress = string.Empty;
            var _host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var IP in _host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    _ipAddress = IP.ToString();
                }
            }
            return _ipAddress;
        }


        /// <summary>
        /// finds the center for center poping controls 
        /// </summary>
        /// <param name="SourceForm"></param>
        /// <param name="BaseControl"></param>
        /// <returns></returns>
        public static System.Drawing.Point findCenter(Form SourceForm, System.Windows.Forms.Control BaseControl)
        {
            System.Drawing.Point _centerPoint = BaseControl.Location;
            try
            {
                _centerPoint = new Point(
                     SourceForm.ClientSize.Width / 2 - BaseControl.Size.Width / 2,
                     SourceForm.ClientSize.Height / 2 - BaseControl.Size.Height / 2);
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
            return _centerPoint;
        }

        /// <summary>
        /// returns a image object from a file path
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static Image getImageFromFile(string FilePath)
        {
            Image _ImageFile = null;
            try
            {
                _ImageFile = Image.FromFile(FilePath);
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
            return _ImageFile;

        }

        /// <summary>
        /// Allows checking of status of approvals
        /// </summary>
        /// <param name="ApprovalID"></param>
        ///// <returns></returns>
        //public static Approval CheckApprovalStatus(int ApprovalID)
        //{
        //    Approval _Status = new Approval();
        //    try
        //    {
        //        if (ApprovalID < 0)
        //        {
        //            _Status.Status = ApprovalStatus.Error;
        //        }
        //        else
        //        {
        //            ApprovalHeader _objApprovalHeader = new ApprovalHeader();
        //            _objApprovalHeader.ApprovalID = ApprovalID;
        //            _objApprovalHeader.GetByID();
        //            if (_objApprovalHeader.EmployeeNo != -1)
        //            {
        //                _Status.AppliedByEmployeeNo = _objApprovalHeader.EmployeeNo;
        //                _Status.CreateTime = _objApprovalHeader.CreateDateTime;
        //                _Status.TransactionID = _objApprovalHeader.TransactionID;
        //                _Status.Status = ApprovalStatus.ApprovalPending;
        //                if (_objApprovalHeader.Rejected == true)
        //                {
        //                    _Status.Status = ApprovalStatus.Rejected;
        //                    _Status.RejectedDate = _objApprovalHeader.RejectedTime;
        //                    _Status.RejectedReason = _objApprovalHeader.RejectedReason;
        //                }
        //                if (_objApprovalHeader.FinalApproval == true)
        //                {
        //                    _Status.Status = ApprovalStatus.FinalApproved;
        //                    _Status.FinalApprovalDate = _objApprovalHeader.FinalApprovalDate;
        //                }

        //                if (_Status.Status == ApprovalStatus.ApprovalPending)
        //                {
        //                    ApprovalDetail _objDetails = new ApprovalDetail();
        //                    _objDetails.ApprovalID = ApprovalID;
        //                    _objDetails.Approved = true;
        //                    _objDetails.GetByID();
        //                    if (_objDetails.ApprovalDetailID != -1)
        //                    {
        //                        _Status.Status = ApprovalStatus.ParticallyApproved;
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _globalUser.ErrorObject = ex;
        //    }
        //    return _Status;
        //}


        ///// <summary>
        ///// removes a approval
        ///// </summary>
        ///// <param name="ApprovalID"></param>
        ///// <returns></returns>
        //public static Approval RemoveApproval(int ApprovalID)
        //{
        //    Approval _Status = new Approval();
        //    try
        //    {
        //        Approval _objApproval = CheckApprovalStatus(ApprovalID);
        //        if (_objApproval.Status == ApprovalStatus.ApprovalPending)
        //        {
        //            ApprovalHeader _objApprovalHeader = new ApprovalHeader();
        //            _objApprovalHeader.ApprovalID = ApprovalID;
        //            _objApprovalHeader.Delete();// delete from header 

        //            ApprovalDetail _objApprovalDetail = new ApprovalDetail();
        //            _objApprovalDetail.ApprovalID = ApprovalID;
        //            _objApprovalDetail.Delete();// delete from header 
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _globalUser.ErrorObject = ex;
        //    }
        //    return _Status;
        //}

        ////Added by Prabath
        //public static Approval RemoveApprovalAdminPanel(int ApprovalID)
        //{
        //    Approval _Status = new Approval();
        //    try
        //    {
        //        Approval _objApproval = CheckApprovalStatus(ApprovalID);
        //        if (_objApproval.Status == ApprovalStatus.FinalApproved || _objApproval.Status == ApprovalStatus.ParticallyApproved || _objApproval.Status == ApprovalStatus.ApprovalPending)
        //        {
        //            ApprovalHeader _objApprovalHeader = new ApprovalHeader();
        //            _objApprovalHeader.ApprovalID = ApprovalID;
        //            _objApprovalHeader.Delete();// delete from header 

        //            ApprovalDetail _objApprovalDetail = new ApprovalDetail();
        //            _objApprovalDetail.ApprovalID = ApprovalID;
        //            _objApprovalDetail.Delete();// delete from header 
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _globalUser.ErrorObject = ex;
        //    }
        //    return _Status;
        //}


        /// <summary>
        /// returns a list of reports in the hrms 
        /// </summary>
        /// <param name="IsPayrollReport"></param>
        /// <returns></returns>
        //public static List<SystemReport> getReportList(bool IsPayrollReport)
        //{
        //    List<SystemReport> _lstReports = new List<SystemReport>();
        //    try
        //    {
        //        SystemReport _objReports = new SystemReport();
        //        _objReports.PayrollReport = IsPayrollReport;
        //        _lstReports = _objReports.GetObjectList();
        //    }
        //    catch (Exception ex)
        //    {
        //        _globalUser.ErrorObject = ex;
        //        //throw;
        //    }
        //    return _lstReports;
        //}

        /// <summary>
        /// builds a xml stream from a dataset
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="ElementName"></param>
        /// <returns></returns>
        public static string buildXMLFileFromDataset(DataSet ds, string ElementName)
        {
            string _strFile = string.Empty;
            //DataRow _dr=null;
            try
            {

                using (var memoryStream = new MemoryStream())
                {
                    using (TextWriter streamWriter = new StreamWriter(memoryStream))
                    {
                        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(DataSet));
                        xmlSerializer.Serialize(streamWriter, ds);
                        _strFile = Encoding.UTF8.GetString(memoryStream.ToArray());
                    }
                }

                // System.Xml.XmlDocument _xml = new XmlDocument();
                ////******************************************************
                //foreach (DataRow _dr in _ds.Tables[0].Rows)
                //{
                //    if (_dr != null)
                //    {
                //        XmlDocument doc = new XmlDocument();
                //        XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement(ElementName));
                //        foreach (DataColumn item in _dr.Table.Columns)
                //        {

                //            switch (CommonFunctions.getColumnType(_ds, item.ColumnName))
                //            {
                //                case ColumnDataType.NumericColum:
                //                    el.AppendChild(doc.CreateElement(item.ColumnName)).InnerText = CommonFunctions.getColumnValue(_ds, item.ColumnName, 0).ToString();
                //                    break;
                //                case ColumnDataType.StringColumn:
                //                    el.AppendChild(doc.CreateElement(item.ColumnName)).InnerText = CommonFunctions.getColumnValue(_ds, item.ColumnName, 0).ToString();
                //                    break;
                //                case ColumnDataType.DateColumn:
                //                    el.AppendChild(doc.CreateElement(item.ColumnName)).InnerText = CommonFunctions.getColumnValue(_ds, item.ColumnName, 0).ToString();
                //                    break;
                //                case ColumnDataType.BinaryColumn:
                //                    string result = System.Text.Encoding.UTF8.GetString(((byte[])CommonFunctions.getColumnValue(_ds, item.ColumnName, 0)));
                //                    el.AppendChild(doc.CreateElement(item.ColumnName)).InnerText = result;     
                //                    break;
                //                case ColumnDataType.BitColumn:
                //                    el.AppendChild(doc.CreateElement(item.ColumnName)).InnerText = ((bool)CommonFunctions.getColumnValue(_ds, item.ColumnName, 0)).ToString();
                //                    break;
                //                default:
                //                    break;
                //            }

                //        }
                //        _strFile = doc.OuterXml;
                //    }
                //}
                //******************************************************
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }
            return _strFile;
        }


        /// <summary>
        /// breaks down a calculation expression into its component parts 
        /// eg
        /// [abc]*[cdf]
        /// into 
        /// [abc]
        /// [cdf]
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static string[] ExpressionBreakdown(string Expression)
        {
            string[] _returnList = null;
            List<string> _lstExpression = new List<string>();
            string strExpression = "";
            try
            {

                foreach (char item in Expression.ToCharArray())
                {
                    if (item.ToString() == "[")
                    {
                        strExpression = item.ToString();
                    }
                    else
                    {
                        if (strExpression.Length > 0 && item.ToString() != "]")
                        {
                            strExpression = strExpression + item.ToString();
                        }
                    }
                    if (item.ToString() == "]" && strExpression.Length > 0)
                    {
                        strExpression = strExpression + item.ToString();
                        int _indexOfEntry = _lstExpression.FindIndex(x => x == strExpression);
                        if (_indexOfEntry <= -1)
                        { _lstExpression.Add(strExpression); }
                        strExpression = "";
                    }
                }
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
                //_lstErrors.Add(ex.Message + " Stack :" + ex.StackTrace.ToString());
            }
            if (_lstExpression.Count > 0)
            {
                _returnList = _lstExpression.ToArray();
            }
            return _returnList;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        /// <summary>
        /// returns the columns from a sql table or sql view 
        /// </summary>
        /// <param name="strTables"></param>
        /// <returns></returns>
        public static DataSet getTableinfo(string strTables)
        {
            DataSet ds;

            try
            {
                string sql = "SELECT COLUMN_NAME,DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + strTables + "'";
                ds = CommonFunctions.getSqlDataset(sql);
            }
            catch (Exception)
            {

                throw;
            }
            return ds;
        }

        /// <summary>
        /// returns the columns from a sql table or sql view 
        /// </summary>
        /// <param name="strTables"></param>
        /// <returns></returns>
        public static DataSet getTableinfoAllDetails(string strTables)
        {
            DataSet ds;

            try
            {
                string sql = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + strTables + "'";
                ds = CommonFunctions.getSqlDataset(sql);
            }
            catch (Exception)
            {

                throw;
            }
            return ds;
        }

        /// <summary>
        /// returns a list of columns of a table
        /// </summary>
        /// <param name="strTables"></param>
        /// <returns></returns>
        public static List<ColumnDetails> getColumnsDetails(string strTables)
        {
            List<ColumnDetails> _lstColDetails = new List<ColumnDetails>();
            try
            {
                DataSet _ds = getTableinfoAllDetails(strTables);
                if (_ds != null)
                {
                    foreach (DataRow item in _ds.Tables[0].Rows)
                    {
                        //COLUMN_NAME,DATA_TYPE
                        if (item["CHARACTER_MAXIMUM_LENGTH"] != null)
                        {
                            if (item["CHARACTER_MAXIMUM_LENGTH"].ToString().Trim() != string.Empty)
                            {
                                _lstColDetails.Add(new ColumnDetails(item["COLUMN_NAME"].ToString(), string.Format("{0}({1})", item["DATA_TYPE"].ToString(), item["CHARACTER_MAXIMUM_LENGTH"].ToString())));
                            }
                            else
                            {
                                _lstColDetails.Add(new ColumnDetails(item["COLUMN_NAME"].ToString(), item["DATA_TYPE"].ToString()));
                            }
                        }

                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
            return _lstColDetails;
        }

        //public static AttendanceDayType getCalenderDate(int EntryID, DateTime RosterDate, CelenderEntryType EntryType)
        //{
        //    AttendanceDayType _objDayType = null;
        //    try
        //    {
        //        AttendanceRosterCalender _objCalender = new AttendanceRosterCalender();
        //        _objCalender.EntryID = EntryID;
        //        _objCalender.EntryType = CelenderEntryType.WorkGroup == CelenderEntryType.WorkGroup ? 0 : 1;
        //        _objCalender.RosterDate = RosterDate.Date;
        //        _objCalender.GetByID();
        //        if (_objCalender.DayTypeID != -1)
        //        {
        //            _objDayType = new AttendanceDayType();
        //            _objDayType.SegmentID = _globalUser.LoginUser.SegmentID;
        //            _objDayType.DayTypeID = _objCalender.DayTypeID;
        //            _objDayType.GetByID();//
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _globalUser.ErrorObject = ex;
        //        //throw;
        //    }
        //    return _objDayType;
        //}

        public static void ExecuteNonSql(string Sql)
        {
            try
            {
                _objCommonSqlDataset.ExecuteNonSqlQuery(Sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public static Object ExecuteScalar(string Sql)
        {
            int LastInsert = -1;
            try
            {

               return _objCommonSqlDataset.ExecuteScalar(Sql);
            }
            catch (Exception ex)
            {
                throw;
                LastInsert = -1;
            }
        }



        /// <summary>
        /// returns the columns from a sql table or sql view 
        /// </summary>
        /// <param name="strStoredProcedure"></param>
        /// <returns></returns>
        public static DataSet getStoredProcedureinfo(string strStoredProcedure)
        {
            DataSet ds;

            try
            {
                string sql = "select PARAMETER_NAME as COLUMN_NAME,DATA_TYPE from information_schema.parameters where specific_name='" + strStoredProcedure + "' order by specific_name";
                ds = CommonFunctions.getSqlDataset(sql);
            }
            catch (Exception)
            {

                throw;
            }
            return ds;
        }

        /// <summary>
        /// Returns the ID if ID Value is not -1 ELSE returns null
        /// </summary>
        /// <param name="_Value"></param>
        /// <returns></returns>
        public static int? checkID(int _Value)
        {
            int? _intRet = null;
            if (_Value > -1)
            {
                _intRet = _Value;
            }
            return _intRet;
        }

        /// <summary>
        /// changes lable captions
        /// </summary>
        /// <param name="ControlsCollection"></param>
        /// <param name="_defualtCaption"></param>
        /// <param name="_newCaption"></param>
        public static void ChangeControlCaption(Control ControlsCollection, string _defualtCaption, string _newCaption)
        {
            try
            {
                foreach (Control _formControl in ControlsCollection.Controls)
                {
                    if (_formControl.Controls.Count > 0)
                    {
                        ChangeControlCaption(_formControl, _defualtCaption, _newCaption);
                    }
                    else
                    {
                        if (_formControl.GetType() == typeof(Label) || _formControl.GetType() == typeof(DevExpress.XtraEditors.LabelControl))
                        {
                            if (((Control)_formControl).Text.Trim().ToUpper() == _defualtCaption.Trim().ToUpper())
                            {
                                if (_formControl.GetType() == typeof(DevExpress.XtraEditors.LabelControl))
                                {
                                    int Left = ((LabelControl)_formControl).Left;
                                    int width = ((LabelControl)_formControl).Width;
                                    ((LabelControl)_formControl).Text = _newCaption;
                                    //((LabelControl)_formControl).Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
                                    if (((LabelControl)_formControl).Width <= width)
                                    {
                                        ((LabelControl)_formControl).Left = Left + width - ((LabelControl)_formControl).Width;
                                    }
                                    else
                                    {
                                        ((LabelControl)_formControl).Left = Left - (((LabelControl)_formControl).Width - width);
                                    }
                                }
                                else
                                {
                                    int Left = ((Label)_formControl).Left;
                                    int width = ((Label)_formControl).Width;
                                    ((Label)_formControl).Text = _newCaption;
                                    //((Label)_formControl).TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                    if (((Label)_formControl).Width <= width)
                                    {
                                        ((Label)_formControl).Left = Left + width - ((Label)_formControl).Width;
                                    }
                                    else
                                    {
                                        ((Label)_formControl).Left = Left - (((Label)_formControl).Width - width);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
        }


        /// <summary>
        /// Change lable captions 
        /// </summary>
        //public static void LoadCustomCaption(object _CaptionChangeObject, int _segmentID)
        //{
        //    DataSet _dsCaptions = null;
        //    bool isForm = false;
        //    GridView _gridControl = null;
        //    try
        //    {
        //        MasterCustomCaption _objCustomCaption = new MasterCustomCaption();
        //        _objCustomCaption.SegmentID = _segmentID;
        //        _dsCaptions = _objCustomCaption.GetList();

        //        try
        //        {
        //            _gridControl = (GridView)_CaptionChangeObject;
        //        }
        //        catch (Exception)
        //        {
        //            isForm = true;
        //            //throw;
        //        }


        //        if (_dsCaptions.Tables[0].Rows.Count > 0)
        //        {
        //            // change lable captions
        //            if (isForm == true)
        //            {
        //                foreach (DataRow item in _dsCaptions.Tables[0].Rows)
        //                {
        //                    if (((bool)item["IsColumnCaption"]) == false)
        //                    {
        //                        if (item["FormName"].ToString().Trim() == string.Empty)
        //                        {
        //                            ChangeControlCaption(((Control)_CaptionChangeObject), item["DefaultCaption"].ToString(), item["CustomCaption"].ToString());
        //                        }
        //                        else
        //                        {
        //                            if (item["FormName"].ToString().Trim().ToUpper() == ((Form)_CaptionChangeObject).Text.ToUpper())
        //                            {
        //                                ChangeControlCaption(((Control)_CaptionChangeObject), item["DefaultCaption"].ToString(), item["CustomCaption"].ToString());
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            /// change grids captions
        //            if (_CaptionChangeObject.GetType() == typeof(GridView))
        //            {
        //                //GridView grdView = (GridView)_gridControl;
        //                foreach (DataRow item in _dsCaptions.Tables[0].Rows)
        //                {
        //                    foreach (DevExpress.XtraGrid.Columns.GridColumn Column in _gridControl.Columns)
        //                    {
        //                        if (Column.ToString().Trim().ToUpper() == item["DefaultCaption"].ToString().Trim().ToUpper())
        //                        {
        //                            Column.Caption = item["CustomCaption"].ToString();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// return if the paycycle is locked or not
        /// </summary>
        /// <param name="SegmentID"></param>
        /// <param name="SalaryYearID"></param>
        /// <param name="MonthNo"></param>
        /// <param name="PayCycleID"></param>
        /// <param name="PayBlockID"></param>
        /// <returns></returns>
        //public static bool PayCycleBlockesStatus(int SegmentID, int SalaryYearID, int MonthNo, int PayCycleID, int PayBlockID)
        //{
        //    bool _blnLocked = false;
        //    try
        //    {
        //        SalaryYearMonthsPayCycles _objMonthlyPayCycle = new SalaryYearMonthsPayCycles();
        //        _objMonthlyPayCycle.SegmentID = SegmentID;
        //        _objMonthlyPayCycle.SalaryYearID = SalaryYearID;
        //        _objMonthlyPayCycle.MonthNo = MonthNo;
        //        _objMonthlyPayCycle.PayCycleID = PayCycleID;
        //        _objMonthlyPayCycle.PayCycleBlockID = PayBlockID;
        //        _objMonthlyPayCycle.GetByID();
        //        if (_objMonthlyPayCycle.PayBlockProcessConfirmUserID != -1)
        //        {
        //            _blnLocked = true;
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        _globalUser.ErrorObject = ex;
        //    }

        //    return _blnLocked;

        //}

        /// <summary>
        /// retuns the a list of segments that are the children of the parenet segment in the orgnizational scheme 
        /// </summary>
        /// <param name="parentSegmentID"></param>
        /// <returns></returns>
        public static List<OrganizationScheme> getSubSegments(int parentSegmentID)
        {
            List<OrganizationScheme> _lstSelectedElements = new List<OrganizationScheme>();
            try
            {
                OrganizationScheme _objOrgniazationSegments = new OrganizationScheme();
                _objOrgniazationSegments.ParentSegmentID = parentSegmentID;
                List<OrganizationScheme> _lstsegments = _objOrgniazationSegments.GetObjectList();

                if (_lstsegments.Count > 0)
                {
                    foreach (OrganizationScheme item in _lstsegments)
                    {
                        List<OrganizationScheme> _lstElements = getSubSegments(item.SegmentID);
                        if (_lstElements.Count > 0)
                        {
                            _lstSelectedElements.AddRange(_lstElements);
                        }
                        _lstSelectedElements.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }
            return _lstSelectedElements;
        }




        /// <summary>
        /// Luach employee filter 
        /// </summary>
        /// <param name="_GlobalUser"></param>
        /// <param name="PaySchemeId"></param>
        /// <param name="AttendanceSummeryId"></param>
        /// <returns></returns>
        //public static Employee[] LaunchEmployeeFilter(clsGobalUser _GlobalUser, int? PaySchemeId = null, int? AttendanceSummeryId = null)
        //{
        //    try
        //    {
        //        _globalUser = _GlobalUser;
        //        Enquires.FrmEmployeeFilter frmEmployees;
        //        if (PaySchemeId == null)
        //        {
        //            frmEmployees = new Enquires.FrmEmployeeFilter(_globalUser);
        //        }
        //        else
        //        {
        //            if (AttendanceSummeryId == null)
        //            { frmEmployees = new Enquires.FrmEmployeeFilter(_globalUser, PaySchemeId); }
        //            else
        //            { frmEmployees = new Enquires.FrmEmployeeFilter(_globalUser, PaySchemeId, AttendanceSummeryId); }

        //        }
        //        frmEmployees.ShowDialog();
        //        _SelectedEmployees = frmEmployees.getSelectedEmployees();
        //    }
        //    catch (Exception ex)
        //    {

        //        _globalUser.ErrorObject = ex;
        //    }
        //    if (_SelectedEmployees.Count > 0)
        //    { return _SelectedEmployees.ToArray(); }
        //    else
        //    {
        //        return new Employee[0];
        //    }
        //}

        //#region Overload method - LaunchEmployeeFilter - CSV 2019-02-13
        //public static Employee[] LaunchEmployeeFilter(clsGobalUser _GlobalUser, bool isSalaryHold, int? PaySchemeId = null, int? AttendanceSummeryId = null)
        //{
        //    try
        //    {
        //        _globalUser = _GlobalUser;
        //        Enquires.FrmEmployeeFilter frmEmployees;
        //        if (PaySchemeId == null)
        //        {
        //            frmEmployees = new Enquires.FrmEmployeeFilter(_globalUser);
        //        }
        //        else
        //        {
        //            if (AttendanceSummeryId == null)
        //            { frmEmployees = new Enquires.FrmEmployeeFilter(_globalUser, PaySchemeId); }
        //            else
        //            { frmEmployees = new Enquires.FrmEmployeeFilter(_globalUser, PaySchemeId, AttendanceSummeryId); }

        //        }
        //        frmEmployees.ShowDialog();
        //        _SelectedEmployees = new List<Employee>();
        //        _SelectedEmployees = frmEmployees.getSelectedEmployees();

        //        //csv 2019-02-08
        //        if (isSalaryHold)
        //        {
        //            MasterEmployeeSalaryHoldList _objMasterEmployeeSalaryHoldList = new MasterEmployeeSalaryHoldList();
        //            _lstSalaryHoldList = _objMasterEmployeeSalaryHoldList.GetObjectList();
        //            foreach (var item in _lstSalaryHoldList)
        //            {
        //                 _SelectedEmployees.Remove(_SelectedEmployees[_SelectedEmployees.FindIndex(m => m.EmployeeNo == item.EmployeeNo)]);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        _globalUser.ErrorObject = ex;
        //    }
        //    if (_SelectedEmployees.Count > 0)
        //    { return _SelectedEmployees.ToArray(); }
        //    else
        //    {
        //        return new Employee[0];
        //    }
        //}
        //#endregion

        //public static Employee[] LaunchEmployeeFilterOlDReport(clsGobalUser _GlobalUser, bool isSalaryHold, string SalaryYearID, string MonthNo, string paycycleID, string payblockID, int? PaySchemeId = null, int? AttendanceSummeryId = null)
        //{
        //    try
        //    {
        //        _globalUser = _GlobalUser;
        //        Enquires.FrmEmployeeFilter frmEmployees;
        //        if (PaySchemeId == null)
        //        {
        //            frmEmployees = new Enquires.FrmEmployeeFilter(_globalUser);
        //        }
        //        else
        //        {
        //            if (AttendanceSummeryId == null)
        //            { frmEmployees = new Enquires.FrmEmployeeFilter(_globalUser, PaySchemeId); }
        //            else
        //            { frmEmployees = new Enquires.FrmEmployeeFilter(_globalUser, PaySchemeId, AttendanceSummeryId); }

        //        }
        //        frmEmployees.ShowDialog();
        //        _SelectedEmployees = new List<Employee>();
        //        _SelectedEmployees = frmEmployees.getSelectedEmployees();

        //        //csv 2019-02-08
        //        if (isSalaryHold)
        //        {
        //            MasterEmployeeSalaryHoldList _objMasterEmployeeSalaryHoldList = new MasterEmployeeSalaryHoldList();
        //            _lstSalaryHoldList = _objMasterEmployeeSalaryHoldList.GetObjectListNew(_globalUser.LoginUser.SegmentID.ToString(), SalaryYearID,MonthNo,paycycleID,payblockID, isSalaryHold);
        //            foreach (var item in _lstSalaryHoldList)
        //            {
        //                _SelectedEmployees.Remove(_SelectedEmployees[_SelectedEmployees.FindIndex(m => m.EmployeeNo == item.EmployeeNo)]);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        _globalUser.ErrorObject = ex;
        //    }
        //    if (_SelectedEmployees.Count > 0)
        //    { return _SelectedEmployees.ToArray(); }
        //    else
        //    {
        //        return new Employee[0];
        //    }
        //}

        //public static Employee[] LaunchEmployeeFilterReportViwer(clsGobalUser _GlobalUser, bool isSalaryHold, int? PaySchemeId, string SalaryYearID, string MonthNo, string paycycleID, string payblockID)
        //{
        //    try
        //    {
        //        _globalUser = _GlobalUser;
        //        Enquires.FrmEmployeeFilter frmEmployees = null;
        //        if (PaySchemeId == null)
        //        {
        //            frmEmployees = new Enquires.FrmEmployeeFilter(_globalUser, SalaryYearID,MonthNo,paycycleID,payblockID);
        //        }                
        //        frmEmployees.ShowDialog();
        //        _SelectedEmployees = new List<Employee>();
        //        _SelectedEmployees = frmEmployees.getSelectedEmployees();

        //        //csv 2019-02-08
        //        if (isSalaryHold)
        //        {
        //            MasterEmployeeSalaryHoldList _objMasterEmployeeSalaryHoldList = new MasterEmployeeSalaryHoldList();
        //            _objMasterEmployeeSalaryHoldList.IsSalaryHold = true;                   
        //            //_objMasterEmployeeSalaryHoldList.IsSalaryHold = true;
        //            _lstSalaryHoldList = _objMasterEmployeeSalaryHoldList.GetObjectList();
        //            foreach (var item in _lstSalaryHoldList)
        //            {
        //                //_SelectedEmployees.Remove(_SelectedEmployees[_SelectedEmployees.FindIndex(m => m.EmployeeNo == item.EmployeeNo)]);
        //                _SelectedEmployees.RemoveAll(x => x.EmployeeNo == item.EmployeeNo);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        _globalUser.ErrorObject = ex;
        //    }
        //    if (_SelectedEmployees.Count > 0)
        //    { return _SelectedEmployees.ToArray(); }
        //    else
        //    {
        //        return new Employee[0];
        //    }
        //}

        //public static Employee[] LaunchEmployeeFilterTextFilePartial(clsGobalUser _GlobalUser, bool isSalaryHold, int? PaySchemeId, string SalaryYearID, string MonthNo, string paycycleID, string payblockID)
        //{
        //    try
        //    {
        //        _globalUser = _GlobalUser;
        //        Enquires.FrmEmployeeFilter frmEmployees = null;
        //        if (PaySchemeId == null)
        //        {
        //            frmEmployees = new Enquires.FrmEmployeeFilter(_globalUser, SalaryYearID, MonthNo, paycycleID, payblockID, 4);
        //        }
        //        frmEmployees.ShowDialog();
        //        _SelectedEmployees = new List<Employee>();
        //        _SelectedEmployees = frmEmployees.getSelectedEmployees();

        //        //csv 2019-02-08
        //        if (isSalaryHold)
        //        {
        //            MasterEmployeeSalaryHoldList _objMasterEmployeeSalaryHoldList = new MasterEmployeeSalaryHoldList();
        //            _objMasterEmployeeSalaryHoldList.IsSalaryHold = true;
        //            //_objMasterEmployeeSalaryHoldList.IsSalaryHold = true;
        //            _lstSalaryHoldList = _objMasterEmployeeSalaryHoldList.GetObjectList();
        //            foreach (var item in _lstSalaryHoldList)
        //            {
        //                //_SelectedEmployees.Remove(_SelectedEmployees[_SelectedEmployees.FindIndex(m => m.EmployeeNo == item.EmployeeNo)]);
        //                _SelectedEmployees.RemoveAll(x => x.EmployeeNo == item.EmployeeNo);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        _globalUser.ErrorObject = ex;
        //    }
        //    if (_SelectedEmployees.Count > 0)
        //    { return _SelectedEmployees.ToArray(); }
        //    else
        //    {
        //        return new Employee[0];
        //    }
        //}

        //public static Employee[] LaunchEmployeeFilterForApproval(clsGobalUser _GlobalUser, int? PaySchemeId = null, int? AttendanceSummeryId = null)
        //{
        //    try
        //    {
        //        _globalUser = _GlobalUser;
        //        Enquires.FrmEmployeeFilter frmEmployees;
        //        if (PaySchemeId == null)
        //        {
        //            frmEmployees = new Enquires.FrmEmployeeFilter(_globalUser,5,"Additional");
        //        }
        //        else
        //        {
        //            if (AttendanceSummeryId == null)
        //            { frmEmployees = new Enquires.FrmEmployeeFilter(_globalUser, PaySchemeId,5,"Additional"); }
        //            else
        //            { frmEmployees = new Enquires.FrmEmployeeFilter(_globalUser, PaySchemeId, AttendanceSummeryId,5,"Additional"); }

        //        }
        //        frmEmployees.ShowDialog();
        //        _SelectedEmployees = frmEmployees.getSelectedEmployees();
        //    }
        //    catch (Exception ex)
        //    {

        //        _globalUser.ErrorObject = ex;
        //    }
        //    if (_SelectedEmployees.Count > 0)
        //    { return _SelectedEmployees.ToArray(); }
        //    else
        //    {
        //        return new Employee[0];
        //    }
        //}
        
        
        
        /// <summary>
        /// returns the data type
        /// </summary>
        /// <param name="ColumnsDataType"></param>
        /// <returns></returns>
        public static ColumnDataType getDataType(string ColumnsDataType)
        {
            ColumnDataType _ColDataType = ColumnDataType.StringColumn;
            try
            {

                if (ColumnsDataType == "int" || ColumnsDataType == "nchar" || ColumnsDataType == "bigint" || ColumnsDataType == "nvarchar")
                {
                    _ColDataType = ColumnDataType.NumericColum;

                }

                if (ColumnsDataType == "binary" || ColumnsDataType == "varbinary")
                {
                    _ColDataType = ColumnDataType.BinaryColumn;

                }

                if (ColumnsDataType == "datetime" || ColumnsDataType == "date")
                {
                    _ColDataType = ColumnDataType.DateColumn;

                }

                if (ColumnsDataType == "bit")
                {
                    _ColDataType = ColumnDataType.BitColumn;

                }

            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }
            return _ColDataType;
        }

        /// <summary>
        /// returns the data type for the column of a dataset
        /// </summary>
        /// <param name="_dsSource"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static ColumnDataType getColumnType(DataSet _dsSource, string columnName)
        {
            ColumnDataType _datatype = ColumnDataType.BinaryColumn;
            try
            {
                var numericTypes = new[] { typeof(Byte), typeof(Decimal), typeof(Double),
                typeof(Int16), typeof(Int32), typeof(Int64), typeof(SByte),
                typeof(Single), typeof(UInt16), typeof(UInt32), typeof(UInt64)};

                var stringTypes = new[] { typeof(string) };


                if (_dsSource != null)
                {
                    if (_dsSource.Tables[0].Columns.Count > 0)
                    {
                        if (_dsSource.Tables[0].Columns.Contains(columnName) == true)
                        {
                            if (numericTypes.Contains(_dsSource.Tables[0].Columns[columnName].DataType) == true)
                            {
                                _datatype = ColumnDataType.NumericColum;
                            }

                            if (stringTypes.Contains(_dsSource.Tables[0].Columns[columnName].DataType) == true)
                            {
                                _datatype = ColumnDataType.StringColumn;
                            }

                            if (_dsSource.Tables[0].Columns[columnName].DataType == typeof(DateTime))
                            {
                                _datatype = ColumnDataType.DateColumn;
                            }


                            if (_dsSource.Tables[0].Columns[columnName].DataType == typeof(Boolean))
                            {
                                _datatype = ColumnDataType.BitColumn;
                            }

                            if (_dsSource.Tables[0].Columns[columnName].DataType == typeof(bool))
                            {
                                _datatype = ColumnDataType.BitColumn;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }
            return _datatype;
        }

        /// <summary>
        /// summerise a given columns values and returns the total for the datatset
        /// </summary>
        /// <param name="_dsSource"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static double getColumnSummeryValue(DataSet _dsSource, string columnName)
        {
            double _summeryValue = 0;
            try
            {
                if (_dsSource != null)
                {
                    if (_dsSource.Tables[0].Columns.Count > 0)
                    {
                        if (_dsSource.Tables[0].Columns.Contains(columnName) == true)
                        {
                            for (int i = 0; i < _dsSource.Tables[0].Rows.Count; i++)
                            {
                                _summeryValue = _summeryValue + double.Parse(_dsSource.Tables[0].Rows[i][columnName].ToString());
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }
            return _summeryValue;
        }

        /// <summary>
        /// returns a columns value from a dataset and column and row 
        /// </summary>
        /// <param name="_dsSource"></param>
        /// <param name="columnName"></param>
        /// <param name="rowindex"></param>
        /// <returns></returns>
        public static object getColumnValue(DataSet _dsSource, string columnName, int rowindex)
        {
            object _value = null;
            try
            {
                if (_dsSource != null)
                {
                    if (_dsSource.Tables[0].Rows.Count >= rowindex)
                        if (_dsSource.Tables[0].Columns.Count > 0)
                        {
                            if (_dsSource.Tables[0].Columns.Contains(columnName) == true)
                            {
                                for (int i = 0; i < _dsSource.Tables[0].Rows.Count; i++)
                                {
                                    if (i == rowindex)
                                    {
                                        _value = _dsSource.Tables[0].Rows[rowindex][columnName];
                                        break;
                                    }
                                }

                            }
                        }
                }
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }
            return _value;
        }

        /// <summary>
        /// used for warning messages in the application
        /// </summary>
        /// <param name="Message"></param>
        public static void MessageWarning(string Message)
        {
            MessageBox.Show(Message, "HRIS:Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// used for question messages in the application
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static DialogResult MessageQuestion(string Message)
        {
            return MessageBox.Show(Message, "HRIS:Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        /// <summary>
        /// used for information messages in the application
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static void MessageInformation(string Message)
        {
            MessageBox.Show(Message, "HRIS:Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// used for error messages in the application
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static void MessageError(string Message)
        {
            MessageBox.Show(Message, "HRIS:Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// used for validation failed messages in the application
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static void MessageValidationFailed(string Message)
        {
            MessageBox.Show(Message, "HRIS:Incorrect Entry", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        public static DevExpress.XtraEditors.TokenEditToken[] getTokens(DataSet _dsSource, string fieldName, string idFieldName)
        {
            List<DevExpress.XtraEditors.TokenEditToken> lstTokens = new List<TokenEditToken>();
            try
            {
                foreach (DataRow item in _dsSource.Tables[0].Rows)
                {
                    if (item != null)
                    {
                        if (lstTokens.IndexOf(new TokenEditToken(item[fieldName].ToString(), item[idFieldName])) <= 0)
                        {
                            lstTokens.Add(new TokenEditToken(item[fieldName].ToString(), item[idFieldName]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
            return lstTokens.ToArray<DevExpress.XtraEditors.TokenEditToken>();
        }

        public static DevExpress.XtraEditors.TokenEditToken[] getTokensNew(DataSet _dsSource, string fieldName, string idFieldName ,string SelectEmployee)
        {
            List<DevExpress.XtraEditors.TokenEditToken> lstTokens = new List<TokenEditToken>();
            try
            {
                DataView dv = new DataView(_dsSource.Tables[0]);
                dv.RowFilter = "EmployeeNo NOT IN (" + SelectEmployee + ")";
                DataTable dt = dv.ToTable();
                dv.RowFilter = "";
                if (SelectEmployee != "")
                {

                    foreach (DataRow item in dt.Rows)
                    {
                        if (item != null)
                        {
                            if (lstTokens.IndexOf(new TokenEditToken(item[fieldName].ToString(), item[idFieldName])) <= 0)
                            {
                                lstTokens.Add(new TokenEditToken(item[fieldName].ToString(), item[idFieldName]));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
            return lstTokens.ToArray<DevExpress.XtraEditors.TokenEditToken>();
        }

        /// <summary>
        /// Returns the employees Horking Hour information according to the employees PayProcess Type
        /// </summary>
        /// <param name="_objEmployee"></param>
        /// <param name="_DetailType"></param>
        //public static double? getEmployeeWorkingHourDetails(int _intEmployeeNo, PayProcessedTypes _DetailType)
        //{
        //    Employee _objEmployee = new Employee();
        //    double? Value = null;
        //    try
        //    {
        //        //--------------------------------------------
        //        _objEmployee.SegmentID = _globalUser.LoginUser.SegmentID;
        //        _objEmployee.EmployeeNo = _intEmployeeNo;
        //        _objEmployee.GetByID();
        //        //--------------------------------------------
        //        if (_objEmployee.PayProcessed != -1)
        //        {
        //            SystemSettingWorkingHours _objWorkingHours = new SystemSettingWorkingHours();
        //            _objWorkingHours.SegmentID = _globalUser.LoginUser.SegmentID;
        //            _objWorkingHours.WorkingHourID = _objEmployee.WorkingHourScheduleID;
        //            _objWorkingHours.GetByID();// ---------------------------------------
        //            if (_objWorkingHours.WorkingHourDescription != string.Empty)
        //            {
        //                switch (_DetailType)
        //                {
        //                    case PayProcessedTypes.MonthPerYear:
        //                        Value = _objWorkingHours.MonthPerYear;
        //                        break;
        //                    case PayProcessedTypes.MonthlyRateOTDaysPerMonth:
        //                        Value = _objWorkingHours.MR_OTDaysPerMonth;
        //                        break;
        //                    case PayProcessedTypes.MonthlyRateAbsentPerMonth:
        //                        Value = _objWorkingHours.MR_AbsentPerMonth;
        //                        break;
        //                    case PayProcessedTypes.MonthlyRateAdvancePerMonth:
        //                        Value = _objWorkingHours.MR_AdvancePerMonth;
        //                        break;
        //                    case PayProcessedTypes.MonthlyRateORPPerMonth:
        //                        Value = _objWorkingHours.MR_ORPPerMonth;
        //                        break;
        //                    case PayProcessedTypes.MonthlyRateHoursPerDay:
        //                        Value = _objWorkingHours.MR_HoursPerDay;
        //                        break;
        //                    case PayProcessedTypes.MonthlyRateHoursPerYear:
        //                        Value = _objWorkingHours.MR_HoursPerYear;
        //                        break;
        //                    case PayProcessedTypes.DailyRateHoursPerDay:
        //                        Value = _objWorkingHours.DR_HoursPerDay;
        //                        break;
        //                    case PayProcessedTypes.DailyRateDaysPerMonth:
        //                        Value = _objWorkingHours.DR_DaysPerMonth;
        //                        break;
        //                    case PayProcessedTypes.HourlyRateHoursPerDay:
        //                        Value = _objWorkingHours.HR_HoursPerDay;
        //                        break;
        //                    case PayProcessedTypes.HourlyRateDaysPerMonth:
        //                        Value = _objWorkingHours.HR_DaysPerMonth;
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        _globalUser.ErrorObject = ex;
        //    }
        //    return Value;
        //}

        /// <summary>
        /// Returns the employees  PayProcess Type
        /// </summary>
        /// <param name="_objEmployee"></param>
        /// <param name="_DetailType"></param>
        public static EmployeePayProcessedTypes getEmployeePayProcessedTypes(int _intEmployeeNo)
        {
            Employee _objEmployee = new Employee();
            EmployeePayProcessedTypes Value = EmployeePayProcessedTypes.Monthly;
            try
            {
                //--------------------------------------------
                _objEmployee.SegmentID = _globalUser.LoginUser.SegmentID;
                _objEmployee.EmployeeNo = _intEmployeeNo;
                _objEmployee.GetByID();
                //--------------------------------------------
                if (_objEmployee.PayProcessed != -1)
                {
                    switch (_objEmployee.PayProcessed)
                    {
                        case 1:
                            Value = EmployeePayProcessedTypes.Hourly;
                            break;
                        case 2:
                            Value = EmployeePayProcessedTypes.Daily;
                            break;
                        case 3:
                            Value = EmployeePayProcessedTypes.Weekly;
                            break;
                        case 4:
                            Value = EmployeePayProcessedTypes.By_Weekly;
                            break;
                        case 5:
                            Value = EmployeePayProcessedTypes.Monthly;
                            break;
                    }

                }
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
            return Value;
        }

        /// <summary>
        /// Checks the date against a holiday calender to check if the date is a holiday or not
        /// </summary>
        /// <param name="WorkingCalenderID"></param>
        /// <param name="CheckDate"></param>
        /// <returns></returns>
        //public static bool getHolidayStatusForWorkingCalender(int SegmentID, int WorkingCalenderID, DateTime CheckDate, int LeaveSchemeID, int LeaveType)
        //{
        //    //this function edit by Dinesh 2017 08 25
        //    bool _blnHoliday = false;
        //    bool _blnSkipHoliday = false;
        //    bool _blnSkipGazetted = false;
        //    bool _blnSkipRestDay = false;
        //    bool _blnSkipOffDay = false;
        //    try
        //    {
        //        LeaveSchemeItems _objLeaveType = new LeaveSchemeItems();
        //        _objLeaveType.SegmentID = SegmentID;
        //        _objLeaveType.LeaveSchemeID = LeaveSchemeID;
        //        _objLeaveType.LeaveTypeID = LeaveType;
        //        _objLeaveType.GetByID();
        //        if (_objLeaveType.LastModifyUser != -1)
        //        {
        //            _blnSkipGazetted = CommonFunctions.IsNullBooelan(_objLeaveType.SkipGazetetd);
        //            _blnSkipHoliday = CommonFunctions.IsNullBooelan(_objLeaveType.SkipHoliday);
        //            _blnSkipRestDay = CommonFunctions.IsNullBooelan(_objLeaveType.SkipRestDay);
        //            _blnSkipOffDay = CommonFunctions.IsNullBooelan(_objLeaveType.SkipOffDay);
        //        }



        //        WorkingCalenderDays _objDays = new WorkingCalenderDays();
        //        _objDays.SegmentID = SegmentID;
        //        _objDays.WorkingCalenderSchemeID = WorkingCalenderID;
        //        _objDays.WorkingDate = CheckDate.Date;
        //        _objDays.GetByID();
        //        if (_objDays.WorkingCalenderDayID != -1)
        //        {
        //            //this function edit by Dinesh 2017 08 25
        //            if (_objDays.DayTypeID == 3 && _blnSkipHoliday == true)
        //            { _blnHoliday = true; }

        //            if (_objDays.DayTypeID == 2 && _blnSkipGazetted == true)
        //            { _blnHoliday = true; }

        //            if (_objDays.DayTypeID == 4 && _blnSkipRestDay == true)
        //            { _blnHoliday = true; }

        //            if (_objDays.DayTypeID == 1 && _blnSkipOffDay == true)
        //            { _blnHoliday = true; }

        //            //NormalWorkingDay = 0,
        //            //StatutoryHoliday = 1,
        //            //GovernmentHoliday = 2,
        //            //RestDay = 3,
        //            //OffDay = 4,
                      //NormalWorkingDayHalfDay=5
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _globalUser.ErrorObject = ex;
        //    }
        //    return _blnHoliday;

        //}
        //public static bool getHolidayStatusForWorkingCalender(int SegmentID, int EmployeeNo,int WorkingCalenderSchemeID,int shiftID, DateTime CheckDate, int LeaveSchemeID, int LeaveType)
        //{
        //    //this function edit by Dinesh 2017 08 25
        //    bool _blnHoliday = false;
        //    bool _blnSkipHoliday = false;
        //    bool _blnSkipGazetted = false;
        //    bool _blnSkipRestDay = false;
        //    bool _blnSkipOffDay = false;
        //    try
        //    {
        //        LeaveSchemeItems _objLeaveType = new LeaveSchemeItems();
        //        _objLeaveType.SegmentID = SegmentID;
        //        _objLeaveType.LeaveSchemeID = LeaveSchemeID;
        //        _objLeaveType.LeaveTypeID = LeaveType;
        //        _objLeaveType.GetByID();
        //        if (_objLeaveType.LastModifyUser != -1)
        //        {
        //            _blnSkipGazetted = CommonFunctions.IsNullBooelan(_objLeaveType.SkipGazetetd);
        //            _blnSkipHoliday = CommonFunctions.IsNullBooelan(_objLeaveType.SkipHoliday);
        //            _blnSkipRestDay = CommonFunctions.IsNullBooelan(_objLeaveType.SkipRestDay);
        //            _blnSkipOffDay = CommonFunctions.IsNullBooelan(_objLeaveType.SkipOffDay);
        //        }

        //        AttendanceEmployeeOverrideDailyRoster _objDays = new AttendanceEmployeeOverrideDailyRoster();

        //        AttendanceEmployeeInfo objEmpInfor = new AttendanceEmployeeInfo();
        //        objEmpInfor.EmployeeNo = EmployeeNo;
        //        objEmpInfor.GetByID();

        //        _objDays.SegmentID = SegmentID;
        //        _objDays.EmployeeNo = EmployeeNo;
        //        _objDays.RosterDate = CheckDate.Date;

        //        _objDays.GetByID();
        //        if (_objDays.EmployeeOverrideRosterEntryID != -1)
        //        {
        //            AttendanceDayType _objAttendanceDayType = new AttendanceDayType();
        //            _objAttendanceDayType.SegmentID = SegmentID;
        //            _objAttendanceDayType.DayTypeID = _objDays.DayTypeID;
        //            _objAttendanceDayType.GetByID();

        //            if (_objAttendanceDayType.DayTypeID != -1)
        //            {
        //                //this function edit by Dinesh 2017 08 25
        //                if (_objAttendanceDayType.DayTypeIdentifier == 1 && _blnSkipGazetted == true)
        //                { 
        //                    _blnHoliday = true;
        //                }

        //                if (_objAttendanceDayType.DayTypeIdentifier == 2 && _blnSkipRestDay == true)
        //                {
        //                    _blnHoliday = true;
        //                }

        //                if (_objAttendanceDayType.DayTypeIdentifier == 3 && _blnSkipOffDay == true)
        //                {
        //                    _blnHoliday = true;
        //                }
        //                if (_objAttendanceDayType.DayTypeIdentifier == 4 && _blnSkipHoliday == true)
        //                { 
        //                    _blnHoliday = true; 
        //                }

        //                //Normal Working Days=0
        //                //Gazetted Holiday=1
        //                //Rest Day=2
        //                //Off Day=3
        //                //Holiday=4
        //                //NormalWorkingHalfDays=5
        //            }
        //        }
        //        else
        //        {
        //            AttendanceWorkgroupDailyRoster _objAttendanceWorkgroupDailyRoster = new AttendanceWorkgroupDailyRoster();


        //            _objAttendanceWorkgroupDailyRoster.SegmentID = SegmentID;
        //            _objAttendanceWorkgroupDailyRoster.WorkgroupID = objEmpInfor.WorkgroupID;
        //            _objAttendanceWorkgroupDailyRoster.RosterDate = CheckDate.Date;
        //            _objAttendanceWorkgroupDailyRoster.GetByID();


        //            AttendanceDayType _objAttendanceDayType = new AttendanceDayType();
        //            _objAttendanceDayType.SegmentID = SegmentID;
        //            _objAttendanceDayType.DayTypeID = _objAttendanceWorkgroupDailyRoster.DayTypeID;
        //            _objAttendanceDayType.GetByID();


        //            //if (_objAttendanceWorkgroupDailyRoster.WorkgroupRosterEntryID != -1 || _objAttendanceWorkgroupDailyRoster != null)
        //            if (_objAttendanceWorkgroupDailyRoster.WorkgroupRosterEntryID != -1)
        //            {

        //                if (_objAttendanceDayType.DayTypeID != -1)
        //                {
        //                    //this function edit by Dinesh 2017 08 25

        //                    //this function edit by Dinesh 2017 08 25
        //                    if (_objAttendanceDayType.DayTypeIdentifier == 1 && _blnSkipGazetted == true)
        //                    {
        //                        _blnHoliday = true;
        //                    }

        //                    if (_objAttendanceDayType.DayTypeIdentifier == 2 && _blnSkipRestDay == true)
        //                    {
        //                        _blnHoliday = true;
        //                    }

        //                    if (_objAttendanceDayType.DayTypeIdentifier == 3 && _blnSkipOffDay == true)
        //                    {
        //                        _blnHoliday = true;
        //                    }
        //                    if (_objAttendanceDayType.DayTypeIdentifier == 4 && _blnSkipHoliday == true)
        //                    {
        //                        _blnHoliday = true;
        //                    }

        //                    //Normal Working Days=0
        //                    //Gazetted Holiday=1
        //                    //Rest Day=2
        //                    //Off Day=3
        //                    //Holiday=4
        //                    //NormalWorkingHalfDays=5
        //                }
        //            }
        //            else
        //            {

        //                WorkingCalenderDays _objworkdDays = new WorkingCalenderDays();
        //                _objworkdDays.WorkingCalenderSchemeID = WorkingCalenderSchemeID;
        //                _objworkdDays.SegmentID = SegmentID;
        //                _objworkdDays.WorkingDate = CheckDate.Date;
        //                _objworkdDays.GetByID();

        //                //_objAttendanceDayType = new AttendanceDayType();
        //                //_objAttendanceDayType.SegmentID = SegmentID;
        //                //_objAttendanceDayType.DayTypeID = _objworkdDays.DayTypeID;
        //                //_objAttendanceDayType.GetByID();


        //                if (_objworkdDays.WorkingCalenderDayID != -1)
        //                {

        //                    //if (_objAttendanceDayType.DayTypeID != -1)
        //                   // {


        //                        //3=blue RestDay 
        //                        //1=red Statuary
        //                        //2=Yellow Gazetted
        //                        //4- Green Offday 
        //                        //this function edit by Dinesh 2017 08 25

        //                        //this function edit by Dinesh 2017 08 25
        //                        if (_objworkdDays.DayTypeID == 1 && _blnSkipHoliday == true)
        //                        {
        //                            _blnHoliday = true;
        //                        }

        //                        if (_objworkdDays.DayTypeID == 2 && _blnSkipGazetted == true)
        //                        {
        //                            _blnHoliday = true;
        //                        }

        //                        if (_objworkdDays.DayTypeID == 3 && _blnSkipRestDay == true)
        //                        {
        //                            _blnHoliday = true;
        //                        }
        //                        if (_objworkdDays.DayTypeID == 4 && _blnSkipOffDay == true)
        //                        {
        //                            _blnHoliday = true;
        //                        }

        //                        //Normal Working Days=0
        //                        //Gazetted Holiday=1
        //                        //Rest Day=2
        //                        //Off Day=3
        //                        //Holiday=4
        //                        //NormalWorkingHalfDays=5
        //                   // }

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _globalUser.ErrorObject = ex;
        //    }
        //    return _blnHoliday;

        //}

        public static DateTime getCombinedDateAndTime(DateTime DateValue, DateTime TimeValue) /*Add By TharakaM 2022-07-19*/
        {
            DateTime? _returnValue = null;
            try
            {
                _returnValue = new DateTime(DateValue.Year, DateValue.Month, DateValue.Day, TimeValue.Hour, TimeValue.Minute, TimeValue.Second);
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }
            return _returnValue.Value;
        }


        //Added by C on 2020-01-06 - If added 0.5 to normal working day , take 1 as request value in periodbreakdwn table
        //public static double getLeaveValueWorkingCalender_New(int SegmentID, int EmployeeNo, int WorkingCalenderSchemeID, int shiftID, DateTime CheckDate, int LeaveSchemeID, int LeaveType, double RequestVale)
        //{
        //    //this function edit by Dinesh 2017 08 25
        //    double dayIncrementValue = 0;

        //    try
        //    {
        //        LeaveSchemeItems _objLeaveType = new LeaveSchemeItems();
        //        _objLeaveType.SegmentID = SegmentID;
        //        _objLeaveType.LeaveSchemeID = LeaveSchemeID;
        //        _objLeaveType.LeaveTypeID = LeaveType;
        //        _objLeaveType.GetByID();

        //        AttendanceEmployeeOverrideDailyRoster _objDays = new AttendanceEmployeeOverrideDailyRoster();

        //        AttendanceEmployeeInfo objEmpInfor = new AttendanceEmployeeInfo();
        //        objEmpInfor.EmployeeNo = EmployeeNo;
        //        objEmpInfor.GetByID();

        //        _objDays.SegmentID = SegmentID;
        //        _objDays.EmployeeNo = EmployeeNo;
        //        _objDays.RosterDate = CheckDate.Date;

        //        _objDays.GetByID();
        //        if (_objDays.EmployeeOverrideRosterEntryID != -1)
        //        {
        //            AttendanceDayType _objAttendanceDayType = new AttendanceDayType();
        //            _objAttendanceDayType.SegmentID = SegmentID;
        //            _objAttendanceDayType.DayTypeID = _objDays.DayTypeID;
        //            _objAttendanceDayType.GetByID();

        //            if (_objAttendanceDayType.DayTypeID != -1)
        //            {
        //                if (_objAttendanceDayType.DayTypeIdentifier == 5)
        //                {
        //                    dayIncrementValue = Convert.ToDouble(0.5);
        //                }
        //                else
        //                {
        //                    //added by c on 2020-01-06
        //                    if (RequestVale == 0.5)
        //                    {
        //                        dayIncrementValue = Convert.ToDouble(0.5);
        //                    }
        //                    else
        //                    {
        //                        dayIncrementValue = Convert.ToDouble(1);
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            AttendanceWorkgroupDailyRoster _objAttendanceWorkgroupDailyRoster = new AttendanceWorkgroupDailyRoster();

        //            _objAttendanceWorkgroupDailyRoster.SegmentID = SegmentID;
        //            _objAttendanceWorkgroupDailyRoster.WorkgroupID = objEmpInfor.WorkgroupID;
        //            _objAttendanceWorkgroupDailyRoster.RosterDate = CheckDate.Date;
        //            _objAttendanceWorkgroupDailyRoster.GetByID();

        //            AttendanceDayType _objAttendanceDayType = new AttendanceDayType();
        //            _objAttendanceDayType.SegmentID = SegmentID;
        //            _objAttendanceDayType.DayTypeID = _objAttendanceWorkgroupDailyRoster.DayTypeID;
        //            _objAttendanceDayType.GetByID();

        //            if (_objAttendanceWorkgroupDailyRoster.WorkgroupRosterEntryID != -1)
        //            {

        //                if (_objAttendanceDayType.DayTypeID != -1)
        //                {

        //                    if (_objAttendanceDayType.DayTypeIdentifier == 5)
        //                    {
        //                        dayIncrementValue = Convert.ToDouble(0.5);
        //                    }
        //                    else
        //                    {
        //                        //added by c on 2020-01-06
        //                        if (RequestVale == 0.5)
        //                        {
        //                            dayIncrementValue = Convert.ToDouble(0.5);
        //                        }
        //                        else
        //                        {
        //                            dayIncrementValue = Convert.ToDouble(1);
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {

        //                WorkingCalenderDays _objworkdDays = new WorkingCalenderDays();
        //                _objworkdDays.WorkingCalenderSchemeID = WorkingCalenderSchemeID;
        //                _objworkdDays.SegmentID = SegmentID;
        //                _objworkdDays.WorkingDate = CheckDate.Date;
        //                _objworkdDays.GetByID();

        //                if (_objworkdDays.WorkingCalenderDayID != -1)
        //                {
        //                    if (_objworkdDays.DayTypeID == 5)
        //                    {

        //                        dayIncrementValue = Convert.ToDouble(0.5);
        //                    }
        //                    else
        //                    {
        //                        //added by c on 2020-01-06
        //                        if (RequestVale == 0.5)
        //                        {
        //                            dayIncrementValue = Convert.ToDouble(0.5);
        //                        }
        //                        else
        //                        {
        //                            dayIncrementValue = Convert.ToDouble(1);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    //dayIncrementValue = Convert.ToDouble(1);
        //                    if (RequestVale == 0.5)
        //                    {
        //                        dayIncrementValue = Convert.ToDouble(0.5);
        //                    }
        //                    else
        //                    {
        //                        dayIncrementValue = Convert.ToDouble(1);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _globalUser.ErrorObject = ex;
        //    }
        //    return dayIncrementValue;
        //}
        
        //public static double getLeaveValueWorkingCalender(int SegmentID, int EmployeeNo, int WorkingCalenderSchemeID, int shiftID, DateTime CheckDate, int LeaveSchemeID, int LeaveType)
        //{
        //    //this function edit by Dinesh 2017 08 25
        //    double dayIncrementValue = 0;
        
        //    try
        //    {

                

        //        LeaveSchemeItems _objLeaveType = new LeaveSchemeItems();
        //        _objLeaveType.SegmentID = SegmentID;
        //        _objLeaveType.LeaveSchemeID = LeaveSchemeID;
        //        _objLeaveType.LeaveTypeID = LeaveType;
        //        _objLeaveType.GetByID();


        //        AttendanceEmployeeOverrideDailyRoster _objDays = new AttendanceEmployeeOverrideDailyRoster();

        //        AttendanceEmployeeInfo objEmpInfor = new AttendanceEmployeeInfo();
        //        objEmpInfor.EmployeeNo = EmployeeNo;
        //        objEmpInfor.GetByID();

        //        _objDays.SegmentID = SegmentID;
        //        _objDays.EmployeeNo = EmployeeNo;
        //        _objDays.RosterDate = CheckDate.Date;

        //        _objDays.GetByID();
        //        if (_objDays.EmployeeOverrideRosterEntryID != -1)
        //        {
        //            AttendanceDayType _objAttendanceDayType = new AttendanceDayType();
        //            _objAttendanceDayType.SegmentID = SegmentID;
        //            _objAttendanceDayType.DayTypeID = _objDays.DayTypeID;
        //            _objAttendanceDayType.GetByID();

        //           // AttendanceShifts _objShift = new AttendanceShifts();
        //          //  _objShift.ShiftID = _objDays.ShiftID;
        //            //_objShift.GetByID();

        //            if (_objAttendanceDayType.DayTypeID != -1)
        //            {
        //                if (_objAttendanceDayType.DayTypeIdentifier==5)
        //                {

        //                    dayIncrementValue = Convert.ToDouble(0.5);
        //                }
        //                else{

        //                    dayIncrementValue = Convert.ToDouble(1);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            AttendanceWorkgroupDailyRoster _objAttendanceWorkgroupDailyRoster = new AttendanceWorkgroupDailyRoster();


        //            _objAttendanceWorkgroupDailyRoster.SegmentID = SegmentID;
        //            _objAttendanceWorkgroupDailyRoster.WorkgroupID = objEmpInfor.WorkgroupID;
        //            _objAttendanceWorkgroupDailyRoster.RosterDate = CheckDate.Date;
        //            _objAttendanceWorkgroupDailyRoster.GetByID();


        //            AttendanceDayType _objAttendanceDayType = new AttendanceDayType();
        //            _objAttendanceDayType.SegmentID = SegmentID;
        //            _objAttendanceDayType.DayTypeID = _objAttendanceWorkgroupDailyRoster.DayTypeID;
        //            _objAttendanceDayType.GetByID();


        //            //AttendanceShifts _objShift = new AttendanceShifts();
        //            //_objShift.ShiftID = _objAttendanceWorkgroupDailyRoster.ShiftID;
        //            //_objShift.GetByID();


        //            //if (_objAttendanceWorkgroupDailyRoster.WorkgroupRosterEntryID != -1 || _objAttendanceWorkgroupDailyRoster != null)
        //            if (_objAttendanceWorkgroupDailyRoster.WorkgroupRosterEntryID != -1)
        //            {

        //                if (_objAttendanceDayType.DayTypeID != -1)
        //                {

        //                    if (_objAttendanceDayType.DayTypeIdentifier == 5)
        //                {

        //                    dayIncrementValue = Convert.ToDouble(0.5);
        //                }
        //                else{

        //                    dayIncrementValue = Convert.ToDouble(1);
        //                }


        //                }
        //            }
        //            else
        //            {

        //                WorkingCalenderDays _objworkdDays = new WorkingCalenderDays();
        //                _objworkdDays.WorkingCalenderSchemeID = WorkingCalenderSchemeID;
        //                _objworkdDays.SegmentID = SegmentID;
        //                _objworkdDays.WorkingDate = CheckDate.Date;
        //                _objworkdDays.GetByID();

       

        //                //_objAttendanceDayType = new AttendanceDayType();
        //                //_objAttendanceDayType.SegmentID = SegmentID;
        //                //_objAttendanceDayType.DayTypeID = _objworkdDays.DayTypeID;
        //                //_objAttendanceDayType.GetByID();


        //                if (_objworkdDays.WorkingCalenderDayID != -1)
        //                {
        //                    if (_objworkdDays.DayTypeID == 5)
        //                    {

        //                        dayIncrementValue = Convert.ToDouble(0.5);
        //                    }
        //                    else
        //                    {

        //                        dayIncrementValue = Convert.ToDouble(1);
        //                    }


        //                }
        //                else
        //                {
        //                    dayIncrementValue = Convert.ToDouble(1);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _globalUser.ErrorObject = ex;
        //    }
        //    return dayIncrementValue;

        //}

        ///// <summary>
        ///// returns the holiday dates for the default calender 
        ///// </summary>
        ///// <param name="_objHolidayCalender"></param>
        ///// <param name="SelectedYear"></param>
        ///// <returns></returns>
        //public static List<HolidaysDates> getHolidayCalender(HolidayCalender _objHolidayCalender, int SelectedYear)
        //{
        //    List<HolidaysDates> lstDates = new List<HolidaysDates>();
        //    DataSet _dsHolidays = new DataSet();

        //    try
        //    {

        //        _dsHolidays = _objHolidayCalender.GetList();
        //        foreach (DataRow item in _dsHolidays.Tables[0].Rows)
        //        {
        //            lstDates.Add(new HolidaysDates((DateTime)item["HolidayDate"], (Holidaytypes)item["HolidayTypeInt"]));
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _globalUser.ErrorObject = ex;
        //    }
        //    return lstDates;
        //}

        //public static List<HolidaysDates> getHolidayCalenderNew(HolidayCalender _objHolidayCalender, int SelectedYear)
        //{
        //    List<HolidaysDates> lstDates = new List<HolidaysDates>();
        //    DataSet _dsHolidays = new DataSet();

        //    try
        //    {

        //        _dsHolidays = _objHolidayCalender.GetListNew();
        //        foreach (DataRow item in _dsHolidays.Tables[0].Rows)
        //        {
        //            lstDates.Add(new HolidaysDates((DateTime)item["HolidayDate"], (Holidaytypes)item["HolidayTypeInt"]));
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _globalUser.ErrorObject = ex;
        //    }
        //    return lstDates;
        //}

        public static object[] getDataSetColumArray(DataSet _dsSource, string fieldName)
        {
            //List<string> lstTokens = new List<string>();
            object[] _strRetArray = null;
            try
            {
                _strRetArray = _dsSource.Tables[0].AsEnumerable().Select(r => r.Field<object>(fieldName)).ToArray();
            }
            catch (Exception ex)
            {
                //_globalBaseclass.ErrorObject = ex;
                _globalUser.ErrorObject = ex;
            }
            return _strRetArray;
        }

        /// <summary>
        /// returns the sum of a dataset field as a double value
        /// </summary>
        /// <param name="_dsSource"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static double getDataSetColumSUM(DataSet _dsSource, string fieldName)
        {
            //List<string> lstTokens = new List<string>();
            double _ColumnSum = 0;
            object[] _strRetArray = null;
            try
            {
                _strRetArray = getDataSetColumArray(_dsSource, fieldName);
                if (_strRetArray.Length > 0)
                {
                    for (int i = 0; i < _strRetArray.Length; i++)
                    {
                        _ColumnSum += (double)_strRetArray[i];
                    }
                }
            }
            catch (Exception ex)
            {
                //_globalBaseclass.ErrorObject = ex;
                _globalUser.ErrorObject = ex;
            }
            return _ColumnSum;
        }


        //public static void PrintControl(PrintingInformation printPreviewInfo)
        //{
        //    try
        //    {
        //        GridPriview printPreviewer = new GridPriview(printPreviewInfo);
        //        printPreviewer.LoadReport();

                
        //        //printPreviewer.WindowState = FormWindowState.Maximized;
        //    }
        //    catch (Exception ex)
        //    {

        //        _globalUser.ErrorObject = ex;
        //    }
        //}


        //public static void PrintControlPivot(PrintingInformation printPreviewInfo)
        //{
        //    try
        //    {
        //        GridPriview printPreviewer = new GridPriview(printPreviewInfo);
        //        printPreviewer.LoadReport();
        //        //printPreviewer.WindowState = FormWindowState.Maximized;
        //    }
        //    catch (Exception ex)
        //    {

        //        _globalUser.ErrorObject = ex;
        //    }
        //}

        /// <summary>
        /// returns the machine name (workstation name)
        /// </summary>
        /// <returns></returns>
        public static string getMachineName()
        {
            return System.Environment.MachineName;
        }

        // /// <summary>
        // /// gets columns data type
        // /// </summary>
        // /// <param name="_dsSource"></param>
        // /// <param name="columnName"></param>
        // /// <returns></returns>
        //public static ColumnDataType getColumnType(DataSet _dsSource, string columnName)
        //{
        //    ColumnDataType _datatype = ColumnDataType.NumericColum;
        //    try
        //    {
        //        var numericTypes = new[] { typeof(Byte), typeof(Decimal), typeof(Double),
        //         typeof(Int16), typeof(Int32), typeof(Int64), typeof(SByte),
        //         typeof(Single), typeof(UInt16), typeof(UInt32), typeof(UInt64)};

        //        var stringTypes = new[] { typeof(string) };


        //        if (_dsSource != null)
        //        {
        //            if (_dsSource.Tables[0].Columns.Count > 0)
        //            {
        //                if (_dsSource.Tables[0].Columns.Contains(columnName) == true)
        //                {
        //                    if (numericTypes.Contains(_dsSource.Tables[0].Columns[columnName].DataType) == true)
        //                    {
        //                        _datatype = ColumnDataType.NumericColum;
        //                    }

        //                    if (stringTypes.Contains(_dsSource.Tables[0].Columns[columnName].DataType) == true)
        //                    {
        //                        _datatype = ColumnDataType.StringColumn;
        //                    }

        //                    if (_dsSource.Tables[0].Columns[columnName].DataType == typeof(DateTime))
        //                    {
        //                        _datatype = ColumnDataType.DateColumn;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //_globalBaseclass.ErrorObject = ex;
        //    }
        //    return _datatype;
        //}


        /// <summary>
        /// Loads the grid control with the dataset
        /// </summary>
        /// <param name="gridViewSource"></param>
        /// <param name="_dsGridSource"></param>
        /// <returns></returns>
        public static GridView LoadGrid(DevExpress.XtraGrid.GridControl gridViewSource, DataSet _dsGridSource)
        {
            GridView grdView = (GridView)gridViewSource.ViewCollection[0];
            try
            {
                if (_dsGridSource != null)
                {
                    // assign a new source
                    gridViewSource.DataSource = null;
                    gridViewSource.DataSource = _dsGridSource.Tables[0];
                    formatGrid(_dsGridSource, grdView);

                    //foreach (DataColumn item in _dsGridSource.Tables[0].Columns)
                    //{
                    //    if (item.ColumnName.ToUpper().IndexOf("ID") > 0 || item.ColumnName.ToUpper() == ("LastModifyDate").ToUpper() || item.ColumnName.ToUpper() == ("LastModifyUser").ToUpper())
                    //    {
                    //        grdView.Columns[item.ColumnName].Visible = false;

                    //    }
                    //    else {
                    //        grdView.Columns[item.ColumnName].BestFit();
                    //    }

                    //    if (item.DataType == typeof(int) )
                    //    {
                    //        grdView.Columns[item.ColumnName].Visible = false;
                    //    }

                    //}

                }
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
            return grdView;
        }


        /// <summary>
        /// Loads the grid control with the dataset
        /// </summary>
        /// <param name="gridViewSource"></param>
        /// <param name="_dsGridSource"></param>
        /// <returns></returns>
        public static GridView LoadGridDataTable(DevExpress.XtraGrid.GridControl gridViewSource, DataTable _dtGridSource)
        {
            GridView grdView = (GridView)gridViewSource.ViewCollection[0];
            try
            {
                if (_dtGridSource != null)
                {
                    // assign a new source
                    gridViewSource.DataSource = null;
                    gridViewSource.DataSource = _dtGridSource;
                    formatGridDataTable(_dtGridSource, grdView);

                    //foreach (DataColumn item in _dsGridSource.Tables[0].Columns)
                    //{
                    //    if (item.ColumnName.ToUpper().IndexOf("ID") > 0 || item.ColumnName.ToUpper() == ("LastModifyDate").ToUpper() || item.ColumnName.ToUpper() == ("LastModifyUser").ToUpper())
                    //    {
                    //        grdView.Columns[item.ColumnName].Visible = false;

                    //    }
                    //    else {
                    //        grdView.Columns[item.ColumnName].BestFit();
                    //    }

                    //    if (item.DataType == typeof(int) )
                    //    {
                    //        grdView.Columns[item.ColumnName].Visible = false;
                    //    }

                    //}

                }
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
            return grdView;
        }

        /// <summary>
        /// returns the machine ip
        /// </summary>
        /// <returns></returns>
        public static string getComputerIP()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }

        /// <summary>
        /// hides the selecdted columns in the grid
        /// </summary>
        /// <param name="grdView"></param>
        /// <param name="HideColumns"></param>
        public static void hideColumns(DevExpress.XtraGrid.Views.Grid.GridView grdView, string[] HideColumns = null)
        {
            if (HideColumns != null)
            {
                if (HideColumns.Length > 0)
                {

                    foreach (string item in HideColumns)
                    {
                        if (grdView.Columns[item] != null)
                        {
                            grdView.Columns[item].Visible = false;
                        }
                    }
                }
            }

        }

        //Done by menushan 2019-11-06

        public static void readOnlyColumns(DevExpress.XtraGrid.Views.Grid.GridView grdView, string[] ReadOnlyColumns = null)
        {
            if (ReadOnlyColumns != null)
            {
                if (ReadOnlyColumns.Length > 0)
                {

                    foreach (string item in ReadOnlyColumns)
                    {
                        if (grdView.Columns[item] != null)
                        {
                            grdView.Columns[item].OptionsColumn.AllowEdit = false;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Shows only the selected columns
        /// </summary>
        /// <param name="grdView"></param>
        /// <param name="ShownColumns"></param>
        public static void ShowColumns(DevExpress.XtraGrid.Views.Grid.GridView grdView, string[] ShownColumns = null)
        {

            try
            {
                hideColumns(grdView);// hides the columns

                if (ShownColumns != null)
                {
                    if (ShownColumns.Length > 0)
                    {

                        foreach (string item in ShownColumns)
                        {
                            if (grdView.Columns[item] != null)
                            {
                                grdView.Columns[item].Visible = true;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                //throw;
            }

        }


        /// <summary>
        /// returns the current year id for a selected employee and leave type
        /// </summary>
        /// <param name="segmentID"></param>
        /// <param name="employeeNo"></param>
        /// <param name="loginUserID"></param>
        /// <param name="leaveTypeID"></param>
        /// <param name="inquireDate"></param>
        /// <returns></returns>
        //public static LeaveEntitlementProcessing.MonthlyProcessyLeaveYearID getSalaryYear(int segmentID, int employeeNo, int loginUserID, int leaveTypeID, DateTime inquireDate)
        //{
        //    LeaveEntitlementProcessing.MonthlyProcessyLeaveYearID _objYearID = new LeaveEntitlementProcessing.MonthlyProcessyLeaveYearID();
        //    try
        //    {
        //        if (employeeNo != null)
        //        {
        //            //------------------------ get the salary year id for both salary year and for monthly process leave types ( Leave Year ID)
        //            _objYearID =
        //               (new LeaveEntitlementProcessing(segmentID, employeeNo, loginUserID)
        //               .getMonthlyLeaveYearID(inquireDate, leaveTypeID));
        //            //-------------------------------------------------------------------------------------------------------------------------
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _globalUser.ErrorObject = ex;
        //    }
        //    return _objYearID;
        //}

        /// <summary>
        /// hides all the columns
        /// </summary>
        /// <param name="grdView"></param>
        /// <param name="HideColumns"></param>
        public static void hideColumns(DevExpress.XtraGrid.Views.Grid.GridView grdView)
        {
            try
            {
                //-----------------------------------------------------------------
                foreach (DevExpress.XtraGrid.Columns.GridColumn item in grdView.Columns)
                {
                    if (grdView.Columns[item.FieldName] != null)
                    {
                        grdView.Columns[item.FieldName].Visible = false;
                    }
                }
                //-----------------------------------------------------------------
            }
            catch (Exception ex)
            {

                // throw;
            }
        }

        /// <summary>
        /// formats the grid 
        /// </summary>
        /// <param name="_dsGridSource"></param>
        /// <param name="grdView"></param>
        /// <param name="HideColumns"></param>
        /// <param name="ShowColumns"></param>
        public static void formatGrid(DataSet _dsGridSource, DevExpress.XtraGrid.Views.Grid.GridView grdView, string[] HideColumns = null, string[] ShowColumns = null)
        {
            try
            {
                if (_dsGridSource != null)
                {
                    foreach (DataColumn item in _dsGridSource.Tables[0].Columns)
                    {
                        if (item.ColumnName.ToUpper().IndexOf("ID") > 0 || item.ColumnName.ToUpper() == ("LastModifyDate").ToUpper() || item.ColumnName.ToUpper() == ("LastModifyUser").ToUpper())
                        {
                            if (ShowColumns != null)
                            {
                                foreach (string ShowColumn in ShowColumns)
                                {
                                    if (item.ColumnName == ShowColumn)
                                    {
                                        if (grdView.Columns[item.ColumnName] != null)
                                        {
                                            grdView.Columns[item.ColumnName].Visible = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (grdView.Columns[item.ColumnName] != null)
                                { grdView.Columns[item.ColumnName].Visible = false; }

                            }



                        }
                        else
                        {
                            if (grdView.Columns[item.ColumnName] != null)
                                grdView.Columns[item.ColumnName].BestFit();
                        }

                    }
                    //-------------------------------------------------------
                    hideColumns(grdView, HideColumns);
                    //LoadCustomCaption(grdView, -1);
                }
                //-------------------------------------------------------

            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }
        }

        /// <summary>
        /// formats the grid 
        /// </summary>
        /// <param name="_dsGridSource"></param>
        /// <param name="grdView"></param>
        /// <param name="HideColumns"></param>
        /// <param name="ShowColumns"></param>
        public static void formatGridDataTable(DataTable _dsGridSource, DevExpress.XtraGrid.Views.Grid.GridView grdView, string[] HideColumns = null, string[] ShowColumns = null)
        {
            try
            {
                if (_dsGridSource != null)
                {
                    foreach (DataColumn item in _dsGridSource.Columns)
                    {
                        if (item.ColumnName.ToUpper().IndexOf("ID") > 0 || item.ColumnName.ToUpper() == ("LastModifyDate").ToUpper() || item.ColumnName.ToUpper() == ("LastModifyUser").ToUpper())
                        {
                            if (ShowColumns != null)
                            {
                                foreach (string ShowColumn in ShowColumns)
                                {
                                    if (item.ColumnName == ShowColumn)
                                    {
                                        if (grdView.Columns[item.ColumnName] != null)
                                        {
                                            grdView.Columns[item.ColumnName].Visible = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (grdView.Columns[item.ColumnName] != null)
                                { grdView.Columns[item.ColumnName].Visible = false; }

                            }



                        }
                        else
                        {
                            if (grdView.Columns[item.ColumnName] != null)
                                grdView.Columns[item.ColumnName].BestFit();
                        }

                    }
                    //-------------------------------------------------------
                    hideColumns(grdView, HideColumns);
                    //LoadCustomCaption(grdView, -1);
                }
                //-------------------------------------------------------

            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }
        }

        //---------------------------Gagith
        //public static EmployeeClaimApplications[] LaunchClaimFilter(clsGobalUser _GlobalUser)
        //{
        //    try
        //    {
        //        _globalUser = _GlobalUser;
        //        Enquires.FrmClaimFilter frmClaimFilter = new Enquires.FrmClaimFilter(_globalUser);
        //        frmClaimFilter.ShowDialog();
        //        _SelectedClaims = frmClaimFilter.getSelectedClaims();
        //    }
        //    catch (Exception ex)
        //    {

        //        _globalUser.ErrorObject = ex;
        //    }
        //    if (_SelectedClaims.Count > 0)
        //    { return _SelectedClaims.ToArray(); }
        //    else
        //    {
        //        return new EmployeeClaimApplications[0];
        //    }
        //}
        //---------------------------------

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

                _globalUser.ErrorObject = ex;
            }
            return _strSqqlFormate;
        }

        /// <summary>
        /// get the salary item value for a given month
        /// </summary>
        /// <param name="EmployeeNo">The employee no</param>
        /// <param name="SalaryYear">salary year id</param>
        /// <param name="Month">month id</param>
        /// <param name="PayCycle">paycycle id</param>
        /// <param name="PayBlock">pay block id</param>
        /// <param name="SalaryItemID">salary item id</param>
        /// <returns></returns>
        //public static double getMonthlySalaryItemValue(int EmployeeNo, int SalaryYear, int Month, int PayCycle, int PayBlock, int SalaryItemID)
        //{
        //    double SalaryItemValue = 0;
        //    try
        //    {
        //        PayrollPayCycleSalaryItems _objItems = new PayrollPayCycleSalaryItems();
        //        _objItems.EmployeeNo = EmployeeNo;
        //        _objItems.SalaryYearID = SalaryYear;
        //        _objItems.MonthNo = Month;
        //        _objItems.PayCycleID = PayCycle;
        //        _objItems.PayCycLeBlockID = PayBlock;
        //        _objItems.SalaryItemID = SalaryItemID;
        //        _objItems.GetByID();
        //        if (_objItems.SalaryItemValue > 0)
        //        {
        //            SalaryItemValue = _objItems.SalaryItemValue;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _globalUser.ErrorObject = ex;
        //    }
        //    return SalaryItemValue;
        //}

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

                _globalUser.ErrorObject = ex;
            }
            return _strSqqlFormate;
        }

        /// <summary>
        /// returns the row value for a gridlookup control
        /// </summary>
        /// <param name="_SourceLookUpControl"></param>
        /// <param name="_LookUpColumnName"></param>
        /// <returns></returns>
        public static object getLookUpRowValue(object _SourceLookUpControl, string _LookUpColumnName)
        {
            object value = null;
            try
            {
                GridLookUpEdit lookUpEdit = _SourceLookUpControl as GridLookUpEdit;
                DataRowView selectedDataRow = (DataRowView)lookUpEdit.GetSelectedDataRow();
                if (selectedDataRow != null)
                { value = selectedDataRow[_LookUpColumnName]; }

            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
            return value;
        }

        /// <summary>
        /// Returns a list of objects in a dataset filtered by as set column and value 
        /// </summary>
        /// <param name="dsSource"></param>
        /// <param name="strFindColumn"></param>
        /// <param name="strValueColumn"></param>
        /// <param name="ftColumnFilter"></param>
        /// <param name="objFindValue"></param>
        /// <returns></returns>
        public static object[] FindDatasetValue(DataSet dsSource, string strFindColumn, string strValueColumn, FilterType ftColumnFilter, object objFindValue)
        {
            List<object> _lstReturn = new List<object>();
            try
            {
                DataRow[] _drFilterResults = dsSource.Tables[0].Select(strFindColumn + "=" + (ftColumnFilter == FilterType.stringField ? "'" + objFindValue.ToString() + "'" : objFindValue.ToString()));
                if (_drFilterResults.Length > 0)
                {
                    foreach (DataRow item in _drFilterResults)
                    {
                        if (item[strValueColumn] != null)
                        {
                            _lstReturn.Add(item[strValueColumn]);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
            if (_lstReturn.Count > 0)
            {
                return _lstReturn.ToArray<object>();
            }
            else
            {
                return null;
                //return _lstReturn.ToArray<object>();
            }
        }

        /// <summary>
        /// Returns a column value for a lookup control
        /// </summary>
        /// <param name="_SourceLookUpControl"></param>
        /// <param name="_LookUpColumnIndex"></param>
        /// <returns></returns>
        public static object getLookUpRowValue(LookUpEdit _SourceLookUpControl, int _LookUpColumnIndex)
        {
            object value = null;
            try
            {
                DataRowView rowView = (DataRowView)_SourceLookUpControl.GetSelectedDataRow();
                DataRow row = rowView.Row;
                if (row != null)
                { value = row[_LookUpColumnIndex]; }


            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
            return value;
        }


        /// <summary>
        /// Returns the selected tree node
        /// </summary>
        /// <param name="SourceTreeView"></param>
        /// <returns></returns>
        public static TreeNode getSelectedTreeNode(TreeView SourceTreeView)
        {
            TreeNode _nodeSelectedNode = null;
            try
            {
                if (SourceTreeView.SelectedNode != null)
                {
                    _nodeSelectedNode = SourceTreeView.SelectedNode;
                }
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
                // _globalBaseclass.ErrorObject = ex;
            }
            return _nodeSelectedNode;
        }


        /// <summary>
        /// Returns a string representing a set of selected tokens
        /// </summary>
        /// <param name="StrCurrentSelection"></param>
        /// <param name="strNewItem"></param>
        /// <returns></returns>
        public static string SelectTokens(object StrCurrentSelection, string strNewItem)
        {
            string strSendout = "";
            bool _blnadd = true;
            try
            {
                if (StrCurrentSelection != null)
                { strSendout = StrCurrentSelection.ToString(); }
                if (StrCurrentSelection != null)
                    if (StrCurrentSelection.ToString().Length > 0)
                    {
                        if ((','+StrCurrentSelection.ToString() + ",").IndexOf(','+strNewItem + ',') >= 0)
                        {
                            _blnadd = false;
                        }
                    }


                if (_blnadd)
                {

                    strSendout = strSendout + (strSendout.Length > 0 ? "," : "");
                    strSendout = strSendout + strNewItem;
                }
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
            return strSendout;
        }


        private static bool ColumnEqual(object A, object B)
        {

            // Compares two values to see if they are equal. Also compares DBNULL.Value.
            // Note: If your DataTable contains object fields, then you must extend this
            // function to handle them in a meaningful way if you intend to group on them.

            if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value
                return true;
            if (A == DBNull.Value || B == DBNull.Value) //  only one is DBNull.Value
                return false;
            return (A.Equals(B));  // value type standard comparison
        }


        //public static void CreateMessageWithAttachment(string server)
        //{
        //    // Specify the file to be attached and sent.
        //    // This example assumes that a file named Data.xls exists in the
        //    // current working directory.
        //    string file = "data.xls";
        //    // Create a message and set up the recipients.
        //    MailMessage message = new MailMessage(
        //       "jane@contoso.com",
        //       "ben@contoso.com",
        //       "Quarterly data report.",
        //       "See the attached spreadsheet.");

        //    // Create  the file attachment for this e-mail message.
        //    Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
        //    // Add time stamp information for the file.
        //    ContentDisposition disposition = data.ContentDisposition;
        //    disposition.CreationDate = System.IO.File.GetCreationTime(file);
        //    disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
        //    disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
        //    // Add the file attachment to this e-mail message.
        //    message.Attachments.Add(data);
        //    //Send the message.
        //    SmtpClient client = new SmtpClient(server);
        //    // Add credentials if the SMTP server requires them.
        //    client.Credentials = CredentialCache.DefaultNetworkCredentials;
        //    client.Send(message);
        //    // Display the values in the ContentDisposition for the attachment.
        //    ContentDisposition cd = data.ContentDisposition;
        //    Console.WriteLine("Content disposition");
        //    Console.WriteLine(cd.ToString());
        //    Console.WriteLine("File {0}", cd.FileName);
        //    Console.WriteLine("Size {0}", cd.Size);
        //    Console.WriteLine("Creation {0}", cd.CreationDate);
        //    Console.WriteLine("Modification {0}", cd.ModificationDate);
        //    Console.WriteLine("Read {0}", cd.ReadDate);
        //    Console.WriteLine("Inline {0}", cd.Inline);
        //    Console.WriteLine("Parameters: {0}", cd.Parameters.Count);
        //    foreach (DictionaryEntry d in cd.Parameters)
        //    {
        //        Console.WriteLine("{0} = {1}", d.Key, d.Value);
        //    }
        //    data.Dispose();
        //}

        /// <summary>
        /// Saves grid Layout to DB
        /// </summary>
        /// <param name="FormName"></param>
        /// <param name="grdView"></param>
        /// <param name="_GlobalUser"></param>
        //public static void SetGridLayout(string FormName, string LayoutFile, DevExpress.XtraGrid.Views.Grid.GridView grdView, clsGobalUser _GlobalUser = null)
        //{
        //    try
        //    {

        //        /*
        //         persudo code 
        //         * SET lay out 
        //         *  if DB has user layout 
        //         *      then use user layout 
        //         *      else
        //         *      use default user layout
        //         *  else
        //         *      not assign lay out
        //         *  endif
                 
        //         *  SAVE LAYOUT
        //         *  KEEP STARTING LAYOUT- xmal file
        //         *  WHEN FORM CLOSING 
        //         *  COMPAIR IF Starting Layout dose not match new Layout
        //         *  IF STARTING LAYOUT dose not exist 
        //         *      SAVE as defult layout
        //         *  else 
        //         *      if User Layout Exist then
        //         *          SAVE User LAyout
        //         *      else 
        //         *          SAVE new Layout
        //         */

        //        Stream str = new System.IO.MemoryStream();
        //        grdView.SaveLayoutToStream(str);
        //        str.Seek(0, System.IO.SeekOrigin.Begin);
        //        StreamReader reader = new StreamReader(str);
        //        string text = reader.ReadToEnd();
        //        if (text != LayoutFile)
        //        {
        //            //----------------------------------
        //            SystemSettingsLayout _ObjSystemSettingsLayout = new SystemSettingsLayout();
        //            _ObjSystemSettingsLayout.FormName = FormName;
        //            _ObjSystemSettingsLayout.ControlName = grdView.Name;
        //            _ObjSystemSettingsLayout.SegmentID = _GlobalUser.LoginUser.SegmentID;
        //            _ObjSystemSettingsLayout.UserID = _GlobalUser.LoginUser.UserID;
        //            _ObjSystemSettingsLayout.GetByID();// check if user layout exists 

        //            if (_ObjSystemSettingsLayout.LayoutID != -1)
        //            {
                        
        //                _ObjSystemSettingsLayout.FormName = FormName;
        //                _ObjSystemSettingsLayout.ControlName = grdView.Name;
        //                _ObjSystemSettingsLayout.SegmentID = _GlobalUser.LoginUser.SegmentID;
        //                _ObjSystemSettingsLayout.UserID = _GlobalUser.LoginUser.UserID;
        //                _ObjSystemSettingsLayout.Layout = text;
        //                _ObjSystemSettingsLayout.Save();
        //            }
        //            else
        //            {
        //                _ObjSystemSettingsLayout = new SystemSettingsLayout();
        //                _ObjSystemSettingsLayout.FormName = FormName;
        //                _ObjSystemSettingsLayout.ControlName = grdView.Name;
        //                DataSet _dsLayouts = _ObjSystemSettingsLayout.GetList();// check if user layout exists
        //                if (_dsLayouts != null)
        //                {
        //                    if (_dsLayouts.Tables[0].Rows.Count > 0)
        //                    {
        //                        _ObjSystemSettingsLayout = new SystemSettingsLayout();
        //                        _ObjSystemSettingsLayout.LayoutID = (int)_dsLayouts.Tables[0].Rows[0]["LayoutID"];
        //                        _ObjSystemSettingsLayout.GetByID();
        //                        if (_ObjSystemSettingsLayout.LayoutID != -1)
        //                        {
        //                            //save the defult layout
        //                            _ObjSystemSettingsLayout = new SystemSettingsLayout();
        //                            _ObjSystemSettingsLayout.FormName = FormName;
        //                            _ObjSystemSettingsLayout.ControlName = grdView.Name;
        //                            _ObjSystemSettingsLayout.Layout = text;
        //                            _ObjSystemSettingsLayout.SegmentID = _GlobalUser.LoginUser.SegmentID;
        //                            _ObjSystemSettingsLayout.UserID = _GlobalUser.LoginUser.UserID;
        //                            _ObjSystemSettingsLayout.Save();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        _ObjSystemSettingsLayout = new SystemSettingsLayout();
        //                        _ObjSystemSettingsLayout.FormName = FormName;
        //                        _ObjSystemSettingsLayout.ControlName = grdView.Name;
        //                        _ObjSystemSettingsLayout.Layout = text;
        //                        _ObjSystemSettingsLayout.SegmentID = _GlobalUser.LoginUser.SegmentID;
        //                        _ObjSystemSettingsLayout.UserID = _GlobalUser.LoginUser.UserID;
        //                        _ObjSystemSettingsLayout.Save();

        //                    }
        //                }
        //                else
        //                {
        //                    //save the defult layout
        //                    _ObjSystemSettingsLayout = new SystemSettingsLayout();
        //                    _ObjSystemSettingsLayout.FormName = FormName;
        //                    _ObjSystemSettingsLayout.ControlName = grdView.Name;
        //                    _ObjSystemSettingsLayout.Layout = text;
        //                    _ObjSystemSettingsLayout.SegmentID = _GlobalUser.LoginUser.SegmentID;
        //                    _ObjSystemSettingsLayout.UserID = _GlobalUser.LoginUser.UserID;
        //                    _ObjSystemSettingsLayout.Save();

        //                }
        //            }
        //        }

        //        //----------------------------------
        //    }
        //    catch (Exception ex)
        //    {

        //        //throw;
        //    }

        //}


        /// <summary>
        /// Sets grid Layout to DB
        /// </summary>
        /// <param name="FormName"></param>
        /// <param name="grdView"></param>
        /// <param name="_GlobalUser"></param>
        //public static MemoryStream getGridLayout(string FormName, string ControlName, clsGobalUser _GlobalUser = null)
        //{
        //    MemoryStream stream = null;
        //    try
        //    {

        //        /*
        //         persudo code 
        //         * SET lay out 
        //         *  if DB has user layout 
        //         *      then use user layout 
        //         *      else
        //         *      use default user layout
        //         *  else
        //         *      not assign lay out
        //         *  endif
                 
        //         *  SAVE LAYOUT
        //         *  KEEP STARTING LAYOUT- xmal file
        //         *  WHEN FORM CLOSING 
        //         *  COMPAIR IF Starting Layout dose not match new Layout
        //         *  IF STARTING LAYOUT dose not exist 
        //         *      SAVE as defult layout
        //         *  else 
        //         *      if User Layout Exist then
        //         *          SAVE User LAyout
        //         *      else 
        //         *          SAVE new Layout
        //         */


        //        //----------------------------------
        //        SystemSettingsLayout _ObjSystemSettingsLayout = new SystemSettingsLayout();
        //        _ObjSystemSettingsLayout.FormName = FormName;
        //        _ObjSystemSettingsLayout.ControlName = ControlName;

        //        if (_GlobalUser != null)
        //        {
        //            _ObjSystemSettingsLayout.SegmentID = _GlobalUser.LoginUser.SegmentID;
        //            _ObjSystemSettingsLayout.UserID = _GlobalUser.LoginUser.UserID;
        //        }
        //        _ObjSystemSettingsLayout.GetByID();
        //        //----------------------------------
        //        if (_ObjSystemSettingsLayout.LayoutID == -1)
        //        {
        //            _ObjSystemSettingsLayout = new SystemSettingsLayout();
        //            _ObjSystemSettingsLayout.FormName = FormName;
        //            _ObjSystemSettingsLayout.ControlName = ControlName;
        //            _ObjSystemSettingsLayout.GetByID();
        //        }

        //        //----------------------------------
        //        if (_ObjSystemSettingsLayout.LayoutID != -1)
        //        {

        //            string text = _ObjSystemSettingsLayout.Layout;
        //            byte[] byteArray = Encoding.ASCII.GetBytes(text);
        //            stream = new MemoryStream(byteArray);
        //            //GridControlView.RestoreLayoutFromStream(stream);
        //        }

        //        //----------------------------------
        //    }
        //    catch (Exception ex)
        //    {

        //        //throw;
        //    }
        //    return stream;

        //}

        /// <summary>
        /// function returns a datatable with distinct valued from a column
        /// </summary>
        /// <param name="TableName"> Table name of the new table </param>
        /// <param name="SourceTable"> The source dataset of which a distinct value is needed</param>
        /// <param name="FieldName"> The field name that needs to be distinct</param>
        /// <returns></returns>
        public static DataTable SelectDistinct(string TableName, DataTable SourceTable, string FieldName)
        {
            DataTable dt = new DataTable(TableName);
            dt.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);

            object LastValue = null;
            foreach (DataRow dr in SourceTable.Select("", FieldName))
            {
                if (LastValue == null || !(ColumnEqual(LastValue, dr[FieldName])))
                {
                    LastValue = dr[FieldName];
                    dt.Rows.Add(new object[] { LastValue });
                }
            }
            //if (ds != null) 
            //    ds.Tables.Add(dt);
            return dt;
        }

        public static string SetTokens(TokenEdit TokenEditControl, string strNewItem)
        {
            string strSendout = "";
            try
            {
                //strNewItem =  strNewItem;
                foreach (string item in strNewItem.Split(','))
                {
                    foreach (TokenEditToken item2 in TokenEditControl.Properties.Tokens)
                    {
                        if (item2.Value.ToString().Trim() == item.ToString().Trim())
                        {
                            strSendout = strSendout + (strSendout.Length > 0 ? "," : "") + item;
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
            return strSendout;
        }
        public static string SetTokens1(TokenEdit TokenEditControl, string strNewItem)
        {
            string strSendout = "";
            try
            {
                //strNewItem =  strNewItem;
                foreach (string item in strNewItem.Split(','))
                {
                    foreach (string item3 in item.Split('-'))
                    {
                        foreach (TokenEditToken item2 in TokenEditControl.Properties.Tokens)
                        {
                            if (item2.Value.ToString().Trim() == item3.ToString().Trim())
                            {
                                strSendout = strSendout + (strSendout.Length > 0 ? "," : "") + item3;
                                break;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
            return strSendout;
        }

        /// <summary>
        /// returns a list of selected token's description in a list seperated by a type
        /// </summary>
        /// <param name="TokenControl"></param>
        /// <returns></returns>
        public static string getSelectedTokenText(TokenEdit TokenControl, string strSperator)
        {
            string _strTokenText = string.Empty;
            try
            {
                foreach (TokenEditToken item in TokenControl.SelectedItems)
                {
                    _strTokenText += (_strTokenText.Length > 0 ? strSperator : "") + item.Description;
                }
            }
            catch (Exception ex)
            {

                //throw;
            }
            return _strTokenText;
        }

        /// <summary>
        /// returns a pay cycle for a given inqury date
        /// </summary>
        /// <param name="SegmentId"></param>
        /// <param name="InrequreDate"></param>
        /// <returns></returns>
        //public static PayCycleBlockes getPayCycleInfo(int SegmentId, DateTime InrequreDate, int EmployeeNo)
        //{
        //    PayCycleBlockes _objPayCycles = new PayCycleBlockes();
        //    try
        //    {
        //        string _sql = string.Format(CommonFunctions.getSqlDateFormate() + "Select * from MasterPayCycleBlockes where BlockStartDate <= '{0}' and BlockEndDate >='{0}' and PaycycleID in (Select PaycycleID from MasterEmployee where Employeeno = {1})", CommonFunctions.getSqlDateValue(InrequreDate.Date), EmployeeNo.ToString());
        //        List<PayCycleBlockes> _lstblocks = _objPayCycles.GetObjectList(_sql);

        //        if (_lstblocks.Count > 0)
        //        {
        //            _objPayCycles = _lstblocks[0];
        //        }
        //        else
        //        {
        //            _objPayCycles = null;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _globalUser.ErrorObject = ex;
        //    }

        //    return _objPayCycles;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DsSource"></param>
        /// <param name="strPrimaryIDFeild"></param>
        /// <param name="strParentIDField"></param>
        /// <param name="strPrimaryName"></param>
        /// <param name="intPrimaryParentNodeID"></param>
        /// <returns></returns>
        public static TreeNode[] getTreeNodes(DataSet DsSource, string strPrimaryIDFeild, string strParentIDField, string strPrimaryName, int? intPrimaryParentNodeID)
        {
            TreeNode[] _trNodes = null;
            List<TreeNode> _lstChildNodes = new List<TreeNode>();
            DataRow[] _drs = DsSource.Tables[0].Select(strParentIDField + ((intPrimaryParentNodeID == null) ? " is null " : " = " + intPrimaryParentNodeID.ToString()));


            if (_drs.Length > 0)
            {
                DataSet _dtFiltered = new DataSet("FilteredDataSet");
                _dtFiltered.Tables.Add(_drs.CopyToDataTable());
                _dtFiltered.Tables[0].TableName = DsSource.Tables[0].TableName;

                TreeNode _tnNode = new TreeNode();

                foreach (DataRow _dr in _drs)
                {
                    int intSelectedParentId = (int)_dr[strPrimaryIDFeild];
                    TreeNode[] _childNodes = getTreeNodes(DsSource, strPrimaryIDFeild, strParentIDField, strPrimaryName, intSelectedParentId);
                    if (_childNodes != null)
                    {
                         _tnNode = new TreeNode(_dr[strPrimaryName].ToString(), _childNodes);
                        _tnNode.Tag = _dr[strPrimaryIDFeild];
                        _tnNode.Name = _dr[strPrimaryName].ToString();
                        _lstChildNodes.Add(_tnNode);
                    }
                    else
                    {
                        _tnNode = new TreeNode(_dr[strPrimaryName].ToString());
                        _tnNode.Tag = _dr[strPrimaryIDFeild];
                        _tnNode.Name = _dr[strPrimaryName].ToString();
                        _lstChildNodes.Add(_tnNode);

                    }
                    //_trNodes = _lstChildNodes.ToArray<TreeNode>();
                }

            }
            _trNodes = _lstChildNodes.ToArray<TreeNode>();
            return _trNodes;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DsSource"></param>
        /// <param name="strPrimaryIDFeild"></param>
        /// <param name="strParentIDField"></param>
        /// <param name="strPrimaryName"></param>
        /// <param name="intPrimaryParentNodeID"></param>
        /// <returns></returns>
        public static TreeNode[] getTreeNodes(DataSet DsSource, string strPrimaryIDFeild, string strParentIDField, string strPrimaryName, int? intPrimaryParentNodeID, string _strDefualtNode = "")
        {
            TreeNode[] _trNodes = null;
            List<TreeNode> _lstChildNodes = new List<TreeNode>();
            DataRow[] _drs = DsSource.Tables[0].Select(strParentIDField + ((intPrimaryParentNodeID == null) ? " is null " : " = " + intPrimaryParentNodeID.ToString()));

            //-----------------------------------------------------------------
            // add default node
            if (_strDefualtNode.Length > 0)
            {
                TreeNode _tnNode = new TreeNode(_strDefualtNode);
                _tnNode.Tag = -1;// this is the default node id
                _tnNode.Name = _strDefualtNode;
                //_tnNode.ImageIndex = 0;
                _tnNode.ForeColor = Color.Red;
                //_tnNode.NodeFont = new System.Drawing.Font("Tahoma", 9, FontStyle.Bold);
                _lstChildNodes.Add(_tnNode);
            }
            //-----------------------------------------------------------------

            if (_drs.Length > 0)
            {
                DataSet _dtFiltered = new DataSet("FilteredDataSet");
                _dtFiltered.Tables.Add(_drs.CopyToDataTable());
                _dtFiltered.Tables[0].TableName = DsSource.Tables[0].TableName;



                foreach (DataRow _dr in _drs)
                {
                    int intSelectedParentId = (int)_dr[strPrimaryIDFeild];
                    TreeNode[] _childNodes = getTreeNodes(DsSource, strPrimaryIDFeild, strParentIDField, strPrimaryName, intSelectedParentId);
                    if (_childNodes != null)
                    {
                        TreeNode _tnNode = new TreeNode(_dr[strPrimaryName].ToString(), _childNodes);
                        _tnNode.Tag = _dr[strPrimaryIDFeild];
                        _tnNode.Name = _dr[strPrimaryName].ToString();
                        _tnNode.ForeColor = Color.Red;
                        //_tnNode.NodeFont = new System.Drawing.Font("Tahoma", 9, FontStyle.Bold);
                        _tnNode.ImageIndex = 0;
                        _lstChildNodes.Add(_tnNode);
                    }
                    else
                    {
                        TreeNode _tnNode = new TreeNode(_dr[strPrimaryName].ToString());
                        _tnNode.Tag = _dr[strPrimaryIDFeild];
                        _tnNode.Name = _dr[strPrimaryName].ToString();
                        _tnNode.ForeColor = Color.Red;
                        //_tnNode.NodeFont = new System.Drawing.Font("Tahoma", 9, FontStyle.Bold);
                        _tnNode.ImageIndex = 0;
                        _lstChildNodes.Add(_tnNode);

                    }
                    //_trNodes = _lstChildNodes.ToArray<TreeNode>();
                }

            }
            _trNodes = _lstChildNodes.ToArray<TreeNode>();
            return _trNodes;

        }



        /// <summary>
        /// check if null type boolean value
        /// </summary>
        /// <param name="CheckBooleanValue"></param>
        /// <returns></returns>
        public static bool IsNullBooelan(bool? CheckBooleanValue)
        {
            if (CheckBooleanValue == null)
            { return false; }
            else { return (Boolean)CheckBooleanValue; }
        }

        /// <summary>
        /// check if a null type sting value
        /// </summary>
        /// <param name="CheckValue"></param>
        /// <returns></returns>
        public static string IsNullstring(object CheckValue)
        {
            if (CheckValue == null)
            { return string.Empty; }
            else { return (string)CheckValue.ToString(); }
        }

        /// <summary>
        /// loads lookup controls
        /// </summary>
        /// <param name="SourceControl"></param>
        /// <param name="_dataObject"></param>
        /// <param name="FilterColumns"></param>
        //public static void LoadLookupControls(LookUpEdit SourceControl, object _dataObject, string[] FilterColumns = null)
        //{
        //    DataSet _dsSourceDataset = null;
        //    try
        //    {

        //        Base _ObjbaseClass = (Base)_dataObject;
        //        if (_dataObject.GetType() != typeof(Employee))
        //        {
        //            _dsSourceDataset = _ObjbaseClass.GetList();
        //        }
        //        else
        //        {
        //            _dsSourceDataset = ((Employee)_ObjbaseClass).GetLimitedList();
        //        }


        //        string _DisplayColum = _ObjbaseClass.MainDisplaycolumns[0];
        //        string _ValueColumn = _ObjbaseClass.MainIDColumns[0];
        //        FilterColumns = _ObjbaseClass.MainDisplaycolumns;
        //        //if (_DisplayColum
        //        _dsSourceDataset.Tables[0].DefaultView.Sort = _DisplayColum;
        //        // Bind the edit value to the ProductID field of the "Order Details" table.
        //        // The edit value matches the value of the ValueMember field
        //        //SourceControl.DataBindings.Add("EditValue", bsMain_OrderDetails, "ProductID");

        //        // Specify the data source to display in the dropdown.
        //        // Bind the edit value to the ProductID field of the "Order Details" table.
        //        // The edit value matches the value of the ValueMember field
        //        //SourceControl.DataBindings.Add("EditValue", bsMain_OrderDetails, "ProductID");

        //        // Specify the data source to display in the dropdown.
        //        SourceControl.Properties.DataSource = _dsSourceDataset.Tables[0];
        //        // The field providing the editor's display text.
        //        SourceControl.Properties.DisplayMember = _DisplayColum;
        //        // The field matching the edit value.
        //        SourceControl.Properties.ValueMember = _ValueColumn;
        //        // clear the text in the text box of the look up control
        //        if (_dsSourceDataset.Tables[0].Rows.Count > 0)
        //        { 
        //           //SourceControl.EditValue = _dsSourceDataset.Tables[0].Rows[0][_ValueColumn]; 
        //          // SourceControl.EditValue = -1;
        //           //SourceControl.SelectedText = "-- Select From Dropdown --";

        //            SourceControl.EditValue = null;
        //            SourceControl.Properties.NullText = "-Please Select-";
                   
        //        }
        //        //*********************************
        //       // SourceControl.SelectedText = "-- Select From Dropdown --";
        //        //SourceControl.
        //        //SourceControl.Text = "-- Select From Dropdown --";

        //        //clear existing columns
        //        SourceControl.Properties.Columns.Clear();
        //        // Add two columns to the dropdown.
        //        int _index = 0, _intAutoSearchColum = 0;


        //        foreach (DataColumn item in _dsSourceDataset.Tables[0].Columns)
        //        {
        //            if (item.ColumnName.ToUpper().IndexOf("ID") <= 0 && item.ColumnName.ToUpper() != ("LastModifyDate").ToUpper() && item.ColumnName.ToUpper() != ("LastModifyUser").ToUpper())
        //            {
        //                if (FilterColumns != null)
        //                {
        //                    if (FilterColumns.Length <= 0)
        //                    { SourceControl.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.ColumnName, 0)); }
        //                    else
        //                    {
        //                        foreach (string item2 in FilterColumns)
        //                        {
        //                            if (item2 == item.ColumnName.ToString())
        //                            {
        //                                SourceControl.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.ColumnName, 0));
        //                                break;
        //                            }
        //                        }

        //                    }
        //                }

        //            }

        //            if (item.ColumnName.ToUpper() == _DisplayColum)
        //            { _intAutoSearchColum = _index; }
        //            _index++;
        //        }
        //        //  Set column widths according to their contents and resize the popup, if required.
        //        SourceControl.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;

        //        // Enable auto completion search mode.
        //        SourceControl.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
        //        // Specify the column against which to perform the search.
        //        SourceControl.Properties.AutoSearchColumnIndex = _intAutoSearchColum;
        //    }
        //    catch (Exception ex)
        //    {

        //        _globalUser.ErrorObject = ex;
        //    }
        //}


        /// <summary>
        /// loads lookup controls
        /// </summary>
        /// <param name="SourceControl"></param>
        /// <param name="_dataObject"></param>
        /// <param name="FilterColumns"></param>
        public static void LoadLookupControls(LookUpEdit SourceControl, DataSet _dsSourceDataset, string DisplayColumn, string ValueColumn, string[] FilterColumns = null)
        {
            try
            {

                string _DisplayColum = DisplayColumn;
                string _ValueColumn = ValueColumn;

                FilterColumns = new string[] { DisplayColumn };
                _dsSourceDataset.Tables[0].DefaultView.Sort = _DisplayColum;
                // Bind the edit value to the ProductID field of the "Order Details" table.
                // The edit value matches the value of the ValueMember field
                //SourceControl.DataBindings.Add("EditValue", bsMain_OrderDetails, "ProductID");

                // Specify the data source to display in the dropdown.
                // Bind the edit value to the ProductID field of the "Order Details" table.
                // The edit value matches the value of the ValueMember field
                //SourceControl.DataBindings.Add("EditValue", bsMain_OrderDetails, "ProductID");

                // Specify the data source to display in the dropdown.
                SourceControl.Properties.DataSource = _dsSourceDataset.Tables[0];
                // The field providing the editor's display text.
                SourceControl.Properties.DisplayMember = _DisplayColum;
                // The field matching the edit value.
                SourceControl.Properties.ValueMember = _ValueColumn;
                // clear the text in the text box of the look up control
                if (_dsSourceDataset.Tables[0].Rows.Count > 0)
                {
                    SourceControl.EditValue = null;
                    SourceControl.Properties.NullText = "-Please Select-";
                    //SourceControl.EditValue = _dsSourceDataset.Tables[0].Rows[0][_ValueColumn];              
                
                }
                //*********************************
                SourceControl.Text = "";
                //clear existing columns
                SourceControl.Properties.Columns.Clear();
                // Add two columns to the dropdown.
                int _index = 0, _intAutoSearchColum = 0;


                foreach (DataColumn item in _dsSourceDataset.Tables[0].Columns)
                {
                    if (item.ColumnName.ToUpper().IndexOf("ID") <= 0 && item.ColumnName.ToUpper() != ("LastModifyDate").ToUpper() && item.ColumnName.ToUpper() != ("LastModifyUser").ToUpper())
                    {
                        if (FilterColumns != null)
                        {
                            if (FilterColumns.Length <= 0)
                            { SourceControl.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.ColumnName, 0)); }
                            else
                            {
                                foreach (string item2 in FilterColumns)
                                {
                                    if (item2 == item.ColumnName.ToString())
                                    {
                                        SourceControl.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.ColumnName, 0));
                                        break;
                                    }
                                }

                            }
                        }

                    }

                    if (item.ColumnName.ToUpper() == _DisplayColum)
                    { _intAutoSearchColum = _index; }
                    _index++;
                }
                //  Set column widths according to their contents and resize the popup, if required.
                SourceControl.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;

                // Enable auto completion search mode.
                SourceControl.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                // Specify the column against which to perform the search.
                SourceControl.Properties.AutoSearchColumnIndex = _intAutoSearchColum;
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
        }

        public static void LoadLookupControlsRes(LookUpEdit SourceControl, DataSet _dsSourceDataset, string DisplayColumn, string ValueColumn, string[] FilterColumns = null)
        {
            try
            {

                string _DisplayColum = DisplayColumn;
                string _ValueColumn = ValueColumn;

                FilterColumns = new string[] { DisplayColumn };
                _dsSourceDataset.Tables[0].DefaultView.Sort = _DisplayColum;
                // Bind the edit value to the ProductID field of the "Order Details" table.
                // The edit value matches the value of the ValueMember field
                //SourceControl.DataBindings.Add("EditValue", bsMain_OrderDetails, "ProductID");

                // Specify the data source to display in the dropdown.
                // Bind the edit value to the ProductID field of the "Order Details" table.
                // The edit value matches the value of the ValueMember field
                //SourceControl.DataBindings.Add("EditValue", bsMain_OrderDetails, "ProductID");

                // Specify the data source to display in the dropdown.
                SourceControl.Properties.DataSource = _dsSourceDataset.Tables[0];
                // The field providing the editor's display text.
                SourceControl.Properties.DisplayMember = _DisplayColum;
                // The field matching the edit value.
                SourceControl.Properties.ValueMember = _ValueColumn;
                // clear the text in the text box of the look up control
                if (_dsSourceDataset.Tables[0].Rows.Count > 0)
                {
                    //SourceControl.EditValue = null;
                    SourceControl.Properties.NullText = "-Please Select-";
                    //SourceControl.EditValue = _dsSourceDataset.Tables[0].Rows[0][_ValueColumn];              

                }
                //*********************************
                SourceControl.Text = "";
                //clear existing columns
                SourceControl.Properties.Columns.Clear();
                // Add two columns to the dropdown.
                int _index = 0, _intAutoSearchColum = 0;


                foreach (DataColumn item in _dsSourceDataset.Tables[0].Columns)
                {
                    if (item.ColumnName.ToUpper().IndexOf("ID") <= 0 && item.ColumnName.ToUpper() != ("LastModifyDate").ToUpper() && item.ColumnName.ToUpper() != ("LastModifyUser").ToUpper())
                    {
                        if (FilterColumns != null)
                        {
                            if (FilterColumns.Length <= 0)
                            { SourceControl.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.ColumnName, 0)); }
                            else
                            {
                                foreach (string item2 in FilterColumns)
                                {
                                    if (item2 == item.ColumnName.ToString())
                                    {
                                        SourceControl.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.ColumnName, 0));
                                        break;
                                    }
                                }

                            }
                        }

                    }

                    if (item.ColumnName.ToUpper() == _DisplayColum)
                    { _intAutoSearchColum = _index; }
                    _index++;
                }
                //  Set column widths according to their contents and resize the popup, if required.
                SourceControl.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;

                // Enable auto completion search mode.
                SourceControl.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                // Specify the column against which to perform the search.
                SourceControl.Properties.AutoSearchColumnIndex = _intAutoSearchColum;
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
        }


        public static void LoadLookupControlsNew(LookUpEdit SourceControl, DataSet _dsSourceDataset, string DisplayColumn, string ValueColumn, string[] FilterColumns = null)
        {
            try
            {

                string _DisplayColum = DisplayColumn;
                string _ValueColumn = ValueColumn;

                FilterColumns = new string[] { DisplayColumn };
                _dsSourceDataset.Tables[0].DefaultView.Sort = _DisplayColum;
                // Bind the edit value to the ProductID field of the "Order Details" table.
                // The edit value matches the value of the ValueMember field
                //SourceControl.DataBindings.Add("EditValue", bsMain_OrderDetails, "ProductID");

                // Specify the data source to display in the dropdown.
                // Bind the edit value to the ProductID field of the "Order Details" table.
                // The edit value matches the value of the ValueMember field
                //SourceControl.DataBindings.Add("EditValue", bsMain_OrderDetails, "ProductID");

                // Specify the data source to display in the dropdown.
                SourceControl.Properties.DataSource = _dsSourceDataset.Tables[0];
                // The field providing the editor's display text.
                SourceControl.Properties.DisplayMember = _DisplayColum;
                // The field matching the edit value.
                SourceControl.Properties.ValueMember = _ValueColumn;
                // clear the text in the text box of the look up control
                if (_dsSourceDataset.Tables[0].Rows.Count > 0)
                {
                    SourceControl.EditValue = null;
                    SourceControl.Properties.NullText = "-Please Select-";
                    //SourceControl.EditValue = _dsSourceDataset.Tables[0].Rows[0][_ValueColumn];              

                }
                //*********************************
                SourceControl.Text = "";
                //clear existing columns
                SourceControl.Properties.Columns.Clear();
                // Add two columns to the dropdown.
                int _index = 0, _intAutoSearchColum = 0;


                foreach (DataColumn item in _dsSourceDataset.Tables[0].Columns)
                {
                    if (item.ColumnName.ToUpper().IndexOf("ID") <= 0 && item.ColumnName.ToUpper() != ("LastModifyDate").ToUpper() && item.ColumnName.ToUpper() != ("LastModifyUser").ToUpper())
                    {
                        if (FilterColumns != null)
                        {
                            if (FilterColumns.Length <= 0)
                            { SourceControl.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.ColumnName, 0)); }
                            else
                            {
                                foreach (string item2 in FilterColumns)
                                {
                                    if (item2 == item.ColumnName.ToString())
                                    {
                                        SourceControl.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.ColumnName, 0));
                                        break;
                                    }
                                }

                            }
                        }

                    }

                    if (item.ColumnName.ToUpper() == _DisplayColum)
                    { _intAutoSearchColum = _index; }
                    _index++;
                }
                //  Set column widths according to their contents and resize the popup, if required.
                SourceControl.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;

                // Enable auto completion search mode.
                SourceControl.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                // Specify the column against which to perform the search.
                SourceControl.Properties.AutoSearchColumnIndex = _intAutoSearchColum;
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
        }

        /// <summary>
        /// loads check box controls
        /// </summary>
        /// <param name="SourceControl"></param>
        /// <param name="_dataObject"></param>
        public static void LoadCheckBoxControls(DevExpress.XtraEditors.CheckedListBoxControl SourceControl, object _dataObject)
        {
            try
            {

                Base _ObjbaseClass = (Base)_dataObject;
                DataSet _dsSourceDataset = _ObjbaseClass.GetList();
                string _DisplayColum = _ObjbaseClass.MainDisplaycolumns[0];
                string _ValueColumn = _ObjbaseClass.MainIDColumns[0];

                // Specify the data source to display in the dropdown.
                SourceControl.DataSource = _dsSourceDataset.Tables[0];
                // The field providing the editor's display text.
                SourceControl.DisplayMember = _DisplayColum;
                // The field matching the edit value.
                SourceControl.ValueMember = _ValueColumn;

            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
        }


        /// <summary>
        /// loads the checkedComboBoxEdit control with data
        /// </summary>
        /// <param name="SourceControl"></param>
        /// <param name="_dataObject"></param>
        public static void LoadCheckedComboBoxEditControls(DevExpress.XtraEditors.CheckedComboBoxEdit SourceControl, object _dataObject)
        {
            try
            {

                Base _ObjbaseClass = (Base)_dataObject;
                DataSet _dsSourceDataset = _ObjbaseClass.GetList();
                string _DisplayColum = _ObjbaseClass.MainDisplaycolumns[0];
                string _ValueColumn = _ObjbaseClass.MainIDColumns[0];

                SourceControl.Properties.Items.Clear();
                // Specify the data source to display in the dropdown.
                SourceControl.Properties.DataSource = _dsSourceDataset.Tables[0];
                //SourceControl.DataBindings.ite = _dsSourceDataset.Tables[0];
                // The field providing the editor's display text.
                SourceControl.Properties.DisplayMember = _DisplayColum;
                // The field matching the edit value.
                SourceControl.Properties.ValueMember = _ValueColumn;
                SourceControl.CheckAll();
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
        }

        public static void LoadCheckedComboBoxEditControlsUnCheck(DevExpress.XtraEditors.CheckedComboBoxEdit SourceControl, object _dataObject)
        {
            try
            {

                Base _ObjbaseClass = (Base)_dataObject;
                DataSet _dsSourceDataset = _ObjbaseClass.GetList();
                string _DisplayColum = _ObjbaseClass.MainDisplaycolumns[0];
                string _ValueColumn = _ObjbaseClass.MainIDColumns[0];

                SourceControl.Properties.Items.Clear();
                // Specify the data source to display in the dropdown.
                SourceControl.Properties.DataSource = _dsSourceDataset.Tables[0];
                //SourceControl.DataBindings.ite = _dsSourceDataset.Tables[0];
                // The field providing the editor's display text.
                SourceControl.Properties.DisplayMember = _DisplayColum;
                // The field matching the edit value.
                SourceControl.Properties.ValueMember = _ValueColumn;
                //SourceControl.CheckAll();
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
        }

        public static void LoadCheckedComboBoxEditControlsWithDataSet(DevExpress.XtraEditors.CheckedComboBoxEdit SourceControl, DataSet ds, string DisplayCol, string ValueCol)
        {
            try
            {
                DataSet _dsSourceDataset = ds;
                string _DisplayColum = DisplayCol;
                string _ValueColumn = ValueCol;
                SourceControl.Properties.Items.Clear();
                // Specify the data source to display in the dropdown.
                SourceControl.Properties.DataSource = _dsSourceDataset.Tables[0];
                //SourceControl.DataBindings.ite = _dsSourceDataset.Tables[0];
                // The field providing the editor's display text.
                SourceControl.Properties.DisplayMember = _DisplayColum;
                // The field matching the edit value.
                SourceControl.Properties.ValueMember = _ValueColumn;
                SourceControl.CheckAll();
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }
        }


        /// loads the checkedComboBoxEdit control with data - Added by C on 2020-01-13
        /// </summary>
        /// <param name="SourceControl"></param>
        /// <param name="_dataObject"></param>
        public static void LoadCheckedComboBoxEditControls(DevExpress.XtraEditors.CheckedComboBoxEdit SourceControl, object _dataObject, Boolean IsByMonthSegment = false)
        {
            try
            {

                Base _ObjbaseClass = (Base)_dataObject;
                DataSet _dsSourceDataset = null;

                if (IsByMonthSegment == false)
                {
                    if (_dataObject.GetType() != typeof(Employee))
                    {
                        _dsSourceDataset = _ObjbaseClass.GetList();
                    }
                    else
                    {
                        _dsSourceDataset = ((Employee)_ObjbaseClass).GetLimitedList();
                    }
                }
                else
                {
                    _dsSourceDataset = _ObjbaseClass.GetListByMonth();
                    //Added To Get Method For Pay Cycle And Pay Cycle Block To Filter By Segment And Selected Month
                }

                string _DisplayColum = _ObjbaseClass.MainDisplaycolumns[0];
                string _ValueColumn = _ObjbaseClass.MainIDColumns[0];

                SourceControl.Properties.Items.Clear();
                // Specify the data source to display in the dropdown.
                SourceControl.Properties.DataSource = _dsSourceDataset.Tables[0];
                //SourceControl.DataBindings.ite = _dsSourceDataset.Tables[0];
                // The field providing the editor's display text.
                SourceControl.Properties.DisplayMember = _DisplayColum;
                // The field matching the edit value.
                SourceControl.Properties.ValueMember = _ValueColumn;
                SourceControl.CheckAll();
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
        }


        //public static void ClearControls(Control.ControlCollection  _ControlCollection,bool _ClearLockedControls = true)
        //{
        //    foreach (Control c in _ControlCollection)
        //    {

        //        try
        //        {

        //            try
        //            {
        //                if (c.Controls.Count > 0)
        //                {
        //                    ClearControls(c.Controls);
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //                _globalUser.ErrorObject = ex;
        //            }

        //            if (c.GetType() == typeof(DevExpress.XtraEditors.TextEdit))
        //            {

        //                ((DevExpress.XtraEditors.TextEdit)c).Text = string.Empty;
        //            }

        //            if (c.GetType() == typeof(DevExpress.XtraEditors.MemoEdit))
        //            {
        //                ((DevExpress.XtraEditors.MemoEdit)c).Text = string.Empty;
        //            }

        //            //if (c.GetType() == typeof(DevExpress.XtraEditors.LookUpEdit))
        //            //{
        //            //    ((DevExpress.XtraEditors.LookUpEdit)c).EditValue = 0;
        //            //}

        //            if (c.GetType() == typeof(DevExpress.XtraEditors.SpinEdit))
        //            {
        //                ((DevExpress.XtraEditors.SpinEdit)c).EditValue = 0;
        //            }

        //            if (c.GetType() == typeof(TextBox))
        //            {
        //                ((TextBox)c).Text = string.Empty;
        //            }


        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }
        //}

        public static string SplitCamelCase(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }

        //public static byte[] ReadFileFromDisk(string filePath)
        //{
        //    byte[] binaryfile = null;
        //    try
        //    {
        //        binaryfile = System.IO.File.ReadAllBytes(filePath);
        //    }
        //    catch (Exception ex)
        //    {

        //        _globalUser.ErrorObject = ex;
        //    }
        //    return binaryfile;
        //}

        /// <summary>
        /// writes a file to disk
        /// </summary>
        /// <param name="binaryFile"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool WriteFiletoDisk(byte[] binaryFile, string filePath)
        {
            bool blnWriteFiled = false;
            try
            {
                System.IO.File.WriteAllBytes(filePath, binaryFile);
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }
            return blnWriteFiled;
        }

        /// <summary>
        /// returns the selected file path with the application path
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static string getTempFilePath(string FilePath)
        {
            return Application.StartupPath + "\\" + FilePath;
        }

        /// <summary>
        /// returns the selected file type
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string SelectFileType(string fileName)
        {
            int x = Array.IndexOf(imageTypes, fileName);
            int y = Array.IndexOf(documrntType, fileName);

            string typeoffile = "non";
            if (Array.IndexOf(imageTypes, fileName.ToUpper()) >= 0)
            {
                typeoffile = "Image";
            }
            if (Array.IndexOf(documrntType, fileName.ToUpper()) >= 0)
            {
                typeoffile = "Doc";
            }
            return typeoffile;
        }


        /// <summary>
        /// Run file with shell commands
        /// </summary>
        /// <param name="FileName"></param>
        public static void ExecuteFile(string FileName)
        {
            try
            {
                // Use ProcessStartInfo class
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.FileName = FileName;
                startInfo.WindowStyle = ProcessWindowStyle.Maximized;
                try
                {
                    // Start the process with the info we specified.
                    // Call WaitForExit and then the using statement will close.
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();
                    }
                }
                catch
                {
                    // Log error.
                }
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }
        }

        /// <summary>
        /// Return temp file name for a given extension
        /// </summary>
        /// <param name="File"></param>
        /// <param name="extention"></param>
        /// <returns></returns>
        public static string getTempFileName(byte[] File, string extention)
        {
            string fileName = "";
            try
            {
                fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + "." + extention;
                CommonFunctions.WriteFiletoDisk(File, fileName);
                System.IO.FileInfo file = new System.IO.FileInfo(fileName);
                if (!file.Exists)
                {
                    fileName = "";
                }
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }
            return fileName;
        }


        //public static void LoadTokenEditFilter(DevExpress.XtraEditors.TokenEdit tokenEditControl, object _objBusinessLayer, string DisplayFiled = null, string ValueField = null)
        //{
        //    try
        //    {
        //        tokenEditControl.Properties.Tokens.Clear();

        //        if (DisplayFiled == null)
        //        { DisplayFiled = ((BusinessLayer.Base)_objBusinessLayer).MainDisplaycolumns[0]; }

        //        if (ValueField == null)
        //        { ValueField = ((BusinessLayer.Base)_objBusinessLayer).MainIDColumns[0]; }

        //        //---------------------------------------------------------------------
        //        tokenEditControl.Properties.Tokens.AddRange(CommonFunctions.getTokens(((BusinessLayer.Base)_objBusinessLayer).GetList(), DisplayFiled, ValueField));
        //        //---------------------------------------------------------------------
        //    }
        //    catch (Exception ex)
        //    {

        //        _globalUser.ErrorObject = ex;
        //    }

        //}

        public static void LoadTokenEditFilterNew(DevExpress.XtraEditors.TokenEdit tokenEditControl, DataSet _sdsources, string DisplayFiled = null, string ValueField = null)
        {
            try
            {

                tokenEditControl.Properties.Tokens.Clear();
                //---------------------------------------------------------------------
                tokenEditControl.Properties.Tokens.AddRange(CommonFunctions.getTokens(_sdsources, DisplayFiled, ValueField));
                //---------------------------------------------------------------------
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }

        }

        public static DateTime getDateTimeCombination(DateTime DateValue, TimeSpan TimeValue)
        {
            return DateValue + TimeValue;
        }

        //public static EmployeeGrievanceMessages[] LaunchGrievanceFilter(clsGobalUser _GlobalUser)
        //{
        //    try
        //    {
        //        _globalUser = _GlobalUser;
        //        Enquires.FrmGrievanceFilter frmGrievanceFilter = new Enquires.FrmGrievanceFilter(_globalUser);
        //        frmGrievanceFilter.ShowDialog();
        //        _SelectedGrievance = frmGrievanceFilter.getSelectedGrievance();
        //    }
        //    catch (Exception ex)
        //    {

        //        _globalUser.ErrorObject = ex;
        //    }
        //    if (_SelectedGrievance.Count > 0)
        //    { return _SelectedGrievance.ToArray(); }
        //    else
        //    {
        //        return new EmployeeGrievanceMessages[0];
        //    }
        //}

        //public static Employee[] LaunchFilterInterval(clsGobalUser _GlobalUser,int type)
        //{
        //    try
        //    {
        //        _globalUser = _GlobalUser;
        //        Enquires.FrmEmployeeFilter frmGrievanceFilter = new Enquires.FrmEmployeeFilter(_globalUser,type,false);
        //        frmGrievanceFilter.ShowDialog();
        //        _SelectedEmployees = frmGrievanceFilter.getSelectedEmployees();
        //    }
        //    catch (Exception ex)
        //    {

        //        _globalUser.ErrorObject = ex;
        //    }
        //    if (_SelectedEmployees.Count > 0)
        //    { return _SelectedEmployees.ToArray(); }
        //    else
        //    {
        //        return new Employee[0];
        //    }
        //}

        //public static Employee[] LaunchFilterGatePass(clsGobalUser _GlobalUser, int type,int GatePassTypeID)
        //{
        //    //try
        //    //{
        //    //    _globalUser = _GlobalUser;
        //    //    Enquires.FrmEmployeeFilter frmGrievanceFilter = new Enquires.FrmEmployeeFilter(_globalUser, type, false, GatePassTypeID);
        //    //    frmGrievanceFilter.ShowDialog();
        //    //    _SelectedEmployees = frmGrievanceFilter.getSelectedEmployees();
        //    //}
        //    //catch (Exception ex)
        //    //{

        //    //    _globalUser.ErrorObject = ex;
        //    //}
        //    //if (_SelectedEmployees.Count > 0)
        //    //{ return _SelectedEmployees.ToArray(); }
        //    //else
        //    //{
        //    //    return new Employee[0];
        //    //}
        //}

        public static void ClearControls(Control.ControlCollection _ControlCollection, bool _ClearLockedControls = true)
        {
            foreach (Control c in _ControlCollection)
            {

                try
                {

                    try
                    {
                        if (c.Controls.Count > 0)
                        {
                            ClearControls(c.Controls);
                        }
                    }
                    catch (Exception ex)
                    {

                        _globalUser.ErrorObject = ex;
                    }

                    if (c.GetType() == typeof(DevExpress.XtraEditors.TextEdit))
                    {
                        ((DevExpress.XtraEditors.TextEdit)c).Text = string.Empty;
                    }

                    if (c.GetType() == typeof(DevExpress.XtraEditors.DateEdit))
                    {
                        ((DevExpress.XtraEditors.DateEdit)c).DateTime = Convert.ToDateTime("01/01/1900");
                    }

                    if (c.GetType() == typeof(DevExpress.XtraEditors.MemoEdit))
                    {
                        ((DevExpress.XtraEditors.MemoEdit)c).Text = string.Empty;
                    }

                    if (c.GetType() == typeof(DevExpress.XtraEditors.CheckEdit))
                    {
                        ((DevExpress.XtraEditors.CheckEdit)c).Checked = false;
                    }

                    if (c.GetType() == typeof(DevExpress.XtraEditors.SpinEdit))
                    {
                        ((DevExpress.XtraEditors.SpinEdit)c).EditValue = 0;
                    }

                    if (c.GetType() == typeof(DevExpress.XtraEditors.TokenEdit))
                    {
                        ((DevExpress.XtraEditors.TokenEdit)c).EditValue = null;
                    }

                    if (c.GetType() == typeof(DevExpress.XtraEditors.ListBoxControl))
                    {
                        ((DevExpress.XtraEditors.ListBoxControl)c).Items.Clear();
                    }

                    //if (c.GetType() == typeof(System.Windows.Forms.TreeView))
                    //{
                    //    ((System.Windows.Forms.TreeView)c).Nodes.Clear();
                    //}

                    

                    if (c.GetType() == typeof(TextBox))
                    {
                        ((TextBox)c).Text = string.Empty;
                    }
                    
                }
                catch (Exception ex)
                {
                    //_globalUser.ErrorObject = ex;
                }
            }
        }

        /// <summary>
        /// reads fils from disk
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] ReadFileFromDisk(string filePath)
        {
            byte[] binaryfile = null;
            try
            {
                if (filePath != "")
                {
                    binaryfile = System.IO.File.ReadAllBytes(filePath);
                }
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }
            return binaryfile;
        }

        /// <summary>
        /// Moves the selected row in a grid
        /// </summary>
        /// <param name="grdView"></param>
        /// <param name="_direction"></param>
        public static void GridMove(DevExpress.XtraGrid.Views.Grid.GridView grdView, Direction _direction)
        {
            switch (_direction)
            {
                case Direction.Up:
                    grdView.MovePrev();
                    break;
                case Direction.Down:
                    grdView.MoveNext();
                    break;
                case Direction.Last:
                    grdView.MoveLast();
                    break;
                case Direction.First:
                    grdView.MoveFirst();
                    break;
                default:
                    break;
            }

        }
        //update by dinesh 2017/06/02
        public static List<string> getSqlDatabases(string SqlServer, string UserID, string Password)
        {
            List<String> _lstdatabases = new List<String>();
            try
            {
                SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();
                connection.DataSource = SqlServer;
                //enter credentials if you want
                connection.UserID = UserID;
                connection.Password = Password;
                String strConn = connection.ToString();

                //create connection
                SqlConnection sqlConn = new SqlConnection(strConn);
                //open connection
                sqlConn.Open();
                //get databases
                DataTable tblDatabases = sqlConn.GetSchema("Databases");
                //close connection
                sqlConn.Close();

                //add to list
                foreach (DataRow row in tblDatabases.Rows)
                {
                    String strDatabaseName = row["database_name"].ToString();
                    _lstdatabases.Add(strDatabaseName);
                }

                //_lstdatabases.Clear(); // clears the database 
                //if (_lstdatabases.Count > 0)
                //{
                //    foreach (string item in _lstdatabases)
                //    {
                //        _lstdatabases.Add(item);
                //    }
                //}

            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
            }
            return _lstdatabases;

        }
        /// <summary>
        /// returns a list of sql servers avalible in the network
        /// </summary>
        /// <returns></returns>
        public static List<string> getSqlServers()
        {
            List<string> _lstSqlServers = new List<string>();
            try
            {
                SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
                DataTable table = instance.GetDataSources();
                foreach (System.Data.DataRow row in table.Rows)
                {
                    if (row[0].ToString() != string.Empty)
                    {
                        _lstSqlServers.Add(row[0].ToString() + "\\" + row[1].ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;
                //throw;
            }
            return _lstSqlServers;
        }
        //------------------------------------------------------
        public static TreeNode[] getTreeNodesWEB(DataSet DsSource, string strPrimaryIDFeild, string strParentIDField, string strPrimaryName, int? intPrimaryParentNodeID)
        {
            TreeNode[] _trNodes = null;
            List<TreeNode> _lstChildNodes = new List<TreeNode>();
            DataRow[] _drs = DsSource.Tables[0].Select(strParentIDField + ((intPrimaryParentNodeID == null) ? " is null " : " = " + intPrimaryParentNodeID.ToString()));


            if (_drs.Length > 0)
            {
                DataSet _dtFiltered = new DataSet("FilteredDataSet");
                _dtFiltered.Tables.Add(_drs.CopyToDataTable());
                _dtFiltered.Tables[0].TableName = DsSource.Tables[0].TableName;

                foreach (DataRow _dr in _drs)
                {
                    int intSelectedParentId = (int)_dr[strPrimaryIDFeild];
                    TreeNode[] _childNodes = getTreeNodes(DsSource, strPrimaryIDFeild, strParentIDField, strPrimaryName, intSelectedParentId);
                    if (_childNodes != null)
                    {
                        TreeNode _tnNode = new TreeNode(_dr[strPrimaryName].ToString(), _childNodes);
                        _tnNode.Tag = _dr[strPrimaryIDFeild];
                        _tnNode.Name = _dr[strPrimaryName].ToString();
                        _lstChildNodes.Add(_tnNode);
                    }
                    else
                    {
                        TreeNode _tnNode = new TreeNode(_dr[strPrimaryName].ToString());
                        _tnNode.Tag = _dr[strPrimaryIDFeild];
                        _tnNode.Name = _dr[strPrimaryName].ToString();
                        _lstChildNodes.Add(_tnNode);

                    }
                    //_trNodes = _lstChildNodes.ToArray<TreeNode>();
                }

            }
            _trNodes = _lstChildNodes.ToArray<TreeNode>();
            return _trNodes;

        }

        public static bool CheckNumericMoreThanZero(double value)
        {

            bool Ischeck = true;
            try
            {
                if (value >= 0)
                {

                    Ischeck = true;
                }
                else {
                    Ischeck = false;
                }


            }
            catch (Exception ex){
                throw ex;
            }

            return Ischeck;
        }

        public static void LoadCheckedComboBoxEditControlsForBonus(DevExpress.XtraEditors.CheckedComboBoxEdit SourceControl, object _dataObject)
        {
            try
            {

                Base _ObjbaseClass = (Base)_dataObject;
                DataSet _dsSourceDataset = _ObjbaseClass.GetList();
                string _DisplayColum = _ObjbaseClass.MainDisplaycolumns[0];
                string _ValueColumn = _ObjbaseClass.MainIDColumns[0];

                SourceControl.Properties.Items.Clear();
                // Specify the data source to display in the dropdown.
                SourceControl.Properties.DataSource = _dsSourceDataset.Tables[0];
                //SourceControl.DataBindings.ite = _dsSourceDataset.Tables[0];
                // The field providing the editor's display text.
                SourceControl.Properties.DisplayMember = _DisplayColum;
                // The field matching the edit value.
                SourceControl.Properties.ValueMember = _ValueColumn;
                //if (SystemSetting != 1)
                //{
                //    SourceControl.CheckAll();
                //}
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
        }

        //public static void LoadLookupControls(LookUpEdit SourceControl, object _dataObject, string[] FilterColumns = null) //Hided By Thus On 19Aug2019
        public static void LoadLookupControls(LookUpEdit SourceControl, object _dataObject, string[] FilterColumns = null, Boolean IsByMonthSegment = false) //Added By Thus On 19Aug2019
        {
            DataSet _dsSourceDataset = null;
            try
            {

                Base _ObjbaseClass = (Base)_dataObject;
                if (IsByMonthSegment == false)
                {
                    if (_dataObject.GetType() != typeof(Employee))
                    {
                        _dsSourceDataset = _ObjbaseClass.GetList();
                    }
                    else
                    {
                        _dsSourceDataset = ((Employee)_ObjbaseClass).GetLimitedList();
                    }
                }
                else
                {
                    _dsSourceDataset = _ObjbaseClass.GetListByMonth();
                    //Added To Get Method For Pay Cycle And Pay Cycle Block To Filter By Segment And Selected Month
                }


                string _DisplayColum = _ObjbaseClass.MainDisplaycolumns[0];
                string _ValueColumn = _ObjbaseClass.MainIDColumns[0];
                FilterColumns = _ObjbaseClass.MainDisplaycolumns;
                //if (_DisplayColum
                _dsSourceDataset.Tables[0].DefaultView.Sort = _DisplayColum;
                // Bind the edit value to the ProductID field of the "Order Details" table.
                // The edit value matches the value of the ValueMember field
                //SourceControl.DataBindings.Add("EditValue", bsMain_OrderDetails, "ProductID");

                // Specify the data source to display in the dropdown.
                // Bind the edit value to the ProductID field of the "Order Details" table.
                // The edit value matches the value of the ValueMember field
                //SourceControl.DataBindings.Add("EditValue", bsMain_OrderDetails, "ProductID");

                // Specify the data source to display in the dropdown.
                SourceControl.Properties.DataSource = _dsSourceDataset.Tables[0];
                // The field providing the editor's display text.
                SourceControl.Properties.DisplayMember = _DisplayColum;
                // The field matching the edit value.
                SourceControl.Properties.ValueMember = _ValueColumn;
                // clear the text in the text box of the look up control
                if (_dsSourceDataset.Tables[0].Rows.Count > 0)
                {
                    //SourceControl.EditValue = _dsSourceDataset.Tables[0].Rows[0][_ValueColumn]; 
                    // SourceControl.EditValue = -1;
                    //SourceControl.SelectedText = "-- Select From Dropdown --";

                    SourceControl.EditValue = null;
                    SourceControl.Properties.NullText = "-Please Select-";

                }
                //*********************************
                // SourceControl.SelectedText = "-- Select From Dropdown --";
                //SourceControl.
                //SourceControl.Text = "-- Select From Dropdown --";

                //clear existing columns
                SourceControl.Properties.Columns.Clear();
                // Add two columns to the dropdown.
                int _index = 0, _intAutoSearchColum = 0;


                foreach (DataColumn item in _dsSourceDataset.Tables[0].Columns)
                {
                    if (item.ColumnName.ToUpper().IndexOf("ID") <= 0 && item.ColumnName.ToUpper() != ("LastModifyDate").ToUpper() && item.ColumnName.ToUpper() != ("LastModifyUser").ToUpper())
                    {
                        if (FilterColumns != null)
                        {
                            if (FilterColumns.Length <= 0)
                            { SourceControl.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.ColumnName, 0)); }
                            else
                            {
                                foreach (string item2 in FilterColumns)
                                {
                                    if (item2 == item.ColumnName.ToString())
                                    {
                                        SourceControl.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.ColumnName, 0));
                                        break;
                                    }
                                }

                            }
                        }

                    }

                    if (item.ColumnName.ToUpper() == _DisplayColum)
                    { _intAutoSearchColum = _index; }
                    _index++;
                }
                //  Set column widths according to their contents and resize the popup, if required.
                SourceControl.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;

                // Enable auto completion search mode.
                SourceControl.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                // Specify the column against which to perform the search.
                SourceControl.Properties.AutoSearchColumnIndex = _intAutoSearchColum;
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
        }

        //Add By Nuwan 
        public static void LoadTokenEditFilterLeave(DevExpress.XtraEditors.TokenEdit tokenEditControl, DataSet PeriodBreakDown, string DisplayFiled = null, string ValueField = null)
        {
            try
            {
                tokenEditControl.Properties.Tokens.Clear();


                //---------------------------------------------------------------------
                tokenEditControl.Properties.Tokens.AddRange(CommonFunctions.getTokensLeave(PeriodBreakDown, "LeaveDate", "LeavePeriodBreakdownID", "LeaveValue", "LeavePeriodName"));
                //---------------------------------------------------------------------
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }

        }
        //Add By Nuwan 
        public static DevExpress.XtraEditors.TokenEditToken[] getTokensLeave(DataSet _dsSource, string fieldName, string idFieldName, string LeaveVAlue, string LeavePeriodName)
        {
            List<DevExpress.XtraEditors.TokenEditToken> lstTokens = new List<TokenEditToken>();
            try
            {
                foreach (DataRow item in _dsSource.Tables[0].Rows)
                {
                    if (item != null)
                    {
                        if (lstTokens.IndexOf(new TokenEditToken(item[fieldName].ToString(), item[idFieldName])) <= 0)
                        {
                            lstTokens.Add(new TokenEditToken(Convert.ToDateTime(item[fieldName].ToString()).ToString("dd/MM/yyyy") + " - " + item[LeaveVAlue].ToString() + " " + item[LeavePeriodName], item[idFieldName]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
            return lstTokens.ToArray<DevExpress.XtraEditors.TokenEditToken>();
        }

        public static DataSet Title()
        {//Added By Thus On 10Sep2019
            DataSet datTitle = new DataSet();
            try
            {
                DataTable tabTitle = new DataTable();

                tabTitle.Columns.Add("TitleView");
                tabTitle.Columns.Add("Title");

                DataRow datRow = tabTitle.NewRow();
                datRow["TitleView"] = "Mr";
                datRow["Title"] = "Mr";
                tabTitle.Rows.Add(datRow);

                datRow = tabTitle.NewRow();
                datRow["TitleView"] = "Master";
                datRow["Title"] = "Master";
                tabTitle.Rows.Add(datRow);

                datRow = tabTitle.NewRow();
                datRow["TitleView"] = "Miss";
                datRow["Title"] = "Miss";
                tabTitle.Rows.Add(datRow);

                datRow = tabTitle.NewRow();
                datRow["TitleView"] = "Mrs";
                datRow["Title"] = "Mrs";
                tabTitle.Rows.Add(datRow);

                datRow = tabTitle.NewRow();
                datRow["TitleView"] = "Dr";
                datRow["Title"] = "Dr";
                tabTitle.Rows.Add(datRow);

                datRow = tabTitle.NewRow();
                datRow["TitleView"] = "Other";
                datRow["Title"] = "Other";
                tabTitle.Rows.Add(datRow);

                datTitle.Tables.Add(tabTitle);

                return datTitle;
            }
            catch (Exception ex)
            {
                _globalUser.ErrorObject = ex;

                return datTitle;
            }
        }

        public static void LoadLookupControlsEdit(LookUpEdit SourceControl, DataSet _dsSourceDataset, string DisplayColumn, string ValueColumn, string[] FilterColumns = null)
        {
            try
            {

                string _DisplayColum = DisplayColumn;
                string _ValueColumn = ValueColumn;

                FilterColumns = new string[] { DisplayColumn };
                _dsSourceDataset.Tables[0].DefaultView.Sort = _DisplayColum;
                // Bind the edit value to the ProductID field of the "Order Details" table.
                // The edit value matches the value of the ValueMember field
                //SourceControl.DataBindings.Add("EditValue", bsMain_OrderDetails, "ProductID");

                // Specify the data source to display in the dropdown.
                // Bind the edit value to the ProductID field of the "Order Details" table.
                // The edit value matches the value of the ValueMember field
                //SourceControl.DataBindings.Add("EditValue", bsMain_OrderDetails, "ProductID");

                // Specify the data source to display in the dropdown.
                SourceControl.Properties.DataSource = _dsSourceDataset.Tables[0];
                // The field providing the editor's display text.
                SourceControl.Properties.DisplayMember = _DisplayColum;
                // The field matching the edit value.
                SourceControl.Properties.ValueMember = _ValueColumn;
                // clear the text in the text box of the look up control
                if (_dsSourceDataset.Tables[0].Rows.Count > 0)
                {
                    SourceControl.EditValue = null;
                    SourceControl.Properties.NullText = "-Please Select-";
                    //SourceControl.EditValue = _dsSourceDataset.Tables[0].Rows[0][_ValueColumn]; 

                }
                //*********************************
                SourceControl.Text = "";
                //clear existing columns
                SourceControl.Properties.Columns.Clear();
                // Add two columns to the dropdown.
                int _index = 0, _intAutoSearchColum = 0;


                foreach (DataColumn item in _dsSourceDataset.Tables[0].Columns)
                {
                    if (item.ColumnName.ToUpper().IndexOf("ID") <= 0 && item.ColumnName.ToUpper() != ("LastModifyDate").ToUpper() && item.ColumnName.ToUpper() != ("LastModifyUser").ToUpper())
                    {
                        if (FilterColumns != null)
                        {
                            if (FilterColumns.Length <= 0)
                            { SourceControl.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.ColumnName, 0)); }
                            else
                            {
                                foreach (string item2 in FilterColumns)
                                {
                                    if (item2 == item.ColumnName.ToString())
                                    {
                                        SourceControl.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.ColumnName, 0));
                                        break;
                                    }
                                }

                            }
                        }

                    }

                    if (item.ColumnName.ToUpper() == _DisplayColum)
                    { _intAutoSearchColum = _index; }
                    _index++;
                }
                //  Set column widths according to their contents and resize the popup, if required.
                SourceControl.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;

                // Enable auto completion search mode.
                SourceControl.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                // Specify the column against which to perform the search.
                SourceControl.Properties.AutoSearchColumnIndex = _intAutoSearchColum;
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
        }

        public static void LoadCheckedComboBoxEditControlsNew(DevExpress.XtraEditors.CheckedComboBoxEdit SourceControl, DataSet _dsSourceDataset, string Displaymember, string ValueMember)
        {
            try
            {

                //Base _ObjbaseClass = (Base)_dataObject;
                //DataSet _dsSourceDataset = null;

                //if (IsByMonthSegment == false)
                //{
                //    if (_dataObject.GetType() != typeof(Employee))
                //    {
                //        _dsSourceDataset = _ObjbaseClass.GetList();
                //    }
                //    else
                //    {
                //        _dsSourceDataset = ((Employee)_ObjbaseClass).GetLimitedList();
                //    }
                //}
                //else
                //{
                //    _dsSourceDataset = _ObjbaseClass.GetListByMonth();
                //    //Added To Get Method For Pay Cycle And Pay Cycle Block To Filter By Segment And Selected Month
                //}

                string _DisplayColum = Displaymember;
                string _ValueColumn = ValueMember;

                SourceControl.Properties.Items.Clear();
                // Specify the data source to display in the dropdown.
                SourceControl.Properties.DataSource = _dsSourceDataset.Tables[0];
                //SourceControl.DataBindings.ite = _dsSourceDataset.Tables[0];
                // The field providing the editor's display text.
                SourceControl.Properties.DisplayMember = _DisplayColum;
                // The field matching the edit value.
                SourceControl.Properties.ValueMember = _ValueColumn;
                SourceControl.CheckAll();
            }
            catch (Exception ex)
            {

                _globalUser.ErrorObject = ex;
            }
        }

         /// <summary>
        /// Convert DataTable to List using a Generic Method - Add by CSV on 2020/11/23
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }


        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();
                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    list.Add(obj);
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

        //public static Employee[] LaunchFilterAlertAssign(clsGobalUser _GlobalUser, int type)
        //{
        //    try
        //    {
        //        _globalUser = _GlobalUser;
        //        Enquires.FrmEmployeeFilter frmGrievanceFilter = new Enquires.FrmEmployeeFilter(_globalUser, type, false);
        //        frmGrievanceFilter.ShowDialog();
        //        _SelectedEmployees = frmGrievanceFilter.getSelectedEmployees();
        //    }
        //    catch (Exception ex)
        //    {

        //        _globalUser.ErrorObject = ex;
        //    }
        //    if (_SelectedEmployees.Count > 0)
        //    { return _SelectedEmployees.ToArray(); }
        //    else
        //    {
        //        return new Employee[0];
        //    }
        //}

        //public static Employee[] LaunchFilterAlertAssign(clsGobalUser _GlobalUser, int? PaySchemeId = null, int? AttendanceSummeryId = null)
        //{
        //    try
        //    {
        //        _globalUser = _GlobalUser;
        //        Enquires.FrmEmployeeFilter frmEmployees;
        //        if (PaySchemeId == null)
        //        {
        //            frmEmployees = new Enquires.FrmEmployeeFilter(_globalUser);
        //        }
        //        else
        //        {
        //            if (AttendanceSummeryId == null)
        //            { frmEmployees = new Enquires.FrmEmployeeFilter(_globalUser, PaySchemeId); }
        //            else
        //            { frmEmployees = new Enquires.FrmEmployeeFilter(_globalUser, PaySchemeId, AttendanceSummeryId); }

        //        }
        //        frmEmployees.ShowDialog();
        //        _SelectedEmployees = frmEmployees.getSelectedEmployees();
        //    }
        //    catch (Exception ex)
        //    {

        //        _globalUser.ErrorObject = ex;
        //    }
        //    if (_SelectedEmployees.Count > 0)
        //    { return _SelectedEmployees.ToArray(); }
        //    else
        //    {
        //        return new Employee[0];
        //    }
        //}
    }

    public static class LoanCalculationData
    {
        public static int _EmployeeNo { get; set; }
        public static int _LoanTypeID { get; set; }
        public static float _LoanAmount { get; set; }
        public static int _LoanDuration { get; set; }
        public static float _LoanInterest { get; set; }

    }






}
