using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using HRMS.DefaultConstants;
using HRMS.DataLayer;

namespace HRMS.BusinessLayer
{
    public class SystemSettingServerInfo:Base
    {
        #region Fields
            
            private int         _LicenceingID              = Constants.NullInt;
            private int         _VersionID              = Constants.NullInt;
            private int         _MaxUserCount              = Constants.NullInt;
            private string         _RegisteredCompanyName              = Constants.NullString;
            private string         _ServerID              = Constants.NullString;


            DALSystemSettingServerInfo objDataWorker = new DALSystemSettingServerInfo();
            DALSystemSettingServerInfo.SystemSettingServerInfo objDataObject = new DALSystemSettingServerInfo.SystemSettingServerInfo();
            private int _RetInt = Constants.NullInt;
            private string _RetText = Constants.NullString;


        #endregion 

        #region Properties

		 public SystemSettingServerInfo()
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

            

		public int LicenceingID 
        {
            get { return _LicenceingID ; }
            set { _LicenceingID  = value; }
        }


            

		public int VersionID 
        {
            get { return _VersionID ; }
            set { _VersionID  = value; }
        }


            

		public int MaxUserCount 
        {
            get { return _MaxUserCount ; }
            set { _MaxUserCount  = value; }
        }


            

		public string RegisteredCompanyName 
        {
            get { return _RegisteredCompanyName ; }
            set { _RegisteredCompanyName  = value; }
        }


            

		public string ServerID 
        {
            get { return _ServerID ; }
            set { _ServerID  = value; }
        }




        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {

             _LicenceingID = GetInt(row, "LicenceingID");
            _VersionID = GetInt(row, "VersionID");
            _MaxUserCount = GetInt(row, "MaxUserCount");
            _RegisteredCompanyName = GetString(row, "RegisteredCompanyName");
            _ServerID = GetString(row, "ServerID");


            return base.MapData(row);
        }

        private void MapDataToDataObject()
        {
            try
            {
                if (objDataObject != null)
                {

                    objDataObject.LicenceingID  = _LicenceingID;
                   objDataObject.VersionID  = _VersionID;
                   objDataObject.MaxUserCount  = _MaxUserCount;
                   objDataObject.RegisteredCompanyName  = _RegisteredCompanyName;
                   objDataObject.ServerID  = _ServerID;


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
            //SystemSettingServerInfo obj = new SystemSettingServerInfo();
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


