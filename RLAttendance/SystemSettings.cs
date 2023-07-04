using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using HRMS.BusinessLayer;

namespace AttendanceSolution
{
    public class SystemSettings
    {
        #region Properties

        int _segmentID;

        public int SegmentID
        {
            get { return _segmentID; }
            set { _segmentID = value; }
        }

        int _currentYearID;

        public int CurrentYearID
        {
            get { return _currentYearID; }
            set { _currentYearID = value; }
        }

        int _currentMonthNo;

        public int CurrentMonthNo
        {
            get { return _currentMonthNo; }
            set { _currentMonthNo = value; }
        }

        string _CurrentYearName;

        public string CurrentYearName
        {
            get { return _CurrentYearName; }
            set { _CurrentYearName = value; }
        }

        DateTime _CurrentYearStart;

        public DateTime CurrentYearStart
        {
            get { return _CurrentYearStart; }
            set { _CurrentYearStart = value; }
        }

        DateTime _CurrentYearEnd;

        public DateTime CurrentYearEnd
        {
            get { return _CurrentYearEnd; }
            set { _CurrentYearEnd = value; }
        }

        public List<PayCycleInfo> CurrentPayCycleInfo
        {
            get { return _currentPayCycleInfo; }
            set { _currentPayCycleInfo = value; }
        }

        public class PayCycleInfo
        {
            int _payCycleID;

            public int PayCycleID
            {
                get { return _payCycleID; }
                set { _payCycleID = value; }
            }

            int _payCycleBlockID;

            public int PayCycleBlockID
            {
                get { return _payCycleBlockID; }
                set { _payCycleBlockID = value; }
            }

            public PayCycleInfo(int CycleID, int BlockID)
            {
                _payCycleID = CycleID;
                _payCycleBlockID = BlockID;
            }
        }

        List<PayCycleInfo> _currentPayCycleInfo;

        string _ErrorMessage;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        #endregion

        public SystemSettings(int SegmentID)
        {
            try
            {
                //SystemSettingSegments _objSystemSettings = new SystemSettingSegments();
                //_objSystemSettings.SegmentID = SegmentID;
                //_objSystemSettings.GetByID();

                //if (_objSystemSettings.SalaryYearID != -1)
                //{
                //    _currentYearID = _objSystemSettings.SalaryYearID;
                //    _currentMonthNo = _objSystemSettings.MonthNo;
                //    SalaryYear _objSalaryYear = new SalaryYear();
                //    _objSalaryYear.SalaryYearID = _currentYearID;
                //    _objSalaryYear.GetByID();
                //    if (_objSalaryYear.SalaryYearName != "")
                //    {
                //        _CurrentYearName = _objSalaryYear.SalaryYearName;
                //        _CurrentYearStart = _objSalaryYear.StartDate;
                //        _CurrentYearEnd = _objSalaryYear.EndDate;
                //        _currentPayCycleInfo = getPayCycleInfo();
                //    }
                //}
                //else
                //{
                //    _ErrorMessage = "Year Settings Not Set !! Please set the defualt values !";
                //}

            }
            catch (Exception ex)
            {
                _ErrorMessage = ex.Message;
            }

        }


        //List<SystemSettings.PayCycleInfo> getPayCycleInfo()
        //{
        //    List<SystemSettings.PayCycleInfo> _PayCycleBlocks = new List<PayCycleInfo>();
        //    SystemSettingCurrentPayCycle _objCurrentPayCycle = new SystemSettingCurrentPayCycle();
        //    try
        //    {
        //        _objCurrentPayCycle.SegmentID = _segmentID;
        //        _objCurrentPayCycle.SalaryYearID = _currentYearID ;
        //        DataTable _dt = _objCurrentPayCycle.GetList().Tables[0];
        //        _PayCycleBlocks.Clear();
        //        foreach (DataRow item in _dt.Rows)
	       //     {
        //            _PayCycleBlocks.Add(new PayCycleInfo((int)item["PayCycleID"], (int)item["PayCycleBlockID"]));
	       //     }
        //    }
        //    catch (Exception EX)
        //    {

        //        _ErrorMessage = EX.Message;
        //    }
        //    return _PayCycleBlocks;

        //}
    }
}
