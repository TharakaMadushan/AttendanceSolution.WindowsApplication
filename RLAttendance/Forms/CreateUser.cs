using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace AttendanceSolution.Forms
{
    public partial class CreateUser : DevExpress.XtraEditors.XtraForm
    {
        private HttpResponseMessage response;
        private HttpClient client;
        private string BaseURL = ConfigurationManager.AppSettings["BaseURL"].ToString();
        private string LocaionID = ConfigurationManager.AppSettings["LocationID"].ToString();
        private string MachineType = ConfigurationManager.AppSettings["MachineType"].ToString();

        public CreateUser()
        {
            InitializeComponent();
        }

        private void CreateUser_Load(object sender, EventArgs e)
        {
            try
            {
                if (Connect())
                {
                    lblIndicate.Appearance.ForeColor = Color.Green;
                }
                else
                {
                    lblIndicate.Appearance.ForeColor = Color.Red;
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

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                string DocNo = "SELECT * FROM MasterEmployee WHERE IsActive=1 AND EmployeeNo=" + (int)lookUpEditEmployee.EditValue + "AND SegmentID=" + (int)lookUpEditSegments.EditValue;

                if (DocNo is null)
                {
                    MessageBox.Show("Invalid Employee!");
                }
                else
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(BaseURL + "api/Auth/register");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    using (var streamWriter = new

                    StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            SegmentID = (int)lookUpEditSegments.EditValue,
                            EmployeeNo = (int)lookUpEditEmployee.EditValue,
                            DocumentEmployeeNo = DocNo.ToString(),
                            Username = textEditUsername.EditValue.ToString(),
                            Password = textEditPassword.EditValue.ToString(),
                            ConfirmPassword = textEditConfirm.EditValue.ToString()
                        }); ;

                        streamWriter.Write(json);
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}