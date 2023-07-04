using System;
using HRMS.BusinessLayer;
using System.IO;
using System.Windows.Forms;

namespace AttendanceSolution
{
    public class clsGobalUser
    {


        #region Properties 
        // Application gobal constant values


        private SystemSettings _SystemSettings;

        private Users _LoginUser = new Users();

        private Exception _ErrorEvent;

        private UserSecurity _UserSecurity = new UserSecurity();

        public UserSecurity UserSecurity
        {
            get { return _UserSecurity; }
            set { _UserSecurity = value; }
        }
        //-----------------------------------
        
        #endregion

        #region ExposeProperties 

        /// <summary>
        /// These are the system settings associated with the segment that is logged in 
        /// the object is initialized in the setting of the Login User ( look in Set _LoginUSer )
        /// </summary>
        public SystemSettings SystemSettings
        {
            get { return _SystemSettings; }
            set { _SystemSettings = value; }
        }

        // exposes all the properties to outside entities

        public Users LoginUser
        {
            get { return _LoginUser; }
            set { 
                    _LoginUser = value;
                }
        }

        /// <summary>
        /// this get the setting from the user
        /// </summary>
        public void getSystemSettings()
        {

            // get the defult system settings for the specified system (logged in user segment)
            if (_LoginUser.SegmentID != -1)
            {
                _SystemSettings = new SystemSettings(_LoginUser.SegmentID);

            }
        
        }

        /// <summary>
        ///  This sets the current error object and writes a record to the app patp
        /// </summary>
        public Exception ErrorObject
        {
            get { return _ErrorEvent; }
            set
            {
                _ErrorEvent = value;
                logEvent(_ErrorEvent);
            }
        }

        #endregion

        string getCurrentAppPath()
        {
            string _path = System.Environment.CurrentDirectory.ToString();
            return _path;
        }

        /// <summary>
        /// set user information into security
        /// </summary>
        public void setUserSecurityInfo()
        {
            _UserSecurity = new HRMS.BusinessLayer.UserSecurity(this._LoginUser.UserID,this._LoginUser.SegmentID,CommonFunctions.getMachineName(),CommonFunctions.getComputerIP());
        }

        private void logEvent(Exception ErrorEvent)
        {
            string strFileName = string.Empty;
            try
            {
                
                string _ErrorString = "HRIS:System issues " + System.Environment.NewLine+ "error Source :" + ErrorEvent.Source + System.Environment.NewLine;
                _ErrorString += "error StackTrace:" + ErrorEvent.Message.ToString() + System.Environment.NewLine;
                _ErrorString += "error StackTrace:" + ErrorEvent.StackTrace.ToString() + System.Environment.NewLine;


                if (_LoginUser.DemoMode == false)
                {
                    MessageBox.Show(_ErrorString, "HRIS:System Issue", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                // Edited by DDEV for maintain a seperate file for a month on 08 SEP 2017
                //strFileName = getCurrentAppPath() + "\\" + " Errorlog_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString();

                strFileName = getCurrentAppPath() + "\\ErrorLOG\\" + "ErrorLog_" + System.DateTime.Now.ToString("yyyyMMM") + ".log";                

                using (var fs = new FileStream(strFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (var stream = new StreamWriter(fs))
                {
                    stream.WriteLine("-------- " + System.DateTime.Now.ToLongDateString() + " -- " + System.DateTime.Now.ToLongTimeString() + " -------------------------------------------------------");
                    stream.WriteLine(_ErrorString);
                    stream.Close();
                    stream.Dispose();
                }
               
                
                

            }
            catch (Exception exEx)
            {
                strFileName = System.Environment.CurrentDirectory + "\\ErrorLOG\\" + "ErrorLog_" + System.DateTime.Now.ToString("yyyyMMM") + ".log";
                //System.IO.StreamWriter _stwErrorLogEX = new System.IO.StreamWriter(System.Environment.CurrentDirectory + "\\ErrorLOG\\" + "ErrorLog_" + System.DateTime.Now.ToString("yyyyMMM") + ".log", true);
                //_stwErrorLogEX.WriteLine("-------- " + System.DateTime.Now.ToLongDateString() + " -- " + System.DateTime.Now.ToLongTimeString() + " -------------------------------------------------------");
                //_stwErrorLogEX.Write("Exception Error : " + exEx.Message.ToString() + System.Environment.NewLine + "Stack Trace:" + exEx.StackTrace.ToString());

                //_stwErrorLogEX.Close();
                //_stwErrorLogEX.Dispose();

                //using (var fs = new FileStream(strFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                //using (var stream = new StreamWriter(fs))
                //{
                //    //System.IO.StreamWriter _stwErrorLogEX = new System.IO.StreamWriter(System.Environment.CurrentDirectory + "\\ErrorLOG\\" + "ErrorLog_" + System.DateTime.Now.ToString("yyyyMMM") + ".log", true);
                //    stream.WriteLine("-------- " + System.DateTime.Now.ToLongDateString() + " -- " + System.DateTime.Now.ToLongTimeString() + " -------------------------------------------------------");
                //    stream.Write("Exception Error : " + exEx.Message.ToString() + System.Environment.NewLine + "Stack Trace:" + exEx.StackTrace.ToString());

                //    stream.Close();
                //    stream.Dispose();
                //} 
            }
        }

    }
}
