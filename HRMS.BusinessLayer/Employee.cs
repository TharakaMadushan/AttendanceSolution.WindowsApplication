using HRMS.DataLayer;
using HRMS.DefaultConstants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HRMS.BusinessLayer
{
    public class Employee : Base
    {
        #region Fields

        private int _SegmentID = Constants.NullInt;
        private int _DesignationID = Constants.NullInt;
        private int _EmployeeNo = Constants.NullInt;
        private int _LevelID = Constants.NullInt;
        private int _EmpSegmentID = Constants.NullInt;
        private string _DocumentEmployeeNo = Constants.NullString;
        private string _NIC = Constants.NullString;
        private string _BadgeNo = Constants.NullString;
        private string _FirstName = Constants.NullString;
        private string _MiddleName = Constants.NullString;
        private string _LastName = Constants.NullString;
        private string _OtherNames = Constants.NullString;
        private string _Initials = Constants.NullString;
        private int _GenderID = Constants.NullInt;
        private int _OrientationID = Constants.NullInt;
        private int _RaceID = Constants.NullInt;
        private DateTime _DOB = Constants.NullDateTime;
        private int _CountryID = Constants.NullInt;
        private DateTime _DateJoined = Constants.NullDateTime;
        private DateTime _DateLeftOrganization = Constants.NullDateTime;
        private string _JobTitle = Constants.NullString;
        private int _ElectrorateID = Constants.NullInt;
        private bool? _IsCitizen = Constants.NullBool;
        private string _BloodType = Constants.NullString;
        private int _MedicalCondtionsID = Constants.NullInt;
        private string _PaySlipName = Constants.NullString;
        private string _TaxationName = Constants.NullString;
        private string _CivilStatusName = Constants.NullString; //Akalanka1337-06/03/22
        private string _PassportNo = Constants.NullString;
        private string _PaidCurrencyCode = Constants.NullString;
        private int _PaySchemeID = Constants.NullInt;
        private string _TaxFileNo = Constants.NullString;
        private string _EPFNo = Constants.NullString;
        private string _KnowMedicalCondtions = Constants.NullString;
        private double _BasicSalary = Constants.NullDouble;
        private int _PreferedTransportMethodID = Constants.NullInt;
        private int _ShiftID = Constants.NullInt;
        private int _CategoryID = Constants.NullInt;
        private int _SectionID = Constants.NullInt;
        private int _TaxTypeID = Constants.NullInt;
        private string _ETFNo = Constants.NullString;
        private int _BankID = Constants.NullInt;
        private int _PayProcessed = Constants.NullInt;
        private int _PayCycleID = Constants.NullInt;
        private int _AttendanceSummeryID = Constants.NullInt;
        private int _WorkingHourScheduleID = Constants.NullInt;
        private int _LeaveSchemeID = Constants.NullInt;
        private int _PerformaneBonus = Constants.NullInt;
        private int _EmployeePaymentBreakdownID = Constants.NullInt;
        private int _ClaimSchemeID = Constants.NullInt;
        private bool? _EmployeeRetired = Constants.NullBool;
        private bool? _EmployeeConfirmed = Constants.NullBool;
        private string _EISPassword = Constants.NullString;
        private int _LastModifyUser = Constants.NullInt;
        private DateTime _LastModifyDate = Constants.NullDateTime;
        private double _ServiceePercentage = Constants.NullDouble;
        private DateTime _PassportExpireDate = Constants.NullDateTime;
        private int _LanguageID = Constants.NullInt;
        private DateTime _EmployeeConfirmedDate = Constants.NullDateTime;
        private int _LocationID = Constants.NullInt;
        private string _JobSpecification = Constants.NullString;
        private int _Title = Constants.NullInt;
        private int _UserID = Constants.NullInt;
        private double _Increment = Constants.NullDouble;
        private bool? _IsActive = Constants.NullBool;
        private int _WorkingCalenderSchemeID = Constants.NullInt;
        private bool? _IsIncrementApplyingAfter = Constants.NullBool;
        private DateTime _IncrementDateFrom = Constants.NullDateTime;
        private int _AttendanceSchemeID = Constants.NullInt;
        private int _ExtraInt1 = Constants.NullInt;
        private int _ExtraInt2 = Constants.NullInt;
        private int _ExtraInt3 = Constants.NullInt;
        private int _ExtraInt4 = Constants.NullInt;
        private int _ExtraInt5 = Constants.NullInt;
        private int _ExtraInt6 = Constants.NullInt;        
        private bool? _ExtraBool1 = Constants.NullBool;
        private bool? _ExtraBool2 = Constants.NullBool;
        private bool? _ExtraBool3 = Constants.NullBool;
        private bool? _ExtraBool4 = Constants.NullBool;
        private bool? _ExtraBool5 = Constants.NullBool;
        private bool? _ExtraBool6 = Constants.NullBool;
        private string _ExtraText1 = Constants.NullString;
        private string _ExtraText2 = Constants.NullString;
        private string _ExtraText3 = Constants.NullString;
        private string _ExtraText4 = Constants.NullString;
        private string _ExtraText5 = Constants.NullString;
        private string _ExtraText6 = Constants.NullString;
        private bool? _IsContractEmployee = Constants.NullBool;//Tharaka 2018/09/06
        private DateTime _InactiveDate = Constants.NullDateTime;
        private string _LeftReason = Constants.NullString; //TharakaM 2020.04.16
        private bool? _EPFEntitlement = Constants.NullBool; //TharakaM 2020.04.17
        private int _CivilStatusId = Constants.NullInt; //TharakaM 2020.04.21
        private int _ReligionID = Constants.NullInt;
        private DateTime _DateRetired = Constants.NullDateTime; //TharakaM 2020.05.22
        private bool? _IsRetired = Constants.NullBool; //TharakaM 2020.05.25
        private string _RetiredYear = Constants.NullString; //TharakaM 2020.05.26
        private int _ProjectDesignationID = Constants.NullInt;
        private int _ProjectSkillID = Constants.NullInt;
        private int _ProjectJobTypeID = Constants.NullInt;
        private int _GradeID = Constants.NullInt;
        private string _NID = Constants.NullString;
        private DateTime _ExtraDate1 = Constants.NullDateTime;
        private DateTime _ExtraDate2 = Constants.NullDateTime;
        private DateTime _ExtraDate3 = Constants.NullDateTime;
        private DateTime _ExtraDate4 = Constants.NullDateTime;
        private DateTime _ExtraDate5 = Constants.NullDateTime;
        private DateTime _ExtraDate6 = Constants.NullDateTime;
        private double _ExtraFloat1 = Constants.NullDouble;
        private double _ExtraFloat2 = Constants.NullDouble;
        private double _ExtraFloat3 = Constants.NullDouble;
        private double _ExtraFloat4 = Constants.NullDouble;
        private double _ExtraFloat5 = Constants.NullDouble;
        private double _ExtraFloat6 = Constants.NullDouble;
        private DateTime _ContractStartDate = Constants.NullDateTime;
        private DateTime _ContractEndDate = Constants.NullDateTime;

        private EmployeeServiceInfo _EmployeeServiceinfo = new EmployeeServiceInfo();
        private DALMasterEmployee objDataWorker = new DALMasterEmployee();
        private DALMasterEmployee.MasterEmployee objDataObject = new DALMasterEmployee.MasterEmployee();
        private int _RetInt = Constants.NullInt;
        private string _RetText = Constants.NullString;

        #endregion Fields

        #region Properties

        public Employee(int UserID)
        {
            _mainDisplaycolumns = new string[] { "EmployeeFullName" };
            _mainIDColumns = new string[] { "EmployeeNo" };
            _UserID = UserID;
            //----------------- establish filter
            Users _objUsers = new Users();
            _objUsers.UserID = UserID;
            _objUsers.GetByID();
            if (_objUsers.UserID != -1)
            {
                _SegmentID = _objUsers.SegmentID;

                OrganizationScheme _objOrgScheme = new OrganizationScheme();
                _objOrgScheme.SegmentID = _SegmentID;
                _objOrgScheme.GetByID();
                if (_objOrgScheme.ParentSegmentID == -1)
                {
                    _SegmentID = -1;// this was done to show all the user for the Main Company segemnt with SegementID = -1
                }
            }
            //-----------------------------------
        }

        /// <summary>
        /// gets data for a set employee
        /// </summary>
        /// <param name="EmployeeNo">System employee no</param>
        public Employee(int SegmentID, int EmployeeNo)
        {
            _mainDisplaycolumns = new string[] { "EmployeeFullName" };
            _mainIDColumns = new string[] { "EmployeeNo" };
            _EmployeeNo = EmployeeNo;
            _SegmentID = SegmentID;
            GetByID();
        }

        public Employee()
        {
            _mainDisplaycolumns = new string[] { "EmployeeFullName" };
            _mainIDColumns = new string[] { "EmployeeNo" };
            //_UserID = UserID;
        }

        public int RetInt
        {
            get { return _RetInt; }
            set { _RetInt = value; }
        }

        public string RetText
        {
            get { return _RetText; }
            set { _RetText = value; }
        }

        public int SegmentID
        {
            get { return _SegmentID; }
            set
            {
                _SegmentID = value;

                OrganizationScheme _objOrgScheme = new OrganizationScheme();
                _objOrgScheme.SegmentID = _SegmentID;
                _objOrgScheme.GetByID();
                if (_objOrgScheme.ParentSegmentID == -1)
                {
                    _SegmentID = -1;
                }
            }
        }

        public int DesignationID
        {
            get { return _DesignationID; }
            set { _DesignationID = value; }
        }

        public int EmployeeNo
        {
            get { return _EmployeeNo; }
            set { _EmployeeNo = value; }
        }

        public int LevelID
        {
            get { return _LevelID; }
            set { _LevelID = value; }
        }

        public int EmpSegmentID
        {
            get { return _EmpSegmentID; }
            set { _EmpSegmentID = value; }
        }

        public string DocumentEmployeeNo
        {
            get { return _DocumentEmployeeNo; }
            set { _DocumentEmployeeNo = value; }
        }

        public string NIC
        {
            get { return _NIC; }
            set { _NIC = value; }
        }

        public string NID
        {
            get { return _NID; }
            set { _NID = value; }
        }

        public string BadgeNo
        {
            get { return _BadgeNo; }
            set { _BadgeNo = value; }
        }

        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        public string MiddleName
        {
            get { return _MiddleName; }
            set { _MiddleName = value; }
        }

        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }

        public string OtherNames
        {
            get { return _OtherNames; }
            set { _OtherNames = value; }
        }

        public string Initials
        {
            get { return _Initials; }
            set { _Initials = value; }
        }

        public int GenderID
        {
            get { return _GenderID; }
            set { _GenderID = value; }
        }

        public int OrientationID
        {
            get { return _OrientationID; }
            set { _OrientationID = value; }
        }

        public int RaceID
        {
            get { return _RaceID; }
            set { _RaceID = value; }
        }

        public DateTime DOB
        {
            get { return _DOB; }
            set { _DOB = value; }
        }

        public int CountryID
        {
            get { return _CountryID; }
            set { _CountryID = value; }
        }

        public DateTime DateJoined
        {
            get { return _DateJoined; }
            set { _DateJoined = value; }
        }

        public DateTime DateLeftOrganization
        {
            get { return _DateLeftOrganization; }
            set { _DateLeftOrganization = value; }
        }

        public string LeftReason //TharakaM 2020.04.16
        {
            get { return _LeftReason; }
            set { _LeftReason = value; }
        }

        public string JobTitle
        {
            get { return _JobTitle; }
            set { _JobTitle = value; }
        }

        public int ElectrorateID
        {
            get { return _ElectrorateID; }
            set { _ElectrorateID = value; }
        }

        public bool? IsCitizen
        {
            get { return _IsCitizen; }
            set { _IsCitizen = value; }
        }

        public string BloodType
        {
            get { return _BloodType; }
            set { _BloodType = value; }
        }

        public int MedicalCondtionsID
        {
            get { return _MedicalCondtionsID; }
            set { _MedicalCondtionsID = value; }
        }

        public string PaySlipName
        {
            get { return _PaySlipName; }
            set { _PaySlipName = value; }
        }

        public string TaxationName
        {
            get { return _TaxationName; }
            set { _TaxationName = value; }
        }

        public string CivilStatusName //Akalanka1337-06/03/22
        {
            get { return _CivilStatusName; }
            set { _CivilStatusName = value; }
        }

        public string PassportNo
        {
            get { return _PassportNo; }
            set { _PassportNo = value; }
        }

        public string PaidCurrencyCode
        {
            get { return _PaidCurrencyCode; }
            set { _PaidCurrencyCode = value; }
        }

        public int PaySchemeID
        {
            get { return _PaySchemeID; }
            set { _PaySchemeID = value; }
        }

        public string TaxFileNo
        {
            get { return _TaxFileNo; }
            set { _TaxFileNo = value; }
        }

        public string EPFNo
        {
            get { return _EPFNo; }
            set { _EPFNo = value; }
        }

        public string KnowMedicalCondtions
        {
            get { return _KnowMedicalCondtions; }
            set { _KnowMedicalCondtions = value; }
        }

        public double BasicSalary
        {
            get { return _BasicSalary; }
            set { _BasicSalary = value; }
        }

        public int PreferedTransportMethodID
        {
            get { return _PreferedTransportMethodID; }
            set { _PreferedTransportMethodID = value; }
        }

        public int ShiftID
        {
            get { return _ShiftID; }
            set { _ShiftID = value; }
        }

        public int CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }

        public int SectionID
        {
            get { return _SectionID; }
            set { _SectionID = value; }
        }

        public int TaxTypeID
        {
            get { return _TaxTypeID; }
            set { _TaxTypeID = value; }
        }

        public string ETFNo
        {
            get { return _ETFNo; }
            set { _ETFNo = value; }
        }

        public int BankID
        {
            get { return _BankID; }
            set { _BankID = value; }
        }

        public int PayProcessed
        {
            get { return _PayProcessed; }
            set { _PayProcessed = value; }
        }

        public int PayCycleID
        {
            get { return _PayCycleID; }
            set { _PayCycleID = value; }
        }

        public int AttendanceSummeryID
        {
            get { return _AttendanceSummeryID; }
            set { _AttendanceSummeryID = value; }
        }

        public int WorkingHourScheduleID
        {
            get { return _WorkingHourScheduleID; }
            set { _WorkingHourScheduleID = value; }
        }

        public int LeaveSchemeID
        {
            get { return _LeaveSchemeID; }
            set { _LeaveSchemeID = value; }
        }

        public int PerformaneBonus
        {
            get { return _PerformaneBonus; }
            set { _PerformaneBonus = value; }
        }

        public int EmployeePaymentBreakdownID
        {
            get { return _EmployeePaymentBreakdownID; }
            set { _EmployeePaymentBreakdownID = value; }
        }

        public int ClaimSchemeID
        {
            get { return _ClaimSchemeID; }
            set { _ClaimSchemeID = value; }
        }

        public bool? EmployeeRetired
        {
            get { return _EmployeeRetired; }
            set { _EmployeeRetired = value; }
        }

        public bool? EmployeeConfirmed
        {
            get { return _EmployeeConfirmed; }
            set { _EmployeeConfirmed = value; }
        }

        public string EISPassword
        {
            get { return _EISPassword; }
            set { _EISPassword = value; }
        }

        public int LastModifyUser
        {
            get { return _LastModifyUser; }
            set { _LastModifyUser = value; }
        }

        public DateTime LastModifyDate
        {
            get { return _LastModifyDate; }
            set { _LastModifyDate = value; }
        }

        public double ServiceePercentage
        {
            get { return _ServiceePercentage; }
            set { _ServiceePercentage = value; }
        }

        public DateTime PassportExpireDate
        {
            get { return _PassportExpireDate; }
            set { _PassportExpireDate = value; }
        }

        public int LanguageID
        {
            get { return _LanguageID; }
            set { _LanguageID = value; }
        }

        public DateTime EmployeeConfirmedDate
        {
            get { return _EmployeeConfirmedDate; }
            set { _EmployeeConfirmedDate = value; }
        }

        public int LocationID
        {
            get { return _LocationID; }
            set { _LocationID = value; }
        }

        public string JobSpecification
        {
            get { return _JobSpecification; }
            set { _JobSpecification = value; }
        }

        public int Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        public double Increment
        {
            get { return _Increment; }
            set { _Increment = value; }
        }

        public bool? IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }

        public int WorkingCalenderSchemeID
        {
            get { return _WorkingCalenderSchemeID; }
            set { _WorkingCalenderSchemeID = value; }
        }

        public DateTime IncrementDateFrom
        {
            get { return _IncrementDateFrom; }
            set { _IncrementDateFrom = value; }
        }

        public bool? IsIncrementApplyingAfter
        {
            get { return _IsIncrementApplyingAfter; }
            set { _IsIncrementApplyingAfter = value; }
        }

        public int AttendanceSchemeID
        {
            get { return _AttendanceSchemeID; }
            set { _AttendanceSchemeID = value; }
        }

        public int ExtraInt1
        {
            get { return _ExtraInt1; }
            set { _ExtraInt1 = value; }
        }

        public int ExtraInt2
        {
            get { return _ExtraInt2; }
            set { _ExtraInt2 = value; }
        }

        public int ExtraInt3
        {
            get { return _ExtraInt3; }
            set { _ExtraInt3 = value; }
        }

        public int ExtraInt4
        {
            get { return _ExtraInt4; }
            set { _ExtraInt4 = value; }
        }

        public int ExtraInt5
        {
            get { return _ExtraInt5; }
            set { _ExtraInt5 = value; }
        }

        public int ExtraInt6
        {
            get { return _ExtraInt6; }
            set { _ExtraInt6 = value; }
        }


        public bool? ExtraBool1
        {
            get { return _ExtraBool1; }
            set { _ExtraBool1 = value; }
        }

        public bool? ExtraBool2
        {
            get { return _ExtraBool2; }
            set { _ExtraBool2 = value; }
        }

        public bool? ExtraBool3
        {
            get { return _ExtraBool3; }
            set { _ExtraBool3 = value; }
        }

        public bool? ExtraBool4
        {
            get { return _ExtraBool4; }
            set { _ExtraBool4 = value; }
        }

        public bool? ExtraBool5
        {
            get { return _ExtraBool5; }
            set { _ExtraBool5 = value; }
        }

        public bool? ExtraBool6
        {
            get { return _ExtraBool6; }
            set { _ExtraBool6 = value; }
        }

        public string ExtraText1
        {
            get { return _ExtraText1; }
            set { _ExtraText1 = value; }
        }

        public string ExtraText2
        {
            get { return _ExtraText2; }
            set { _ExtraText2 = value; }
        }

        public string ExtraText3
        {
            get { return _ExtraText3; }
            set { _ExtraText3 = value; }
        }

        public string ExtraText4
        {
            get { return _ExtraText4; }
            set { _ExtraText4 = value; }
        }

        public string ExtraText5
        {
            get { return _ExtraText5; }
            set { _ExtraText5 = value; }
        }

        public string ExtraText6
        {
            get { return _ExtraText6; }
            set { _ExtraText6 = value; }
        }

        public double ExtraFloat1
        {
            get { return _ExtraFloat1; }
            set { _ExtraFloat1 = value; }
        }

        public double ExtraFloat2
        {
            get { return _ExtraFloat2; }
            set { _ExtraFloat2 = value; }
        }

        public double ExtraFloat3
        {
            get { return _ExtraFloat3; }
            set { _ExtraFloat3 = value; }
        }

        public double ExtraFloat4
        {
            get { return _ExtraFloat4; }
            set { _ExtraFloat4 = value; }
        }

        public double ExtraFloat5
        {
            get { return _ExtraFloat5; }
            set { _ExtraFloat5 = value; }
        }

        public double ExtraFloat6
        {
            get { return _ExtraFloat6; }
            set { _ExtraFloat6 = value; }
        }

        //Tharaka2018/09/06
        public bool? IsContractEmployee
        {
            get { return _IsContractEmployee; }
            set { _IsContractEmployee = value; }
        }

        public DateTime InactiveDate
        {
            get { return _InactiveDate; }
            set { _InactiveDate = value; }
        }

        public bool? EPFEntitlement //TharakaM 2020.04.17
        {
            get { return _EPFEntitlement; }
            set { _EPFEntitlement = value; }
        }

        public int CivilStatusId //TharakaM 2020.04.21
        {
            get { return _CivilStatusId; }
            set { _CivilStatusId = value; }
        }

        public int ReligionID //TharakaM 2020.04.21
        {
            get { return _ReligionID; }
            set { _ReligionID = value; }
        }

        public bool? IsRetired
        {
            get { return _IsRetired; }
            set { _IsRetired = value; }
        }

        public DateTime DateRetired
        {
            get { return _DateRetired; }
            set { _DateRetired = value; }
        }

        public string RetiredYear
        {
            get { return _RetiredYear; }
            set { _RetiredYear = value; }
        }

        public int ProjectDesignationID
        {
            get { return _ProjectDesignationID; }
            set { _ProjectDesignationID = value; }
        }

        public int ProjectSkillID
        {
            get { return _ProjectSkillID; }
            set { _ProjectSkillID = value; }
        }

        public int ProjectJobTypeID
        {
            get { return _ProjectJobTypeID; }
            set { _ProjectJobTypeID = value; }
        }

        public int GradeID
        {
            get { return _GradeID; }
            set { _GradeID = value; }
        }

        public DateTime ExtraDate1
        {
            get { return _ExtraDate1; }
            set { _ExtraDate1 = value; }
        }

        public DateTime ExtraDate2
        {
            get { return _ExtraDate2; }
            set { _ExtraDate2 = value; }
        }

        public DateTime ExtraDate3
        {
            get { return _ExtraDate3; }
            set { _ExtraDate3 = value; }
        }

        public DateTime ExtraDate4
        {
            get { return _ExtraDate4; }
            set { _ExtraDate4 = value; }
        }

        public DateTime ExtraDate5
        {
            get { return _ExtraDate5; }
            set { _ExtraDate5 = value; }
        }

        public DateTime ExtraDate6
        {
            get { return _ExtraDate6; }
            set { _ExtraDate6 = value; }
        }

        public DateTime ContractStartDate
        {
            get { return _ContractStartDate; }
            set { _ContractStartDate = value; }
        }

        public DateTime ContractEndDate
        {
            get { return _ContractEndDate; }
            set { _ContractEndDate = value; }
        }


        public EmployeeServiceInfo EmployeeServiceInformation
        {
            get { return _EmployeeServiceinfo; }
        }

        #endregion Properties

        #region Methods

        public class EmployeeServiceInfo
        {
            private bool _IsConfirmed = false;

            public bool IsConfirmed
            {
                get { return _IsConfirmed; }
                set { _IsConfirmed = value; }
            }

            private bool _IsRetired = false;

            public bool IsRetired
            {
                get { return _IsRetired; }
                set { _IsRetired = value; }
            }

            private DateTime? _JoinDate = null;

            public DateTime? JoinDate
            {
                get { return _JoinDate; }
                set { _JoinDate = value; }
            }

            private DateTime? _ConfirmDate = null;

            public DateTime? ConfirmDate
            {
                get { return _ConfirmDate; }
                set { _ConfirmDate = value; }
            }

            private DateTime? _ResignDate = null;

            public DateTime? ResignDate
            {
                get { return _ResignDate; }
                set { _ResignDate = value; }
            }

            private string _ReasonOfResign = null; //TharakaM 2020.04.15

            public string ReasonOfResign //TharakaM 2020.04.15
            {
                get { return _ReasonOfResign; }
                set { _ReasonOfResign = value; }
            }

            public double LengthOFSericeSinceJoinDateInYears
            {
                get
                {
                    return (DateTime.Now - (DateTime)_JoinDate).TotalDays / 365;
                }
            }

            public double LengthOFSericeSinceJoinDateInMonths
            {
                get
                {
                    return (DateTime.Now - (DateTime)_JoinDate).TotalDays / 30;
                }
            }

            public double LengthOFSericeSinceJoinDateInDays
            {
                get
                {
                    return (DateTime.Now - (DateTime)_JoinDate).TotalDays;
                }
            }

            public double LengthOFSericeSinceConfirmDateInYears
            {
                get
                {
                    double _dblReturn = -1;
                    if (_IsConfirmed)
                    {
                        return (DateTime.Now - (DateTime)_ConfirmDate).Days / 365;
                    }
                    return _dblReturn;
                }
            }

            public double LengthOFSericeSinceConfirmDateInMonths
            {
                get
                {
                    double _dblReturn = -1;
                    if (_IsConfirmed)
                    {
                        return (DateTime.Now - (DateTime)_ConfirmDate).Days / 30;
                    }
                    return _dblReturn;
                }
            }

            public double LengthOFSericeSinceConfirmDateInDays
            {
                get
                {
                    double _dblReturn = -1;
                    if (_IsConfirmed)
                    {
                        return (DateTime.Now - (DateTime)_ConfirmDate).Days;
                    }
                    return _dblReturn;
                }
            }
        }

        public override string GetName()
        {
            return "Employees_Details";
        }

        public override bool MapData(DataRow row)
        {
            _SegmentID = GetInt(row, "SegmentID");
            _DesignationID = GetInt(row, "DesignationID");
            _EmployeeNo = GetInt(row, "EmployeeNo");
            _LevelID = GetInt(row, "LevelID");
            _EmpSegmentID = GetInt(row, "EmpSegmentID");
            _DocumentEmployeeNo = GetString(row, "DocumentEmployeeNo");
            _NIC = GetString(row, "NIC");
            _BadgeNo = GetString(row, "BadgeNo");
            _FirstName = GetString(row, "FirstName");
            _MiddleName = GetString(row, "MiddleName");
            _LastName = GetString(row, "LastName");
            _OtherNames = GetString(row, "OtherNames");
            _Initials = GetString(row, "Initials");
            _GenderID = GetInt(row, "GenderID");
            _OrientationID = GetInt(row, "OrientationID");
            _RaceID = GetInt(row, "RaceID");
            _DOB = GetDateTime(row, "DOB");
            _CountryID = GetInt(row, "CountryID");
            _DateJoined = GetDateTime(row, "DateJoined");
            _DateLeftOrganization = GetDateTime(row, "DateLeftOrganization");
            _JobTitle = GetString(row, "JobTitle");
            _ElectrorateID = GetInt(row, "ElectrorateID");
            _IsCitizen = GetBool(row, "IsCitizen");
            _BloodType = GetString(row, "BloodType");
            _MedicalCondtionsID = GetInt(row, "MedicalCondtionsID");
            _PaySlipName = GetString(row, "PaySlipName");
            _TaxationName = GetString(row, "TaxationName");
            _PassportNo = GetString(row, "PassportNo");
            _PaidCurrencyCode = GetString(row, "PaidCurrencyCode");
            _PaySchemeID = GetInt(row, "PaySchemeID");
            _TaxFileNo = GetString(row, "TaxFileNo");
            _EPFNo = GetString(row, "EPFNo");
            _KnowMedicalCondtions = GetString(row, "KnowMedicalCondtions");
            _BasicSalary = GetDouble(row, "BasicSalary");
            _PreferedTransportMethodID = GetInt(row, "PreferedTransportMethodID");
            _ShiftID = GetInt(row, "ShiftID");
            _CategoryID = GetInt(row, "CategoryID");
            _SectionID = GetInt(row, "SectionID");
            _TaxTypeID = GetInt(row, "TaxTypeID");
            _ETFNo = GetString(row, "ETFNo");
            _BankID = GetInt(row, "BankID");
            _PayProcessed = GetInt(row, "PayProcessed");
            _PayCycleID = GetInt(row, "PayCycleID");
            _AttendanceSummeryID = GetInt(row, "AttendanceSummeryID");
            _WorkingHourScheduleID = GetInt(row, "WorkingHourScheduleID");
            _LeaveSchemeID = GetInt(row, "LeaveSchemeID");
            _PerformaneBonus = GetInt(row, "PerformaneBonus");
            _EmployeePaymentBreakdownID = GetInt(row, "EmployeePaymentBreakdownID");
            _ClaimSchemeID = GetInt(row, "ClaimSchemeID");
            _EmployeeRetired = GetBool(row, "EmployeeRetired");
            _EmployeeConfirmed = GetBool(row, "EmployeeConfirmed");
            _EISPassword = GetString(row, "EISPassword");
            _LastModifyUser = GetInt(row, "LastModifyUser");
            _LastModifyDate = GetDateTime(row, "LastModifyDate");
            _ServiceePercentage = GetDouble(row, "ServiceePercentage");
            _PassportExpireDate = GetDateTime(row, "PassportExpireDate");
            _LanguageID = GetInt(row, "LanguageID");
            _EmployeeConfirmedDate = GetDateTime(row, "EmployeeConfirmedDate");
            _LocationID = GetInt(row, "LocationID");
            _JobSpecification = GetString(row, "JobSpecification");
            _Title = GetInt(row, "Title");
            _Increment = GetDouble(row, "Increment");
            _IsActive = GetBool(row, "IsActive");
            _WorkingCalenderSchemeID = GetInt(row, "WorkingCalenderSchemeID");
            _IsIncrementApplyingAfter = GetBool(row, "IsIncrementApplyingAfter");
            _IncrementDateFrom = GetDateTime(row, "IncrementDateFrom");
            _AttendanceSchemeID = GetInt(row, "AttendanceSchemeID");
            _ExtraInt1 = GetInt(row, "ExtraInt1");
            _ExtraInt2 = GetInt(row, "ExtraInt2");
            _ExtraInt3 = GetInt(row, "ExtraInt3");
            _ExtraBool1 = GetBool(row, "ExtraBool1");
            _ExtraBool2 = GetBool(row, "ExtraBool2");
            _ExtraBool3 = GetBool(row, "ExtraBool3");
            _ExtraText1 = GetString(row, "ExtraText1");
            _ExtraText2 = GetString(row, "ExtraText2");
            _ExtraText3 = GetString(row, "ExtraText3");
            _IsContractEmployee = GetBool(row, "IsContractEmployee");//Tharaka 2018/06/09
            _InactiveDate = GetDateTime(row, "InactiveDate");
            _LeftReason = GetString(row, "LeftReason"); //TharakaM 2020.04.16
            _EPFEntitlement = GetBool(row, "EPFEntitlement"); //TharakaM 2020.04.17
            _CivilStatusId = GetInt(row, "CivilStatusId"); //TharakaM 2020.04.21
            _ReligionID = GetInt(row, "ReligionID"); //TharakaM 2020.04.21
            _DateRetired = GetDateTime(row, "DateRetired"); //TharakaM 2020.05.22
            _IsRetired = GetBool(row, "IsRetired"); //TharakaM 2020.05.22
            _RetiredYear = GetString(row, "RetiredYear"); //TharakaM 2020.05.26
            _ProjectDesignationID = GetInt(row, "ProjectDesignationID");
            _ProjectSkillID = GetInt(row, "ProjectSkillID");
            _ProjectJobTypeID = GetInt(row, "ProjectJobTypeID");
            _GradeID = GetInt(row, "GradeID");
            _NID = GetString(row, "NID");
            _ExtraInt4 = GetInt(row, "ExtraInt4");
            _ExtraInt5 = GetInt(row, "ExtraInt5");
            _ExtraInt6 = GetInt(row, "ExtraInt6");
            _ExtraBool4 = GetBool(row, "ExtraBool4");
            _ExtraBool5 = GetBool(row, "ExtraBool5");
            _ExtraBool6 = GetBool(row, "ExtraBool6");
            _ExtraText4 = GetString(row, "ExtraText4");
            _ExtraText5 = GetString(row, "ExtraText5");
            _ExtraText6 = GetString(row, "ExtraText6");           
            _ExtraDate1 = GetDateTime(row, "ExtraDate1");
            _ExtraDate2 = GetDateTime(row, "ExtraDate2");
            _ExtraDate3 = GetDateTime(row, "ExtraDate3");
            _ExtraDate4 = GetDateTime(row, "ExtraDate4");
            _ExtraDate5 = GetDateTime(row, "ExtraDate5");
            _ExtraDate6 = GetDateTime(row, "ExtraDate6");
            _ExtraFloat1 = GetFloat(row, "ExtraFloat1");
            _ExtraFloat2 = GetFloat(row, "ExtraFloat2");
            _ExtraFloat3 = GetFloat(row, "ExtraFloat3");
            _ExtraFloat4 = GetFloat(row, "ExtraFloat4");
            _ExtraFloat5 = GetFloat(row, "ExtraFloat5");
            _ExtraFloat6 = GetFloat(row, "ExtraFloat6");
            _ContractStartDate= GetDateTime(row, "ContractStartDate");
            _ContractEndDate = GetDateTime(row, "ContractEndDate");
            try
            {
                _EmployeeServiceinfo = new EmployeeServiceInfo();
                _EmployeeServiceinfo.IsConfirmed = _EmployeeConfirmed == null ? false : (bool)_EmployeeConfirmed;
                _EmployeeServiceinfo.IsRetired = _EmployeeRetired == null ? false : (bool)_EmployeeRetired;
                _EmployeeServiceinfo.JoinDate = _DateJoined == null ? null : (DateTime?)_DateJoined;
                _EmployeeServiceinfo.ConfirmDate = _EmployeeConfirmedDate == null ? null : (DateTime?)_EmployeeConfirmedDate;
                _EmployeeServiceinfo.ResignDate = _DateLeftOrganization == null ? null : (DateTime?)_DateLeftOrganization;
                _EmployeeServiceinfo.ReasonOfResign = _LeftReason == null ? null : _LeftReason; //TharakaM 2020.04.16
                                                                                                // _EmployeeServiceinfo.IsContractEmployee = _IsContractEmployee == null ? null : (bool)_IsContractEmployee;
            }
            catch (Exception)
            {
                //throw;
            }

            return base.MapData(row);
        }

        private void MapDataToDataObject()
        {
            try
            {
                if (objDataObject != null)
                {
                    objDataObject.UserID = _UserID;
                    objDataObject.SegmentID = _SegmentID;
                    objDataObject.DesignationID = _DesignationID;
                    objDataObject.EmployeeNo = _EmployeeNo;
                    objDataObject.LevelID = _LevelID;
                    objDataObject.EmpSegmentID = _EmpSegmentID;
                    objDataObject.DocumentEmployeeNo = _DocumentEmployeeNo;
                    objDataObject.NIC = _NIC;
                    objDataObject.BadgeNo = _BadgeNo;
                    objDataObject.FirstName = _FirstName;
                    objDataObject.MiddleName = _MiddleName;
                    objDataObject.LastName = _LastName;
                    objDataObject.OtherNames = _OtherNames;
                    objDataObject.Initials = _Initials;
                    objDataObject.GenderID = _GenderID;
                    objDataObject.OrientationID = _OrientationID;
                    objDataObject.RaceID = _RaceID;
                    objDataObject.DOB = _DOB;
                    objDataObject.CountryID = _CountryID;
                    objDataObject.DateJoined = _DateJoined;
                    objDataObject.DateLeftOrganization = _DateLeftOrganization;
                    objDataObject.JobTitle = _JobTitle;
                    objDataObject.ElectrorateID = _ElectrorateID;
                    objDataObject.IsCitizen = _IsCitizen;
                    objDataObject.BloodType = _BloodType;
                    objDataObject.MedicalCondtionsID = _MedicalCondtionsID;
                    objDataObject.PaySlipName = _PaySlipName;
                    objDataObject.TaxationName = _TaxationName;
                    objDataObject.PassportNo = _PassportNo;
                    objDataObject.PaidCurrencyCode = _PaidCurrencyCode;
                    objDataObject.PaySchemeID = _PaySchemeID;
                    objDataObject.TaxFileNo = _TaxFileNo;
                    objDataObject.EPFNo = _EPFNo;
                    objDataObject.KnowMedicalCondtions = _KnowMedicalCondtions;
                    objDataObject.BasicSalary = _BasicSalary;
                    objDataObject.PreferedTransportMethodID = _PreferedTransportMethodID;
                    objDataObject.ShiftID = _ShiftID;
                    objDataObject.CategoryID = _CategoryID;
                    objDataObject.SectionID = _SectionID;
                    objDataObject.TaxTypeID = _TaxTypeID;
                    objDataObject.ETFNo = _ETFNo;
                    objDataObject.BankID = _BankID;
                    objDataObject.PayProcessed = _PayProcessed;
                    objDataObject.PayCycleID = _PayCycleID;
                    objDataObject.AttendanceSummeryID = _AttendanceSummeryID;
                    objDataObject.WorkingHourScheduleID = _WorkingHourScheduleID;
                    objDataObject.LeaveSchemeID = _LeaveSchemeID;
                    objDataObject.PerformaneBonus = _PerformaneBonus;
                    objDataObject.EmployeePaymentBreakdownID = _EmployeePaymentBreakdownID;
                    objDataObject.ClaimSchemeID = _ClaimSchemeID;
                    objDataObject.EmployeeRetired = _EmployeeRetired;
                    objDataObject.EmployeeConfirmed = _EmployeeConfirmed;
                    objDataObject.EISPassword = _EISPassword;
                    objDataObject.LastModifyUser = _LastModifyUser;
                    objDataObject.LastModifyDate = _LastModifyDate;
                    objDataObject.ServiceePercentage = _ServiceePercentage;
                    objDataObject.PassportExpireDate = _PassportExpireDate;
                    objDataObject.LanguageID = _LanguageID;
                    objDataObject.EmployeeConfirmedDate = _EmployeeConfirmedDate;
                    objDataObject.LocationID = _LocationID;
                    objDataObject.JobSpecification = _JobSpecification;
                    objDataObject.Title = _Title;
                    objDataObject.Increment = _Increment;
                    objDataObject.IsActive = _IsActive;
                    objDataObject.WorkingCalenderSchemeID = _WorkingCalenderSchemeID;
                    objDataObject.IsIncrementApplyingAfter = _IsIncrementApplyingAfter;
                    objDataObject.IncrementDateFrom = _IncrementDateFrom;
                    objDataObject.AttendanceSchemeID = _AttendanceSchemeID;
                    objDataObject.ExtraInt1 = _ExtraInt1;
                    objDataObject.ExtraInt2 = _ExtraInt2;
                    objDataObject.ExtraInt3 = _ExtraInt3;
                    objDataObject.ExtraBool1 = _ExtraBool1;
                    objDataObject.ExtraBool2 = _ExtraBool2;
                    objDataObject.ExtraBool3 = _ExtraBool3;
                    objDataObject.ExtraText1 = _ExtraText1;
                    objDataObject.ExtraText2 = _ExtraText2;
                    objDataObject.ExtraText3 = _ExtraText3;
                    objDataObject.IsContractEmployee = _IsContractEmployee;
                    objDataObject.InactiveDate = _InactiveDate;
                    objDataObject.LeftReason = _LeftReason; //TharakaM 2020.04.16
                    objDataObject.EPFEntitlement = _EPFEntitlement; //TharakaM 2020.04.17
                    objDataObject.CivilStatusId = _CivilStatusId; //TharakaM 2020.04.21
                    objDataObject.ReligionID = _ReligionID; //TharakaM 2020.04.21
                    objDataObject.DateRetired = _DateRetired; //TharakaM 2020.05.22
                    objDataObject.IsRetired = _IsRetired; //TharakaM 2020.05.22
                    objDataObject.RetiredYear = _RetiredYear;
                    objDataObject.ProjectDesignationID = _ProjectDesignationID;
                    objDataObject.ProjectSkillID = _ProjectSkillID;
                    objDataObject.ProjectJobTypeID = _ProjectJobTypeID;
                    objDataObject.GradeID = _GradeID;
                    objDataObject.NID = _NID;
                    objDataObject.ExtraInt4 = _ExtraInt4;
                    objDataObject.ExtraInt5 = _ExtraInt5;
                    objDataObject.ExtraInt6 = _ExtraInt6;
                    objDataObject.ExtraBool4 = _ExtraBool4;
                    objDataObject.ExtraBool5 = _ExtraBool5;
                    objDataObject.ExtraBool6 = _ExtraBool6;
                    objDataObject.ExtraText4= _ExtraText4;
                    objDataObject.ExtraText5 = _ExtraText5;
                    objDataObject.ExtraText6 = _ExtraText6;
                    objDataObject.ExtraDate1 = _ExtraDate1;
                    objDataObject.ExtraDate2 = _ExtraDate2;
                    objDataObject.ExtraDate3 = _ExtraDate3;
                    objDataObject.ExtraDate4 = _ExtraDate4;
                    objDataObject.ExtraDate5 = _ExtraDate5;
                    objDataObject.ExtraDate6 = _ExtraDate6;
                    objDataObject.ExtraFloat1 = _ExtraFloat1;
                    objDataObject.ExtraFloat2 = _ExtraFloat2;
                    objDataObject.ExtraFloat3 = _ExtraFloat3;
                    objDataObject.ExtraFloat4 = _ExtraFloat4;
                    objDataObject.ExtraFloat5 = _ExtraFloat5;
                    objDataObject.ExtraFloat6 = _ExtraFloat6;
                    objDataObject.ContractStartDate = _ContractStartDate;
                    objDataObject.ContractEndDate = _ContractEndDate;
                }
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
            }
        }

        public override DataSet GetList()
        {
            DataSet ds = null;
            try
            {
                MapDataToDataObject();
                objDataWorker.objCurrent = objDataObject;
                ds = objDataWorker.getList().DataSet;
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
            }
            return ds;
        }

        public DataSet GetListApprisal()
        {
            DataSet ds = null;
            try
            {
                MapDataToDataObject();
                objDataWorker.objCurrent = objDataObject;
                ds = objDataWorker.getListApprisal().DataSet;
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
            }
            return ds;
        }

        public List<Employee> GetObjectList()
        {
            DataSet ds = null;
            List<Employee> _lstObjects = new List<Employee>();
            try
            {
                MapDataToDataObject();
                objDataWorker.objCurrent = objDataObject;
                ds = objDataWorker.getList().DataSet;

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            Employee _objItem = new Employee();
                            _objItem.MapData(item);
                            _lstObjects.Add(_objItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
            }
            return _lstObjects;
        }

        public DataSet GetLimitedList(bool Limited = false)
        {
            DataSet ds = null;
            try
            {
                MapDataToDataObject();
                objDataWorker.objCurrent = objDataObject;
                ds = objDataWorker.getLimitedList().DataSet;
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
            }
            return ds;
        }

        public void GetByID()
        {
            //MasterEmployee obj = new MasterEmployee();
            try
            {
                MapDataToDataObject();
                objDataWorker.objCurrent = objDataObject;
                DataSet ds = objDataWorker.getList().DataSet;
                this.MapData(ds);
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
            }
        }

        public List<Employee> getListByEmployeeList(int SegmentIDP, DataTable EmplTable)
        {//Added By Thus On 05Sep2019
            List<Employee> emplList = new List<Employee>();

            try
            {
                //MapDataToDataObject();
                //objDataWorker.objCurrent = objDataObject;
                DataTable ds = objDataWorker.getListByEmployeeList(SegmentIDP, EmplTable);
                //this.MapData(ds);

                if (ds != null && ds.Rows.Count > 0)
                {
                    emplList = (from DataRow row in ds.Rows
                                select new Employee()
                                {
                                    _SegmentID = GetInt(row, "SegmentID"),
                                    _DesignationID = GetInt(row, "DesignationID"),
                                    _EmployeeNo = GetInt(row, "EmployeeNo"),
                                    _LevelID = GetInt(row, "LevelID"),
                                    _EmpSegmentID = GetInt(row, "EmpSegmentID"),
                                    _DocumentEmployeeNo = GetString(row, "DocumentEmployeeNo"),
                                    _NIC = GetString(row, "NIC"),
                                    _BadgeNo = GetString(row, "BadgeNo"),
                                    _FirstName = GetString(row, "FirstName"),
                                    _MiddleName = GetString(row, "MiddleName"),
                                    _LastName = GetString(row, "LastName"),
                                    _OtherNames = GetString(row, "OtherNames"),
                                    _Initials = GetString(row, "Initials"),
                                    _GenderID = GetInt(row, "GenderID"),
                                    _OrientationID = GetInt(row, "OrientationID"),
                                    _RaceID = GetInt(row, "RaceID"),
                                    _DOB = GetDateTime(row, "DOB"),
                                    _CountryID = GetInt(row, "CountryID"),
                                    _DateJoined = GetDateTime(row, "DateJoined"),
                                    _DateLeftOrganization = GetDateTime(row, "DateLeftOrganization"),
                                    _JobTitle = GetString(row, "JobTitle"),
                                    _ElectrorateID = GetInt(row, "ElectrorateID"),
                                    _IsCitizen = GetBool(row, "IsCitizen"),
                                    _BloodType = GetString(row, "BloodType"),
                                    _MedicalCondtionsID = GetInt(row, "MedicalCondtionsID"),
                                    _PaySlipName = GetString(row, "PaySlipName"),
                                    _TaxationName = GetString(row, "TaxationName"),
                                    _PassportNo = GetString(row, "PassportNo"),
                                    _PaidCurrencyCode = GetString(row, "PaidCurrencyCode"),
                                    _PaySchemeID = GetInt(row, "PaySchemeID"),
                                    _TaxFileNo = GetString(row, "TaxFileNo"),
                                    _EPFNo = GetString(row, "EPFNo"),
                                    _KnowMedicalCondtions = GetString(row, "KnowMedicalCondtions"),
                                    _BasicSalary = GetDouble(row, "BasicSalary"),
                                    _PreferedTransportMethodID = GetInt(row, "PreferedTransportMethodID"),
                                    _ShiftID = GetInt(row, "ShiftID"),
                                    _CategoryID = GetInt(row, "CategoryID"),
                                    _SectionID = GetInt(row, "SectionID"),
                                    _TaxTypeID = GetInt(row, "TaxTypeID"),
                                    _ETFNo = GetString(row, "ETFNo"),
                                    _BankID = GetInt(row, "BankID"),
                                    _PayProcessed = GetInt(row, "PayProcessed"),
                                    _PayCycleID = GetInt(row, "PayCycleID"),
                                    _AttendanceSummeryID = GetInt(row, "AttendanceSummeryID"),
                                    _WorkingHourScheduleID = GetInt(row, "WorkingHourScheduleID"),
                                    _LeaveSchemeID = GetInt(row, "LeaveSchemeID"),
                                    _PerformaneBonus = GetInt(row, "PerformaneBonus"),
                                    _EmployeePaymentBreakdownID = GetInt(row, "EmployeePaymentBreakdownID"),
                                    _ClaimSchemeID = GetInt(row, "ClaimSchemeID"),
                                    _EmployeeRetired = GetBool(row, "EmployeeRetired"),
                                    _EmployeeConfirmed = GetBool(row, "EmployeeConfirmed"),
                                    _EISPassword = GetString(row, "EISPassword"),
                                    _LastModifyUser = GetInt(row, "LastModifyUser"),
                                    _LastModifyDate = GetDateTime(row, "LastModifyDate"),
                                    _ServiceePercentage = GetDouble(row, "ServiceePercentage"),
                                    _PassportExpireDate = GetDateTime(row, "PassportExpireDate"),
                                    _LanguageID = GetInt(row, "LanguageID"),
                                    _EmployeeConfirmedDate = GetDateTime(row, "EmployeeConfirmedDate"),
                                    _LocationID = GetInt(row, "LocationID"),
                                    _JobSpecification = GetString(row, "JobSpecification"),
                                    _Title = GetInt(row, "Title"),
                                    _Increment = GetDouble(row, "Increment"),
                                    _IsActive = GetBool(row, "IsActive"),
                                    _WorkingCalenderSchemeID = GetInt(row, "WorkingCalenderSchemeID"),
                                    _IsIncrementApplyingAfter = GetBool(row, "IsIncrementApplyingAfter"),
                                    _IncrementDateFrom = GetDateTime(row, "IncrementDateFrom"),
                                    _AttendanceSchemeID = GetInt(row, "AttendanceSchemeID"),
                                    _ExtraInt1 = GetInt(row, "ExtraInt1"),
                                    _ExtraInt2 = GetInt(row, "ExtraInt2"),
                                    _ExtraInt3 = GetInt(row, "ExtraInt3"),
                                    _ExtraBool1 = GetBool(row, "ExtraBool1"),
                                    _ExtraBool2 = GetBool(row, "ExtraBool2"),
                                    _ExtraBool3 = GetBool(row, "ExtraBool3"),
                                    _ExtraText1 = GetString(row, "ExtraText1"),
                                    _ExtraText2 = GetString(row, "ExtraText2"),
                                    _ExtraText3 = GetString(row, "ExtraText3"),
                                    _IsContractEmployee = GetBool(row, "IsContractEmployee"),
                                    _InactiveDate = GetDateTime(row, "InactiveDate"),
                                    _LeftReason = GetString(row, "LeftReason"), //TharakaM 2020.04.16
                                    _EPFEntitlement = GetBool(row, "EPFEntitlement"), //TharakaM 2020.04.17
                                    _CivilStatusId = GetInt(row, "CivilStatusId"), //TharakaM 2020.04.21
                                    _ReligionID = GetInt(row, "ReligionID"), //TharakaM 2020.04.21
                                    _DateRetired = GetDateTime(row, "DateRetired"), //TharakaM 2020.05.22
                                    _IsRetired = GetBool(row, "IsRetired"), //TharakaM 2020.05.22
                                    _RetiredYear = GetString(row, "RetiredYear"), //TharakaM 2020.05.22
                                    _ProjectDesignationID = GetInt(row, "ProjectDesignationID"),
                                    _ProjectSkillID = GetInt(row, "ProjectSkillID"),
                                    _ProjectJobTypeID = GetInt(row, "ProjectJobTypeID"),
                                    _GradeID = GetInt(row, "GradeID"),
                                    _NID = GetString(row, "NID"),
                                    _ExtraInt4 = GetInt(row, "ExtraInt4"),
                                    _ExtraInt5 = GetInt(row, "ExtraInt5"),
                                    _ExtraInt6 = GetInt(row, "ExtraInt6"),
                                    _ExtraBool4 =GetBool(row, "ExtraBool4"),
                                    _ExtraBool5 =GetBool(row, "ExtraBool5"),
                                    _ExtraBool6 = GetBool(row, "ExtraBool6"),
                                    _ExtraText4 =  GetString(row, "ExtraText4"),
                                    _ExtraText5 =  GetString(row, "ExtraText5"),
                                    _ExtraText6 = GetString(row, "ExtraText6"),
                                    _ExtraDate1 = GetDateTime(row, "ExtraDate1"),
                                    _ExtraDate2 = GetDateTime(row, "ExtraDate2"),
                                    _ExtraDate3 = GetDateTime(row, "ExtraDate3"),
                                    _ExtraDate4 = GetDateTime(row, "ExtraDate4"),
                                    _ExtraDate5 = GetDateTime(row, "ExtraDate5"),
                                    _ExtraDate6 = GetDateTime(row, "ExtraDate6"),
                                    _ExtraFloat1 = GetDouble(row, "ExtraFloat1"),
                                    _ExtraFloat2 = GetDouble(row, "ExtraFloat2"),
                                    _ExtraFloat3 = GetDouble(row, "ExtraFloat3"),
                                    _ExtraFloat4 = GetDouble(row, "ExtraFloat4"),
                                    _ExtraFloat5 = GetDouble(row, "ExtraFloat5"),
                                    _ExtraFloat6 = GetDouble(row, "ExtraFloat6"),
                                    _ContractStartDate= GetDateTime(row, "ContractStartDate"),
                                    _ContractEndDate = GetDateTime(row, "ContractEndDate"),
                                    _EmployeeServiceinfo = new EmployeeServiceInfo()
                                    {
                                        IsConfirmed = _EmployeeConfirmed == null ? false : (bool)_EmployeeConfirmed,
                                        IsRetired = _EmployeeRetired == null ? false : (bool)_EmployeeRetired,
                                        JoinDate = _DateJoined == null ? null : (DateTime?)_DateJoined,
                                        ConfirmDate = _EmployeeConfirmedDate == null ? null : (DateTime?)_EmployeeConfirmedDate,
                                        ResignDate = _DateLeftOrganization == null ? null : (DateTime?)_DateLeftOrganization,
                                        ReasonOfResign = _LeftReason == null ? null : (string)_LeftReason, //TharakaM 2020.04.16
                                    }
                                }).ToList();
                }

                return emplList;
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
                throw ex;
            }
        }

        public bool Delete()
        {
            bool _blnReturnValue = true;
            try
            {
                MapDataToDataObject();
                objDataWorker.objCurrent = objDataObject;
                objDataWorker.RemoveEntry();// delete or deactivated the entry
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
                _blnReturnValue = false;
            }

            return _blnReturnValue;
        }

        public bool Save()
        {
            bool _blnReturnValue = true;
            try
            {
                MapDataToDataObject();
                objDataWorker.objCurrent = objDataObject;
                QueryResult _Qr = objDataWorker.UpdateEntry();

                if (_Qr.RetText != "")
                {
                    _RetInt = _Qr.RetID;
                    _RetText = _Qr.RetText;
                }
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
                _blnReturnValue = false;
            }
            return _blnReturnValue;
        }

        public DataSet GetListSelect(string SelectEmployee)
        {
            DataSet ds = null;
            try
            {
                objDataWorker.objCurrent = objDataObject;
                ds = objDataWorker.getListSelect(SelectEmployee).DataSet;
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
            }
            return ds;
        }

        //edit by dinesh 2017-07-03
        public void EmpList(string BadgeNo)
        {
            try
            {
                SqlParameter[] sqlparaVals = new SqlParameter[1];

                sqlparaVals[0] = new SqlParameter("@BadgeNo", SqlDbType.VarChar);
                sqlparaVals[0].Value = BadgeNo;

                DataSet ds = new DataSet();
                SqlHelper.FillDataset(DBProvider.GetPerfectConnStr, "SP_MasterEmployee_Select_Attenfdance", ds, new string[] { "MasterEmployee" }, sqlparaVals);

                this.MapData(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetByBadgeID()
        {
            //MasterEmployee obj = new MasterEmployee();
            try
            {
                MapDataToDataObject();
                objDataWorker.objCurrent = objDataObject;
                DataSet ds = objDataWorker.getListBadgeNo().DataSet;
                this.MapData(ds);
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
            }
        }

        public DataSet GetListExcelUpload(int SegmentID)
        {
            DataSet ds = null;
            try
            {
                objDataWorker.objCurrent = objDataObject;
                ds = objDataWorker.getListExcelUpload(SegmentID).DataSet;
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
            }
            return ds;
        }

        public bool SaveExcelUpload(int SegmentID, int LastModifyUser)
        {
            bool _blnReturnValue = true;
            try
            {
                objDataWorker.objCurrent = objDataObject;
                QueryResult _Qr = objDataWorker.SaveExcelUpload(SegmentID, LastModifyUser);

                if (_Qr.RetText != "")
                {
                    _RetInt = _Qr.RetID;
                    _RetText = _Qr.RetText;
                }
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
                _blnReturnValue = false;
            }
            return _blnReturnValue;
        }

        public bool DeleteExcelUpload(int SegmentID)
        {
            bool _blnReturnValue = true;
            try
            {
                objDataWorker.objCurrent = objDataObject;
                objDataWorker.RemoveEntryExcelUpload(SegmentID);// delete or deactivated the entry
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
                _blnReturnValue = false;
            }

            return _blnReturnValue;
        }

        public DataSet GetDocList()
        {
            DataSet ds = null;
            try
            {
                MapDataToDataObject();
                objDataWorker.objCurrent = objDataObject;
                ds = objDataWorker.getDocList().DataSet;
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
            }
            return ds;
        }

        #endregion Methods

        public bool UpdateDocEmpNo()
        {
            bool _blnReturnValue = true;
            try
            {
                MapDataToDataObject();
                objDataWorker.objCurrent = objDataObject;
                QueryResult _Qr = objDataWorker.UpdateDocEntry();

                if (_Qr.RetText != "")
                {
                    _RetInt = _Qr.RetID;
                    _RetText = _Qr.RetText;
                }
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
                _blnReturnValue = false;
            }
            return _blnReturnValue;
        }

        public DataSet GetList_SAP_Posting()
        {
            DataSet ds = null;
            try
            {
                MapDataToDataObject();
                objDataWorker.objCurrent = objDataObject;
                ds = objDataWorker.getList_SAP_Posting().DataSet;
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
            }
            return ds;
        }

        public DataSet GetListExcelUpload_withApprovals(int SegmentID)
        {
            DataSet ds = null;
            try
            {
                objDataWorker.objCurrent = objDataObject;
                ds = objDataWorker.getListExcelUpload_With_Approvals(SegmentID).DataSet;
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
            }
            return ds;
        }

        public bool SaveExcelUpload_withApprovals(int SegmentID, int LastModifyUser)
        {
            bool _blnReturnValue = true;
            try
            {
                objDataWorker.objCurrent = objDataObject;
                QueryResult _Qr = objDataWorker.SaveExcelUpload_WithApprovals(SegmentID, LastModifyUser);

                if (_Qr.RetText != "")
                {
                    _RetInt = _Qr.RetID;
                    _RetText = _Qr.RetText;
                }
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
                _blnReturnValue = false;
            }
            return _blnReturnValue;
        }

        public bool SaveExcelUpload_Temp(int SegmentID, int LastModifyUser, DataTable dtEmpTemp)
        {
            bool _blnReturnValue = true;
            try
            {
                objDataWorker.objCurrent = objDataObject;
                QueryResult _Qr = objDataWorker.SaveExcelUpload_Temp(SegmentID, LastModifyUser, dtEmpTemp);

                if (_Qr.RetText != "")
                {
                    _RetInt = _Qr.RetID;
                    _RetText = _Qr.RetText;
                }
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
                _blnReturnValue = false;
            }
            return _blnReturnValue;
        }
    }
}