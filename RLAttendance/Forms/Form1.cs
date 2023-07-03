using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Riss.Devices;
using System.Net;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Configuration;
using DevExpress.XtraEditors;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace RLAttendance
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        private Device device;
        private DeviceConnection deviceConnection;
        string apiBaseUrl = "";
        HttpResponseMessage response;
        HttpClient client;
        string BaseURL = ConfigurationManager.AppSettings["BaseURL"].ToString();
        string LocaionID = ConfigurationManager.AppSettings["LocationID"].ToString();
        string MachineType= ConfigurationManager.AppSettings["MachineType"].ToString();
        DataTable LocationMachine = new DataTable();
        DataTable dtAttendance = new DataTable();

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
                if ( Connect())
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
                else if(MachineType == "2")
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void DownloadAttendanceRL(DateTime dtp_Begin, DateTime dtp_End,int machineID)
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
                    //device.IpAddress = "192.168.001.225";
                    device.IpAddress = getIPAddress((int)lookUpEditLoc.EditValue);
                    device.IpPort = getPortNo((int)lookUpEditLoc.EditValue);
                    device.CommunicationType = CommunicationType.Tcp;




                    deviceConnection = DeviceConnection.CreateConnection(ref device);

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

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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

                   
                    var responseTask = await client.GetAsync("api/AttMange/getLoacationDetails/"+LocaionID);                    

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

        public  void LoadLookupControlsEdit(LookUpEdit SourceControl, DataSet _dsSourceDataset, string DisplayColumn, string ValueColumn, string[] FilterColumns = null)
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

        public  void LoadLookupControls(LookUpEdit SourceControl, DataSet _dsSourceDataset, string DisplayColumn, string ValueColumn, string[] FilterColumns = null)
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

        public string getIPAddress(int MachineID)
        {
            string IPAddress = "";

            if(LocationMachine.Rows.Count>0)
            {
                DataView dv = new DataView(LocationMachine);
                dv.RowFilter = "MachineEntryID=" + MachineID;
                DataTable filter = dv.ToTable();
                if(filter.Rows.Count>0)
                {
                    IPAddress = filter.Rows[0]["IPAddress"].ToString();
                }
                dv.RowFilter = "";
            }

            return IPAddress;
        }

        public int getPortNo(int MachineID)
        {
            int PortNo = 0;

            if (LocationMachine.Rows.Count > 0)
            {
                DataView dv = new DataView(LocationMachine);
                dv.RowFilter = "PortNo=" + MachineID;
                DataTable filter = dv.ToTable();
                if (filter.Rows.Count > 0)
                {
                    PortNo = Convert.ToInt32(filter.Rows[0]["PortNo"].ToString());
                }
                dv.RowFilter = "";
            }

            return PortNo;
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


                    var responseTask = await client.GetAsync("api/AttMange/getAttendanceDetails/" + lookUpEditviewMachine.EditValue.ToString()+"/"+dateEditViewFrom.DateTime.Date.ToString("yyyy-MM-dd") +"/"+ dateEditViewTodate.DateTime.Date.ToString("yyyy-MM-dd"));

                    if (responseTask.IsSuccessStatusCode)
                    {
                        string responseBody = await responseTask.Content.ReadAsStringAsync();

                        DataSet Machine = (DataSet)JsonConvert.DeserializeObject(responseBody, (typeof(DataSet)));

                        if(Machine!=null)
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

        public  bool Connect()
        {
            try
            {
                System.Net.WebClient client = new System.Net.WebClient();
                string result = client.DownloadString(BaseURL+ "api/AttMange/getConnectivity");
                if(Convert.ToBoolean( result))
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

        public  void WriteErrorLog(string Message)
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

        public  void DownloadAttendanceZKT(DateTime dtp_Begin, DateTime dtp_End, int machineID)
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





    }
}
