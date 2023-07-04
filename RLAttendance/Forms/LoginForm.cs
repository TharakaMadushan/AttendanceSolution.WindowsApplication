using HRMS.BusinessLayer;
using HRMS.DataLayer;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace AttendanceSolution.Forms
{
    public partial class LoginForm : DevExpress.XtraEditors.XtraForm
    {
        private HttpResponseMessage response;
        private HttpClient client;
        private string BaseURL = ConfigurationManager.AppSettings["BaseURL"].ToString();
        private string LocaionID = ConfigurationManager.AppSettings["LocationID"].ToString();
        private string MachineType = ConfigurationManager.AppSettings["MachineType"].ToString();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void hyLblRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            CreateUser registerForm = new CreateUser();
            registerForm.Show();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (Connect())
                {
                    lblIndicate.Appearance.ForeColor = Color.Green;
                    LoadLookupEdits();
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

        public void LoadLookupEdits()
        {
            OrganizationScheme _oraganization = new OrganizationScheme();
            CommonFunctions.LoadLookupControls(lookUpEditSegments, _oraganization);

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
                #region WebPost Method

                //int Segment = 2;
                //string Uname = textEditUsername.EditValue.ToString();
                //string Pass = textEditPassword.EditValue.ToString();

                //using (var client = new HttpClient())
                //{
                //    client.BaseAddress = new Uri(BaseURL);
                //    client.DefaultRequestHeaders.Accept.Clear();
                //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //    var data = @"{""SegmentID"": Segment ,""Username"": Uname.ToString(),""Password"": Pass.ToString()}";
                //    var content = new StringContent(data, Encoding.UTF8, "application/json");
                //    response = await client.PostAsync("api/Auth/login", content);

                //    if (response.IsSuccessStatusCode)
                //    {
                       
                //    }
                //    else
                //    {
                //        MessageBox.Show(response.StatusCode.ToString());
                //    }
                //}

                #endregion MyRegion

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(BaseURL + "api/Auth/login");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new

                StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        SegmentID = (int)lookUpEditSegments.EditValue,
                        Username = textEditUsername.EditValue.ToString(),
                        Password = textEditPassword.EditValue.ToString()
                    });

                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                webPostMethod();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}