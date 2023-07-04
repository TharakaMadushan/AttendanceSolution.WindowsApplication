using System.Reflection;
using System.Web;
using System.Configuration;
using System;

namespace HRMS.DataLayer
{
    /// <summary>
    /// Developed By: Dhammika Devarathne
    /// Edited By: Dhammika Devarathne
    /// Edited Date: 03 May 2013
    /// Version: 1.0
    /// </summary>
    public class DBProvider
    {
        public  enum ConnectionProperty
        {
            Database,
            Server,
            UserID,
            Password
        }

        string DecryptedConnetionString = "";
        /// <summary>
        /// returns the string value of the current app connection string 
        /// </summary>
        /// <param name="PropertyType"></param>
        /// <returns></returns>
        public static string GetConnectionStringProperty(ConnectionProperty PropertyType)
        {
            string _strProperty = string.Empty;
            try
            {
                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
                builder.ConnectionString = GetPerfectConnStr;

                switch (PropertyType)
                {
                    case ConnectionProperty.Database:
                        _strProperty = builder.InitialCatalog;
                        break;
                    case ConnectionProperty.Server:
                        _strProperty = builder.DataSource;
                        break;
                    case ConnectionProperty.UserID:
                        _strProperty = builder.UserID;
                        break;
                    case ConnectionProperty.Password:
                        _strProperty = builder.Password;
                        break;
                }
            }
            catch (System.Exception ex)
            {
                
                //throw;
            }
            return _strProperty;
        }

        public static string GetPerfectConnStr
        {
            
            get
            {
               // return ConfigurationManager.ConnectionStrings["DBConn"].ToString();
                string connectionStringerror = "";
                string connectionString = "";
                Configuration configuration = null;
                    try
                    {

                        //if(DecryptedConnetionString.e)

                        string applicationName =Environment.GetCommandLineArgs()[0];

                // Open the configuration file and retrieve the connectionStrings section.
                      configuration = ConfigurationManager.OpenExeConfiguration(applicationName);
                ConnectionStringsSection configSection = configuration.GetSection("connectionStrings") as ConnectionStringsSection;
                if ((!(configSection.ElementInformation.IsLocked)) && (!(configSection.SectionInformation.IsLocked)))
                {
                    if (!configSection.SectionInformation.IsProtected)
                    {
                        //this line will encrypt the file
                        configSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                        configuration.Save(ConfigurationSaveMode.Modified);
                    }

                    if (configSection.SectionInformation.IsProtected)//encrypt is true so encrypt
                    {
                        //this line will decrypt the file. 
                        configSection.SectionInformation.UnprotectSection();

                       
                        if (ConfigurationManager.ConnectionStrings.Count > 0)
                        {
                            //connectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                            connectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                            connectionStringerror = connectionString;
                            string DecryptedConnetionString = connectionString;


                            configSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                            configSection.SectionInformation.ForceSave = true;
                            // Save the current configuration

                            
                            configuration = null;

                        }

                       
                    }


                }

                        //string connectionString = "";
                        //if (ConfigurationManager.ConnectionStrings.Count > 0)
                        //{
                        //    //connectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                        //    connectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                        //    connectionStringerror = connectionString;
                        //}

                        return connectionString;
                        //return "Data Source=EDINDATABASE\\SQLSERVER2014;Initial Catalog=HRIS;User ID=sa;Password=ed!nb0r0u9H;";

                    }
                    catch (System.Exception ex)
                    {
                        System.IO.StreamWriter _stwErrorLog = new System.IO.StreamWriter(System.Environment.CurrentDirectory + "\\" + "Errorlog_" + System.DateTime.Now.ToString("yyyyMMddmmHHss")+".log" );
                        _stwErrorLog.Write("Error : " + connectionStringerror + "" + ex.Message.ToString() + System.Environment.NewLine + "Stack Trace:" + ex.StackTrace.ToString());
                        _stwErrorLog.Close();
                        _stwErrorLog.Dispose();


                        throw;
                    }

               

            }
        }

        public string GetPerfectConnString()
        {

        
                // return ConfigurationManager.ConnectionStrings["DBConn"].ToString();
                string connectionStringerror = "";
                string connectionString = "";
                Configuration configuration = null;
                try
                {

                    if (DecryptedConnetionString == string.Empty)
                    {

                        string applicationName = Environment.GetCommandLineArgs()[0];

                        // Open the configuration file and retrieve the connectionStrings section.
                        configuration = ConfigurationManager.OpenExeConfiguration(applicationName);
                        ConnectionStringsSection configSection = configuration.GetSection("connectionStrings") as ConnectionStringsSection;
                        if ((!(configSection.ElementInformation.IsLocked)) && (!(configSection.SectionInformation.IsLocked)))
                        {
                            if (!configSection.SectionInformation.IsProtected)
                            {
                                //this line will encrypt the file
                                configSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                                configuration.Save(ConfigurationSaveMode.Modified);
                            }

                            if (configSection.SectionInformation.IsProtected)//encrypt is true so encrypt
                            {
                                //this line will decrypt the file. 
                                configSection.SectionInformation.UnprotectSection();


                                if (ConfigurationManager.ConnectionStrings.Count > 0)
                                {
                                    //connectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                                    connectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                                    connectionStringerror = connectionString;
                                     DecryptedConnetionString = connectionString;


                                    configSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                                    configSection.SectionInformation.ForceSave = true;
                                    // Save the current configuration


                                    configuration = null;

                                }


                            }


                        }

                        //string connectionString = "";
                        //if (ConfigurationManager.ConnectionStrings.Count > 0)
                        //{
                        //    //connectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                        //    connectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                        //    connectionStringerror = connectionString;
                        //}

                        return connectionString;
                        //return "Data Source=EDINDATABASE\\SQLSERVER2014;Initial Catalog=HRIS;User ID=sa;Password=ed!nb0r0u9H;";
                    }
                    else {
                        return DecryptedConnetionString;
                    
                    }
                }
                catch (System.Exception ex)
                {
                    System.IO.StreamWriter _stwErrorLog = new System.IO.StreamWriter(System.Environment.CurrentDirectory + "\\" + "Errorlog_" + System.DateTime.Now.ToString("yyyyMMddmmHHss") + ".log");
                    _stwErrorLog.Write("Error : " + connectionStringerror + "" + ex.Message.ToString() + System.Environment.NewLine + "Stack Trace:" + ex.StackTrace.ToString());
                    _stwErrorLog.Close();
                    _stwErrorLog.Dispose();
                    throw;
                }



            }
        }
}
