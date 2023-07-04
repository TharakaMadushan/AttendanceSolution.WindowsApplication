using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using HRMS.DefaultConstants;
using HRMS.DataLayer;

namespace HRMS.BusinessLayer
{
    public class SecurityActiveUsers:Base
    {
        #region Fields
            
            private int         _LoginInstance              = Constants.NullInt;
            private int         _SegmentID              = Constants.NullInt;
            private DateTime         _LoginDate              = Constants.NullDateTime;
            private string         _LoginTime              = Constants.NullString;
            private DateTime         _LogOutDate              = Constants.NullDateTime;
            private string         _LogOutTime              = Constants.NullString;
            private bool?         _ConnectionAlive              = Constants.NullBool;
            private int         _UserID              = Constants.NullInt;
            private string         _WorkStationName              = Constants.NullString;
            private string         _ComputerIP              = Constants.NullString;
            private int _SystemUserTypeID = Constants.NullInt;


            DALSecurityActiveUsers objDataWorker = new DALSecurityActiveUsers();
            DALSecurityActiveUsers.SecurityActiveUsers objDataObject = new DALSecurityActiveUsers.SecurityActiveUsers();
            private int _RetInt = Constants.NullInt;
            private string _RetText = Constants.NullString;


        #endregion 

        #region Properties

		 public SecurityActiveUsers()
        {
            _mainDisplaycolumns = new string[] { "" };
            _mainIDColumns = new string[] { "" };
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

            

		public int LoginInstance 
        {
            get { return _LoginInstance ; }
            set { _LoginInstance  = value; }
        }


            

		public int SegmentID 
        {
            get { return _SegmentID ; }
            set { _SegmentID  = value; }
        }


            

		public DateTime LoginDate 
        {
            get { return _LoginDate ; }
            set { _LoginDate  = value; }
        }


            

		public string LoginTime 
        {
            get { return _LoginTime ; }
            set { _LoginTime  = value; }
        }


            

		public DateTime LogOutDate 
        {
            get { return _LogOutDate ; }
            set { _LogOutDate  = value; }
        }


            

		public string LogOutTime 
        {
            get { return _LogOutTime ; }
            set { _LogOutTime  = value; }
        }


            

		public bool? ConnectionAlive 
        {
            get { return _ConnectionAlive ; }
            set { _ConnectionAlive  = value; }
        }


            

		public int UserID 
        {
            get { return _UserID ; }
            set { _UserID  = value; }
        }


            

		public string WorkStationName 
        {
            get { return _WorkStationName ; }
            set { _WorkStationName  = value; }
        }


            

		public string ComputerIP 
        {
            get { return _ComputerIP ; }
            set { _ComputerIP  = value; }
        }

        public int SystemUserTypeID
        {
            get { return _SystemUserTypeID; }
            set { _SystemUserTypeID = value; }
        }


        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {

             _LoginInstance = GetInt(row, "LoginInstance");
            _SegmentID = GetInt(row, "SegmentID");
            _LoginDate = GetDateTime(row, "LoginDate");
            _LoginTime = GetString(row, "LoginTime");
            _LogOutDate = GetDateTime(row, "LogOutDate");
            _LogOutTime = GetString(row, "LogOutTime");
            _ConnectionAlive = GetBool(row, "ConnectionAlive");
            _UserID = GetInt(row, "UserID");
            _WorkStationName = GetString(row, "WorkStationName");
            _ComputerIP = GetString(row, "ComputerIP");
            _SystemUserTypeID = GetInt(row, "SystemUserTypeID");


            return base.MapData(row);
        }

        private void MapDataToDataObject()
        {
            try
            {
                if (objDataObject != null)
                {

                    objDataObject.LoginInstance  = _LoginInstance;
                   objDataObject.SegmentID  = _SegmentID;
                   objDataObject.LoginDate  = _LoginDate;
                   objDataObject.LoginTime  = _LoginTime;
                   objDataObject.LogOutDate  = _LogOutDate;
                   objDataObject.LogOutTime  = _LogOutTime;
                   objDataObject.ConnectionAlive  = _ConnectionAlive;
                   objDataObject.UserID  = _UserID;
                   objDataObject.WorkStationName  = _WorkStationName;
                   objDataObject.ComputerIP  = _ComputerIP;
                   objDataObject.SystemUserTypeID = _SystemUserTypeID;

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


        public void GetByID()
        {
            //SecurityActiveUsers obj = new SecurityActiveUsers();
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


