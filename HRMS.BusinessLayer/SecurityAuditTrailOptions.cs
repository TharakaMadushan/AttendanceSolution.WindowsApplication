using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using HRMS.DefaultConstants;
using HRMS.DataLayer;

namespace HRMS.BusinessLayer
{
    public class SecurityAuditTrailOptions:Base
    {
        #region Fields
            
            private int         _AduitLoginID              = Constants.NullInt;
            private int         _UserID              = Constants.NullInt;
            private int         _OptionID              = Constants.NullInt;
            private int         _OptionFacilityID              = Constants.NullInt;
            private DateTime         _UseDateTime              = Constants.NullDateTime;


            DALSecurityAuditTrailOptions objDataWorker = new DALSecurityAuditTrailOptions();
            DALSecurityAuditTrailOptions.SecurityAuditTrailOptions objDataObject = new DALSecurityAuditTrailOptions.SecurityAuditTrailOptions();
            private int _RetInt = Constants.NullInt;
            private string _RetText = Constants.NullString;


        #endregion 

        #region Properties

		 public SecurityAuditTrailOptions()
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

            

		public int AduitLoginID 
        {
            get { return _AduitLoginID ; }
            set { _AduitLoginID  = value; }
        }


            

		public int UserID 
        {
            get { return _UserID ; }
            set { _UserID  = value; }
        }


            

		public int OptionID 
        {
            get { return _OptionID ; }
            set { _OptionID  = value; }
        }


            

		public int OptionFacilityID 
        {
            get { return _OptionFacilityID ; }
            set { _OptionFacilityID  = value; }
        }


            

		public DateTime UseDateTime 
        {
            get { return _UseDateTime ; }
            set { _UseDateTime  = value; }
        }




        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {

             _AduitLoginID = GetInt(row, "AduitLoginID");
            _UserID = GetInt(row, "UserID");
            _OptionID = GetInt(row, "OptionID");
            _OptionFacilityID = GetInt(row, "OptionFacilityID");
            _UseDateTime = GetDateTime(row, "UseDateTime");


            return base.MapData(row);
        }

        private void MapDataToDataObject()
        {
            try
            {
                if (objDataObject != null)
                {

                    objDataObject.AduitLoginID  = _AduitLoginID;
                   objDataObject.UserID  = _UserID;
                   objDataObject.OptionID  = _OptionID;
                   objDataObject.OptionFacilityID  = _OptionFacilityID;
                   objDataObject.UseDateTime  = _UseDateTime;


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
            //SecurityAuditTrailOptions obj = new SecurityAuditTrailOptions();
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


