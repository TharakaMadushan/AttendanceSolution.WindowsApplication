using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using HRMS.DefaultConstants;
using HRMS.DataLayer;

namespace HRMS.BusinessLayer
{
    public class Users:Base
    {
        #region Fields
            
            private int         _UserID              = Constants.NullInt;
            private int         _SegmentID              = Constants.NullInt;
            private string         _UserName              = Constants.NullString;
            private string         _UserActualName              = Constants.NullString;
            private string         _Password              = Constants.NullString;
            private string         _BadgeNo              = Constants.NullString;
            private int         _EmployeeNo              = Constants.NullInt;
            private string         _SegmentFilter              = Constants.NullString;
            private string         _DesignationsFilters              = Constants.NullString;
            private string         _EmployeeLevelsFilters              = Constants.NullString;
            private string         _EmployeeAntiFilter              = Constants.NullString;
            private string         _SectionFilter              = Constants.NullString;
            private string         _CategoryFilter              = Constants.NullString;
            private string         _LocationFilter              = Constants.NullString;
            private string         _OrientationFilter              = Constants.NullString;
            private string         _GenderFilter              = Constants.NullString;
            private string         _TransportMethodFilter              = Constants.NullString;
            private DateTime         _LastModifyDate              = Constants.NullDateTime;
            private int         _LastModifyUser              = Constants.NullInt;
            private int          _LastInsertID = Constants.NullInt;
            private bool? _IsDeactivate = Constants.NullBool;


            //edit by dinesh
            private bool _ResetPassword = Convert.ToBoolean(Constants.NullBool);
            private DateTime _PasswordResetDate = Constants.NullDateTime;
            private int _PasswordResetBy = Constants.NullInt;
            private int _UserGroupID = Constants.NullInt;
         

            private bool _DemoMode = true;

            DALSecurityUsers objDataWorker = new DALSecurityUsers();
            DALSecurityUsers.SecurityUsers objDataObject = new DALSecurityUsers.SecurityUsers();
            private int _RetInt = Constants.NullInt;
            private string _RetText = Constants.NullString;


        #endregion 

        #region Properties

		 public Users()
        {
            _mainDisplaycolumns = new string[] { "UserActualName" };
            _mainIDColumns = new string[] { "UserID" };
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

         public bool DemoMode
         {
             get { return _DemoMode; }
         }
            

		public int UserID 
        {
            get { return _UserID ; }
            set { _UserID  = value; }
        }


            

		public int SegmentID 
        {
            get { return _SegmentID ; }
            set { _SegmentID  = value; }
        }


            

		public string UserName 
        {
            get { return _UserName ; }
            set { _UserName  = value; }
        }


            

		public string UserActualName 
        {
            get { return _UserActualName ; }
            set { _UserActualName  = value; }
        }


            

		public string Password 
        {
            get { return _Password ; }
            set { _Password  = value; }
        }


            

		public string BadgeNo 
        {
            get { return _BadgeNo ; }
            set { _BadgeNo  = value; }
        }


            

		public int EmployeeNo 
        {
            get { return _EmployeeNo ; }
            set { _EmployeeNo  = value; }
        }


            

		public string SegmentFilter 
        {
            get { return _SegmentFilter ; }
            set { _SegmentFilter  = value; }
        }


            

		public string DesignationsFilters 
        {
            get { return _DesignationsFilters ; }
            set { _DesignationsFilters  = value; }
        }


            

		public string EmployeeLevelsFilters 
        {
            get { return _EmployeeLevelsFilters ; }
            set { _EmployeeLevelsFilters  = value; }
        }


            

		public string EmployeeAntiFilter 
        {
            get { return _EmployeeAntiFilter ; }
            set { _EmployeeAntiFilter  = value; }
        }


            

		public string SectionFilter 
        {
            get { return _SectionFilter ; }
            set { _SectionFilter  = value; }
        }


            

		public string CategoryFilter 
        {
            get { return _CategoryFilter ; }
            set { _CategoryFilter  = value; }
        }


            

		public string LocationFilter 
        {
            get { return _LocationFilter ; }
            set { _LocationFilter  = value; }
        }


            

		public string OrientationFilter 
        {
            get { return _OrientationFilter ; }
            set { _OrientationFilter  = value; }
        }


            

		public string GenderFilter 
        {
            get { return _GenderFilter ; }
            set { _GenderFilter  = value; }
        }


            

		public string TransportMethodFilter 
        {
            get { return _TransportMethodFilter ; }
            set { _TransportMethodFilter  = value; }
        }


            

		public DateTime LastModifyDate 
        {
            get { return _LastModifyDate ; }
            set { _LastModifyDate  = value; }
        }


            

		public int LastModifyUser 
        {
            get { return _LastModifyUser ; }
            set { _LastModifyUser  = value; }
        }

        public int LastInsertID 
        {
            get { return _LastInsertID; }
            set { _LastInsertID = value; }
        }

        public DateTime PasswordResetDate
        {
            get { return _PasswordResetDate; }
            set { _PasswordResetDate = value; }
        }

        public int PasswordResetBy
        {
            get { return _PasswordResetBy; }
            set { _PasswordResetBy = value; }
        }
        public bool ResetPassword
        {
            get { return _ResetPassword; }
            set { _ResetPassword = value; }
        }

        public int UserGroupID
        {
            get { return _UserGroupID; }
            set { _UserGroupID = value; }
        }

        public bool? IsDeactivate
        {
            get { return _IsDeactivate; }
            set { _IsDeactivate = value; }
        }


        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {

             _UserID = GetInt(row, "UserID");
            _SegmentID = GetInt(row, "SegmentID");
            _UserName = GetString(row, "UserName");
            _UserActualName = GetString(row, "UserActualName");
            _Password = GetString(row, "Password");
            _BadgeNo = GetString(row, "BadgeNo");
            _EmployeeNo = GetInt(row, "EmployeeNo");
            _SegmentFilter = GetString(row, "SegmentFilter");
            _DesignationsFilters = GetString(row, "DesignationsFilters");
            _EmployeeLevelsFilters = GetString(row, "EmployeeLevelsFilters");
            _EmployeeAntiFilter = GetString(row, "EmployeeAntiFilter");
            _SectionFilter = GetString(row, "SectionFilter");
            _CategoryFilter = GetString(row, "CategoryFilter");
            _LocationFilter = GetString(row, "LocationFilter");
            _OrientationFilter = GetString(row, "OrientationFilter");
            _GenderFilter = GetString(row, "GenderFilter");
            _TransportMethodFilter = GetString(row, "TransportMethodFilter");
            _LastModifyDate = GetDateTime(row, "LastModifyDate");
            _LastModifyUser = GetInt(row, "LastModifyUser");
            _LastInsertID = GetInt(row, "LastInsertID");

            //edit by dinesh
            _ResetPassword = GetBool(row, "ResetPassword");
            _PasswordResetDate = GetDateTime(row, "PasswordResetDate");
            _PasswordResetBy = GetInt(row, "PasswordResetBy");
            _UserGroupID = GetInt(row, "UserGroupID");
            _IsDeactivate = GetBool(row, "IsDeactivate");

            return base.MapData(row);
        }

        private void MapDataToDataObject()
        {
            try
            {
                if (objDataObject != null)
                {

                    objDataObject.UserID  = _UserID;
                   objDataObject.SegmentID  = _SegmentID;
                   objDataObject.UserName  = _UserName;
                   objDataObject.UserActualName  = _UserActualName;
                   objDataObject.Password  = _Password;
                   objDataObject.BadgeNo  = _BadgeNo;
                   objDataObject.EmployeeNo  = _EmployeeNo;
                   objDataObject.SegmentFilter  = _SegmentFilter;
                   objDataObject.DesignationsFilters  = _DesignationsFilters;
                   objDataObject.EmployeeLevelsFilters  = _EmployeeLevelsFilters;
                   objDataObject.EmployeeAntiFilter  = _EmployeeAntiFilter;
                   objDataObject.SectionFilter  = _SectionFilter;
                   objDataObject.CategoryFilter  = _CategoryFilter;
                   objDataObject.LocationFilter  = _LocationFilter;
                   objDataObject.OrientationFilter  = _OrientationFilter;
                   objDataObject.GenderFilter  = _GenderFilter;
                   objDataObject.TransportMethodFilter  = _TransportMethodFilter;
                   objDataObject.LastModifyDate  = _LastModifyDate;
                   objDataObject.LastModifyUser  = _LastModifyUser;
                   objDataObject.LastInsertID = _LastInsertID;



                    //edit by dinesh
                   objDataObject.ResetPassword = _ResetPassword;
                   objDataObject.PasswordResetDate = _PasswordResetDate;
                   objDataObject.PasswordResetBy = _PasswordResetBy;
                   objDataObject.UserGroupID = _UserGroupID;
                   objDataObject.IsDeactivate = _IsDeactivate;


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

        public  List<Users> GetObjectList()
        {
            DataSet ds = null;
            List<Users> _lstObjects = new List<Users>();
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
                            Users _objItem = new Users();
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


        public void GetByID()
        {
            //SecurityUsers obj = new SecurityUsers();
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
                QueryResult _Qr= objDataWorker.UpdateEntry();

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

        #endregion


    }
}




