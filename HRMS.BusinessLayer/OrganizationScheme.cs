using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using HRMS.DefaultConstants;
using HRMS.DataLayer;

namespace HRMS.BusinessLayer
{
    public class OrganizationScheme:Base
    {
        #region Fields
            
            private int         _SegmentID              = Constants.NullInt;
            private string         _SegmentName              = Constants.NullString;
            private int         _SegmentTypeID              = Constants.NullInt;
            private int         _ParentSegmentID              = Constants.NullInt;
            private DateTime         _LastModifyDate              = Constants.NullDateTime;
            private int         _LastModifyUser              = Constants.NullInt;
            private double         _SalaryBudget              = Constants.NullDouble;
            private double         _RecuitmentBudget              = Constants.NullDouble;
            private double         _TrainingBudget              = Constants.NullDouble;
            private double         _DiscressionaryBudget              = Constants.NullDouble;


            DALTransactionOrganizationScheme objDataWorker = new DALTransactionOrganizationScheme();
            DALTransactionOrganizationScheme.TransactionOrganizationScheme objDataObject = new DALTransactionOrganizationScheme.TransactionOrganizationScheme();
            private int _RetInt = Constants.NullInt;
            private string _RetText = Constants.NullString;


        #endregion 

        #region Properties

		 public OrganizationScheme()
        {
            _mainDisplaycolumns = new string[] { "SegmentName" };
            _mainIDColumns = new string[] { "SegmentID" };
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
            get { return _SegmentID ; }
            set { _SegmentID  = value; }
        }


            

		public string SegmentName 
        {
            get { return _SegmentName ; }
            set { _SegmentName  = value; }
        }


            

		public int SegmentTypeID 
        {
            get { return _SegmentTypeID ; }
            set { _SegmentTypeID  = value; }
        }


            

		public int ParentSegmentID 
        {
            get { return _ParentSegmentID ; }
            set { _ParentSegmentID  = value; }
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


            

		public double SalaryBudget 
        {
            get { return _SalaryBudget ; }
            set { _SalaryBudget  = value; }
        }


            

		public double RecuitmentBudget 
        {
            get { return _RecuitmentBudget ; }
            set { _RecuitmentBudget  = value; }
        }


            

		public double TrainingBudget 
        {
            get { return _TrainingBudget ; }
            set { _TrainingBudget  = value; }
        }


            

		public double DiscressionaryBudget 
        {
            get { return _DiscressionaryBudget ; }
            set { _DiscressionaryBudget  = value; }
        }




        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {

             _SegmentID = GetInt(row, "SegmentID");
            _SegmentName = GetString(row, "SegmentName");
            _SegmentTypeID = GetInt(row, "SegmentTypeID");
            _ParentSegmentID = GetInt(row, "ParentSegmentID");
            _LastModifyDate = GetDateTime(row, "LastModifyDate");
            _LastModifyUser = GetInt(row, "LastModifyUser");
            _SalaryBudget = GetDouble(row, "SalaryBudget");
            _RecuitmentBudget = GetDouble(row, "RecuitmentBudget");
            _TrainingBudget = GetDouble(row, "TrainingBudget");
            _DiscressionaryBudget = GetDouble(row, "DiscressionaryBudget");


            return base.MapData(row);
        }

        private void MapDataToDataObject()
        {
            try
            {
                if (objDataObject != null)
                {

                    objDataObject.SegmentID  = _SegmentID;
                   objDataObject.SegmentName  = _SegmentName;
                   objDataObject.SegmentTypeID  = _SegmentTypeID;
                   objDataObject.ParentSegmentID  = _ParentSegmentID;
                   objDataObject.LastModifyDate  = _LastModifyDate;
                   objDataObject.LastModifyUser  = _LastModifyUser;
                   objDataObject.SalaryBudget  = _SalaryBudget;
                   objDataObject.RecuitmentBudget  = _RecuitmentBudget;
                   objDataObject.TrainingBudget  = _TrainingBudget;
                   objDataObject.DiscressionaryBudget  = _DiscressionaryBudget;


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


        public  DataSet GetLoginLocations()
        {
            DataSet ds = null;
            try
            {
                MapDataToDataObject();
                objDataWorker.objCurrent = objDataObject;
                ds = CommonFunctions.getSqlDataset("Select * from TransactionOrganizationScheme where SegmentTypeID in (Select SegmentTypeID from MasterSegmentType where ActiveTranactionPoint = 1) ");
            }
            catch (Exception ex)
            {
                ErrorObject = ex;
            }
            return ds;
        }



        public  List<OrganizationScheme> GetObjectList()
        {
            DataSet ds = null;
            List<OrganizationScheme> _lstObjects = new List<OrganizationScheme>();
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
                            OrganizationScheme _objItem = new OrganizationScheme();
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
            //TransactionOrganizationScheme obj = new TransactionOrganizationScheme();
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


