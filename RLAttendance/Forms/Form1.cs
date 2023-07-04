using DevExpress.XtraEditors;
using GetACSEvent;
using Newtonsoft.Json;
using Riss.Devices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RLAttendance
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        private Device device;
        private DeviceConnection deviceConnection;
        private string apiBaseUrl = "";
        private HttpResponseMessage response;
        private HttpClient client;
        private string BaseURL = ConfigurationManager.AppSettings["BaseURL"].ToString();
        private string LocaionID = ConfigurationManager.AppSettings["LocationID"].ToString();
        private string MachineType = ConfigurationManager.AppSettings["MachineType"].ToString();
        private DataTable LocationMachine = new DataTable();
        private DataTable dtAttendance = new DataTable();

        public int m_UserID = -1;
        private string CsTemp = null;
        private int m_lLogNum = 0;
        private string MinorType = null;
        private string MajorType = null;
        public int m_lGetAcsEventHandle = -1;
        private Thread m_pDisplayListThread = null;
        public int m_iUserID = -1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                dateEditFrom.DateTime = DateTime.Now.Date;
                dateEditTo.DateTime = DateTime.Now.Date;
                dateEditViewFrom.DateTime = DateTime.Now.Date;
                dateEditViewTodate.DateTime = DateTime.Now.Date;

                if (Connect())
                {
                    lblConStatus.Text = "Connection Status - Connected";
                    lblConStatus.Appearance.ForeColor = Color.DodgerBlue;
                    LocationMachine.Clear();
                    dtAttendance.Columns.Add("EmployeeBadgeNo", typeof(string));
                    dtAttendance.Columns.Add("TransactionDate", typeof(DateTime));
                    dtAttendance.Columns.Add("TransactionTime", typeof(DateTime));
                    dtAttendance.Columns.Add("MachineEntryID", typeof(int));

                    CuurentLocationDetails();
                    DropDownLoad();
                }
                else
                {
                    lblConStatus.Text = "Connection Status - Not Connected";
                    lblConStatus.Appearance.ForeColor = Color.Maroon;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnDoanload_Click(object sender, EventArgs e)
        {
            try
            {
                dtAttendance.Clear();

                if (MachineType == "1")
                {
                    DownloadAttendanceRL(dateEditFrom.DateTime.Date, dateEditTo.DateTime.Date, 1);
                }
                else if (MachineType == "2")
                {
                    DownloadAttendanceZKT_New(dateEditFrom.DateTime, dateEditTo.DateTime, 2);
                }
                else if (MachineType == "3")
                {
                    DownloadAttendanceFingerTech(dateEditFrom.DateTime, dateEditTo.DateTime, 3);
                }
                else if (MachineType == "4")
                {
                    DownloadAttendanceHikVision(dateEditFrom.DateTime, dateEditTo.DateTime, 4);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ProcessAcsEvent(ref CHCNetSDK.NET_DVR_ACS_EVENT_CFG struCFG, ref bool flag)
        {
            try
            {
                ShowCardList(struCFG);
            }
            catch
            {
                MessageBox.Show("AddAcsEventToList Failed", "Error", MessageBoxButtons.OK);
                flag = false;
            }
        }

        public void ShowCardList(CHCNetSDK.NET_DVR_ACS_EVENT_CFG struCFG)
        {
            if (this.InvokeRequired)
            {
                Delegate delegateProc = new ShowCardListThread(AddAcsEventToList);
                this.BeginInvoke(delegateProc, struCFG);
            }
            else
            {
                AddAcsEventToList(struCFG);
            }
        }

        private string GetStrLogTime(ref CHCNetSDK.NET_DVR_TIME time)
        {
            string res = time.dwYear.ToString() + ":" + time.dwMonth.ToString() + ":"
                + time.dwDay.ToString() + ":" + time.dwHour.ToString() + ":" + time.dwMinute.ToString()
                + ":" + time.dwSecond.ToString();
            return res;
        }

        private string GetStrLogTimeNew(ref CHCNetSDK.NET_DVR_TIME time)
        {
            string res = time.dwYear.ToString() + "/" + time.dwMonth.ToString() + "/"
                + time.dwDay.ToString() + " " + time.dwHour.ToString() + ":" + time.dwMinute.ToString()
                + ":" + time.dwSecond.ToString();
            return res;
        }

        private string ProcessMajorType(ref uint dwMajor)
        {
            string res = null;
            switch (dwMajor)
            {
                case 1:
                    res = "Alarm";
                    break;

                case 2:
                    res = "Exception";
                    break;

                case 3:
                    res = "Operation";
                    break;

                case 5:
                    res = "Event";
                    break;

                default:
                    res = "Unknown";
                    break;
            }
            return res;
        }

        private void ProcessMinorType(ref CHCNetSDK.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            switch (struEventCfg.dwMajor)
            {
                case CHCNetSDK.MAJOR_ALARM:
                    AlarmMinorTypeMap(ref struEventCfg);
                    break;

                case CHCNetSDK.MAJOR_EXCEPTION:
                    ExceptionMinorTypeMap(ref struEventCfg);
                    break;

                case CHCNetSDK.MAJOR_OPERATION:
                    OperationMinorTypeMap(ref struEventCfg);
                    break;

                case CHCNetSDK.MAJOR_EVENT:
                    EventMinorTypeMap(ref struEventCfg);
                    break;

                default:
                    CsTemp = "Unknown";
                    break;
            }
        }

        private void OperationMinorTypeMap(ref CHCNetSDK.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            switch (struEventCfg.dwMinor)
            {
                case CHCNetSDK.MINOR_LOCAL_UPGRADE:
                    CsTemp = "LOCAL_UPGRADE";
                    break;

                case CHCNetSDK.MINOR_REMOTE_LOGIN:
                    CsTemp = "REMOTE_LOGIN";
                    break;

                case CHCNetSDK.MINOR_REMOTE_LOGOUT:
                    CsTemp = "REMOTE_LOGOUT";
                    break;

                case CHCNetSDK.MINOR_REMOTE_ARM:
                    CsTemp = "REMOTE_ARM";
                    break;

                case CHCNetSDK.MINOR_REMOTE_DISARM:
                    CsTemp = "REMOTE_DISARM";
                    break;

                case CHCNetSDK.MINOR_REMOTE_REBOOT:
                    CsTemp = "REMOTE_REBOOT";
                    break;

                case CHCNetSDK.MINOR_REMOTE_UPGRADE:
                    CsTemp = "REMOTE_UPGRADE";
                    break;

                case CHCNetSDK.MINOR_REMOTE_CFGFILE_OUTPUT:
                    CsTemp = "REMOTE_CFGFILE_OUTPUT";
                    break;

                case CHCNetSDK.MINOR_REMOTE_CFGFILE_INTPUT:
                    CsTemp = "REMOTE_CFGFILE_INTPUT";
                    break;

                case CHCNetSDK.MINOR_REMOTE_ALARMOUT_OPEN_MAN:
                    CsTemp = "REMOTE_ALARMOUT_OPEN_MAN";
                    break;

                case CHCNetSDK.MINOR_REMOTE_ALARMOUT_CLOSE_MAN:
                    CsTemp = "REMOTE_ALARMOUT_CLOSE_MAN";
                    break;

                case CHCNetSDK.MINOR_REMOTE_OPEN_DOOR:
                    CsTemp = "REMOTE_OPEN_DOOR";
                    break;

                case CHCNetSDK.MINOR_REMOTE_CLOSE_DOOR:
                    CsTemp = "REMOTE_CLOSE_DOOR";
                    break;

                case CHCNetSDK.MINOR_REMOTE_ALWAYS_OPEN:
                    CsTemp = "REMOTE_ALWAYS_OPEN";
                    break;

                case CHCNetSDK.MINOR_REMOTE_ALWAYS_CLOSE:
                    CsTemp = "REMOTE_ALWAYS_CLOSE";
                    break;

                case CHCNetSDK.MINOR_REMOTE_CHECK_TIME:
                    CsTemp = "REMOTE_CHECK_TIME";
                    break;

                case CHCNetSDK.MINOR_NTP_CHECK_TIME:
                    CsTemp = "NTP_CHECK_TIME";
                    break;

                case CHCNetSDK.MINOR_REMOTE_CLEAR_CARD:
                    CsTemp = "REMOTE_CLEAR_CARD";
                    break;

                case CHCNetSDK.MINOR_REMOTE_RESTORE_CFG:
                    CsTemp = "REMOTE_RESTORE_CFG";
                    break;

                case CHCNetSDK.MINOR_ALARMIN_ARM:
                    CsTemp = "ALARMIN_ARM";
                    break;

                case CHCNetSDK.MINOR_ALARMIN_DISARM:
                    CsTemp = "ALARMIN_DISARM";
                    break;

                case CHCNetSDK.MINOR_LOCAL_RESTORE_CFG:
                    CsTemp = "LOCAL_RESTORE_CFG";
                    break;

                case CHCNetSDK.MINOR_REMOTE_CAPTURE_PIC:
                    CsTemp = "REMOTE_CAPTURE_PIC";
                    break;

                case CHCNetSDK.MINOR_MOD_NET_REPORT_CFG:
                    CsTemp = "MOD_NET_REPORT_CFG";
                    break;

                case CHCNetSDK.MINOR_MOD_GPRS_REPORT_PARAM:
                    CsTemp = "MOD_GPRS_REPORT_PARAM";
                    break;

                case CHCNetSDK.MINOR_MOD_REPORT_GROUP_PARAM:
                    CsTemp = "MOD_REPORT_GROUP_PARAM";
                    break;

                case CHCNetSDK.MINOR_UNLOCK_PASSWORD_OPEN_DOOR:
                    CsTemp = "UNLOCK_PASSWORD_OPEN_DOOR";
                    break;

                case CHCNetSDK.MINOR_AUTO_RENUMBER:
                    CsTemp = "AUTO_RENUMBER";
                    break;

                case CHCNetSDK.MINOR_AUTO_COMPLEMENT_NUMBER:
                    CsTemp = "AUTO_COMPLEMENT_NUMBER";
                    break;

                case CHCNetSDK.MINOR_NORMAL_CFGFILE_INPUT:
                    CsTemp = "NORMAL_CFGFILE_INPUT";
                    break;

                case CHCNetSDK.MINOR_NORMAL_CFGFILE_OUTTPUT:
                    CsTemp = "NORMAL_CFGFILE_OUTTPUT";
                    break;

                case CHCNetSDK.MINOR_CARD_RIGHT_INPUT:
                    CsTemp = "CARD_RIGHT_INPUT";
                    break;

                case CHCNetSDK.MINOR_CARD_RIGHT_OUTTPUT:
                    CsTemp = "CARD_RIGHT_OUTTPUT";
                    break;

                case CHCNetSDK.MINOR_LOCAL_USB_UPGRADE:
                    CsTemp = "LOCAL_USB_UPGRADE";
                    break;

                case CHCNetSDK.MINOR_REMOTE_VISITOR_CALL_LADDER:
                    CsTemp = "REMOTE_VISITOR_CALL_LADDER";
                    break;

                case CHCNetSDK.MINOR_REMOTE_HOUSEHOLD_CALL_LADDER:
                    CsTemp = "REMOTE_HOUSEHOLD_CALL_LADDER";
                    break;

                case CHCNetSDK.MINOR_REMOTE_ACTUAL_GUARD:
                    CsTemp = "REMOTE_ACTUAL_GUARD";
                    break;

                case CHCNetSDK.MINOR_REMOTE_ACTUAL_UNGUARD:
                    CsTemp = "REMOTE_ACTUAL_UNGUARD";
                    break;

                case CHCNetSDK.MINOR_REMOTE_CONTROL_NOT_CODE_OPER_FAILED:
                    CsTemp = "REMOTE_CONTROL_NOT_CODE_OPER_FAILED";
                    break;

                case CHCNetSDK.MINOR_REMOTE_CONTROL_CLOSE_DOOR:
                    CsTemp = "REMOTE_CONTROL_CLOSE_DOOR";
                    break;

                case CHCNetSDK.MINOR_REMOTE_CONTROL_OPEN_DOOR:
                    CsTemp = "REMOTE_CONTROL_OPEN_DOOR";
                    break;

                case CHCNetSDK.MINOR_REMOTE_CONTROL_ALWAYS_OPEN_DOOR:
                    CsTemp = "REMOTE_CONTROL_ALWAYS_OPEN_DOOR";
                    break;

                default:
                    CsTemp = Convert.ToString(struEventCfg.dwMinor, 16);
                    break;
            }
        }

        private void EventMinorTypeMap(ref CHCNetSDK.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            switch (struEventCfg.dwMinor)
            {
                case CHCNetSDK.MINOR_LEGAL_CARD_PASS:
                    CsTemp = "LEGAL_CARD_PASS";
                    break;

                case CHCNetSDK.MINOR_CARD_AND_PSW_PASS:
                    CsTemp = "CARD_AND_PSW_PASS";
                    break;

                case CHCNetSDK.MINOR_CARD_AND_PSW_FAIL:
                    CsTemp = "CARD_AND_PSW_FAIL";
                    break;

                case CHCNetSDK.MINOR_CARD_AND_PSW_TIMEOUT:
                    CsTemp = "CARD_AND_PSW_TIMEOUT";
                    break;

                case CHCNetSDK.MINOR_CARD_AND_PSW_OVER_TIME:
                    CsTemp = "CARD_AND_PSW_OVER_TIME";
                    break;

                case CHCNetSDK.MINOR_CARD_NO_RIGHT:
                    CsTemp = "CARD_NO_RIGHT";
                    break;

                case CHCNetSDK.MINOR_CARD_INVALID_PERIOD:
                    CsTemp = "CARD_INVALID_PERIOD";
                    break;

                case CHCNetSDK.MINOR_CARD_OUT_OF_DATE:
                    CsTemp = "CARD_OUT_OF_DATE";
                    break;

                case CHCNetSDK.MINOR_INVALID_CARD:
                    CsTemp = "INVALID_CARD";
                    break;

                case CHCNetSDK.MINOR_ANTI_SNEAK_FAIL:
                    CsTemp = "ANTI_SNEAK_FAIL";
                    break;

                case CHCNetSDK.MINOR_INTERLOCK_DOOR_NOT_CLOSE:
                    CsTemp = "INTERLOCK_DOOR_NOT_CLOSE";
                    break;

                case CHCNetSDK.MINOR_NOT_BELONG_MULTI_GROUP:
                    CsTemp = "NOT_BELONG_MULTI_GROUP";
                    break;

                case CHCNetSDK.MINOR_INVALID_MULTI_VERIFY_PERIOD:
                    CsTemp = "INVALID_MULTI_VERIFY_PERIOD";
                    break;

                case CHCNetSDK.MINOR_MULTI_VERIFY_SUPER_RIGHT_FAIL:
                    CsTemp = "MULTI_VERIFY_SUPER_RIGHT_FAIL";
                    break;

                case CHCNetSDK.MINOR_MULTI_VERIFY_REMOTE_RIGHT_FAIL:
                    CsTemp = "MULTI_VERIFY_REMOTE_RIGHT_FAIL";
                    break;

                case CHCNetSDK.MINOR_MULTI_VERIFY_SUCCESS:
                    CsTemp = "MULTI_VERIFY_SUCCESS";
                    break;

                case CHCNetSDK.MINOR_LEADER_CARD_OPEN_BEGIN:
                    CsTemp = "LEADER_CARD_OPEN_BEGIN";
                    break;

                case CHCNetSDK.MINOR_LEADER_CARD_OPEN_END:
                    CsTemp = "LEADER_CARD_OPEN_END";
                    break;

                case CHCNetSDK.MINOR_ALWAYS_OPEN_BEGIN:
                    CsTemp = "ALWAYS_OPEN_BEGIN";
                    break;

                case CHCNetSDK.MINOR_ALWAYS_OPEN_END:
                    CsTemp = "ALWAYS_OPEN_END";
                    break;

                case CHCNetSDK.MINOR_LOCK_OPEN:
                    CsTemp = "LOCK_OPEN";
                    break;

                case CHCNetSDK.MINOR_LOCK_CLOSE:
                    CsTemp = "LOCK_CLOSE";
                    break;

                case CHCNetSDK.MINOR_DOOR_BUTTON_PRESS:
                    CsTemp = "DOOR_BUTTON_PRESS";
                    break;

                case CHCNetSDK.MINOR_DOOR_BUTTON_RELEASE:
                    CsTemp = "DOOR_BUTTON_RELEASE";
                    break;

                case CHCNetSDK.MINOR_DOOR_OPEN_NORMAL:
                    CsTemp = "DOOR_OPEN_NORMAL";
                    break;

                case CHCNetSDK.MINOR_DOOR_CLOSE_NORMAL:
                    CsTemp = "DOOR_CLOSE_NORMAL";
                    break;

                case CHCNetSDK.MINOR_DOOR_OPEN_ABNORMAL:
                    CsTemp = "DOOR_OPEN_ABNORMAL";
                    break;

                case CHCNetSDK.MINOR_DOOR_OPEN_TIMEOUT:
                    CsTemp = "DOOR_OPEN_TIMEOUT";
                    break;

                case CHCNetSDK.MINOR_ALARMOUT_ON:
                    CsTemp = "ALARMOUT_ON";
                    break;

                case CHCNetSDK.MINOR_ALARMOUT_OFF:
                    CsTemp = "ALARMOUT_OFF";
                    break;

                case CHCNetSDK.MINOR_ALWAYS_CLOSE_BEGIN:
                    CsTemp = "ALWAYS_CLOSE_BEGIN";
                    break;

                case CHCNetSDK.MINOR_ALWAYS_CLOSE_END:
                    CsTemp = "ALWAYS_CLOSE_END";
                    break;

                case CHCNetSDK.MINOR_MULTI_VERIFY_NEED_REMOTE_OPEN:
                    CsTemp = "MULTI_VERIFY_NEED_REMOTE_OPEN";
                    break;

                case CHCNetSDK.MINOR_MULTI_VERIFY_SUPERPASSWD_VERIFY_SUCCESS:
                    CsTemp = "MULTI_VERIFY_SUPERPASSWD_VERIFY_SUCCESS";
                    break;

                case CHCNetSDK.MINOR_MULTI_VERIFY_REPEAT_VERIFY:
                    CsTemp = "MULTI_VERIFY_REPEAT_VERIFY";
                    break;

                case CHCNetSDK.MINOR_MULTI_VERIFY_TIMEOUT:
                    CsTemp = "MULTI_VERIFY_TIMEOUT";
                    break;

                case CHCNetSDK.MINOR_DOORBELL_RINGING:
                    CsTemp = "DOORBELL_RINGING";
                    break;

                case CHCNetSDK.MINOR_FINGERPRINT_COMPARE_PASS:
                    CsTemp = "FINGERPRINT_COMPARE_PASS";
                    break;

                case CHCNetSDK.MINOR_FINGERPRINT_COMPARE_FAIL:
                    CsTemp = "FINGERPRINT_COMPARE_FAIL";
                    break;

                case CHCNetSDK.MINOR_CARD_FINGERPRINT_VERIFY_PASS:
                    CsTemp = "CARD_FINGERPRINT_VERIFY_PASS";
                    break;

                case CHCNetSDK.MINOR_CARD_FINGERPRINT_VERIFY_FAIL:
                    CsTemp = "CARD_FINGERPRINT_VERIFY_FAIL";
                    break;

                case CHCNetSDK.MINOR_CARD_FINGERPRINT_VERIFY_TIMEOUT:
                    CsTemp = "CARD_FINGERPRINT_VERIFY_TIMEOUT";
                    break;

                case CHCNetSDK.MINOR_CARD_FINGERPRINT_PASSWD_VERIFY_PASS:
                    CsTemp = "CARD_FINGERPRINT_PASSWD_VERIFY_PASS";
                    break;

                case CHCNetSDK.MINOR_CARD_FINGERPRINT_PASSWD_VERIFY_FAIL:
                    CsTemp = "CARD_FINGERPRINT_PASSWD_VERIFY_FAIL";
                    break;

                case CHCNetSDK.MINOR_CARD_FINGERPRINT_PASSWD_VERIFY_TIMEOUT:
                    CsTemp = "CARD_FINGERPRINT_PASSWD_VERIFY_TIMEOUT";
                    break;

                case CHCNetSDK.MINOR_FINGERPRINT_PASSWD_VERIFY_PASS:
                    CsTemp = "FINGERPRINT_PASSWD_VERIFY_PASS";
                    break;

                case CHCNetSDK.MINOR_FINGERPRINT_PASSWD_VERIFY_FAIL:
                    CsTemp = "FINGERPRINT_PASSWD_VERIFY_FAIL";
                    break;

                case CHCNetSDK.MINOR_FINGERPRINT_PASSWD_VERIFY_TIMEOUT:
                    CsTemp = "FINGERPRINT_PASSWD_VERIFY_TIMEOUT";
                    break;

                case CHCNetSDK.MINOR_FINGERPRINT_INEXISTENCE:
                    CsTemp = "FINGERPRINT_INEXISTENCE";
                    break;

                case CHCNetSDK.MINOR_CARD_PLATFORM_VERIFY:
                    CsTemp = "CARD_PLATFORM_VERIFY";
                    break;

                case CHCNetSDK.MINOR_MAC_DETECT:
                    CsTemp = "MINOR_MAC_DETECT";
                    break;

                case CHCNetSDK.MINOR_LEGAL_MESSAGE:
                    CsTemp = "MINOR_LEGAL_MESSAGE";
                    break;

                case CHCNetSDK.MINOR_ILLEGAL_MESSAGE:
                    CsTemp = "MINOR_ILLEGAL_MESSAGE";
                    break;

                case CHCNetSDK.MINOR_DOOR_OPEN_OR_DORMANT_FAIL:
                    CsTemp = "DOOR_OPEN_OR_DORMANT_FAIL";
                    break;

                case CHCNetSDK.MINOR_AUTH_PLAN_DORMANT_FAIL:
                    CsTemp = "AUTH_PLAN_DORMANT_FAIL";
                    break;

                case CHCNetSDK.MINOR_CARD_ENCRYPT_VERIFY_FAIL:
                    CsTemp = "CARD_ENCRYPT_VERIFY_FAIL";
                    break;

                case CHCNetSDK.MINOR_SUBMARINEBACK_REPLY_FAIL:
                    CsTemp = "SUBMARINEBACK_REPLY_FAIL";
                    break;

                case CHCNetSDK.MINOR_DOOR_OPEN_OR_DORMANT_OPEN_FAIL:
                    CsTemp = "DOOR_OPEN_OR_DORMANT_OPEN_FAIL";
                    break;

                case CHCNetSDK.MINOR_DOOR_OPEN_OR_DORMANT_LINKAGE_OPEN_FAIL:
                    CsTemp = "DOOR_OPEN_OR_DORMANT_LINKAGE_OPEN_FAIL";
                    break;

                case CHCNetSDK.MINOR_TRAILING:
                    CsTemp = "TRAILING";
                    break;

                case CHCNetSDK.MINOR_REVERSE_ACCESS:
                    CsTemp = "REVERSE_ACCESS";
                    break;

                case CHCNetSDK.MINOR_FORCE_ACCESS:
                    CsTemp = "FORCE_ACCESS";
                    break;

                case CHCNetSDK.MINOR_CLIMBING_OVER_GATE:
                    CsTemp = "CLIMBING_OVER_GATE";
                    break;

                case CHCNetSDK.MINOR_PASSING_TIMEOUT:
                    CsTemp = "PASSING_TIMEOUT";
                    break;

                case CHCNetSDK.MINOR_INTRUSION_ALARM:
                    CsTemp = "INTRUSION_ALARM";
                    break;

                case CHCNetSDK.MINOR_FREE_GATE_PASS_NOT_AUTH:
                    CsTemp = "FREE_GATE_PASS_NOT_AUTH";
                    break;

                case CHCNetSDK.MINOR_DROP_ARM_BLOCK:
                    CsTemp = "DROP_ARM_BLOCK";
                    break;

                case CHCNetSDK.MINOR_DROP_ARM_BLOCK_RESUME:
                    CsTemp = "DROP_ARM_BLOCK_RESUME";
                    break;

                case CHCNetSDK.MINOR_LOCAL_FACE_MODELING_FAIL:
                    CsTemp = "LOCAL_FACE_MODELING_FAIL";
                    break;

                case CHCNetSDK.MINOR_STAY_EVENT:
                    CsTemp = "STAY_EVENT";
                    break;

                case CHCNetSDK.MINOR_PASSWORD_MISMATCH:
                    CsTemp = "PASSWORD_MISMATCH";
                    break;

                case CHCNetSDK.MINOR_EMPLOYEE_NO_NOT_EXIST:
                    CsTemp = "EMPLOYEE_NO_NOT_EXIST";
                    break;

                case CHCNetSDK.MINOR_COMBINED_VERIFY_PASS:
                    CsTemp = "COMBINED_VERIFY_PASS";
                    break;

                case CHCNetSDK.MINOR_COMBINED_VERIFY_TIMEOUT:
                    CsTemp = "COMBINED_VERIFY_TIMEOUT";
                    break;

                case CHCNetSDK.MINOR_VERIFY_MODE_MISMATCH:
                    CsTemp = "VERIFY_MODE_MISMATCH";
                    break;

                default:
                    CsTemp = Convert.ToString(struEventCfg.dwMinor, 16);
                    break;
            }
        }

        private void ExceptionMinorTypeMap(ref CHCNetSDK.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            switch (struEventCfg.dwMinor)
            {
                case CHCNetSDK.MINOR_NET_BROKEN:
                    CsTemp = "NET_BROKEN";
                    break;

                case CHCNetSDK.MINOR_RS485_DEVICE_ABNORMAL:
                    CsTemp = "RS485_DEVICE_ABNORMAL";
                    break;

                case CHCNetSDK.MINOR_RS485_DEVICE_REVERT:
                    CsTemp = "RS485_DEVICE_REVERT";
                    break;

                case CHCNetSDK.MINOR_DEV_POWER_ON:
                    CsTemp = "DEV_POWER_ON";
                    break;

                case CHCNetSDK.MINOR_DEV_POWER_OFF:
                    CsTemp = "DEV_POWER_OFF";
                    break;

                case CHCNetSDK.MINOR_WATCH_DOG_RESET:
                    CsTemp = "WATCH_DOG_RESET";
                    break;

                case CHCNetSDK.MINOR_LOW_BATTERY:
                    CsTemp = "LOW_BATTERY";
                    break;

                case CHCNetSDK.MINOR_BATTERY_RESUME:
                    CsTemp = "BATTERY_RESUME";
                    break;

                case CHCNetSDK.MINOR_AC_OFF:
                    CsTemp = "AC_OFF";
                    break;

                case CHCNetSDK.MINOR_AC_RESUME:
                    CsTemp = "AC_RESUME";
                    break;

                case CHCNetSDK.MINOR_NET_RESUME:
                    CsTemp = "NET_RESUME";
                    break;

                case CHCNetSDK.MINOR_FLASH_ABNORMAL:
                    CsTemp = "FLASH_ABNORMAL";
                    break;

                case CHCNetSDK.MINOR_CARD_READER_OFFLINE:
                    CsTemp = "CARD_READER_OFFLINE";
                    break;

                case CHCNetSDK.MINOR_CARD_READER_RESUME:
                    CsTemp = "CARD_READER_RESUME";
                    break;

                case CHCNetSDK.MINOR_INDICATOR_LIGHT_OFF:
                    CsTemp = "INDICATOR_LIGHT_OFF";
                    break;

                case CHCNetSDK.MINOR_INDICATOR_LIGHT_RESUME:
                    CsTemp = "INDICATOR_LIGHT_RESUME";
                    break;

                case CHCNetSDK.MINOR_CHANNEL_CONTROLLER_OFF:
                    CsTemp = "CHANNEL_CONTROLLER_OFF";
                    break;

                case CHCNetSDK.MINOR_CHANNEL_CONTROLLER_RESUME:
                    CsTemp = "CHANNEL_CONTROLLER_RESUME";
                    break;

                case CHCNetSDK.MINOR_SECURITY_MODULE_OFF:
                    CsTemp = "SECURITY_MODULE_OFF";
                    break;

                case CHCNetSDK.MINOR_SECURITY_MODULE_RESUME:
                    CsTemp = "SECURITY_MODULE_RESUME";
                    break;

                case CHCNetSDK.MINOR_BATTERY_ELECTRIC_LOW:
                    CsTemp = "BATTERY_ELECTRIC_LOW";
                    break;

                case CHCNetSDK.MINOR_BATTERY_ELECTRIC_RESUME:
                    CsTemp = "BATTERY_ELECTRIC_RESUME";
                    break;

                case CHCNetSDK.MINOR_LOCAL_CONTROL_NET_BROKEN:
                    CsTemp = "LOCAL_CONTROL_NET_BROKEN";
                    break;

                case CHCNetSDK.MINOR_LOCAL_CONTROL_NET_RSUME:
                    CsTemp = "LOCAL_CONTROL_NET_RSUME";
                    break;

                case CHCNetSDK.MINOR_MASTER_RS485_LOOPNODE_BROKEN:
                    CsTemp = "MASTER_RS485_LOOPNODE_BROKEN";
                    break;

                case CHCNetSDK.MINOR_MASTER_RS485_LOOPNODE_RESUME:
                    CsTemp = "MASTER_RS485_LOOPNODE_RESUME";
                    break;

                case CHCNetSDK.MINOR_LOCAL_CONTROL_OFFLINE:
                    CsTemp = "LOCAL_CONTROL_OFFLINE";
                    break;

                case CHCNetSDK.MINOR_LOCAL_CONTROL_RESUME:
                    CsTemp = "LOCAL_CONTROL_RESUME";
                    break;

                case CHCNetSDK.MINOR_LOCAL_DOWNSIDE_RS485_LOOPNODE_BROKEN:
                    CsTemp = "LOCAL_DOWNSIDE_RS485_LOOPNODE_BROKEN";
                    break;

                case CHCNetSDK.MINOR_LOCAL_DOWNSIDE_RS485_LOOPNODE_RESUME:
                    CsTemp = "LOCAL_DOWNSIDE_RS485_LOOPNODE_RESUME";
                    break;

                case CHCNetSDK.MINOR_DISTRACT_CONTROLLER_ONLINE:
                    CsTemp = "DISTRACT_CONTROLLER_ONLINE";
                    break;

                case CHCNetSDK.MINOR_DISTRACT_CONTROLLER_OFFLINE:
                    CsTemp = "DISTRACT_CONTROLLER_OFFLINE";
                    break;

                case CHCNetSDK.MINOR_ID_CARD_READER_NOT_CONNECT:
                    CsTemp = "ID_CARD_READER_NOT_CONNECT";
                    break;

                case CHCNetSDK.MINOR_ID_CARD_READER_RESUME:
                    CsTemp = "ID_CARD_READER_RESUME";
                    break;

                case CHCNetSDK.MINOR_FINGER_PRINT_MODULE_NOT_CONNECT:
                    CsTemp = "FINGER_PRINT_MODULE_NOT_CONNECT";
                    break;

                case CHCNetSDK.MINOR_FINGER_PRINT_MODULE_RESUME:
                    CsTemp = "FINGER_PRINT_MODULE_RESUME";
                    break;

                case CHCNetSDK.MINOR_CAMERA_NOT_CONNECT:
                    CsTemp = "CAMERA_NOT_CONNECT";
                    break;

                case CHCNetSDK.MINOR_CAMERA_RESUME:
                    CsTemp = "CAMERA_RESUME";
                    break;

                case CHCNetSDK.MINOR_COM_NOT_CONNECT:
                    CsTemp = "COM_NOT_CONNECT";
                    break;

                case CHCNetSDK.MINOR_COM_RESUME:
                    CsTemp = "COM_RESUME";
                    break;

                case CHCNetSDK.MINOR_DEVICE_NOT_AUTHORIZE:
                    CsTemp = "DEVICE_NOT_AUTHORIZE";
                    break;

                case CHCNetSDK.MINOR_PEOPLE_AND_ID_CARD_DEVICE_ONLINE:
                    CsTemp = "PEOPLE_AND_ID_CARD_DEVICE_ONLINE";
                    break;

                case CHCNetSDK.MINOR_PEOPLE_AND_ID_CARD_DEVICE_OFFLINE:
                    CsTemp = "PEOPLE_AND_ID_CARD_DEVICE_OFFLINE";
                    break;

                case CHCNetSDK.MINOR_LOCAL_LOGIN_LOCK:
                    CsTemp = "LOCAL_LOGIN_LOCK";
                    break;

                case CHCNetSDK.MINOR_LOCAL_LOGIN_UNLOCK:
                    CsTemp = "LOCAL_LOGIN_UNLOCK";
                    break;

                case CHCNetSDK.MINOR_SUBMARINEBACK_COMM_BREAK:
                    CsTemp = "SUBMARINEBACK_COMM_BREAK";
                    break;

                case CHCNetSDK.MINOR_SUBMARINEBACK_COMM_RESUME:
                    CsTemp = "SUBMARINEBACK_COMM_RESUME";
                    break;

                case CHCNetSDK.MINOR_MOTOR_SENSOR_EXCEPTION:
                    CsTemp = "MOTOR_SENSOR_EXCEPTION";
                    break;

                case CHCNetSDK.MINOR_CAN_BUS_EXCEPTION:
                    CsTemp = "CAN_BUS_EXCEPTION";
                    break;

                case CHCNetSDK.MINOR_CAN_BUS_RESUME:
                    CsTemp = "CAN_BUS_RESUME";
                    break;

                case CHCNetSDK.MINOR_GATE_TEMPERATURE_OVERRUN:
                    CsTemp = "GATE_TEMPERATURE_OVERRUN";
                    break;

                case CHCNetSDK.MINOR_IR_EMITTER_EXCEPTION:
                    CsTemp = "IR_EMITTER_EXCEPTION";
                    break;

                case CHCNetSDK.MINOR_IR_EMITTER_RESUME:
                    CsTemp = "IR_EMITTER_RESUME";
                    break;

                case CHCNetSDK.MINOR_LAMP_BOARD_COMM_EXCEPTION:
                    CsTemp = "LAMP_BOARD_COMM_EXCEPTION";
                    break;

                case CHCNetSDK.MINOR_LAMP_BOARD_COMM_RESUME:
                    CsTemp = "LAMP_BOARD_COMM_RESUME";
                    break;

                case CHCNetSDK.MINOR_IR_ADAPTOR_COMM_EXCEPTION:
                    CsTemp = "IR_ADAPTOR_COMM_EXCEPTION";
                    break;

                case CHCNetSDK.MINOR_IR_ADAPTOR_COMM_RESUME:
                    CsTemp = "IR_ADAPTOR_COMM_RESUME";
                    break;

                default:
                    CsTemp = Convert.ToString(struEventCfg.dwMinor, 16);
                    break;
            }
        }

        private void AlarmMinorTypeMap(ref CHCNetSDK.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            switch (struEventCfg.dwMinor)
            {
                case CHCNetSDK.MINOR_ALARMIN_SHORT_CIRCUIT:
                    CsTemp = "ALARMIN_SHORT_CIRCUIT";
                    break;

                case CHCNetSDK.MINOR_ALARMIN_BROKEN_CIRCUIT:
                    CsTemp = "ALARMIN_BROKEN_CIRCUIT";
                    break;

                case CHCNetSDK.MINOR_ALARMIN_EXCEPTION:
                    CsTemp = "ALARMIN_EXCEPTION";
                    break;

                case CHCNetSDK.MINOR_ALARMIN_RESUME:
                    CsTemp = "ALARMIN_RESUME";
                    break;

                case CHCNetSDK.MINOR_HOST_DESMANTLE_ALARM:
                    CsTemp = "HOST_DESMANTLE_ALARM";
                    break;

                case CHCNetSDK.MINOR_HOST_DESMANTLE_RESUME:
                    CsTemp = "HOST_DESMANTLE_RESUME";
                    break;

                case CHCNetSDK.MINOR_CARD_READER_DESMANTLE_ALARM:
                    CsTemp = "CARD_READER_DESMANTLE_ALARM";
                    break;

                case CHCNetSDK.MINOR_CARD_READER_DESMANTLE_RESUME:
                    CsTemp = "CARD_READER_DESMANTLE_RESUME";
                    break;

                case CHCNetSDK.MINOR_CASE_SENSOR_ALARM:
                    CsTemp = "CASE_SENSOR_ALARM";
                    break;

                case CHCNetSDK.MINOR_CASE_SENSOR_RESUME:
                    CsTemp = "CASE_SENSOR_RESUME";
                    break;

                case CHCNetSDK.MINOR_STRESS_ALARM:
                    CsTemp = "STRESS_ALARM";
                    break;

                case CHCNetSDK.MINOR_OFFLINE_ECENT_NEARLY_FULL:
                    CsTemp = "OFFLINE_ECENT_NEARLY_FULL";
                    break;

                case CHCNetSDK.MINOR_CARD_MAX_AUTHENTICATE_FAIL:
                    CsTemp = "CARD_MAX_AUTHENTICATE_FAIL";
                    break;

                case CHCNetSDK.MINOR_SD_CARD_FULL:
                    CsTemp = "MINOR_SD_CARD_FULL";
                    break;

                case CHCNetSDK.MINOR_LINKAGE_CAPTURE_PIC:
                    CsTemp = "MINOR_LINKAGE_CAPTURE_PIC";
                    break;

                case CHCNetSDK.MINOR_SECURITY_MODULE_DESMANTLE_ALARM:
                    CsTemp = "MINOR_SECURITY_MODULE_DESMANTLE_ALARM";
                    break;

                case CHCNetSDK.MINOR_SECURITY_MODULE_DESMANTLE_RESUME:
                    CsTemp = "MINOR_SECURITY_MODULE_DESMANTLE_RESUME";
                    break;

                case CHCNetSDK.MINOR_POS_START_ALARM:
                    CsTemp = "MINOR_POS_START_ALARM";
                    break;

                case CHCNetSDK.MINOR_POS_END_ALARM:
                    CsTemp = "MINOR_POS_END_ALARM";
                    break;

                case CHCNetSDK.MINOR_FACE_IMAGE_QUALITY_LOW:
                    CsTemp = "MINOR_FACE_IMAGE_QUALITY_LOW";
                    break;

                case CHCNetSDK.MINOR_FINGE_RPRINT_QUALITY_LOW:
                    CsTemp = "MINOR_FINGE_RPRINT_QUALITY_LOW";
                    break;

                case CHCNetSDK.MINOR_FIRE_IMPORT_SHORT_CIRCUIT:
                    CsTemp = "MINOR_FIRE_IMPORT_SHORT_CIRCUIT";
                    break;

                case CHCNetSDK.MINOR_FIRE_IMPORT_BROKEN_CIRCUIT:
                    CsTemp = "MINOR_FIRE_IMPORT_BROKEN_CIRCUIT";
                    break;

                case CHCNetSDK.MINOR_FIRE_IMPORT_RESUME:
                    CsTemp = "MINOR_FIRE_IMPORT_RESUME";
                    break;

                case CHCNetSDK.MINOR_FIRE_BUTTON_TRIGGER:
                    CsTemp = "FIRE_BUTTON_TRIGGER";
                    break;

                case CHCNetSDK.MINOR_FIRE_BUTTON_RESUME:
                    CsTemp = "FIRE_BUTTON_RESUME";
                    break;

                case CHCNetSDK.MINOR_MAINTENANCE_BUTTON_TRIGGER:
                    CsTemp = "MAINTENANCE_BUTTON_TRIGGER";
                    break;

                case CHCNetSDK.MINOR_MAINTENANCE_BUTTON_RESUME:
                    CsTemp = "MAINTENANCE_BUTTON_RESUME";
                    break;

                case CHCNetSDK.MINOR_EMERGENCY_BUTTON_TRIGGER:
                    CsTemp = "EMERGENCY_BUTTON_TRIGGER";
                    break;

                case CHCNetSDK.MINOR_EMERGENCY_BUTTON_RESUME:
                    CsTemp = "EMERGENCY_BUTTON_RESUME";
                    break;

                case CHCNetSDK.MINOR_DISTRACT_CONTROLLER_ALARM:
                    CsTemp = "DISTRACT_CONTROLLER_ALARM";
                    break;

                case CHCNetSDK.MINOR_DISTRACT_CONTROLLER_RESUME:
                    CsTemp = "DISTRACT_CONTROLLER_RESUME";
                    break;

                case CHCNetSDK.MINOR_CHANNEL_CONTROLLER_DESMANTLE_ALARM:
                    CsTemp = "MINOR_CHANNEL_CONTROLLER_DESMANTLE_ALARM";
                    break;

                case CHCNetSDK.MINOR_CHANNEL_CONTROLLER_DESMANTLE_RESUME:
                    CsTemp = "MINOR_CHANNEL_CONTROLLER_DESMANTLE_RESUME";
                    break;

                case CHCNetSDK.MINOR_CHANNEL_CONTROLLER_FIRE_IMPORT_ALARM:
                    CsTemp = "MINOR_CHANNEL_CONTROLLER_FIRE_IMPORT_ALARM";
                    break;

                case CHCNetSDK.MINOR_CHANNEL_CONTROLLER_FIRE_IMPORT_RESUME:
                    CsTemp = "MINOR_CHANNEL_CONTROLLER_FIRE_IMPORT_RESUME";
                    break;

                case CHCNetSDK.MINOR_LEGAL_EVENT_NEARLY_FULL:
                    CsTemp = "MINOR_LEGAL_EVENT_NEARLY_FULL";
                    break;

                default:
                    CsTemp = Convert.ToString(struEventCfg.dwMinor, 16);
                    break;
            }
        }

        private void AddAcsEventToList(CHCNetSDK.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            DataRow dr = null;
            //dr = dt.NewRow();
            //this.listViewEvent.BeginUpdate();
            ListViewItem Item = new ListViewItem();
            Item.Text = (++m_lLogNum).ToString();

            string LogTime = GetStrLogTimeNew(ref struEventCfg.struTime);

            Item.SubItems.Add(LogTime);

            string Major = ProcessMajorType(ref struEventCfg.dwMajor);
            Item.SubItems.Add(Major);

            ProcessMinorType(ref struEventCfg);
            Item.SubItems.Add(CsTemp);

            string Minor = CsTemp;

            CsTemp = System.Text.Encoding.UTF8.GetString(struEventCfg.struAcsEventInfo.byCardNo);
            Item.SubItems.Add(CsTemp);

            CardTypeMap(ref struEventCfg);
            Item.SubItems.Add(CsTemp);

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.byWhiteListNo.ToString());//WhiteList

            ProcessReportChannel(ref struEventCfg);
            Item.SubItems.Add(CsTemp);

            ProcessCardReader(ref struEventCfg);
            Item.SubItems.Add(CsTemp);

            CsTemp = struEventCfg.struAcsEventInfo.dwCardReaderNo.ToString();
            Item.SubItems.Add(CsTemp);

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.dwDoorNo.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.dwVerifyNo.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.dwAlarmInNo.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.dwAlarmOutNo.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.dwCaseSensorNo.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.dwRs485No.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.dwMultiCardGroupNo.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.wAccessChannel.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.byDeviceNo.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.dwEmployeeNo.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.byDistractControlNo.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.wLocalControllerID.ToString());

            ProcessInternatAccess(ref struEventCfg);
            Item.SubItems.Add(CsTemp);

            ProcessByType(ref struEventCfg);
            Item.SubItems.Add(CsTemp);

            ProcessMacAdd(ref struEventCfg);
            Item.SubItems.Add(CsTemp);

            ProcessSwipeCard(ref struEventCfg);
            Item.SubItems.Add(CsTemp);

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.dwSerialNo.ToString());

            Item.SubItems.Add("0"/*struEventCfg.struAcsEventInfo.byChannelControllerID.ToString()*/);

            Item.SubItems.Add("0"/*struEventCfg.struAcsEventInfo.byChannelControllerLampID.ToString()*/);

            Item.SubItems.Add("0"/*struEventCfg.struAcsEventInfo.byChannelControllerIRAdaptorID.ToString()*/);

            Item.SubItems.Add("0"/*struEventCfg.struAcsEventInfo.byChannelControllerIREmitterID.ToString()*/);

            //if (struEventCfg.wInductiveEventType < (ushort)GetAcsEventType.NumOfInductiveEvent())
            //{
            Item.SubItems.Add("0"/*GetAcsEventType.FindKeyOfInductive(struEventCfg.wInductiveEventType)*/);
            //}
            //else
            //{
            //    Item.SubItems.Add("Invalid");
            //}

            Item.SubItems.Add("0");//RecordChannelNum

            //ProcessbyUserType(ref struEventCfg);
            Item.SubItems.Add("0");

            //ProcessVerifyMode(ref struEventCfg);
            Item.SubItems.Add("0");

            //CsTemp = System.Text.Encoding.UTF8.GetString(struEventCfg.struAcsEventInfo.byEmployeeNo);
            Item.SubItems.Add("0");

            CsTemp = null;
            //this.listViewEvent.Items.Add(Item);

            dr["EmployeeBadgeNo"] = struEventCfg.struAcsEventInfo.dwEmployeeNo.ToString("000000");
            dr["TransactionDate"] = Convert.ToDateTime(LogTime);
            dr["TransactionTime"] = Convert.ToDateTime(LogTime);
            dr["MachineEntryID"] = struEventCfg.struAcsEventInfo.byDeviceNo.ToString();

            //dr["Device"] = struEventCfg.struAcsEventInfo.byDeviceNo.ToString();
            //dr["EmployeeBadgeNo"] = struEventCfg.struAcsEventInfo.dwEmployeeNo.ToString("000000");
            ////dr["LogTime"] = LogTime;
            //dr["LogTime"] = Convert.ToDateTime(LogTime);
            //dr["SerialNo"] = struEventCfg.struAcsEventInfo.dwSerialNo.ToString();
            //dr["Major"] = Major;
            //dr["Minor"] = Minor;
            //dr["EmployeeNoString"] = struEventCfg.struAcsEventInfo.dwEmployeeNo.ToString("00000");
            //dt.Rows.Add(dr);

            //this.listViewEvent.EndUpdate();
        }

        private void CardTypeMap(ref CHCNetSDK.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            switch (struEventCfg.struAcsEventInfo.byCardType)
            {
                case 1:
                    CsTemp = "Ordinary Card";
                    break;

                case 2:
                    CsTemp = "Disabled Card";
                    break;

                case 3:
                    CsTemp = "Block List Card";
                    break;

                case 4:
                    CsTemp = "Patrol Card";
                    break;

                case 5:
                    CsTemp = "Stress Card";
                    break;

                case 6:
                    CsTemp = "Super Card";
                    break;

                case 7:
                    CsTemp = "Guest Card";
                    break;

                case 8:
                    CsTemp = "Release Card";
                    break;

                default:
                    CsTemp = "No effect";
                    break;
            }
        }

        private void ProcessReportChannel(ref CHCNetSDK.NET_DVR_ACS_EVENT_CFG EventCfg)
        {
            switch (EventCfg.struAcsEventInfo.byReportChannel)
            {
                case 1:
                    CsTemp = "Upload";
                    break;

                case 2:
                    CsTemp = "Center 1 Upload";
                    break;

                case 3:
                    CsTemp = "Center 2 Upload";
                    break;

                default:
                    CsTemp = "No effect";
                    break;
            }
        }

        private void ProcessCardReader(ref CHCNetSDK.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            switch (struEventCfg.struAcsEventInfo.byCardReaderKind)
            {
                case 1:
                    CsTemp = "IC Reader";
                    break;

                case 2:
                    CsTemp = "Certificate Reader";
                    break;

                case 3:
                    CsTemp = "Two-dimension Reader";
                    break;

                case 4:
                    CsTemp = "Finger Print Head";
                    break;

                default:
                    CsTemp = "No effect";
                    break;
            }
        }

        private void ProcessInternatAccess(ref CHCNetSDK.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            switch (struEventCfg.struAcsEventInfo.byInternetAccess)
            {
                case 1:
                    CsTemp = "Up Network Port 1";
                    break;

                case 2:
                    CsTemp = "Up Network Port 2";
                    break;

                case 3:
                    CsTemp = "Down Network Port 1";
                    break;

                default:
                    CsTemp = "No effect";
                    break;
            }
        }

        private void ProcessByType(ref CHCNetSDK.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            switch (struEventCfg.struAcsEventInfo.byType)
            {
                case 0:
                    CsTemp = "Instant Zone";
                    break;

                case 1:
                    CsTemp = "24 Hour Zone";
                    break;

                case 2:
                    CsTemp = "Delay Zone";
                    break;

                case 3:
                    CsTemp = "Internal Zone";
                    break;

                case 4:
                    CsTemp = "Key Zone";
                    break;

                case 5:
                    CsTemp = "Fire Zone";
                    break;

                case 6:
                    CsTemp = "Perimeter Zone";
                    break;

                case 7:
                    CsTemp = "24 Hour Silent Zone";
                    break;

                case 8:
                    CsTemp = "24 Hour Auxiliary Zone";
                    break;

                case 9:
                    CsTemp = "24 Hour Vibration Zone";
                    break;

                case 10:
                    CsTemp = "Acs Emergency Open Zone";
                    break;

                case 11:
                    CsTemp = "Acs Emergency Close Zone";
                    break;

                default:
                    CsTemp = "No Effect";
                    break;
            }
        }

        private void ProcessMacAdd(ref CHCNetSDK.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            if (struEventCfg.struAcsEventInfo.byMACAddr[0] != 0)
            {
                CsTemp = struEventCfg.struAcsEventInfo.byMACAddr[0].ToString() + ":" +
                    struEventCfg.struAcsEventInfo.byMACAddr[1].ToString() + ":" +
                    struEventCfg.struAcsEventInfo.byMACAddr[2].ToString() + ":" +
                    struEventCfg.struAcsEventInfo.byMACAddr[3].ToString() + ":" +
                    struEventCfg.struAcsEventInfo.byMACAddr[4].ToString() + ":" +
                    struEventCfg.struAcsEventInfo.byMACAddr[5].ToString();
            }
            else
            {
                CsTemp = "No Effect";
            }
        }

        private void ProcessSwipeCard(ref CHCNetSDK.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            if (struEventCfg.struAcsEventInfo.bySwipeCardType == 1)
            {
                CsTemp = "QR Code";
            }
            else
            {
                CsTemp = "No effect";
            }
        }

        public delegate void ShowCardListThread(CHCNetSDK.NET_DVR_ACS_EVENT_CFG struCFG);

        public void ProcessEvent()
        {
            int dwStatus = 0;
            Boolean Flag = true;
            CHCNetSDK.NET_DVR_ACS_EVENT_CFG struCFG = new CHCNetSDK.NET_DVR_ACS_EVENT_CFG();
            struCFG.dwSize = (uint)Marshal.SizeOf(struCFG);
            int dwOutBuffSize = (int)struCFG.dwSize;
            struCFG.init();
            while (Flag)
            {
                dwStatus = CHCNetSDK.NET_DVR_GetNextRemoteConfig(m_lGetAcsEventHandle, ref struCFG, dwOutBuffSize);
                switch (dwStatus)
                {
                    case CHCNetSDK.NET_SDK_GET_NEXT_STATUS_SUCCESS://成功读取到数据，处理完本次数据后需调用next
                        ProcessAcsEvent(ref struCFG, ref Flag);
                        break;

                    case CHCNetSDK.NET_SDK_GET_NEXT_STATUS_NEED_WAIT:
                        Thread.Sleep(200);
                        break;

                    case CHCNetSDK.NET_SDK_GET_NEXT_STATUS_FAILED:
                        CHCNetSDK.NET_DVR_StopRemoteConfig(m_lGetAcsEventHandle);
                        MessageBox.Show("NET_SDK_GET_NEXT_STATUS_FAILED" + CHCNetSDK.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                        Flag = false;
                        break;

                    case CHCNetSDK.NET_SDK_GET_NEXT_STATUS_FINISH:
                        CHCNetSDK.NET_DVR_StopRemoteConfig(m_lGetAcsEventHandle);
                        Flag = false;
                        break;

                    default:
                        MessageBox.Show("NET_SDK_GET_NEXT_STATUS_UNKOWN" + CHCNetSDK.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                        Flag = false;
                        CHCNetSDK.NET_DVR_StopRemoteConfig(m_lGetAcsEventHandle);
                        break;
                }
            }
        }

        private void DownloadAttendanceHikVision(DateTime FromDate, DateTime ToDate, int machineID)
        {
            if (lookUpEditLoc.EditValue == null)
            {
                MessageBox.Show("Please Select a Machine");
            }
            else
            {
                string _ipAddress = string.Empty;
                int _portNo = 0;
                string _sipAddress = string.Empty;
                int _sportNo = 0;
                string _sdk = "";
                string _passWord = "";
                string _userName = "";

                _ipAddress = getIPAddress((int)lookUpEditLoc.EditValue);
                _portNo = getPortNo((int)lookUpEditLoc.EditValue);
                _sipAddress = getSIPAddress((int)lookUpEditLoc.EditValue);
                _sportNo = getSPortNo((int)lookUpEditLoc.EditValue);
                _sdk = getSDK((int)lookUpEditLoc.EditValue);
                _passWord = getUsername((int)lookUpEditLoc.EditValue);
                _userName = getPassword((int)lookUpEditLoc.EditValue);

                CHCNetSDK.NET_DVR_USER_LOGIN_INFO struLoginInfo = new CHCNetSDK.NET_DVR_USER_LOGIN_INFO();
                CHCNetSDK.NET_DVR_DEVICEINFO_V40 struDeviceInfoV40 = new CHCNetSDK.NET_DVR_DEVICEINFO_V40();
                struDeviceInfoV40.struDeviceV30.sSerialNumber = new byte[CHCNetSDK.SERIALNO_LEN];

                struLoginInfo.sDeviceAddress = _ipAddress;
                struLoginInfo.sDeviceAddress = _sipAddress;
                struLoginInfo.sUserName = _userName;
                struLoginInfo.sPassword = _passWord;
                ushort.TryParse(Convert.ToInt32(_portNo).ToString(), out struLoginInfo.wPort);

                if (_ipAddress != string.Empty || _sipAddress != string.Empty)
                {
                    if (_userName != "" && _passWord != "")
                    {
                        int lUserID = -1;
                        lUserID = CHCNetSDK.NET_DVR_Login_V40(ref struLoginInfo, ref struDeviceInfoV40);

                        if (lUserID >= 0)
                        {
                            m_iUserID = lUserID;
                            m_UserID = lUserID;
                            dtAttendance.Clear();
                            //listViewEvent.Items.Clear();

                            CHCNetSDK.NET_DVR_ACS_EVENT_COND struCond = new CHCNetSDK.NET_DVR_ACS_EVENT_COND();
                            struCond.Init();
                            struCond.dwSize = (uint)Marshal.SizeOf(struCond);

                            MajorType = "Event";
                            struCond.dwMajor = GetAcsEventType.ReturnMajorTypeValue(ref MajorType);

                            MinorType = "ALL";
                            struCond.dwMinor = GetAcsEventType.ReturnMinorTypeValue(ref MinorType);

                            struCond.struStartTime.dwYear = dateEditFrom.DateTime.Year;
                            struCond.struStartTime.dwMonth = dateEditFrom.DateTime.Month;
                            struCond.struStartTime.dwDay = dateEditFrom.DateTime.Day;
                            struCond.struStartTime.dwHour = dateEditFrom.DateTime.Hour;
                            struCond.struStartTime.dwMinute = dateEditFrom.DateTime.Minute;
                            struCond.struStartTime.dwSecond = dateEditFrom.DateTime.Second;

                            struCond.struEndTime.dwYear = dateEditTo.DateTime.Year;
                            struCond.struEndTime.dwMonth = dateEditTo.DateTime.Month;
                            struCond.struEndTime.dwDay = dateEditTo.DateTime.Day;
                            struCond.struEndTime.dwHour = dateEditTo.DateTime.Hour;
                            struCond.struEndTime.dwMinute = dateEditTo.DateTime.Minute;
                            struCond.struEndTime.dwSecond = dateEditTo.DateTime.Second;

                            struCond.byPicEnable = 0;
                            struCond.szMonitorID = "";
                            struCond.wInductiveEventType = 65535;

                            uint dwSize = struCond.dwSize;
                            IntPtr ptrCond = Marshal.AllocHGlobal((int)dwSize);
                            Marshal.StructureToPtr(struCond, ptrCond, false);
                            m_lGetAcsEventHandle = CHCNetSDK.NET_DVR_StartRemoteConfig(m_UserID, CHCNetSDK.NET_DVR_GET_ACS_EVENT, ptrCond, (int)dwSize, null, IntPtr.Zero);
                            if (-1 == m_lGetAcsEventHandle)
                            {
                                Marshal.FreeHGlobal(ptrCond);
                                MessageBox.Show("NET_DVR_StartRemoteConfig FAIL, ERROR CODE" + CHCNetSDK.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                                return;
                            }

                            m_pDisplayListThread = new Thread(ProcessEvent);
                            m_pDisplayListThread.Start();
                            Marshal.FreeHGlobal(ptrCond);
                            gridControl1.DataSource = dtAttendance;
                        }
                        else
                        {
                            uint nErr = CHCNetSDK.NET_DVR_GetLastError();
                            if (nErr == CHCNetSDK.NET_DVR_PASSWORD_ERROR)
                            {
                                MessageBox.Show("user name or password error!");
                                if (1 == struDeviceInfoV40.bySupportLock)
                                {
                                    string strTemp1 = string.Format("Left {0} try opportunity", struDeviceInfoV40.byRetryLoginTime);
                                    MessageBox.Show(strTemp1);
                                }
                            }
                            else if (nErr == CHCNetSDK.NET_DVR_USER_LOCKED)
                            {
                                if (1 == struDeviceInfoV40.bySupportLock)
                                {
                                    string strTemp1 = string.Format("user is locked, the remaining lock time is {0}", struDeviceInfoV40.dwSurplusLockTime);
                                    MessageBox.Show(strTemp1);
                                }
                            }
                            else
                            {
                                MessageBox.Show("net error or dvr is busy!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Connection Issue, Username or Password Invalid!");
                    }
                }
            }
        }

        private void DownloadAttendanceFingerTech(DateTime FromDate, DateTime ToDate, int machineID)
        {
            if (lookUpEditLoc.EditValue == null)
            {
                MessageBox.Show("Please Select a Machine");
            }
            else
            {
                zkemkeeper.CZKEM axCZKEM1 = new zkemkeeper.CZKEM();
                AxBioBridgeSDKv3.AxBioBridgeSDKv3lib axBioBridgeSDKv3lib1 = new AxBioBridgeSDKv3.AxBioBridgeSDKv3lib();

                int iMachineNumber = 1;
                int EmployeeBadgeNo = 0;
                int Year = 0;
                int Month = 0;
                int day = 0;
                int Hours = 0;
                int Minitues = 0;
                int Secounds = 0;
                int Version = 0;
                int InOut = 0;
                int work = 0;
                int iSize = 0;
                DateTime _date, _time;
                string StrEmployeeBadgeNo = "";
                string _ipAddress = string.Empty;
                int _portNo = 0;
                int _lastModifyUser = -1;
                int _machineType = -1;
                string _machineURL = string.Empty;
                string _comPort = string.Empty;

                int idwEnrollNumber = 0;
                int idwErrorCode = 0;
                int iGLCount = 0;
                int iIndex = 0;
                string sTime = "";

                string sdwEnrollNumber = "";
                int idwTMachineNumber = 0;
                int idwEMachineNumber = 0;
                int idwVerifyMode = 0;
                int idwInOutMode = 0;
                int idwYear = 0;
                int idwMonth = 0;
                int idwDay = 0;
                int idwHour = 0;
                int idwMinute = 0;
                int idwSecond = 0;
                int idwWorkcode = 0;
                int _EntryId = 0;
                string _sipAddress = string.Empty;
                int _sportNo = 0;
                string _sdk = "";

                DataSet _scheduleList = new DataSet();
                DateTime dateTime = ToDate.Date;
                DateTime newDate = FromDate.Date;

                _ipAddress = getIPAddress(Convert.ToInt32(lookUpEditLoc.EditValue));
                _portNo = getPortNo(Convert.ToInt32(lookUpEditLoc.EditValue));
                _sipAddress = getSIPAddress(Convert.ToInt32(lookUpEditLoc.EditValue));
                _sportNo = getSPortNo(Convert.ToInt32(lookUpEditLoc.EditValue));
                _sdk = getSDK(Convert.ToInt32(lookUpEditLoc.EditValue));

                device.CommunicationType = CommunicationType.Tcp;

                DataTable dt = new DataTable();
                dt.Columns.Add("EmployeeBadgeNo", typeof(string));
                dt.Columns.Add("TransactionDate", typeof(DateTime));
                dt.Columns.Add("TransactionTime", typeof(DateTime));

                if (_ipAddress != string.Empty)
                {
                    #region Local Ip Conigurations

                    if (axBioBridgeSDKv3lib1.Connect_TCPIP("", 1, device.IpAddress, device.IpPort, Convert.ToInt32(_comPort)) == 0)
                    {
                        if (axBioBridgeSDKv3lib1.ReadGeneralLog(ref iSize) == 0)
                        {
                            while (axBioBridgeSDKv3lib1.GetGeneralLog(ref EmployeeBadgeNo, ref Year, ref Month, ref day, ref Hours, ref Minitues, ref Secounds, ref Version, ref InOut, ref work) == 0)
                            {
                                DataRow dr = null;

                                _date = Convert.ToDateTime(Year.ToString("0000") + "-" + Month.ToString("00") + "-" + day.ToString("00"));
                                _time = Convert.ToDateTime(Year.ToString("0000") + "-" + Month.ToString("00") + "-" + day.ToString("00") + " " + Hours.ToString("00") + ":" + Minitues.ToString("00") + ":" + Secounds.ToString("00"));

                                int n;
                                bool isNumeric = int.TryParse(EmployeeBadgeNo.ToString(), out n);

                                if (EmployeeBadgeNo != -1 && isNumeric == true)
                                {
                                    if (EmployeeBadgeNo.ToString().Trim().Length == 1)
                                    {
                                        StrEmployeeBadgeNo = "00000" + EmployeeBadgeNo.ToString().Trim();
                                    }
                                    else if (EmployeeBadgeNo.ToString().Trim().Length == 2)
                                    {
                                        StrEmployeeBadgeNo = "0000" + EmployeeBadgeNo.ToString().Trim();
                                    }
                                    else if (EmployeeBadgeNo.ToString().Trim().Length == 3)
                                    {
                                        StrEmployeeBadgeNo = "000" + EmployeeBadgeNo.ToString().Trim();
                                    }
                                    else if (EmployeeBadgeNo.ToString().Trim().Length == 4)
                                    {
                                        StrEmployeeBadgeNo = "00" + EmployeeBadgeNo.ToString().Trim();
                                    }
                                    else if (EmployeeBadgeNo.ToString().Trim().Length == 5)
                                    {
                                        StrEmployeeBadgeNo = "0" + EmployeeBadgeNo.ToString().Trim();
                                        // _globalBaseclass.TraceEvent = string.Format("Att Download ..inside while - Append - badge#-{0}", EmployeeBadgeNo.Trim());
                                    }
                                    else if (EmployeeBadgeNo.ToString().Trim().Length == 6)
                                    {
                                        StrEmployeeBadgeNo = EmployeeBadgeNo.ToString().Trim();
                                        // _globalBaseclass.TraceEvent = string.Format("Att Download ..inside while - Append - badge#-{0}", EmployeeBadgeNo.Trim());
                                    }
                                    else
                                    {
                                        StrEmployeeBadgeNo = EmployeeBadgeNo.ToString().Trim();
                                    }
                                    //}
                                }

                                //}
                                //else
                                //{
                                //    //WriteErrorLog("BadgeNoLength -"+ EmployeeBadgeNo.ToString().Trim().Length);
                                //    StrEmployeeBadgeNo = EmployeeBadgeNo.ToString().Trim();
                                //}

                                if (dateTime.Date >= _date.Date && newDate.Date <= _date.Date)
                                {
                                    dr = dt.NewRow();
                                    dr["EmployeeBadgeNo"] = StrEmployeeBadgeNo;
                                    dr["TransactionDate"] = _date;
                                    dr["TransactionTime"] = _time;
                                    dt.Rows.Add(dr);
                                }

                                //List1.Items.Add(("No: " + Convert.ToString(enrollNo) + " Date:" + Convert.ToString(day_Renamed) + "/" + Convert.ToString(mth) + "/" + Convert.ToString(yr) + " Time: " + Convert.ToString(hr) + ":" + Convert.ToString(min) + ":" + Convert.ToString(sec) + " Verify: " + Convert.ToString(ver) + " I/O: " + Convert.ToString(io) + " Work Code: " + Convert.ToString(work)));
                            }

                            //while (axBioBridgeSDKv3lib1.SSR_GetGeneralLog(ref EmployeeBadgeNo, ref Year, ref Month, ref day, ref Hours, ref Minitues, ref Secounds, ref Version, ref InOut, ref work) == 0);
                        }
                        else
                        {
                            MessageBox.Show("Data NOT avaliable");
                        }
                    }

                    #endregion Local Ip Conigurations
                }
                else
                {
                    #region Static Ip Conigurations

                    if (axBioBridgeSDKv3lib1.Connect_TCPIP("", 1, _sipAddress, _sportNo, Convert.ToInt32(_comPort)) == 0)
                    {
                        if (axBioBridgeSDKv3lib1.ReadGeneralLog(ref iSize) == 0)
                        {
                            while (axBioBridgeSDKv3lib1.GetGeneralLog(ref EmployeeBadgeNo, ref Year, ref Month, ref day, ref Hours, ref Minitues, ref Secounds, ref Version, ref InOut, ref work) == 0)
                            {
                                DataRow dr = null;

                                _date = Convert.ToDateTime(Year.ToString("0000") + "-" + Month.ToString("00") + "-" + day.ToString("00"));
                                _time = Convert.ToDateTime(Year.ToString("0000") + "-" + Month.ToString("00") + "-" + day.ToString("00") + " " + Hours.ToString("00") + ":" + Minitues.ToString("00") + ":" + Secounds.ToString("00"));

                                int n;
                                bool isNumeric = int.TryParse(EmployeeBadgeNo.ToString(), out n);

                                if (EmployeeBadgeNo != -1 && isNumeric == true)
                                {
                                    if (EmployeeBadgeNo.ToString().Trim().Length == 1)
                                    {
                                        StrEmployeeBadgeNo = "00000" + EmployeeBadgeNo.ToString().Trim();
                                    }
                                    else if (EmployeeBadgeNo.ToString().Trim().Length == 2)
                                    {
                                        StrEmployeeBadgeNo = "0000" + EmployeeBadgeNo.ToString().Trim();
                                    }
                                    else if (EmployeeBadgeNo.ToString().Trim().Length == 3)
                                    {
                                        StrEmployeeBadgeNo = "000" + EmployeeBadgeNo.ToString().Trim();
                                    }
                                    else if (EmployeeBadgeNo.ToString().Trim().Length == 4)
                                    {
                                        StrEmployeeBadgeNo = "00" + EmployeeBadgeNo.ToString().Trim();
                                    }
                                    else if (EmployeeBadgeNo.ToString().Trim().Length == 5)
                                    {
                                        StrEmployeeBadgeNo = "0" + EmployeeBadgeNo.ToString().Trim();
                                        // _globalBaseclass.TraceEvent = string.Format("Att Download ..inside while - Append - badge#-{0}", EmployeeBadgeNo.Trim());
                                    }
                                    else if (EmployeeBadgeNo.ToString().Trim().Length == 6)
                                    {
                                        StrEmployeeBadgeNo = EmployeeBadgeNo.ToString().Trim();
                                        // _globalBaseclass.TraceEvent = string.Format("Att Download ..inside while - Append - badge#-{0}", EmployeeBadgeNo.Trim());
                                    }
                                    else
                                    {
                                        StrEmployeeBadgeNo = EmployeeBadgeNo.ToString().Trim();
                                    }
                                    //}
                                }

                                //}
                                //else
                                //{
                                //    //WriteErrorLog("BadgeNoLength -"+ EmployeeBadgeNo.ToString().Trim().Length);
                                //    StrEmployeeBadgeNo = EmployeeBadgeNo.ToString().Trim();
                                //}

                                if (dateTime.Date >= _date.Date && newDate.Date <= _date.Date)
                                {
                                    dr = dt.NewRow();
                                    dr["EmployeeBadgeNo"] = StrEmployeeBadgeNo;
                                    dr["TransactionDate"] = _date;
                                    dr["TransactionTime"] = _time;
                                    dt.Rows.Add(dr);
                                }

                                //List1.Items.Add(("No: " + Convert.ToString(enrollNo) + " Date:" + Convert.ToString(day_Renamed) + "/" + Convert.ToString(mth) + "/" + Convert.ToString(yr) + " Time: " + Convert.ToString(hr) + ":" + Convert.ToString(min) + ":" + Convert.ToString(sec) + " Verify: " + Convert.ToString(ver) + " I/O: " + Convert.ToString(io) + " Work Code: " + Convert.ToString(work)));
                            }

                            //while (axBioBridgeSDKv3lib1.SSR_GetGeneralLog(ref EmployeeBadgeNo, ref Year, ref Month, ref day, ref Hours, ref Minitues, ref Secounds, ref Version, ref InOut, ref work) == 0);
                        }
                        else
                        {
                            MessageBox.Show("Data NOT avaliable");
                        }
                    }

                    #endregion Static Ip Conigurations
                }

                if (dt.Rows.Count > 0)
                {
                    gridControl1.DataSource = dt;
                }
            }
        }

        public void DownloadAttendanceRL(DateTime dtp_Begin, DateTime dtp_End, int machineID)
        {
            try
            {
                if (lookUpEditLoc.EditValue == null)
                {
                    MessageBox.Show("Please Select a Machine");
                }
                else
                {
                    device = new Device();
                    device.Model = "ZDC2911";
                    device.DN = 1;
                    device.Password = "0";
                    device.ConnectionModel = 5;
                    device.IpAddress = getIPAddress(Convert.ToInt32(lookUpEditLoc.EditValue));
                    device.IpPort = getPortNo(Convert.ToInt32(lookUpEditLoc.EditValue));
                    //_sipAddress = getSIPAddress((int)lookUpEditLoc.EditValue);
                    //_sportNo = getSPortNo((int)lookUpEditLoc.EditValue);
                    device.CommunicationType = CommunicationType.Tcp;

                    deviceConnection = DeviceConnection.CreateConnection(ref device);

                    if (device.IpAddress != string.Empty)
                    {
                        if (deviceConnection.Open() > 0)
                        {
                            object extraProperty = new object();
                            object extraData = new object();
                            extraData = Global.DeviceBusy;

                            try
                            {
                                List<DateTime> dtList = GetDateTimeList(dtp_Begin, dtp_End);
                                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                                extraProperty = false;
                                extraData = dtList;
                                result = deviceConnection.GetProperty(DeviceProperty.AttRecordsCount, extraProperty, ref device,
                                    ref extraData);
                                if (false == result)
                                {
                                    MessageBox.Show("Get All Glog Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                int recordCount = (int)extraData;
                                if (0 == recordCount)
                                {//为0时说明没有新日志记录
                                 //lvw_GLogList.Items.Clear();
                                    return;
                                }

                                List<bool> boolList = new List<bool>();
                                boolList.Add(false);//所有日志
                                boolList.Add(false);//清除新日志标记，false则不清除
                                extraProperty = boolList;
                                extraData = dtList;
                                result = deviceConnection.GetProperty(DeviceProperty.AttRecords, extraProperty, ref device, ref extraData);
                                if (result)
                                {
                                    List<Record> recordList = (List<Record>)extraData;
                                    AddRecordToListView(recordList);
                                }
                                else
                                {
                                    MessageBox.Show("Get All Glog Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                extraData = Global.DeviceIdle;
                                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Get All Glog Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        device = new Device();
                        device.Model = "ZDC2911";
                        device.DN = 1;
                        device.Password = "0";
                        device.ConnectionModel = 5;
                        //device.IpAddress = "124.43.69.18";
                        device.IpAddress = getIPAddress((int)lookUpEditLoc.EditValue);
                        //device.IpPort = 4370;
                        device.IpPort = getPortNo((int)lookUpEditLoc.EditValue);
                        device.CommunicationType = CommunicationType.Tcp;

                        deviceConnection = DeviceConnection.CreateConnection(ref device);

                        if (device.IpAddress != string.Empty)
                        {
                            if (deviceConnection.Open() > 0)
                            {
                                object extraProperty = new object();
                                object extraData = new object();
                                extraData = Global.DeviceBusy;

                                try
                                {
                                    List<DateTime> dtList = GetDateTimeList(dtp_Begin, dtp_End);
                                    bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                                    extraProperty = false;
                                    extraData = dtList;
                                    result = deviceConnection.GetProperty(DeviceProperty.AttRecordsCount, extraProperty, ref device,
                                        ref extraData);
                                    if (false == result)
                                    {
                                        MessageBox.Show("Get All Glog Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }

                                    int recordCount = (int)extraData;
                                    if (0 == recordCount)
                                    {//为0时说明没有新日志记录
                                     //lvw_GLogList.Items.Clear();
                                        return;
                                    }

                                    List<bool> boolList = new List<bool>();
                                    boolList.Add(false);//所有日志
                                    boolList.Add(false);//清除新日志标记，false则不清除
                                    extraProperty = boolList;
                                    extraData = dtList;
                                    result = deviceConnection.GetProperty(DeviceProperty.AttRecords, extraProperty, ref device, ref extraData);
                                    if (result)
                                    {
                                        List<Record> recordList = (List<Record>)extraData;
                                        AddRecordToListView(recordList);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Get All Glog Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                finally
                                {
                                    extraData = Global.DeviceIdle;
                                    deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Get All Glog Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void DownloadAttendanceZKT(DateTime dtp_Begin, DateTime dtp_End, int machineID)
        {
            //zkemkeeper.CZKEMClass axCZKEM1 = new CZKEMClass();
            //int iMachineNumber = 1;//the serial number of the device.After connecting the device ,this value will be changed.

            //DateTime _dtCurrentTime = DateTime.Now;
            //DateTime _dtUTCTime = DateTime.Now;
            // _dtCurrentTime = _dtUTCTime;

            //DateTime _date, _time;
            //string StrEmployeeBadgeNo = "";
            //string _ipAddress = string.Empty;
            //string _portNo = string.Empty;
            //int _lastModifyUser = -1;
            //int _machineType = -1;
            //string _machineURL = string.Empty;

            //int idwEnrollNumber = 0;
            //int idwErrorCode = 0;
            //int iGLCount = 0;
            //int iIndex = 0;
            //string sTime = "";

            //string sdwEnrollNumber = "";
            //int idwTMachineNumber = 0;
            //int idwEMachineNumber = 0;
            //int idwVerifyMode = 0;
            //int idwInOutMode = 0;
            //int idwYear = 0;
            //int idwMonth = 0;
            //int idwDay = 0;
            //int idwHour = 0;
            //int idwMinute = 0;
            //int idwSecond = 0;
            //int idwWorkcode = 0;

            //try
            //{
            //    DataSet _scheduleList = new DataSet();

            //    DateTime dateTime = _dtCurrentTime;
            //    TimeSpan _currentTs = _dtCurrentTime.TimeOfDay;

            //    //WriteErrorLog("From Time-"+ _previousTs.ToString());
            //    //WriteErrorLog("To Time-" + _dtCurrentTime.TimeOfDay.ToString());
            //    //WriteErrorLog("Current Date-" + dateTime.Date.ToString());
            //    //WriteErrorLog("Back Date-" + newDate.Date.ToString());

            //                WriteErrorLog("==================================START=============================================================");

            //                _ipAddress = getIPAddress((int)lookUpEditLoc.EditValue);
            //                _portNo = getPortNo((int)lookUpEditLoc.EditValue);
            //                _lastModifyUser = (int)_objMachineDetails.LastModifyUser;
            //                _machineType = Convert.ToInt16(_objMachineDetails.MachineType.ToString());
            //                _machineURL = _objMachineDetails.GetMachineType(_machineType) + "//" + _ipAddress + "\\" + _portNo;

            //                char[] _strsperator = { ':' };
            //                DateTime _strTime = (DateTime)item2["ScheduleTime"];

            //                DataTable dt = new DataTable();
            //                dt.Columns.Add("EmployeeBadgeNo", typeof(string));
            //                dt.Columns.Add("TransactionDate", typeof(DateTime));
            //                dt.Columns.Add("TransactionTime", typeof(DateTime));

            //                WriteErrorLog("Schedule time matched.connecting to device" + _objMachineDetails.MachineName + " @" + _objMachineDetails.MachineLocation);
            //                if (Connection(_ipAddress, Convert.ToInt32(_portNo)))
            //                {
            //                    if (axCZKEM1.Connect_Net(_ipAddress, Convert.ToInt32(_portNo)))
            //                    {
            //                        WriteErrorLog("Device" + _objMachineDetails.MachineName + " @" + _objMachineDetails.MachineLocation + " Connected....");
            //                        axCZKEM1.RegEvent(iMachineNumber, 65535);
            //                        axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device
            //                        if (axCZKEM1.ReadGeneralLogData(iMachineNumber))//read all the attendance records to the memory
            //                        {
            //                            WriteErrorLog("Device" + _objMachineDetails.MachineName + " @" + _objMachineDetails.MachineLocation + " Start Reading Log....");
            //                            while (axCZKEM1.SSR_GetGeneralLogData(iMachineNumber, out sdwEnrollNumber, out idwVerifyMode,
            //                                       out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
            //                            {
            //                                _date = Convert.ToDateTime(idwYear.ToString("0000") + "-" + idwMonth.ToString("00") + "-" + idwDay.ToString("00"));
            //                                _time = Convert.ToDateTime(idwYear.ToString("0000") + "-" + idwMonth.ToString("00") + "-" + idwDay.ToString("00") + " " + idwHour.ToString("00") + ":" + idwMinute.ToString("00") + ":" + Secounds.ToString("00"));

            //                                if (dateTime.Date >= _date.Date && newDate.Date <= _date.Date)
            //                                {
            //                                    _objEmployee = new Employee();
            //                                    StringBuilder _stremployee = new StringBuilder();

            //                                    //int n;
            //                                    //bool isNumeric = int.TryParse(sdwEnrollNumber.Trim(), out n);

            //                                    if (sdwEnrollNumber.Trim() != "")
            //                                    {
            //                                        if (sdwEnrollNumber.ToString().Trim().Length == 1)
            //                                        {
            //                                            StrEmployeeBadgeNo = "00000" + sdwEnrollNumber.ToString().Trim();
            //                                        }
            //                                        if (sdwEnrollNumber.ToString().Trim().Length == 2)
            //                                        {
            //                                            StrEmployeeBadgeNo = "0000" + sdwEnrollNumber.ToString().Trim();
            //                                        }
            //                                        if (sdwEnrollNumber.ToString().Trim().Length == 3)
            //                                        {
            //                                            StrEmployeeBadgeNo = "000" + sdwEnrollNumber.ToString().Trim();
            //                                        }
            //                                        if (sdwEnrollNumber.ToString().Trim().Length == 4)
            //                                        {
            //                                            StrEmployeeBadgeNo = "00" + sdwEnrollNumber.ToString().Trim();
            //                                        }

            //                                        if (sdwEnrollNumber.ToString().Trim().Length == 5)
            //                                        {
            //                                            StrEmployeeBadgeNo = "0" + sdwEnrollNumber.ToString().Trim();
            //                                        }

            //                                        if (sdwEnrollNumber.ToString().Trim().Length == 6)
            //                                        {
            //                                            StrEmployeeBadgeNo = sdwEnrollNumber.ToString().Trim();
            //                                        }

            //                                        DataRow dr = null;
            //                                        dr = dt.NewRow();
            //                                        dr["EmployeeBadgeNo"] = StrEmployeeBadgeNo;
            //                                        dr["TransactionDate"] = _date;
            //                                        dr["TransactionTime"] = _time;
            //                                        dt.Rows.Add(dr);
            //                                        //WriteErrorLog("Date-" + _date.Date.ToString());
            //                                    }
            //                                }
            //                            }
            //                        }
            //                        else
            //                        {
            //                            axCZKEM1.GetLastError(ref idwErrorCode);

            //                            if (idwErrorCode != 0)
            //                            {
            //                                WriteErrorLog("Reading data from terminal failed,ErrorCode: " + idwErrorCode.ToString());
            //                            }
            //                            else
            //                            {
            //                                WriteErrorLog("No data from terminal returns!");
            //                            }
            //                        }
            //                        axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device

            //                    }
            //                    else
            //                    {
            //                        WriteErrorLog("Connection failed to open to  -" + _objMachineDetails.MachineName + " @" + _objMachineDetails.MachineLocation);

            //                    }
            //                }
            //                else
            //                {
            //                    WriteErrorLog("Port Closed");
            //                }

            //                WriteErrorLog("==================================END===============================================================");

            //    //axCZKEM1.Disconnect();

            //}
            //catch (Exception ex)
            //{
            //    axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device
            //    //axCZKEM1.Disconnect();
            //    WriteErrorLog(ex.ToString());
            //}
        }

        public void DownloadAttendanceFingerTec(DateTime dtp_Begin, DateTime dtp_End, int machineID)
        {
            ////string _msgloop = "Called to download process";
            ////WriteErrorLog(_msgloop);

            //DateTime _dtCurrentTime = DateTime.Now;
            //DateTime _dtUTCTime = DateTime.UtcNow;

            ////_msgloop = "now: " + _dtCurrentTime + "--- utc: " + _dtUTCTime;
            ////WriteErrorLog(_msgloop);

            ////_dtCurrentTime = DateTime.Parse(TimeZoneInfo.ConvertTimeFromUtc(_dtUTCTime, TimeZoneInfo.FindSystemTimeZoneById("Sri Lanka Standard Time")).ToString("HH:mm", CultureInfo.CurrentCulture));

            ////_msgloop = "to local: " + _dtCurrentTime;
            ////WriteErrorLog(_msgloop);

            //DataSet _dsMachines = null;
            //DataSet _dsMachineSchedule = null;

            //bool _blnProcess = true;
            //int _intMachineIndex = -1;
            //int _intMachineID = -1;
            //bool _blnProcedToDownload = true;
            ///*------------------------------------------*/
            //string EmployeeBadgeNo = "";
            //int Year = 0;
            //int Month = 0;
            //int day = 0;
            //int Hours = 0;
            //int Minitues = 0;
            //int Secounds = 0;
            //int Version = 0;
            //int InOut = 0;
            //int work = 0;
            //int iSize = 0;
            //DateTime _date, _time;

            //string _datestring, _timestring;
            //string StrEmployeeBadgeNo = "";

            //string _ipAddress = string.Empty;
            //int _portNo = string.Empty;
            //int _lastModifyUser = -1;
            //int _machineType = -1;
            //string _machineURL = string.Empty;
            //string _comPort = string.Empty;

            ///*------------------------------------------*/
            //try
            //{
            //    bool _blnActive = false;

            //    DataSet _scheduleList = new DataSet();

            //    DateTime dateTime = _dtCurrentTime;
            //    TimeSpan _currentTs = _dtCurrentTime.TimeOfDay;
            //    TimeSpan timeSpan = new TimeSpan().Subtract(TimeSpan.FromMinutes(0));
            //    TimeSpan _previousTs = _currentTs + timeSpan;

            //    DateTime newDate = DateTime.Now.AddDays(backDays);

            //    if (_scheduleList.Tables[0].Rows.Count > 0)
            //    {
            //        WriteErrorLog("Download Process Initialize.......");
            //        bool _recordsfound = true;

            //            if (lookUpEditLoc.EditValue!=null)
            //            {
            //                DataTable dt = new DataTable();
            //                dt.Columns.Add("EmployeeBadgeNo", typeof(string));
            //                dt.Columns.Add("TransactionDate", typeof(DateTime));
            //                dt.Columns.Add("TransactionTime", typeof(DateTime));

            //            _ipAddress = getIPAddress((int)lookUpEditLoc.EditValue);
            //            _portNo = getPortNo((int)lookUpEditLoc.EditValue);

            //            _machineType = 1;

            //                char[] _strsperator = { ':' };
            //                DateTime _strTime = (DateTime)item2["ScheduleTime"];

            //                //string _msgloop3 = "schedule time: " + _strTime.Hour + ":" + _strTime.Minute + "|| " + "Current time is: " + _dtCurrentTime.Hour + ":" + _dtCurrentTime.Minute;
            //                //WriteErrorLog(_msgloop3);

            //                string _msgloop2 = "schedule time matched.connecting to device";
            //                WriteErrorLog(_msgloop2);

            //                BioBridgeSDKClass axBioBridgeSDKv3lib1 = new BioBridgeSDKClass();

            //                if (axBioBridgeSDKv3lib1.Connect_TCPIP(_objMachineDetails.GetMachineType(_machineType), _intMachineID, _ipAddress, Convert.ToInt16(_portNo), Convert.ToInt16(_comPort)) == 0)
            //                {
            //                    string _msg = "Connection Opened to device.... Start Time : " + _dtCurrentTime.ToString();
            //                    WriteErrorLog(_msg);

            //                    if (axBioBridgeSDKv3lib1.ReadGeneralLog(ref iSize) == 0)
            //                    {
            //                        while (axBioBridgeSDKv3lib1.SSR_GetGeneralLog(ref EmployeeBadgeNo, ref Year, ref Month, ref day, ref Hours, ref Minitues, ref Secounds, ref Version, ref InOut, ref work) == 0)
            //                        {
            //                            DataRow dr = null;

            //                            _date = Convert.ToDateTime(Year.ToString("0000") + "-" + Month.ToString("00") + "-" + day.ToString("00"));
            //                            _time = Convert.ToDateTime(Year.ToString("0000") + "-" + Month.ToString("00") + "-" + day.ToString("00") + " " + Hours.ToString("00") + ":" + Minitues.ToString("00") + ":" + Secounds.ToString("00"));

            //                            int n;
            //                            bool isNumeric = int.TryParse(EmployeeBadgeNo.Trim(), out n);
            //                        //WriteErrorLog("BadgeNo -" + EmployeeBadgeNo.ToString().Trim());
            //                        if (EmployeeBadgeNo.Trim() != "" && isNumeric == true)
            //                        {
            //                            //check system setting table entry - have record and check the badgeentry length as 5
            //                            StrEmployeeBadgeNo = EmployeeBadgeNo.ToString().Trim();

            //                        }

            //                                //WriteErrorLog("BadgeNoLength -"+ EmployeeBadgeNo.ToString().Trim().Length);

            //                            if (dateTime.Date >= _date.Date && newDate.Date <= _date.Date)
            //                            {
            //                                dr = dt.NewRow();
            //                                dr["EmployeeBadgeNo"] = StrEmployeeBadgeNo;
            //                                dr["TransactionDate"] = _date;
            //                                dr["TransactionTime"] = _time;
            //                                dt.Rows.Add(dr);
            //                            }

            //                            //List1.Items.Add(("No: " + Convert.ToString(enrollNo) + " Date:" + Convert.ToString(day_Renamed) + "/" + Convert.ToString(mth) + "/" + Convert.ToString(yr) + " Time: " + Convert.ToString(hr) + ":" + Convert.ToString(min) + ":" + Convert.ToString(sec) + " Verify: " + Convert.ToString(ver) + " I/O: " + Convert.ToString(io) + " Work Code: " + Convert.ToString(work)));
            //                        }
            //                        //while (axBioBridgeSDKv3lib1.SSR_GetGeneralLog(ref EmployeeBadgeNo, ref Year, ref Month, ref day, ref Hours, ref Minitues, ref Secounds, ref Version, ref InOut, ref work) == 0);
            //                    }
            //                    else
            //                    {
            //                        string _msg1 = "Data NOT avaliable on  -" + lookUpEditLoc.Text + "";
            //                        string _line = "----------------------------------------------------------------------------------";
            //                        WriteErrorLog(_msg1);
            //                        //WriteErrorLog(_line);
            //                    }
            //                }
            //                else
            //                {
            //                    string _msg = "Connection failed to open to  -" + lookUpEditLoc.Text + "";
            //                    string _line = "----------------------------------------------------------------------------------";
            //                    WriteErrorLog(_msg);
            //                    // WriteErrorLog(_line);
            //                }

            //        }

            //        WriteErrorLog("Download Process Completed.......");
            //    }

            //}
            //catch (Exception ex)
            //{
            //    WriteErrorLog(ex.ToString());
            //}
        }

        public string GetMachineType(int MachineType)
        {
            string _strMachineType = string.Empty;
            /*         
              TA100C
              TA103C
              ICLOCK360
              TX628
            */

            switch (MachineType)
            {
                case 0:
                    _strMachineType = "TA100C";
                    break;
                case 1:
                    _strMachineType = "TA103C";
                    break;
                case 2:
                    _strMachineType = "ICLOCK360";
                    break;
                case 3:
                    _strMachineType = "TX628";
                    break;
            }

            return _strMachineType;


        }

        public void DownloadAttendanceZKT_New(DateTime FromDate, DateTime ToDate, int machineID)
        {
            if (lookUpEditLoc.EditValue == null)
            {
                MessageBox.Show("Please Select a Machine");
            }
            else
            {
                zkemkeeper.CZKEM axCZKEM1 = new zkemkeeper.CZKEM();
                AxBioBridgeSDKv3.AxBioBridgeSDKv3lib axBioBridgeSDKv3lib1 = new AxBioBridgeSDKv3.AxBioBridgeSDKv3lib();

                int iMachineNumber = 1;
                int EmployeeBadgeNo = 0;
                int Year = 0;
                int Month = 0;
                int day = 0;
                int Hours = 0;
                int Minitues = 0;
                int Secounds = 0;
                int Version = 0;
                int InOut = 0;
                int work = 0;
                int iSize = 0;
                DateTime _date, _time;
                string StrEmployeeBadgeNo = "";
                string _ipAddress = string.Empty;
                int _portNo = 0;
                int _lastModifyUser = -1;
                int _machineType = -1;
                string _machineURL = string.Empty;
                string _comPort = string.Empty;

                int idwEnrollNumber = 0;
                int idwErrorCode = 0;
                int iGLCount = 0;
                int iIndex = 0;
                string sTime = "";

                string sdwEnrollNumber = "";
                int idwTMachineNumber = 0;
                int idwEMachineNumber = 0;
                int idwVerifyMode = 0;
                int idwInOutMode = 0;
                int idwYear = 0;
                int idwMonth = 0;
                int idwDay = 0;
                int idwHour = 0;
                int idwMinute = 0;
                int idwSecond = 0;
                int idwWorkcode = 0;
                int _EntryId = 0;
                string _sipAddress = string.Empty;
                int _sportNo = 0;
                string _sdk = "";

                DataSet _scheduleList = new DataSet();
                DateTime dateTime = ToDate.Date;
                DateTime newDate = FromDate.Date;

                _ipAddress = getIPAddress(Convert.ToInt32(lookUpEditLoc.EditValue));
                _portNo = getPortNo(Convert.ToInt32(lookUpEditLoc.EditValue));
                _sipAddress = getSIPAddress(Convert.ToInt32(lookUpEditLoc.EditValue));
                _sportNo = getSPortNo(Convert.ToInt32(lookUpEditLoc.EditValue));
                _sdk = getSDK(Convert.ToInt32(lookUpEditLoc.EditValue));

               
                //dtAttendance.Columns.Add("EmployeeBadgeNo", typeof(string));
                //dtAttendance.Columns.Add("TransactionDate", typeof(DateTime));
                //dtAttendance.Columns.Add("TransactionTime", typeof(DateTime));

                if (_ipAddress != string.Empty)
                {
                    #region Local Ip Conigurations

                    if (_sdk == "1.0.0.0")
                    {
                        #region SDK version 1

                        if (axCZKEM1.Connect_Net(_ipAddress, _portNo))
                        {
                            axCZKEM1.RegEvent(iMachineNumber, 65535);
                            axCZKEM1.EnableDevice(iMachineNumber, false);

                            if (axCZKEM1.ReadGeneralLogData(iMachineNumber))
                            {
                                while (axCZKEM1.SSR_GetGeneralLogData(iMachineNumber, out sdwEnrollNumber, out idwVerifyMode, out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode)) //get records from the memory
                                {
                                    _date = Convert.ToDateTime(idwYear.ToString("0000") + "-" + idwMonth.ToString("00") + "-" + idwDay.ToString("00"));
                                    _time = Convert.ToDateTime(idwYear.ToString("0000") + "-" + idwMonth.ToString("00") + "-" + idwDay.ToString("00") + " " + idwHour.ToString("00") + ":" + idwMinute.ToString("00") + ":" + idwSecond.ToString("00"));

                                    if (dateTime.Date >= _date.Date && newDate.Date <= _date.Date)
                                    {
                                        StringBuilder _stremployee = new StringBuilder();

                                        //int n;
                                        //bool isNumeric = int.TryParse(sdwEnrollNumber.Trim(), out n);

                                        if (sdwEnrollNumber.Trim() != "")
                                        {
                                            if (sdwEnrollNumber.ToString().Trim().Length == 1)
                                            {
                                                StrEmployeeBadgeNo = "00000" + sdwEnrollNumber.ToString().Trim();
                                            }
                                            if (sdwEnrollNumber.ToString().Trim().Length == 2)
                                            {
                                                StrEmployeeBadgeNo = "0000" + sdwEnrollNumber.ToString().Trim();
                                            }
                                            if (sdwEnrollNumber.ToString().Trim().Length == 3)
                                            {
                                                StrEmployeeBadgeNo = "000" + sdwEnrollNumber.ToString().Trim();
                                            }
                                            if (sdwEnrollNumber.ToString().Trim().Length == 4)
                                            {
                                                StrEmployeeBadgeNo = "00" + sdwEnrollNumber.ToString().Trim();
                                            }

                                            if (sdwEnrollNumber.ToString().Trim().Length == 5)
                                            {
                                                StrEmployeeBadgeNo = "0" + sdwEnrollNumber.ToString().Trim();
                                            }

                                            if (sdwEnrollNumber.ToString().Trim().Length == 6)
                                            {
                                                StrEmployeeBadgeNo = sdwEnrollNumber.ToString().Trim();
                                            }

                                            DataRow dr = null;
                                            dr = dtAttendance.NewRow();
                                            dr["EmployeeBadgeNo"] = StrEmployeeBadgeNo;
                                            dr["TransactionDate"] = _date;
                                            dr["TransactionTime"] = _time;
                                            dtAttendance.Rows.Add(dr);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                axCZKEM1.GetLastError(ref idwErrorCode);

                                if (idwErrorCode != 0)
                                {
                                    MessageBox.Show("Reading data from terminal failed,ErrorCode: " + idwErrorCode.ToString());
                                }
                                else
                                {
                                    MessageBox.Show("No data from terminal returns!");
                                }
                            }
                            axCZKEM1.EnableDevice(iMachineNumber, true); //enable the device
                        }
                        else
                        {
                            MessageBox.Show("Connection Failed to Open");
                        }

                        #endregion SDK version 1
                    }
                    else
                    {
                        #region SDK Version 2

                        if (axBioBridgeSDKv3lib1.Connect_TCPIP(GetMachineType(Convert.ToInt32(MachineType)), 1, _ipAddress.TrimEnd(), Convert.ToInt32(_portNo), Convert.ToInt32(_comPort)) == 0)
                        {
                            if (axBioBridgeSDKv3lib1.ReadGeneralLog(ref iSize) == 0)
                            {
                                while (axBioBridgeSDKv3lib1.GetGeneralLog(ref EmployeeBadgeNo, ref Year, ref Month, ref day, ref Hours, ref Minitues, ref Secounds, ref Version, ref InOut, ref work) == 0)
                                {
                                    DataRow dr = null;

                                    _date = Convert.ToDateTime(Year.ToString("0000") + "-" + Month.ToString("00") + "-" + day.ToString("00"));
                                    _time = Convert.ToDateTime(Year.ToString("0000") + "-" + Month.ToString("00") + "-" + day.ToString("00") + " " + Hours.ToString("00") + ":" + Minitues.ToString("00") + ":" + Secounds.ToString("00"));

                                    int n;
                                    bool isNumeric = int.TryParse(EmployeeBadgeNo.ToString(), out n);

                                    if (EmployeeBadgeNo != -1 && isNumeric == true)
                                    {
                                        if (EmployeeBadgeNo.ToString().Trim().Length == 1)
                                        {
                                            StrEmployeeBadgeNo = "00000" + EmployeeBadgeNo.ToString().Trim();
                                        }
                                        else if (EmployeeBadgeNo.ToString().Trim().Length == 2)
                                        {
                                            StrEmployeeBadgeNo = "0000" + EmployeeBadgeNo.ToString().Trim();
                                        }
                                        else if (EmployeeBadgeNo.ToString().Trim().Length == 3)
                                        {
                                            StrEmployeeBadgeNo = "000" + EmployeeBadgeNo.ToString().Trim();
                                        }
                                        else if (EmployeeBadgeNo.ToString().Trim().Length == 4)
                                        {
                                            StrEmployeeBadgeNo = "00" + EmployeeBadgeNo.ToString().Trim();
                                        }
                                        else if (EmployeeBadgeNo.ToString().Trim().Length == 5)
                                        {
                                            StrEmployeeBadgeNo = "0" + EmployeeBadgeNo.ToString().Trim();
                                            // _globalBaseclass.TraceEvent = string.Format("Att Download ..inside while - Append - badge#-{0}", EmployeeBadgeNo.Trim());
                                        }
                                        else if (EmployeeBadgeNo.ToString().Trim().Length == 6)
                                        {
                                            StrEmployeeBadgeNo = EmployeeBadgeNo.ToString().Trim();
                                            // _globalBaseclass.TraceEvent = string.Format("Att Download ..inside while - Append - badge#-{0}", EmployeeBadgeNo.Trim());
                                        }
                                        else
                                        {
                                            StrEmployeeBadgeNo = EmployeeBadgeNo.ToString().Trim();
                                        }
                                        //}
                                    }

                                    //}
                                    //else
                                    //{
                                    //    //WriteErrorLog("BadgeNoLength -"+ EmployeeBadgeNo.ToString().Trim().Length);
                                    //    StrEmployeeBadgeNo = EmployeeBadgeNo.ToString().Trim();
                                    //}

                                    if (dateTime.Date >= _date.Date && newDate.Date <= _date.Date)
                                    {
                                        dr = dtAttendance.NewRow();
                                        dr["EmployeeBadgeNo"] = StrEmployeeBadgeNo;
                                        dr["TransactionDate"] = _date;
                                        dr["TransactionTime"] = _time;
                                        dtAttendance.Rows.Add(dr);
                                    }

                                    //List1.Items.Add(("No: " + Convert.ToString(enrollNo) + " Date:" + Convert.ToString(day_Renamed) + "/" + Convert.ToString(mth) + "/" + Convert.ToString(yr) + " Time: " + Convert.ToString(hr) + ":" + Convert.ToString(min) + ":" + Convert.ToString(sec) + " Verify: " + Convert.ToString(ver) + " I/O: " + Convert.ToString(io) + " Work Code: " + Convert.ToString(work)));
                                }

                                //while (axBioBridgeSDKv3lib1.SSR_GetGeneralLog(ref EmployeeBadgeNo, ref Year, ref Month, ref day, ref Hours, ref Minitues, ref Secounds, ref Version, ref InOut, ref work) == 0);
                            }
                            else
                            {
                                MessageBox.Show("Data NOT avaliable");
                            }
                        }

                        #endregion SDK Version 2
                    }

                    #endregion Local Ip Conigurations
                }
                else
                {
                    #region Static Ip Conigurations

                    if (_sdk == "1.0.0.0")
                    {
                        #region SDK version 1

                        if (axCZKEM1.Connect_Net(_sipAddress, _sportNo))
                        {
                            axCZKEM1.RegEvent(iMachineNumber, 65535);
                            axCZKEM1.EnableDevice(iMachineNumber, false);

                            if (axCZKEM1.ReadGeneralLogData(iMachineNumber))
                            {
                                while (axCZKEM1.SSR_GetGeneralLogData(iMachineNumber, out sdwEnrollNumber, out idwVerifyMode, out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode)) //get records from the memory
                                {
                                    _date = Convert.ToDateTime(idwYear.ToString("0000") + "-" + idwMonth.ToString("00") + "-" + idwDay.ToString("00"));
                                    _time = Convert.ToDateTime(idwYear.ToString("0000") + "-" + idwMonth.ToString("00") + "-" + idwDay.ToString("00") + " " + idwHour.ToString("00") + ":" + idwMinute.ToString("00") + ":" + idwSecond.ToString("00"));

                                    if (dateTime.Date >= _date.Date && newDate.Date <= _date.Date)
                                    {
                                        StringBuilder _stremployee = new StringBuilder();

                                        //int n;
                                        //bool isNumeric = int.TryParse(sdwEnrollNumber.Trim(), out n);

                                        if (sdwEnrollNumber.Trim() != "")
                                        {
                                            if (sdwEnrollNumber.ToString().Trim().Length == 1)
                                            {
                                                StrEmployeeBadgeNo = "00000" + sdwEnrollNumber.ToString().Trim();
                                            }
                                            if (sdwEnrollNumber.ToString().Trim().Length == 2)
                                            {
                                                StrEmployeeBadgeNo = "0000" + sdwEnrollNumber.ToString().Trim();
                                            }
                                            if (sdwEnrollNumber.ToString().Trim().Length == 3)
                                            {
                                                StrEmployeeBadgeNo = "000" + sdwEnrollNumber.ToString().Trim();
                                            }
                                            if (sdwEnrollNumber.ToString().Trim().Length == 4)
                                            {
                                                StrEmployeeBadgeNo = "00" + sdwEnrollNumber.ToString().Trim();
                                            }

                                            if (sdwEnrollNumber.ToString().Trim().Length == 5)
                                            {
                                                StrEmployeeBadgeNo = "0" + sdwEnrollNumber.ToString().Trim();
                                            }

                                            if (sdwEnrollNumber.ToString().Trim().Length == 6)
                                            {
                                                StrEmployeeBadgeNo = sdwEnrollNumber.ToString().Trim();
                                            }

                                            DataRow dr = null;
                                            dr = dtAttendance.NewRow();
                                            dr["EmployeeBadgeNo"] = StrEmployeeBadgeNo;
                                            dr["TransactionDate"] = _date;
                                            dr["TransactionTime"] = _time;
                                            dtAttendance.Rows.Add(dr);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                axCZKEM1.GetLastError(ref idwErrorCode);

                                if (idwErrorCode != 0)
                                {
                                    MessageBox.Show("Reading data from terminal failed,ErrorCode: " + idwErrorCode.ToString());
                                }
                                else
                                {
                                    MessageBox.Show("No data from terminal returns!");
                                }
                            }
                            axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device
                        }
                        else
                        {
                            MessageBox.Show("Connection Failed to Open");
                        }

                        #endregion SDK version 1
                    }
                    else
                    {
                        #region SDK Version 2

                        if (axBioBridgeSDKv3lib1.Connect_TCPIP(GetMachineType(Convert.ToInt32(MachineType)), 1, _sipAddress, _sportNo, Convert.ToInt32(_comPort)) == 0)
                        {
                            if (axBioBridgeSDKv3lib1.ReadGeneralLog(ref iSize) == 0)
                            {
                                while (axBioBridgeSDKv3lib1.GetGeneralLog(ref EmployeeBadgeNo, ref Year, ref Month, ref day, ref Hours, ref Minitues, ref Secounds, ref Version, ref InOut, ref work) == 0)
                                {
                                    DataRow dr = null;

                                    _date = Convert.ToDateTime(Year.ToString("0000") + "-" + Month.ToString("00") + "-" + day.ToString("00"));
                                    _time = Convert.ToDateTime(Year.ToString("0000") + "-" + Month.ToString("00") + "-" + day.ToString("00") + " " + Hours.ToString("00") + ":" + Minitues.ToString("00") + ":" + Secounds.ToString("00"));

                                    int n;
                                    bool isNumeric = int.TryParse(EmployeeBadgeNo.ToString(), out n);

                                    if (EmployeeBadgeNo != -1 && isNumeric == true)
                                    {
                                        if (EmployeeBadgeNo.ToString().Trim().Length == 1)
                                        {
                                            StrEmployeeBadgeNo = "00000" + EmployeeBadgeNo.ToString().Trim();
                                        }
                                        else if (EmployeeBadgeNo.ToString().Trim().Length == 2)
                                        {
                                            StrEmployeeBadgeNo = "0000" + EmployeeBadgeNo.ToString().Trim();
                                        }
                                        else if (EmployeeBadgeNo.ToString().Trim().Length == 3)
                                        {
                                            StrEmployeeBadgeNo = "000" + EmployeeBadgeNo.ToString().Trim();
                                        }
                                        else if (EmployeeBadgeNo.ToString().Trim().Length == 4)
                                        {
                                            StrEmployeeBadgeNo = "00" + EmployeeBadgeNo.ToString().Trim();
                                        }
                                        else if (EmployeeBadgeNo.ToString().Trim().Length == 5)
                                        {
                                            StrEmployeeBadgeNo = "0" + EmployeeBadgeNo.ToString().Trim();
                                            // _globalBaseclass.TraceEvent = string.Format("Att Download ..inside while - Append - badge#-{0}", EmployeeBadgeNo.Trim());
                                        }
                                        else if (EmployeeBadgeNo.ToString().Trim().Length == 6)
                                        {
                                            StrEmployeeBadgeNo = EmployeeBadgeNo.ToString().Trim();
                                            // _globalBaseclass.TraceEvent = string.Format("Att Download ..inside while - Append - badge#-{0}", EmployeeBadgeNo.Trim());
                                        }
                                        else
                                        {
                                            StrEmployeeBadgeNo = EmployeeBadgeNo.ToString().Trim();
                                        }
                                        //}
                                    }

                                    //}
                                    //else
                                    //{
                                    //    //WriteErrorLog("BadgeNoLength -"+ EmployeeBadgeNo.ToString().Trim().Length);
                                    //    StrEmployeeBadgeNo = EmployeeBadgeNo.ToString().Trim();
                                    //}

                                    if (dateTime.Date >= _date.Date && newDate.Date <= _date.Date)
                                    {
                                        dr = dtAttendance.NewRow();
                                        dr["EmployeeBadgeNo"] = StrEmployeeBadgeNo;
                                        dr["TransactionDate"] = _date;
                                        dr["TransactionTime"] = _time;
                                        dtAttendance.Rows.Add(dr);
                                    }

                                    //List1.Items.Add(("No: " + Convert.ToString(enrollNo) + " Date:" + Convert.ToString(day_Renamed) + "/" + Convert.ToString(mth) + "/" + Convert.ToString(yr) + " Time: " + Convert.ToString(hr) + ":" + Convert.ToString(min) + ":" + Convert.ToString(sec) + " Verify: " + Convert.ToString(ver) + " I/O: " + Convert.ToString(io) + " Work Code: " + Convert.ToString(work)));
                                }

                                //while (axBioBridgeSDKv3lib1.SSR_GetGeneralLog(ref EmployeeBadgeNo, ref Year, ref Month, ref day, ref Hours, ref Minitues, ref Secounds, ref Version, ref InOut, ref work) == 0);
                            }
                            else
                            {
                                MessageBox.Show("Data NOT avaliable");
                            }
                        }

                        #endregion SDK Version 2
                    }

                    #endregion Static Ip Conigurations
                }

                if (dtAttendance.Rows.Count > 0)
                {
                    gridControl1.DataSource = dtAttendance;
                }
            }
        }

        private List<DateTime> GetDateTimeList(DateTime dtp_Begin, DateTime dtp_End)
        {
            List<DateTime> dtList = new List<DateTime>();
            dtList.Add(dtp_Begin.Date);
            dtList.Add(dtp_End.Date);
            return dtList;
        }

        private void AddRecordToListView(List<Record> recordList)
        {
            int no = 1;
            dtAttendance.Rows.Clear();

            DataRow dr = null;

            foreach (Record record in recordList)
            {
                string type = ConvertObject.IOMode(record.Verify);
                string mode = ConvertObject.GLogType(record.Action);

                dr = dtAttendance.NewRow();
                dr["EmployeeBadgeNo"] = record.DIN.ToString();
                dr["TransactionDate"] = Convert.ToDateTime(record.Clock.ToString("yyyy-MM-dd")).Date;
                dr["TransactionTime"] = Convert.ToDateTime(record.Clock.ToString("yyyy-MM-dd HH:mm:ss"));
                dr["MachineEntryID"] = Convert.ToInt32(lookUpEditLoc.EditValue);
                dtAttendance.Rows.Add(dr);
                //ListViewItem item = new ListViewItem(new string[]{ no.ToString(), record.DN.ToString(), record.DIN.ToString(),
                //type, mode, record.Clock.ToString("yyyy-MM-dd HH:mm:ss") });
                //lvw_GLogList.Items.Add(item);
                no++;
            }

            gridControl1.DataSource = dtAttendance;
        }

        public async void DropDownLoad()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var responseTask = await client.GetAsync("api/AttMange/getLoacationDetails/" + LocaionID);

                    if (responseTask.IsSuccessStatusCode)
                    {
                        string responseBody = await responseTask.Content.ReadAsStringAsync();

                        DataSet Machine = (DataSet)JsonConvert.DeserializeObject(responseBody, (typeof(DataSet)));
                        //DataSet Machine = (DataSet)JsonConvert.DeserializeObject<DataSet>(responseBody);
                        //DataSet Machine = jsonParsed["Table"].ToObject<DataSet>();
                        LoadLookupControls(lookUpEditLoc, Machine, "location", "machineEntryID");
                        LoadLookupControls(lookUpEditviewMachine, Machine, "location", "machineEntryID");

                        LocationMachine = Machine.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public async void CuurentLocationDetails()
        {
            try
            {
                lblLocationName.Text = "";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var responseTask = await client.GetAsync("api/AttMange/getCurrentLoacationDetails/" + LocaionID);

                    if (responseTask.IsSuccessStatusCode)
                    {
                        string responseBody = await responseTask.Content.ReadAsStringAsync();
                        string Machine = (string)JsonConvert.DeserializeObject(responseBody, (typeof(string)));
                        lblLocationName.Text = Machine;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void LoadLookupControlsEdit(LookUpEdit SourceControl, DataSet _dsSourceDataset, string DisplayColumn, string ValueColumn, string[] FilterColumns = null)
        {
            try
            {
                string _DisplayColum = DisplayColumn;
                string _ValueColumn = ValueColumn;

                FilterColumns = new string[] { DisplayColumn };
                _dsSourceDataset.Tables[0].DefaultView.Sort = _DisplayColum;
                // Bind the edit value to the ProductID field of the "Order Details" table.
                // The edit value matches the value of the ValueMember field
                //SourceControl.DataBindings.Add("EditValue", bsMain_OrderDetails, "ProductID");

                // Specify the data source to display in the dropdown.
                // Bind the edit value to the ProductID field of the "Order Details" table.
                // The edit value matches the value of the ValueMember field
                //SourceControl.DataBindings.Add("EditValue", bsMain_OrderDetails, "ProductID");

                // Specify the data source to display in the dropdown.
                SourceControl.Properties.DataSource = _dsSourceDataset.Tables[0];
                // The field providing the editor's display text.
                SourceControl.Properties.DisplayMember = _DisplayColum;
                // The field matching the edit value.
                SourceControl.Properties.ValueMember = _ValueColumn;
                // clear the text in the text box of the look up control
                if (_dsSourceDataset.Tables[0].Rows.Count > 0)
                {
                    SourceControl.EditValue = null;
                    SourceControl.Properties.NullText = "-Please Select-";
                    //SourceControl.EditValue = _dsSourceDataset.Tables[0].Rows[0][_ValueColumn];
                }
                //*********************************
                SourceControl.Text = "";
                //clear existing columns
                SourceControl.Properties.Columns.Clear();
                // Add two columns to the dropdown.
                int _index = 0, _intAutoSearchColum = 0;

                foreach (DataColumn item in _dsSourceDataset.Tables[0].Columns)
                {
                    if (item.ColumnName.ToUpper().IndexOf("ID") <= 0 && item.ColumnName.ToUpper() != ("LastModifyDate").ToUpper() && item.ColumnName.ToUpper() != ("LastModifyUser").ToUpper())
                    {
                        if (FilterColumns != null)
                        {
                            if (FilterColumns.Length <= 0)
                            { SourceControl.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.ColumnName, 0)); }
                            else
                            {
                                foreach (string item2 in FilterColumns)
                                {
                                    if (item2 == item.ColumnName.ToString())
                                    {
                                        SourceControl.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.ColumnName, 0));
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (item.ColumnName.ToUpper() == _DisplayColum)
                    { _intAutoSearchColum = _index; }
                    _index++;
                }
                //  Set column widths according to their contents and resize the popup, if required.
                SourceControl.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;

                // Enable auto completion search mode.
                SourceControl.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                // Specify the column against which to perform the search.
                SourceControl.Properties.AutoSearchColumnIndex = _intAutoSearchColum;
            }
            catch (Exception ex)
            {
            }
        }

        private void lookUpEditLoc_EditValueChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(lookUpEditLoc.EditValue.ToString());
        }

        public void LoadLookupControls(LookUpEdit SourceControl, DataSet _dsSourceDataset, string DisplayColumn, string ValueColumn, string[] FilterColumns = null)
        {
            try
            {
                string _DisplayColum = DisplayColumn;
                string _ValueColumn = ValueColumn;

                FilterColumns = new string[] { DisplayColumn };
                _dsSourceDataset.Tables[0].DefaultView.Sort = _DisplayColum;
                // Bind the edit value to the ProductID field of the "Order Details" table.
                // The edit value matches the value of the ValueMember field
                //SourceControl.DataBindings.Add("EditValue", bsMain_OrderDetails, "ProductID");

                // Specify the data source to display in the dropdown.
                // Bind the edit value to the ProductID field of the "Order Details" table.
                // The edit value matches the value of the ValueMember field
                //SourceControl.DataBindings.Add("EditValue", bsMain_OrderDetails, "ProductID");

                // Specify the data source to display in the dropdown.
                SourceControl.Properties.DataSource = _dsSourceDataset.Tables[0];
                // The field providing the editor's display text.
                SourceControl.Properties.DisplayMember = _DisplayColum;
                // The field matching the edit value.
                SourceControl.Properties.ValueMember = _ValueColumn;
                // clear the text in the text box of the look up control
                if (_dsSourceDataset.Tables[0].Rows.Count > 0)
                {
                    SourceControl.EditValue = null;
                    SourceControl.Properties.NullText = "-Please Select-";
                    //SourceControl.EditValue = _dsSourceDataset.Tables[0].Rows[0][_ValueColumn];
                }
                //*********************************
                SourceControl.Text = "";
                //clear existing columns
                SourceControl.Properties.Columns.Clear();
                // Add two columns to the dropdown.
                int _index = 0, _intAutoSearchColum = 0;

                foreach (DataColumn item in _dsSourceDataset.Tables[0].Columns)
                {
                    if (item.ColumnName.ToUpper().IndexOf("ID") <= 0 && item.ColumnName.ToUpper() != ("LastModifyDate").ToUpper() && item.ColumnName.ToUpper() != ("LastModifyUser").ToUpper())
                    {
                        if (FilterColumns != null)
                        {
                            if (FilterColumns.Length <= 0)
                            { SourceControl.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.ColumnName, 0)); }
                            else
                            {
                                foreach (string item2 in FilterColumns)
                                {
                                    if (item2 == item.ColumnName.ToString())
                                    {
                                        SourceControl.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.ColumnName, 0));
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (item.ColumnName == _DisplayColum)
                    { _intAutoSearchColum = _index; }
                    _index++;
                }
                //  Set column widths according to their contents and resize the popup, if required.
                SourceControl.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;

                // Enable auto completion search mode.
                SourceControl.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                // Specify the column against which to perform the search.
                SourceControl.Properties.AutoSearchColumnIndex = _intAutoSearchColum;
            }
            catch (Exception ex)
            {
            }
        }

        public string getSDK(int MachineID)
        {
            string SDK = "";

            if (LocationMachine.Rows.Count > 0)
            {
                DataView dv = new DataView(LocationMachine);
                dv.RowFilter = "MachineEntryID=" + MachineID;
                DataTable filter = dv.ToTable();
                if (filter.Rows.Count > 0)
                {
                    SDK = filter.Rows[0]["SDKversion"].ToString();
                }
                dv.RowFilter = "";
            }

            return SDK;
        }

        public string getIPAddress(int MachineID)
        {
            string IPAddress = "";

            if (LocationMachine.Rows.Count > 0)
            {
                DataView dv = new DataView(LocationMachine);
                dv.RowFilter = "MachineEntryID=" + MachineID;
                DataTable filter = dv.ToTable();
                if (filter.Rows.Count > 0)
                {
                    IPAddress = filter.Rows[0]["IPAddress"].ToString();
                }
                dv.RowFilter = "";
            }

            return IPAddress;
        }

        public string getSIPAddress(int MachineID)
        {
            string SIPAddress = "";

            if (LocationMachine.Rows.Count > 0)
            {
                DataView dv = new DataView(LocationMachine);
                dv.RowFilter = "MachineEntryID=" + MachineID;
                DataTable filter = dv.ToTable();
                if (filter.Rows.Count > 0)
                {
                    SIPAddress = filter.Rows[0]["StaticIPAddress"].ToString();
                }
                dv.RowFilter = "";
            }

            return SIPAddress;
        }

        public int getPortNo(int MachineID)
        {
            int PortNo = 0;

            if (LocationMachine.Rows.Count > 0)
            {
                DataView dv = new DataView(LocationMachine);
                dv.RowFilter = "MachineEntryID=" + MachineID;
                DataTable filter = dv.ToTable();
                if (filter.Rows.Count > 0)
                {
                    PortNo = Convert.ToInt32(filter.Rows[0]["PortNo"].ToString());
                }
                dv.RowFilter = "";
            }

            return PortNo;
        }

        public int getSPortNo(int MachineID)
        {
            int SPortNo = 0;

            if (LocationMachine.Rows.Count > 0)
            {
                DataView dv = new DataView(LocationMachine);
                dv.RowFilter = "MachineEntryID=" + MachineID;
                DataTable filter = dv.ToTable();
                if (filter.Rows.Count > 0)
                {
                    SPortNo = Convert.ToInt32(filter.Rows[0]["StaticPortNo"].ToString());
                }
                dv.RowFilter = "";
            }

            return SPortNo;
        }

        public string getUsername(int MachineID)
        {
            string Username = "";

            if (LocationMachine.Rows.Count > 0)
            {
                DataView dv = new DataView(LocationMachine);
                dv.RowFilter = "MachineEntryID=" + MachineID;
                DataTable filter = dv.ToTable();
                if (filter.Rows.Count > 0)
                {
                    Username = filter.Rows[0]["Username"].ToString();
                }
                dv.RowFilter = "";
            }

            return Username;
        }

        public string getPassword(int MachineID)
        {
            string Password = "";

            if (LocationMachine.Rows.Count > 0)
            {
                DataView dv = new DataView(LocationMachine);
                dv.RowFilter = "MachineEntryID=" + MachineID;
                DataTable filter = dv.ToTable();
                if (filter.Rows.Count > 0)
                {
                    Password = filter.Rows[0]["Password"].ToString();
                }
                dv.RowFilter = "";
            }

            return Password;
        }

        public int getMachineType(int MachineID)
        {
            int _machineType = 0;

            if (LocationMachine.Rows.Count > 0)
            {
                DataView dv = new DataView(LocationMachine);
                dv.RowFilter = "MachineEntryID=" + MachineID;
                DataTable filter = dv.ToTable();
                if (filter.Rows.Count > 0)
                {
                    _machineType = Convert.ToInt32(filter.Rows[0]["MachineType"].ToString());
                }
            }

            return _machineType;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                AttendanceDownload();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public async void AttendanceDownload()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var responseTask = await client.GetAsync("api/AttMange/getAttendanceDetails/" + lookUpEditviewMachine.EditValue.ToString() + "/" + dateEditViewFrom.DateTime.Date.ToString("yyyy-MM-dd") + "/" + dateEditViewTodate.DateTime.Date.ToString("yyyy-MM-dd"));

                    if (responseTask.IsSuccessStatusCode)
                    {
                        string responseBody = await responseTask.Content.ReadAsStringAsync();

                        DataSet Machine = (DataSet)JsonConvert.DeserializeObject(responseBody, (typeof(DataSet)));

                        if (Machine != null)
                        {
                            gcAttendanceDetails.DataSource = Machine.Tables[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool Connect()
        {
            try
            {
                System.Net.WebClient client = new System.Net.WebClient();
                string result = client.DownloadString(BaseURL + "api/AttMange/getConnectivity");
                if (Convert.ToBoolean(result))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return false;
        }

        public async void webPostMethod()
        {
            try
            {
                if (lookUpEditLoc.EditValue == null)
                {
                    MessageBox.Show("Please Select a Machine");
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(BaseURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var data = JsonConvert.SerializeObject(dtAttendance);
                        var content = new StringContent(data, Encoding.UTF8, "application/json");
                        response = await client.PostAsync("api/AttMange/saveAttendanceDetails", content);

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show(response.IsSuccessStatusCode.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            webPostMethod();
        }

        public void WriteErrorLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\AttendanceLogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
    }
}