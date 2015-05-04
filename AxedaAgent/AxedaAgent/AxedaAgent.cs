// -----------------------------------------------------------------------------
//           $Copyright: (C) Copyright by Bayer Healthcare 2013. All rights reserved. $
// -----------------------------------------------------------------------------
// 
//  $Source: AxedaAgent/AxedaAgent.cs $
//  $Revision: 1.4 $
//  $Date: 2012/10/26 15:07:15EDT $
//  $Author: MLHAY $
//  $ProjectName: g:/MKS_Files/Service_Engineering/ApplicationFramework/AxedaAgent/project.pj $
// 
// -----------------------------------------------------------------------------

namespace Medrad.ServiceEngineering.ApplicationFramework.Axeda.Agent
{
	using Medrad.ServiceEngineering.ApplicationFramework.Axeda.Agent;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Security.Permissions;
    using System.ServiceProcess;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Holds the General Axeda Configuration Data
    /// </summary>
    public class AxedaAgent : INotifyPropertyChanged
    {
        #region Constants and Fields

        /// <summary>
        /// Holds the reference to the Axeda Agent
        /// </summary>
        private readonly AgentWs agentWs;
		/// <summary>
		/// Holds the reference to the Axeda Agent File Path Config
		/// </summary>
		private readonly AxedaFilePathConfiguration afpc;

        /// <summary>
        /// Holds the Axeda license file's data.
        /// </summary>
        private LicenseFile licenseFile;

        /// <summary>
        /// Holds the Axeda enterprise proxy file's data.
        /// </summary>
        private EnterpriseProxy enterpriseProxy;

        /// <summary>
        /// Holds the Axeda deployment file's data.
        /// </summary>
        private DeploymentConfiguration deploymentConfiguration;

        /// <summary>
        /// Holds the  type of proxy server being used
        /// </summary>
        private ProxyServerType proxyType;

        /// <summary>
        /// Holds the URL for the auto proxy configuration.
        /// </summary>
        private string autoProxyUrl;

        /// <summary>
        /// Holds the DRM server IP address or hostname 
        /// </summary>
        private string serverAddress;

        /// <summary>
        /// Holds the port number on DRM server
        /// </summary>
        private int? serverPortNumber;

        /// <summary>
        /// Holds the encryption type
        /// </summary>
        private EncryptionType encryptionType;

        /// <summary>
        /// Holds the proxy IP address or hostname
        /// </summary>
        private string proxyAddress;

        /// <summary>
        /// Holds the proxy port number
        /// </summary>
        private int? proxyPortNumber;

        /// <summary>
        /// Holds the proxy user name
        /// </summary>
        private string proxyUserName;

        /// <summary>
        /// Holds the encrypted proxy password
        /// </summary>
        private string proxyEncryptedPassword;

        /// <summary>
        /// Holds the Ssl encryption level
        /// </summary>
        private SslEncryptionLevel sslEncryptionLevel;

        /// <summary>
        /// Holds the Ssl server authentication flag
        /// </summary>
        private bool? sslAuthenticationEnabled;

        /// <summary>
        /// Holds the name of the database
        /// </summary>
        private string databaseName;

        /// <summary>
        /// Holds the "enable Ssl" flag.
        /// </summary>
        private bool? sslEnabled;

        /// <summary>
        /// Holds the model name for the Axeda device.
        /// </summary>
        private string modelName;

        /// <summary>
        /// Holds the serial number for the Axeda device.
        /// </summary>
        private string serialNumber;
        
        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AxedaAgent"/> class.
        /// </summary>
        public AxedaAgent()
        {
            // First Initialize the Axeda Agent functionality
			this.afpc = AxedaFilePathConfiguration.GetFilePaths ();
         	this.agentWs = AgentWs.GetInstance();
           	this.RefreshData();
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the proxy type for the device
        /// </summary>
        /// <value>
        /// The type of the proxy.
        /// </value>
        public ProxyServerType ProxyType
        {
            get
            {
                return this.proxyType;
            }

            set
            {
                if (value != this.proxyType)
                {
                    this.proxyType = value;
                    this.NotifyPropertyChanged("ProxyType");
                }
            }
        }

        /// <summary>
        /// Gets or sets the auto proxy script url.
        /// </summary>
        /// <value>
        /// The auto proxy URL.
        /// </value>
        /// <TODO>Update to a URI prior to allowing modification of proxy server url.</TODO>
        /// <remarks>
        /// For ease of use, we're making this URL a string opposed to a URI.  A ToDo will be placed here to
        /// update the code in the future to use a URI.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
                                                         "CA1056:UriPropertiesShouldNotBeStrings",
                                                         Justification = "This is used for display only right now.")]
        public string AutoProxyUrl
        {
            get
            {
                return this.autoProxyUrl;
            }

            set
            {
                if (value != this.autoProxyUrl)
                {
                    // We'll do a little bit of house cleaning to make sure we're setting
                    // a valid web address.  First, we'll make sure all slashes are going in the 
                    // the right direction and we have no extra whitespace.  
                    value = value.Replace('\\', '/').Trim();

                    // Next, well make sure we have HTTP or HTTPS prepended to the URL (unless we are
                    // dealing with an empty string.  We will make everything to uppercase to make our 
                    // search easier.  
                    if ((value.ToUpper().Contains("HTTP://") == false) &&
                        (value.ToUpper().Contains("HTTPS://") == false) &&
                        (string.IsNullOrEmpty(value) == false))
                    {
                        // We were missing a prefix, so we'll standardize and use http
                        value = "http://" + value;
                    } 

                    this.autoProxyUrl = value;
                    this.NotifyPropertyChanged("AutoProxyUrl");
                }
            }
        }

        /// <summary>
        /// Gets or sets the DRM server IP address or hostname 
        /// </summary>
        public string ServerAddress
        {
            get
            {
                return this.serverAddress;
            }

            set
            {
                if (value != this.serverAddress)
                {
                    this.serverAddress = value;
                    this.NotifyPropertyChanged("ServerAddress");
                }
            }
        }

        /// <summary>
        /// Gets or sets the port number on DRM server
        /// </summary>
        /// <value>
        /// The server port number.
        /// </value>
        public int? ServerPortNumber
        {
            get
            {
                return this.serverPortNumber;
            }

            set
            {
                if (value != this.serverPortNumber)
                {
                    this.serverPortNumber = value;
                    this.NotifyPropertyChanged("ServerPortNumber");
                }
            }
        }

        /// <summary>
        /// Gets or sets the encryption type
        /// </summary>
        /// <value>
        /// The type of the encryption.
        /// </value>
        public EncryptionType EncryptionType
        {
            get
            {
                return this.encryptionType;
            }

            set
            {
                if (value != this.encryptionType)
                {
                    this.encryptionType = value;
                    this.NotifyPropertyChanged("EncryptionType");
                }
            }
        }

        /// <summary>
        /// Gets or sets the proxy IP address or hostname
        /// </summary>
        /// <value>
        /// The proxy address.
        /// </value>
        public string ProxyAddress
        {
            get
            {
                return this.proxyAddress;
            }

            set
            {
                if (value != this.proxyAddress)
                {
                    this.proxyAddress = value;
                    this.NotifyPropertyChanged("ProxyAddress");
                }
            }
        }

        /// <summary>
        /// Gets or sets the proxy port number
        /// </summary>
        /// <value>
        /// The proxy port number.
        /// </value>
        public int? ProxyPortNumber
        {
            get
            {
                return this.proxyPortNumber;
            }

            set
            {
                if (value != this.proxyPortNumber)
                {
                    this.proxyPortNumber = value;
                    this.NotifyPropertyChanged("ProxyPortNumber");
                }
            }
        }

        /// <summary>
        /// Gets or sets the HTTP proxy user name
        /// </summary>
        /// <value>
        /// The name of the proxy user.
        /// </value>
        public string ProxyUserName
        {
            get
            {
                return this.proxyUserName;
            }

            set
            {
                if (value != this.proxyUserName)
                {
                    this.proxyUserName = value;
                    this.NotifyPropertyChanged("ProxyUserName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the HTTP proxy password (encrypted)
        /// </summary>
        /// <value>
        /// The proxy encrypted password.
        /// </value>
        public string ProxyEncryptedPassword
        {
            get
            {
                return this.proxyEncryptedPassword;
            }

            set
            {
                if (value != this.proxyEncryptedPassword)
                {
                    this.proxyEncryptedPassword = value;
                    this.NotifyPropertyChanged("ProxyEncryptedPassword");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Ssl encryption level.
        /// </summary>
        /// <value>
        /// The SSL encryption level.
        /// </value>
        public SslEncryptionLevel SslEncryptionLevel
        {
            get
            {
                return this.sslEncryptionLevel;
            }

            set
            {
                if (value != this.sslEncryptionLevel)
                {
                    this.sslEncryptionLevel = value;
                    this.NotifyPropertyChanged("SslEncryptionLevel");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Ssl server authentication flag.
        /// </summary>
        /// <value>
        /// The SSL authentication enabled flag.
        /// </value>
        public bool? SslAuthenticationEnabled
        {
            get
            {
                return this.sslAuthenticationEnabled;
            }

            set
            {
                if (value != this.sslAuthenticationEnabled)
                {
                    this.sslAuthenticationEnabled = value;
                    this.NotifyPropertyChanged("SslAuthenticationEnabled");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>
        /// The name of the database.
        /// </value>
        public string DatabaseName
        {
            get
            {
                return this.databaseName;
            }

            set
            {
                if (value != this.databaseName)
                {
                    this.databaseName = value;
                    this.NotifyPropertyChanged("DatabaseName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the "enable Ssl" flag.
        /// </summary>
        /// <value>
        /// The SSL enabled.
        /// </value>
        public bool? SslEnabled
        {
            get
            {
                return this.sslEnabled;
            }

            set
            {
                if (value != this.sslEnabled)
                {
                    this.sslEnabled = value;
                    this.NotifyPropertyChanged("SslEnabled");
                }
            }
        }

        /// <summary>
        /// Gets or sets the model name for the Axeda device.
        /// </summary>
        /// <value>
        /// The name of the model.
        /// </value>
        public string ModelName
        {
            get
            {
                return this.modelName;
            }

            set
            {
                if (value != this.modelName)
                {
                    this.modelName = value;
                    this.NotifyPropertyChanged("ModelName");
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the serial number for the Axeda device.
        /// </summary>
        /// <value>
        /// The serial number.
        /// </value>
        public string SerialNumber
        {
            get
            {
                return this.serialNumber;
            }

            set
            {
                if (value != this.serialNumber)
                {
                    this.serialNumber = value;
                    this.NotifyPropertyChanged("SerialNumber");
                }
            }
        }
        
        /// <summary>
        /// Gets the agent data
        /// </summary>
        public AgentStatus AgentStatus
        {
            get
            {
                this.agentWs.Refresh();
                return this.agentWs.Status;
            }
        }

        #endregion
        
        #region Public Methods and Operators

        /// <summary>
        /// Restarts the agent.
        /// </summary>
        public void RestartAgent()
        {
          	this.StopAgent();
            StartAgent();
        }

        /// <summary>
        /// Refreshes the agent data
        /// </summary>
        /// <returns>
        /// The agent extensions version.
        /// </returns>
        public string AgentExtensionsVersion()
        {
            this.agentWs.Refresh();
            return this.agentWs.AgentExtensionVersion;
        }

        /// <summary>
        /// Refreshes the data.
        /// </summary>
        public void RefreshData()
        {
            this.licenseFile = new LicenseFile();
            this.enterpriseProxy = new EnterpriseProxy();
            this.deploymentConfiguration = new DeploymentConfiguration();
            this.agentWs.Refresh();
            this.InitializeData(this.deploymentConfiguration, this.enterpriseProxy, this.licenseFile);
        }

        /// <summary>
        /// Prints the data.
        /// </summary>
        public void PrintData()
        {
            Console.WriteLine(this.licenseFile);
            Console.WriteLine(this.enterpriseProxy);
            Console.WriteLine(this.deploymentConfiguration);
            Console.WriteLine(this.agentWs.ToString());
            Console.WriteLine(this.ToString());
        }

        /// <summary>
        /// Writes the status of the GUI to the Axeda Agent.   
        /// </summary>
        /// <param name="message">The message to be written.</param>
        /// <returns>The status of the write</returns>
        public AgentDataWriteStatus WriteGuiStatus(string message)
        {
            return this.agentWs.WriteStringDataProperty("VirtualCare.InjectorGuiStatus", message);
        }

        /// <summary>
        /// Writes the status of the GUI to the Axeda Agent.
        /// </summary>
        /// <returns>
        /// The status of the write
        /// </returns>
        public AgentDataWriteStatus RequestSupportTechnical()
        {
            ProvisioningData provisioningData = new ProvisioningData();
            string message = "Contact Name: " + provisioningData.ClinicalContactName + System.Environment.NewLine +
                             "Contact Number:" + provisioningData.ClinicalPhoneAndExtension; 
            return this.agentWs.WriteStringDataProperty("VirtualCare.RequestSupportTechnical", message);
        }

        /// <summary>
        /// Writes the status of the GUI to the Axeda Agent.
        /// </summary>
        /// <param name="contactName">
        /// The contact name for support to contact.
        /// </param>
        /// <param name="phoneNumberAndExtension">
        /// The phone number for support to contact.
        /// </param>
        /// <returns>
        /// The status of the write
        /// </returns>
        public AgentDataWriteStatus RequestSupportTechnical(string contactName, string phoneNumberAndExtension)
        {
            string message = "Contact Name: " + contactName + System.Environment.NewLine +
                             "Contact Number:" + phoneNumberAndExtension;
            return this.agentWs.WriteStringDataProperty("VirtualCare.RequestSupportTechnical", message);
        }

        /// <summary>
        /// Determines whether the request technical support acknowledged [the specified status].
        /// </summary>
        /// <returns>
        /// Response value.  The response will be "Acknowledged" when the agent has handled the status.  
        /// </returns>
        public AgentReadResponse ReadSupportTechnicalResponse()
        {
            return this.agentWs.ReadStringDataProperty("VirtualCare.RequestSupportTechnical");
        }

        /// <summary>
        /// Determines whether the request technical support acknowledged [the specified status].
        /// </summary>
        /// <returns>
        /// Response value.  The response will be "Acknowledged" when the agent has handled the status.  
        /// </returns>
        public bool IsSupportTechnicalRequestAcknowledged()
        {
            var response = this.agentWs.ReadStringDataProperty("VirtualCare.RequestSupportTechnical");
            if (response.Status == AgentDataReadStatus.Ok)
            {
                return response.Value == "Acknowledged"; 
            }

            // If we're still here, return false because we failed for some reason.  
            return false;
        }

        /// <summary>
        /// Requests the support other.
        /// </summary>
        /// <param name="contactName">
        /// The contact name for support to contact.
        /// </param>
        /// <param name="phoneNumberAndExtension">
        /// The phone number for support to contact.
        /// </param>
        /// <returns>
        /// The Status of the write
        /// </returns>
        public AgentDataWriteStatus RequestSupportOther(string contactName, string phoneNumberAndExtension)
        {
            string message = "Contact Name: " + contactName + System.Environment.NewLine +
                             "Contact Number:" + phoneNumberAndExtension; 
            return this.agentWs.WriteStringDataProperty("VirtualCare.RequestSupportOther", message);
        }

        /// <summary>
        /// Requests the support other.
        /// </summary>
        /// <returns>
        /// The Status of the write
        /// </returns>
        public AgentDataWriteStatus RequestSupportOther()
        {
            ProvisioningData provisioningData = new ProvisioningData();
            string message = "Contact Name: " + provisioningData.ClinicalContactName + System.Environment.NewLine + 
                             "Contact Number:" + provisioningData.ClinicalPhoneAndExtension; 
            return this.agentWs.WriteStringDataProperty("VirtualCare.RequestSupportOther", message);
        }

        /// <summary>
        /// Determines whether the request other support acknowledged [the specified status].
        /// </summary>
        /// <returns>
        /// Response value.  The response will be "Acknowledged" when the agent has handled the status.  
        /// </returns>
        public AgentReadResponse ReadSupportOtherResponse()
        {
            return this.agentWs.ReadStringDataProperty("VirtualCare.RequestSupportOther");
        }

        /// <summary>
        /// Determines whether the request other support acknowledged [the specified status].
        /// </summary>
        /// <returns>
        /// Response value.  The response will be "Acknowledged" when the agent has handled the status.  
        /// </returns>
        public bool IsSupportOtherRequestAcknowledged()
        {
            var response = this.agentWs.ReadStringDataProperty("VirtualCare.RequestSupportOther");
            if (response.Status == AgentDataReadStatus.Ok)
            {
                return response.Value == "Acknowledged";
            }

            // If we're still here, return false because we failed for some reason.  
            return false;
        }

        /// <summary>
        /// Sets the proxy password from a plain text password.
        /// </summary>
        /// <param name="clearTextPassword">The clear text password.</param>
        public void SetProxyPassword(string clearTextPassword)
        {
            string encryptedPassword = EncryptProxyPassword(clearTextPassword); 
            if (!string.IsNullOrEmpty(encryptedPassword))
            {
                this.ProxyEncryptedPassword = encryptedPassword; 
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Universal Configuration: \n" +
                   "\tServerAddress= " + this.ServerAddress + "\n" +
                   "\tServerPortNumber= " + this.ServerPortNumber + "\n" +
                   "\tEncryptionType= " + this.EncryptionType + "\n" +
                   "\tProxyType= " + this.ProxyType + "\n" +
                   "\tProxyAddress= " + this.ProxyAddress + "\n" +
                   "\tProxyPortNumber= " + this.ProxyPortNumber + "\n" +
                   "\tProxyUserName= " + this.ProxyUserName + "\n" +
                   "\tProxyEncryptedPassword= " + this.ProxyEncryptedPassword + "\n" +
                   "\tProxyPAC Address= " + this.AutoProxyUrl + "\n" +
                   "\tSslEncryptionLevel= " + this.SslEncryptionLevel + "\n" +
                   "\tSslAuthenticationFlag= " + this.SslAuthenticationEnabled + "\n" +
                   "\tEnableSSLFlag= " + this.SslEnabled + "\n" +
                   "\tDatabaseName= " + this.DatabaseName + "\n" +
                   "\tModelName = " + this.ModelName + "\n" +
                   "\tSerialNumber = " + this.SerialNumber;
        }

        /// <summary>
        /// Writes the deployment configuration.
        /// </summary>
        public void WriteDeploymentConfiguration()
        {
            this.WriteDeploymentConfiguration(AxedaFilePathConfiguration.GetFilePaths().DeployConfigPath); 
        }

        /// <summary>
        /// Writes the deployment configuration.
        /// </summary>
        /// <param name="deploymentFilePath">The deployment file path.</param>
        public void WriteDeploymentConfiguration(string deploymentFilePath)
        {          
            // If the caller passed an empty string, just set it to our default string
            if (string.IsNullOrEmpty(deploymentFilePath))
            {
                deploymentFilePath = AxedaFilePathConfiguration.GetFilePaths().DeployConfigPath; 
            }

            // Remove the old deployment file if it exists
            if (File.Exists(deploymentFilePath))
            {
                File.Delete(deploymentFilePath);
            }

            // We'll write out deployment configuration
            using (var outfile = new StreamWriter(deploymentFilePath))
            {
                outfile.Write(this.ToXmlString());
            }

            // Now we'll add the protection to the file to prevent the use of a tampered file
            ProtectDeploymentFile(deploymentFilePath);
        }
        #endregion

        #region Methods


        /// <summary>
        /// Starts the agent.
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static void StartAgent()
		{
			bool status = false;
			int count = 0;
			string scriptFile = "/etc/init.d/S72-axeda-gateway";
			if (!File.Exists (scriptFile)) 
			{
				throw new FileNotFoundException (scriptFile + " not found");
			}
				while (!status && count <= 10)
				{
					using (var process = new Process())
					{
						process.StartInfo.FileName = "/bin/sh";
						process.StartInfo.Arguments = scriptFile + " start";
						process.Start();
						process.WaitForExit();
					}
					Thread.Sleep(5000);
					if (Process.GetProcessesByName("xGate").Length > 0)
					{
						status = true;
						return;
					}
					if (count == 10)
						throw new Exception("cannot start Axeda Gateway Agent");
					count++;            
				}
		}



        /// <summary>
        /// Encrypts the password.
        /// </summary>
        /// <param name="clearPassword">The clear password.</param>
        /// <returns>The encrypted password</returns>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static string EncryptProxyPassword(string clearPassword)
        {
            string EncryptionToolPath = AxedaFilePathConfiguration.GetFilePaths().EncryptionToolPath;
            string encryptedPassword;

            // Check to see if the password is empty.  If it is, return nothing.  
            if (string.IsNullOrEmpty(clearPassword))
            {
                return string.Empty;
            }

            if (!File.Exists(EncryptionToolPath))
            {
                throw new FileNotFoundException(EncryptionToolPath + " not found");
            }

            // create a temporary file path for us to use
            var tempFilePath = Path.GetTempFileName();
            using (var outfile = new StreamWriter(tempFilePath))
            {
                outfile.Write(clearPassword);
            }

            // We want to span a new process to encrypt the password.  We can only do this 
            // through the console.  Note that we need to use the cmd /c parameter to allow all of our
            // command line parameters to be processed correctly (this is due to the piped input).  
            using (var p = new Process())
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WorkingDirectory = AxedaFilePathConfiguration.GetFilePaths().WorkingDirectory; 
                p.StartInfo.FileName = "/bin/sh";
                p.StartInfo.Arguments = "-c '" + EncryptionToolPath + " -encrypt -base64 -stdin < " + tempFilePath+"'";

                // We're going to start the process and read the re-directed stream output
                p.Start();
                encryptedPassword = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                // If we get a non-passing status, just return an empty string
                encryptedPassword = p.ExitCode != 0 ? string.Empty : encryptedPassword.Trim();
            }

            // Delete the temporary file file
            File.Delete(tempFilePath);

            // We will trim whitespace if this isn't null
            return encryptedPassword;
        }

        /// <summary>
        /// Encrypts the password.
        /// </summary>
        /// <param name="deploymentFile">The deployment file.</param>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static void ProtectDeploymentFile(string deploymentFile = "")
        {
            string EncryptionToolPath = AxedaFilePathConfiguration.GetFilePaths().EncryptionToolPath;

            // We'll want to do a bit of checking to make sure all we have the tool, and
            // we were passed a valid file.  
            if (string.IsNullOrEmpty(deploymentFile))
            {
                deploymentFile = AxedaFilePathConfiguration.GetFilePaths().DeployConfigPath;
            }
            if (string.IsNullOrEmpty(deploymentFile))
            {
                throw new InvalidDataException("Empty deployment file path.");
            }

            if (!File.Exists(deploymentFile))
            {
                throw new FileNotFoundException(deploymentFile + " not found.");
            }

            if (!File.Exists(EncryptionToolPath))
            {
                throw new FileNotFoundException(EncryptionToolPath + " not found.");
            }

            // We'll set up our new file.  If it already exists, we'll do it.  
            string dataFilePath = Path.ChangeExtension(deploymentFile, ".dat");
            if (File.Exists(dataFilePath))
            {
                File.Delete(dataFilePath);
            }

            // We want to span a new process to encrypt the password.  We can only do this 
            // through the console.  Note that we need to use the cmd /c parameter to allow all of our
            // command line parameters to be processed correctly (this is due to the piped input).  
            using (var p = new Process())
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = false;
                p.StartInfo.FileName = "/bin/sh";
                p.StartInfo.Arguments = @"-c '" + EncryptionToolPath
                                        + " -target DeployConfig -encrypt -mac -stdin < " + deploymentFile
                                        + " > " + dataFilePath+"'";
                p.StartInfo.WorkingDirectory = AxedaFilePathConfiguration.GetFilePaths().WorkingDirectory;

                // We're going to start the process and read the re-directed stream output
                p.Start();
                p.WaitForExit();
            }

            // We made it this far, so we should be good!
        }

        /// <summary>
        /// Stops the agent.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes"), 
         PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private void StopAgent()
        {

            // We'll do a quick test to make sure the agent isn't already stopped.  If it is, we'll
            // just move on as there is nothing else to do.  
                if (Process.GetProcessesByName("xGate").Length <= 0)
                {
                    return;
                }

            // We'll try to stop the agent with the web service methods first
            //if (!this.agentWs.StopServiceAgent())
            //{
                // Holds the patch to the shutdown application from Axeda
                string ShutdownToolPath = AxedaFilePathConfiguration.GetFilePaths().ShutdownToolPath;

                // If the Web Service call fails, just try the old fashion way to stop it.  
                if (!File.Exists(ShutdownToolPath))
                {
                    throw new FileNotFoundException(ShutdownToolPath + " not found");
                }

                // We want to spawn a new process to shutdown the Axeda Gateway.
                using (var p = new Process())
                {
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.FileName = "/bin/sh";
                    p.StartInfo.Arguments = "-c '" + ShutdownToolPath + " 127.0.0.1'";

                    // We're going to start the process and wait for it to complete
                    p.Start();
                    p.WaitForExit();
                }
            //}

            // Now that we've made the call to stop the Agent, we'll spin until it is complete or we timeout.
			bool status = false;
			int count = 0;
			string scriptFile = "/etc/init.d/S72-axeda-gateway";
			if (!File.Exists (scriptFile)) 
			{
				throw new FileNotFoundException (scriptFile + " not found");
			}
			if (Process.GetProcessesByName("xGate").Length <= 0)
			{
				return;
			}
			else
			{
				while (!status && count <= 10)
				{
					using (var process = new Process())
					{
						process.StartInfo.FileName = "/bin/sh";
						process.StartInfo.Arguments = scriptFile + " stop";
						process.Start();
						process.WaitForExit();
					}
					Thread.Sleep(5000);
					if (Process.GetProcessesByName("xGate").Length <= 0)
					{
						status = true;
						return;
					}
					if (count == 10)
						throw new Exception("cannot stop Axeda Gateway Agent");
					count++;            
				}

			}
          }

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="info">The info regarding the changed property.</param>
        private void NotifyPropertyChanged(string info)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        /// <summary>
        /// Initializes the data data in the Agent configuration.
        /// </summary>
        /// <param name="inDeployConfig">The deploy config.</param>
        /// <param name="inEnterpriseProxy">The enterprise proxy.</param>
        /// <param name="inLicenseFile">The license file.</param>
        private void InitializeData(DeploymentConfiguration inDeployConfig, EnterpriseProxy inEnterpriseProxy, LicenseFile inLicenseFile)
        {
            // To Ease the configuration process, we'll go in stages to build a final configuration.  The Axeda Agent 
            // Configures itself based on very specific setup files, so we'll read our representations and set the values as 
            // needed.

            // First, we'll read the shortest configuration, which is the license file.  
            this.ModelName = string.IsNullOrEmpty(inDeployConfig.ModelName)
                            ? inLicenseFile.ModelName
                            : inDeployConfig.ModelName;

            this.SerialNumber = string.IsNullOrEmpty(inDeployConfig.SerialNumber)
                            ? inLicenseFile.SerialNumber
                            : inDeployConfig.SerialNumber;

            // Second, we'll read the pre-configured settings from the enterprise proxy.  We won't worry about checking for 
            // nulls or any other bounds checking at this point, we really just want to populate our initial "universal" 
            // configuration.
            if (inDeployConfig.ProxyHttpUsed.HasValue && inDeployConfig.ProxyHttpUsed.Value)
            {
                this.ProxyType = ProxyServerType.Http;
                this.ProxyAddress = inDeployConfig.ProxyHttpAddress;
                this.ProxyPortNumber = inDeployConfig.ProxyHttpPortNumber;
                this.ProxyUserName = inDeployConfig.ProxyHttpUserName;
                this.ProxyEncryptedPassword = inDeployConfig.ProxyHttpEncryptedPassword;
                this.AutoProxyUrl = null;
            }
            else if (inDeployConfig.ProxySocksUsed.HasValue && inDeployConfig.ProxySocksUsed.Value)
            {
                this.ProxyType = ProxyServerType.Socks;
                this.ProxyAddress = inDeployConfig.ProxySocksAddress;
                this.ProxyPortNumber = inDeployConfig.ProxySocksPortNumber;
                this.ProxyUserName = inDeployConfig.ProxySocksUserName;
                this.ProxyEncryptedPassword = inDeployConfig.ProxySocksEncryptedPassword;
                this.AutoProxyUrl = null;
            }
            else if (!string.IsNullOrEmpty(inDeployConfig.AutoProxyUrl))
            {
                this.ProxyType = ProxyServerType.Auto;
                this.ProxyAddress = null;
                this.ProxyPortNumber = null;
                this.ProxyUserName = null;
                this.ProxyEncryptedPassword = null;
                this.AutoProxyUrl = inDeployConfig.AutoProxyUrl;
            }
            else if (inEnterpriseProxy.ProxyHttpUsed.HasValue && inEnterpriseProxy.ProxyHttpUsed.Value)
            {
                this.ProxyType = ProxyServerType.Http;
                this.ProxyAddress = inEnterpriseProxy.ProxyHttpAddress;
                this.ProxyPortNumber = inEnterpriseProxy.ProxyHttpPortNumber;
                this.ProxyUserName = inEnterpriseProxy.ProxyHttpUserName;
                this.ProxyEncryptedPassword = inEnterpriseProxy.ProxyHttpEncryptedPassword;
                this.AutoProxyUrl = null;
            }
            else if (inEnterpriseProxy.ProxySocksUsed.HasValue && inEnterpriseProxy.ProxySocksUsed.Value)
            {
                this.ProxyType = ProxyServerType.Socks;
                this.ProxyAddress = inEnterpriseProxy.ProxySocksAddress;
                this.ProxyPortNumber = inEnterpriseProxy.ProxySocksPortNumber;
                this.ProxyUserName = inEnterpriseProxy.ProxySocksUserName;
                this.ProxyEncryptedPassword = inEnterpriseProxy.ProxySocksEncryptedPassword;
                this.AutoProxyUrl = null;
            }
            else
            {
                this.ProxyType = ProxyServerType.None;
                this.ProxyAddress = null;
                this.ProxyPortNumber = null;
                this.ProxyUserName = null;
                this.ProxyEncryptedPassword = null;
                this.AutoProxyUrl = null;
            }

            this.ServerAddress = string.IsNullOrEmpty(inDeployConfig.ServerAddress)
                                ? inEnterpriseProxy.ServerAddress
                                : inDeployConfig.ServerAddress;

            this.ServerPortNumber = inDeployConfig.ServerPortNumber.HasValue
                                   ? inDeployConfig.ServerPortNumber
                                   : inEnterpriseProxy.ServerPortNumber;

            this.DatabaseName = string.IsNullOrEmpty(inDeployConfig.DatabaseName)
                                ? inEnterpriseProxy.DatabaseName
                                : inDeployConfig.DatabaseName;

            this.SslEnabled = inDeployConfig.EnableSslFlag.HasValue
                                ? inDeployConfig.EnableSslFlag
                                : inEnterpriseProxy.EnableSslFlag;

            this.SslAuthenticationEnabled = inDeployConfig.SslAuthenticationFlag.HasValue
                                            ? inDeployConfig.SslAuthenticationFlag
                                            : inEnterpriseProxy.SslAuthenticationFlag;

            this.SslEncryptionLevel = inDeployConfig.SslEncryptionLevel != SslEncryptionLevel.Unknown
                                        ? inDeployConfig.SslEncryptionLevel
                                        : inEnterpriseProxy.SslEncryptionLevel;

            this.EncryptionType = inDeployConfig.EncryptionType.Equals(EncryptionType.Unknown)
                                  ? inEnterpriseProxy.EncryptionType
                                  : inDeployConfig.EncryptionType;
        }

        /// <summary>
        /// Initializes the data data in the Agent configuration.
        /// </summary>
        /// <returns> The well-formed Axeda XML representation of the Deployment parameters.
        /// </returns>
        /// <remarks>
        /// This function will provide default values for the non-configurable parameters.   
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Axeda Expects Lower Case Values")]
        private string ToXmlString()
        {
            var output = new StringBuilder();
            output.Append(@"<AgentRequest v=""1.0"" destination=""agent"">");
            output.Append(@"<UpdateConfiguration>"); 
            output.Append(@"<Configurations>");
            output.Append(@"<Config>");
            output.Append(@"<Network>");
            output.Append(@"<Connection/>");
            output.Append(@"<ProxyServer>");

            if (this.ProxyType == ProxyServerType.None)
            {
                output.Append(@"<HTTP enabled=""false""/>");
                output.Append(@"<SOCKS enabled=""false""/>");
            }
            else if (this.ProxyType == ProxyServerType.Auto)
            {
                output.Append(@"<PAC url=""" + this.autoProxyUrl + @""">");

                // Add the Authentication if Needed
                if (!string.IsNullOrEmpty(this.ProxyUserName) &&
                    !string.IsNullOrEmpty(this.ProxyEncryptedPassword))
                {
                    output.Append(@"<Authentication user=""" + this.ProxyUserName + @""" ");
                    output.Append(@"password=""" + this.ProxyEncryptedPassword + @"""/>");
                }

                // Finish the statement.
                output.Append(@"</PAC>"); 
                output.Append(@"<HTTP enabled=""false""/>");
                output.Append(@"<SOCKS enabled=""false""/>");
            }
            else if (this.ProxyType == ProxyServerType.Http)
            {
                output.Append(@"<HTTP enabled=""true""  "); 
                output.Append(@"host=""" + this.ProxyAddress + @""" ");
                output.Append(@"port=""" + this.ProxyPortNumber + @""">");

                // Add the Authentication if Needed
                if (!string.IsNullOrEmpty(this.ProxyUserName) &&
                    !string.IsNullOrEmpty(this.ProxyEncryptedPassword))
                {
                    output.Append(@"<Authentication user=""" + this.ProxyUserName + @""" ");
                    output.Append(@"password=""" + this.ProxyEncryptedPassword + @"""/>");
                }

                // Finish the statement.
                output.Append(@"</HTTP>"); 
                output.Append(@"<SOCKS enabled=""false""/>");
            }
            else if (this.ProxyType == ProxyServerType.Socks)
            {
                output.Append(@"<HTTP enabled=""false""/>");
                output.Append(@"<SOCKS enabled=""true""  "); 
                output.Append(@"host=""" + this.ProxyAddress + @""" ");
                output.Append(@"port=""" + this.ProxyPortNumber + @""">");
                
                // Add the Authentication if Needed
                if (!string.IsNullOrEmpty(this.ProxyUserName) &&
                    !string.IsNullOrEmpty(this.ProxyEncryptedPassword))
                {
                    output.Append(@"<Authentication user=""" + this.ProxyUserName + @""" ");
                    output.Append(@"password=""" + this.ProxyEncryptedPassword + @"""/>");
                }

                // Finish the statement.
                output.Append(@"</SOCKS>"); 
            }

            output.Append(@"</ProxyServer>");
            output.Append(@"<HTTPConfig timeout=""300"" setting=""1.1""/>");
            output.Append(@"</Network>");
            output.Append(@"<ProjectSettings>");
            output.Append(@"<EnterpriseServers>");
            output.Append(@"<SSL enabled=""" + this.SslEnabled.ToString().ToLowerInvariant() + @""" ");  
            output.Append(@"strength=""" + ConversionUtils.SslLevelToString(this.SslEncryptionLevel) + @""" ");
            output.Append(@"certificate=""" + this.SslAuthenticationEnabled.ToString().ToLowerInvariant() + @"""/>");         
            output.Append(@"<Primary host=""" + this.ServerAddress + @""" "); 
            output.Append(@"port=""" + this.ServerPortNumber + @""" ");
            output.Append(@"db=""drm-data_source"" ");
            output.Append(@"encryption=""" + this.EncryptionType.ToString().ToLowerInvariant() + @"""/>");
            output.Append(@"</EnterpriseServers>");
            output.Append(@"<APM enabled=""false""/>");
            output.Append(@"<Identification mn=""" + this.ModelName + @""" sn=""" + this.SerialNumber + @"""/>");
            output.Append(@"<AgentAuthentication user="""" password=""""/>");
            output.Append(@"<AutoDiscovery>");
            output.Append(@"<PlugIn name=""SNMP"" enabled=""false"" />");
            output.Append(@"</AutoDiscovery>");
            output.Append(@"<AuthProxyConfig audit=""yes"">");
            output.Append(@"<AuthProviders>");
            output.Append(@"<AuthProvider enabled=""false"" method=""radius""/>");
            output.Append(@"</AuthProviders>");
            output.Append(@"</AuthProxyConfig>");
            output.Append(@"</ProjectSettings>");
            output.Append(@"<CustomInfos>");
            output.Append(@"<CustomInfo id=""SNMP"">");
            output.Append(@"<SNMPConfig debug=""false"">");
            output.Append(@"<SNMPProfiles/>");
            output.Append(@"<SNMPDeviceGroups/>");
            output.Append(@"<SNMPDiscovery broadcast=""false"">");
            output.Append(@"<ExcludedAddresses/>");
            output.Append(@"</SNMPDiscovery>");
            output.Append(@"</SNMPConfig>");
            output.Append(@"</CustomInfo>");
            output.Append(@"<CustomInfo id=""LocalPolicyManager"">");
            output.Append(@"<LocalPolicies enabled=""false""/>");
            output.Append(@"</CustomInfo>");
            output.Append(@"</CustomInfos>");
            output.Append(@"<ManagedDevices/>");
            output.Append(@"</Config>");
            output.Append(@"</Configurations>");
            output.Append(@"</UpdateConfiguration>");
            output.Append(@"</AgentRequest>");

            return output.Replace(Environment.NewLine, string.Empty).ToString(); 
        }

        #endregion
    }
}
