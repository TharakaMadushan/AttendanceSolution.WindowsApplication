using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using HRMS.DefaultConstants;
using HRMS.DataLayer;

namespace HRMS.BusinessLayer
{
    public class SystemSetting:Base
    {
        #region Fields
            
            private int         _SystemSettingID              = Constants.NullInt;
            private string         _SystemCode              = Constants.NullString;
            private int         _SegmentID              = Constants.NullInt;
            private int         _UserGroupID              = Constants.NullInt;
            private int         _SystemSettingUserID              = Constants.NullInt;
            private string         _SystemSettingItem              = Constants.NullString;
            private bool?         _SystemSettingActive              = Constants.NullBool;
            private string         _SystemSettingType              = Constants.NullString;
            private DateTime         _ActiveDateFrom              = Constants.NullDateTime;
            private DateTime         _ActiveDateTo              = Constants.NullDateTime;
            private DateTime         _LastModifyDate              = Constants.NullDateTime;
            private int         _LastModifyUser              = Constants.NullInt;
            private string         _ExtraField1              = Constants.NullString;
            private string         _ExtraField2              = Constants.NullString;
            private string         _ExtraField3              = Constants.NullString;


            DALSystemSetting objDataWorker = new DALSystemSetting();
            DALSystemSetting.SystemSetting objDataObject = new DALSystemSetting.SystemSetting();
            private int _RetInt = Constants.NullInt;
            private string _RetText = Constants.NullString;


        #endregion 

        #region Properties

		 public SystemSetting()
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

            

		public int SystemSettingID 
        {
            get { return _SystemSettingID ; }
            set { _SystemSettingID  = value; }
        }


            

		public string SystemCode 
        {
            get { return _SystemCode ; }
            set { _SystemCode  = value; }
        }


            

		public int SegmentID 
        {
            get { return _SegmentID ; }
            set { _SegmentID  = value; }
        }


            

		public int UserGroupID 
        {
            get { return _UserGroupID ; }
            set { _UserGroupID  = value; }
        }


            

		public int SystemSettingUserID 
        {
            get { return _SystemSettingUserID ; }
            set { _SystemSettingUserID  = value; }
        }


            

		public string SystemSettingItem 
        {
            get { return _SystemSettingItem ; }
            set { _SystemSettingItem  = value; }
        }


            

		public bool? SystemSettingActive 
        {
            get { return _SystemSettingActive ; }
            set { _SystemSettingActive  = value; }
        }


            

		public string SystemSettingType 
        {
            get { return _SystemSettingType ; }
            set { _SystemSettingType  = value; }
        }


            

		public DateTime ActiveDateFrom 
        {
            get { return _ActiveDateFrom ; }
            set { _ActiveDateFrom  = value; }
        }


            

		public DateTime ActiveDateTo 
        {
            get { return _ActiveDateTo ; }
            set { _ActiveDateTo  = value; }
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


            

		public string ExtraField1 
        {
            get { return _ExtraField1 ; }
            set { _ExtraField1  = value; }
        }


            

		public string ExtraField2 
        {
            get { return _ExtraField2 ; }
            set { _ExtraField2  = value; }
        }


            

		public string ExtraField3 
        {
            get { return _ExtraField3 ; }
            set { _ExtraField3  = value; }
        }




        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {

             _SystemSettingID = GetInt(row, "SystemSettingID");
            _SystemCode = GetString(row, "SystemCode");
            _SegmentID = GetInt(row, "SegmentID");
            _UserGroupID = GetInt(row, "UserGroupID");
            _SystemSettingUserID = GetInt(row, "SystemSettingUserID");
            _SystemSettingItem = GetString(row, "SystemSettingItem");
            _SystemSettingActive = GetBool(row, "SystemSettingActive");
            _SystemSettingType = GetString(row, "SystemSettingType");
            _ActiveDateFrom = GetDateTime(row, "ActiveDateFrom");
            _ActiveDateTo = GetDateTime(row, "ActiveDateTo");
            _LastModifyDate = GetDateTime(row, "LastModifyDate");
            _LastModifyUser = GetInt(row, "LastModifyUser");
            _ExtraField1 = GetString(row, "ExtraField1");
            _ExtraField2 = GetString(row, "ExtraField2");
            _ExtraField3 = GetString(row, "ExtraField3");


            return base.MapData(row);
        }

        private void MapDataToDataObject()
        {
            try
            {
                if (objDataObject != null)
                {

                    objDataObject.SystemSettingID  = _SystemSettingID;
                   objDataObject.SystemCode  = _SystemCode;
                   objDataObject.SegmentID  = _SegmentID;
                   objDataObject.UserGroupID  = _UserGroupID;
                   objDataObject.SystemSettingUserID  = _SystemSettingUserID;
                   objDataObject.SystemSettingItem  = _SystemSettingItem;
                   objDataObject.SystemSettingActive  = _SystemSettingActive;
                   objDataObject.SystemSettingType  = _SystemSettingType;
                   objDataObject.ActiveDateFrom  = _ActiveDateFrom;
                   objDataObject.ActiveDateTo  = _ActiveDateTo;
                   objDataObject.LastModifyDate  = _LastModifyDate;
                   objDataObject.LastModifyUser  = _LastModifyUser;
                   objDataObject.ExtraField1  = _ExtraField1;
                   objDataObject.ExtraField2  = _ExtraField2;
                   objDataObject.ExtraField3  = _ExtraField3;


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

        public List<SystemSetting> GetObjectList()
        {
            DataSet ds = null;
            List<SystemSetting> _lstObjects = new List<SystemSetting>();
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
                            SystemSetting _objItem = new SystemSetting();
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
            //SystemSetting obj = new SystemSetting();
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

        //Done by menushan 2019-11-06
       public bool SaveBulk(DataTable dataBulk)
       {
           bool _blnReturnValue = true;
           try
           {
               MapDataToDataObject();
               objDataWorker.objCurrent = objDataObject;
               QueryResult _Qr = objDataWorker.UpdateEntryBulk(dataBulk);

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


