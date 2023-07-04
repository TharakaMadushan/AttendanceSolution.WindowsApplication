using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRMS.BusinessLayer;
using System.Data;

namespace HRMS.BusinessLayer
{
    public class UserSecurity
    {
        SecurityActiveUsers _objActiveUsers = new SecurityActiveUsers();
        SystemSettingServerInfo _objServerInfo = new SystemSettingServerInfo();
        SecurityAuditTrailOptions _objAuditTrailOptions = new SecurityAuditTrailOptions();

        #region Properties

        private int _LoginUserID;
        /// <summary>
        /// Login User ID
        /// </summary>
        public int LoginUserID
        {
            get { return _LoginUserID; }
            set { _LoginUserID = value; }
        }

        int _segmentID;
        /// <summary>
        /// Selected Segment ID
        /// </summary>
        public int SegmentID
        {
            get { return _segmentID; }
            set { _segmentID = value; }
        }

        private string _MachineName;

        public string MachineName
        {
            get { return _MachineName; }
            set { _MachineName = value; }
        }

        private string _MachineIP;

        public string MachineIP
        {
            get { return _MachineIP; }
            set { _MachineIP = value; }
        }


        #endregion

        #region Methods

        /// <summary>
        /// Initilize Security Functions
        /// </summary>
        public UserSecurity()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LoginUserID">login user id</param>
        /// <param name="SegmentID">The logged in segment id</param>
        /// <param name="MachineName">The Workstation name</param>
        /// <param name="MachineIP">The workstation IP</param>
        public UserSecurity(int LoginUserID, int SegmentID, string MachineName, string MachineIP)
        {
            try
            {
                _LoginUserID = LoginUserID;
                _segmentID = SegmentID;
                _MachineName = MachineName;
                _MachineIP = MachineIP;
            }
            catch (Exception ex)
            {
                //throw;
            }
        }

        /// <summary>
        /// Check if the user is currently active at a diffrent site or not 
        /// </summary>
        /// <returns></returns>
        public bool IsActiveUserHRMS()
        {
            bool _blnIsActiveUser = false;
            DataSet _dsActiveUser = null;
            try
            {
                _objActiveUsers = new SecurityActiveUsers();
                _objActiveUsers.ConnectionAlive = true;
                _objActiveUsers.SystemUserTypeID = 1;
                _dsActiveUser = _objActiveUsers.GetList();

                #region Commented By nuwan 219/10/15
                foreach (DataRow item in _dsActiveUser.Tables[0].Rows)
                {
                    if ((int)item["UserID"] == _LoginUserID)
                    {
                        _blnIsActiveUser = true;
                        break;
                    }
                }
                #endregion


            }
            catch (Exception ex)
            {
                //throw;
            }
            return _blnIsActiveUser;
        }


        /// <summary>
        /// Check if the user is currently active at a diffrent site or not 
        /// </summary>
        /// <returns></returns>
        public bool IsActiveUserTAS()
        {
            bool _blnIsActiveUser = false;
            DataSet _dsActiveUser = null;
            try
            {
                _objActiveUsers = new SecurityActiveUsers();
                _objActiveUsers.ConnectionAlive = true;
                _objActiveUsers.SystemUserTypeID = 2;
                _dsActiveUser = _objActiveUsers.GetList();

                #region Commented By nuwan 219/10/15
                foreach (DataRow item in _dsActiveUser.Tables[0].Rows)
                {
                    if ((int)item["UserID"] == _LoginUserID)
                    {
                        _blnIsActiveUser = true;
                        break;
                    }
                }
                #endregion


            }
            catch (Exception ex)
            {
                //throw;
            }
            return _blnIsActiveUser;
        }


        /// <summary>
        /// To check the if the licenced user limit has exceaded or not 
        /// </summary>
        /// <returns></returns>
        public bool IsMaximumUserExeed()
        {
            bool _IsMaximumUserExeed = false;
            DataSet _ds = null;
            int _count = 0;
            try
            {
                _objServerInfo = new SystemSettingServerInfo();
                _objServerInfo.GetByID();
                if (_objServerInfo.LicenceingID != -1)
                {
                    _objActiveUsers = new SecurityActiveUsers();
                    _objActiveUsers.ConnectionAlive = true;
                    //_objActiveUsers.SegmentID = _segmentID;
                    _ds = _objActiveUsers.GetList();
                    _count = _ds.Tables[0].Rows.Count;

                    if (_count >= _objServerInfo.MaxUserCount)
                    {
                        _IsMaximumUserExeed = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
            return _IsMaximumUserExeed;
        }

        /// <summary>
        /// saves the current user
        /// </summary>
        void SetValueToActiveUser()
        {
            try
            {
                _objActiveUsers.SegmentID = _segmentID;
                _objActiveUsers.UserID = _LoginUserID;
                _objActiveUsers.LoginDate = DateTime.Now;
                _objActiveUsers.LoginTime = DateTime.Now.ToShortTimeString();
                _objActiveUsers.ConnectionAlive = true;
                _objActiveUsers.WorkStationName = _MachineName;
                _objActiveUsers.ComputerIP = _MachineIP;
                _objActiveUsers.SystemUserTypeID = 1;
                _objActiveUsers.Save();
            }
            catch (Exception ex)
            {

                // throw;
            }
        }

        void SetValueToActiveUserHRMS()
        {
            try
            {
                _objActiveUsers.SegmentID = _segmentID;
                _objActiveUsers.UserID = _LoginUserID;
                _objActiveUsers.LoginDate = DateTime.Now;
                _objActiveUsers.LoginTime = DateTime.Now.ToShortTimeString();
                _objActiveUsers.ConnectionAlive = true;
                _objActiveUsers.WorkStationName = _MachineName;
                _objActiveUsers.ComputerIP = _MachineIP;
                _objActiveUsers.SystemUserTypeID = 1;
                _objActiveUsers.Save();
            }
            catch (Exception ex)
            {

                // throw;
            }
        }

        void SetValueToActiveUserTAS()
        {
            try
            {
                _objActiveUsers.SegmentID = _segmentID;
                _objActiveUsers.UserID = _LoginUserID;
                _objActiveUsers.LoginDate = DateTime.Now;
                _objActiveUsers.LoginTime = DateTime.Now.ToShortTimeString();
                _objActiveUsers.ConnectionAlive = true;
                _objActiveUsers.WorkStationName = _MachineName;
                _objActiveUsers.ComputerIP = _MachineIP;
                _objActiveUsers.SystemUserTypeID = 2;
                _objActiveUsers.Save();
            }
            catch (Exception ex)
            {

                // throw;
            }
        }


        /// <summary>
        /// set the current user as active
        /// </summary>
        public void LogLoginUser()
        {
            try
            {
                if (IsMaximumUserExeed() == false && IsActiveUserHRMS() == false)
                {
                    SetValueToActiveUser();
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
        }

        public void LogLoginUserHRMS()
        {
            try
            {
                if (IsMaximumUserExeed() == false && IsActiveUserHRMS() == false)
                {
                    SetValueToActiveUserHRMS();
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
        }

        public void LogLoginUserTAS()
        {
            try
            {
                if (IsMaximumUserExeed() == false && IsActiveUserTAS() == false)
                {
                    SetValueToActiveUserTAS();
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
        }

        /// <summary>
        /// unloges the current user
        /// </summary>
        public void UnlogLoginUser()
        {
            try
            {
                _objActiveUsers = new SecurityActiveUsers();
                _objActiveUsers.SegmentID = _segmentID;
                _objActiveUsers.UserID = _LoginUserID;
                _objActiveUsers.WorkStationName = _MachineName;
                _objActiveUsers.ConnectionAlive = true;
                
                _objActiveUsers.GetByID();
                if (_objActiveUsers.LoginInstance != -1)
                {
                    _objActiveUsers.ConnectionAlive = false;
                    _objActiveUsers.LogOutDate = DateTime.Now;
                    _objActiveUsers.LogOutTime = DateTime.Now.ToShortTimeString();
                    _objActiveUsers.Save();
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
        }


        public void UnlogLoginUserTAS()
        {
            try
            {
                _objActiveUsers = new SecurityActiveUsers();
                _objActiveUsers.SegmentID = _segmentID;
                _objActiveUsers.UserID = _LoginUserID;
                _objActiveUsers.WorkStationName = _MachineName;
                _objActiveUsers.ConnectionAlive = true;
                _objActiveUsers.SystemUserTypeID = 2;
                _objActiveUsers.GetByID();
                if (_objActiveUsers.LoginInstance != -1)
                {
                    _objActiveUsers.ConnectionAlive = false;
                    _objActiveUsers.LogOutDate = DateTime.Now;
                    _objActiveUsers.LogOutTime = DateTime.Now.ToShortTimeString();
                    _objActiveUsers.Save();
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
        }

        public void UnlogLoginUserHRMS()
        {
            try
            {
                _objActiveUsers = new SecurityActiveUsers();
                _objActiveUsers.SegmentID = _segmentID;
                _objActiveUsers.UserID = _LoginUserID;
                _objActiveUsers.WorkStationName = _MachineName;
                _objActiveUsers.ConnectionAlive = true;
                _objActiveUsers.SystemUserTypeID = 1;
                _objActiveUsers.GetByID();
                if (_objActiveUsers.LoginInstance != -1)
                {
                    _objActiveUsers.ConnectionAlive = false;
                    _objActiveUsers.LogOutDate = DateTime.Now;
                    _objActiveUsers.LogOutTime = DateTime.Now.ToShortTimeString();
                    _objActiveUsers.Save();
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
        }

        /// <summary>
        /// loges each option used by the user
        /// </summary>
        /// <param name="OptionID"></param>
        public void LogUsedOption(int OptionID)
        {
            try
            {
                _objAuditTrailOptions.UserID = _LoginUserID;
                _objAuditTrailOptions.OptionID = OptionID;
                _objAuditTrailOptions.UseDateTime = DateTime.Now;
                _objAuditTrailOptions.OptionFacilityID = 1;
                _objAuditTrailOptions.Save();
            }
            catch (Exception ex)
            {
                //throw;
            }
        }

        public bool IsMaxLoginUsers() //Limit Maximum Login Users as 5  - Done By TM 2020.10.08
        {
            bool _IsMaxLoginUsers = false;
            DataSet _ds = null;
            int _count = 0;

            try
            {
                SystemSetting _objSettings = new SystemSetting();
                _objSettings.SystemCode = "LLU";
                _objSettings.GetByID();
                if (_objSettings.SystemSettingUserID != -1)
                {
                    _objActiveUsers = new SecurityActiveUsers();
                    _objActiveUsers.ConnectionAlive = true;
                    _objActiveUsers.SegmentID = _objSettings.SegmentID;
                    _ds = _objActiveUsers.GetList();
                    _count = _ds.Tables[0].Rows.Count;

                    if (_count > int.Parse(_objSettings.ExtraField1))
                    {
                        _IsMaxLoginUsers = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _IsMaxLoginUsers;
        }


        #endregion

    }
}
