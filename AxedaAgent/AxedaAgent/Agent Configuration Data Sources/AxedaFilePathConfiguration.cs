using System;
using System.Reflection;
using System.IO;
using System.Xml;

namespace Medrad.ServiceEngineering.ApplicationFramework.Axeda.Agent
{
    public class AxedaFilePathConfiguration
    {
        /// <summary>
        /// Holds the the singleton for the Axeda File Paths class
        /// </summary>
        private static AxedaFilePathConfiguration instance;

        /// <summary>
        /// Default File Path for config file
        /// </summary>
        private const string defaultXmlFileLoc = "AxedaFilePathConfig.xml";

        /// <summary>
        /// Current File Path for config file
        /// </summary>
        private string currentXmlFileLoc;

        /// <summary>
        /// stores the directory path of the executable used to call this funciton
        /// </summary>
        public string WorkingDirectory { get; private set; }

        /// <summary>
        /// stores the directory path of the executable used to call this funciton
        /// </summary>
        public string DeployConfigPath { get; private set; }

        /// <summary>
        /// stores the absolute file path of the xgEnterpriseProxy.xml file
        /// </summary>
        public string EnterpriseProxyConfigPath { get; private set; }

        /// <summary>
        /// stores the dabsolute file path of the EProtect tool
        /// </summary>
        public string EncryptionToolPath { get; private set; }

        /// <summary>
        /// stores the absolute file path of the EShutdown tool
        /// </summary>
        public string ShutdownToolPath { get; private set; }

        /// <summary>
        /// stores the absolute file path of the xgLicense.xml file
        /// </summary>
        public string LicenseConfigPath { get; private set; }

        /// <summary>
        /// stores the absolute file path of the xGate tool
        /// </summary>
        public string GatewayToolPath { get; private set; }

        /// <summary>
        /// stores the absolute file path of the Provisioning.xml file
        /// </summary>
        public string ProvisioningFilePath { get; private set; }

        /// <summary>
        /// stores the absolute file path of the Upload file
        /// </summary>
        public string UploadFilePath { get; private set; }

        /// <summary>
        /// xml root node key
        /// </summary>
        private const string XmlRootNode = "AxedaConfiguration";

        /// <summary>
        /// xml root note path, used for accessing the node element
        /// </summary>
        private const string XmlRootNodePath = "/AxedaConfiguration/";


        // FilePathNode
        /// <summary>
        /// xml file path node key
        /// </summary>
        private const string XmlFilePathNode = "FilePaths";

        /// <summary>
        /// Xml file path node's path, for accessing the node element.
        /// </summary>
        private const string XmlFilePathNodePath = XmlRootNodePath + XmlFilePathNode + "/";

        /// <summary>
        /// Xml Working Directory node key
        /// </summary>
        private const string XmlWorkingDirectory = "workingdir";

        /// <summary>
        /// xml Deployment configuration node key
        /// </summary>
        private const string XmlDeploymentConfig = "deployconfigfile";

        /// <summary>
        /// xml Enterprise Proxy configuration node key
        /// </summary>
        private const string XmlEnterpriseProxy = "enterpriseproxyfile";

        /// <summary>
        /// xml Encryption tool node key
        /// </summary>
        private const string XmlEncryptionTool = "encryptiontool";

        /// <summary>
        /// xml Shutdown tool node key
        /// </summary>
        private const string XmlShutdownTool = "shutdowntool";

        /// <summary>
        /// xml License configuration node key
        /// </summary>
        private const string XmlLicenseFile = "licencefile";

        /// <summary>
        /// xml Gateway tool node key
        /// </summary>
        private const string XmlGatewayTool = "gatewaytool";

        /// <summary>
        /// xml Provisioning configuration node key
        /// </summary>
        private const string XmlProvisioningFile = "provisioningfile";

        /// <summary>
        /// xml Upload Configuration node key
        /// </summary>
        private const string XmlUploadFile = "uploadfile";

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns>
        /// The current <see cref="instance"/> of the AxedaFilePathConfiguration.
        /// </returns>
        public static AxedaFilePathConfiguration GetFilePaths()
        {
            return instance ?? (instance = new AxedaFilePathConfiguration());
        }

        /// <summary>
        /// Default contructor for creating or loading from the file path configuration file
        /// </summary>
        public AxedaFilePathConfiguration(string xmlFileLoc = defaultXmlFileLoc)
        {
            currentXmlFileLoc = xmlFileLoc;
            if (File.Exists(xmlFileLoc))
            {
                LoadFromFile();
				updateSM (this.WorkingDirectory);
            }
            else
            {
                setDefaultFilePath();
                createXml();
				updateSM (this.WorkingDirectory);
            }
        }

		/// <summary>
		/// Create the Axeda File path configuration xml file
		/// </summary>
		/// <returns>
		/// void
		/// </returns>

		private void updateSM(string workingDirectory)
		{
			string xgSMPath = workingDirectory+"DefaultProject/xgSM.xml";
			if (!File.Exists (xgSMPath)) {
				throw new FileNotFoundException (xgSMPath + "not found");
			} else {

				XmlDocument xDoc = new XmlDocument ();
				xDoc.Load (xgSMPath);
				XmlNode file = xDoc.SelectSingleNode ("/PersistedData/SMSchemas/Schema/File");
				file.Attributes [0].Value = this.UploadFilePath;
				xDoc.Save (xgSMPath);

			}
		}
        /// <summary>
        /// Create the Axeda File path configuration xml file
        /// </summary>
        /// <returns>
        /// void
        /// </returns>
        private void createXml()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;
            using (XmlWriter writer = XmlWriter.Create(this.currentXmlFileLoc, settings))
            {
                writer.WriteStartElement(XmlRootNode);
                writer.WriteStartElement(XmlFilePathNode);
                writer.WriteElementString(XmlWorkingDirectory, WorkingDirectory);
                writer.WriteElementString(XmlDeploymentConfig, DeployConfigPath);
                writer.WriteElementString(XmlEnterpriseProxy, EnterpriseProxyConfigPath);
                writer.WriteElementString(XmlEncryptionTool, EncryptionToolPath);
                writer.WriteElementString(XmlShutdownTool, ShutdownToolPath);
                writer.WriteElementString(XmlLicenseFile, LicenseConfigPath);
                writer.WriteElementString(XmlGatewayTool, GatewayToolPath);
                writer.WriteElementString(XmlProvisioningFile, ProvisioningFilePath);
                writer.WriteElementString(XmlUploadFile, UploadFilePath);
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.Flush();
            }

        }

        /// <summary>
        /// Sets defaults file paths based on the platform
        /// </summary>
        /// <returns>
        /// void
        /// </returns>
        private void setDefaultFilePath()
        {
            int p = (int)Environment.OSVersion.Platform;
            if ((p == 4) || (p == 6) || (p == 128)) // Linux Platform
            {
                WorkingDirectory = "/home/Axeda/Gateway/";
                DeployConfigPath = WorkingDirectory + "xgDeployConfig.xml";
                EnterpriseProxyConfigPath = WorkingDirectory + "xgEnterpriseProxy.xml";
                EncryptionToolPath = WorkingDirectory + "EProtect";
                ShutdownToolPath = WorkingDirectory + "EShutdown";
                LicenseConfigPath = WorkingDirectory + "xgLicense.xml";
                GatewayToolPath = WorkingDirectory + "xgate";
                ProvisioningFilePath = WorkingDirectory + "provisioning.xml";
                UploadFilePath = WorkingDirectory + "Upload.xml";
            }
            else  // Windows Platform
            {
                WorkingDirectory = @"c:/Axeda/Gateway/";
                DeployConfigPath = WorkingDirectory + "xgDeployConfig.xml";
                EnterpriseProxyConfigPath = WorkingDirectory + "xgEnterpriseProxy.xml";
                EncryptionToolPath = WorkingDirectory + "EProtect.exe";
                ShutdownToolPath = WorkingDirectory + "EShutdown.exe";
                LicenseConfigPath = WorkingDirectory + "xgLicense.xml";
                GatewayToolPath = WorkingDirectory + "xgate.exe";
                ProvisioningFilePath = WorkingDirectory + "provisioning.xml";
                UploadFilePath = WorkingDirectory + "Upload.xml";
            }

        }

        /// <summary>
        /// Loads the file paths from the axeda file path configuration file.
        /// </summary>
        /// <returns>
        /// void
        /// </returns>
        private void LoadFromFile()
        {
            var configurations = new XmlDocument();

            try
            {
                configurations.Load(this.currentXmlFileLoc);
                var root = configurations.DocumentElement;
                if (root == null)
                {
                    throw new ApplicationException("The Axeda Agent's File Path configuration file is missing a root element.");
                }

                // File Paths
                var FilePathsNode = root.SelectSingleNode(XmlRootNodePath + XmlFilePathNode);
                if (FilePathsNode == null)
                {
                    throw new ApplicationException("The Axeda Agent's File Path configuration file is missing an File Paths element.");
                }

                var WorkingDirectoryNode = FilePathsNode.SelectSingleNode(XmlFilePathNodePath + XmlWorkingDirectory);
                if (WorkingDirectoryNode != null)
                {
                    this.WorkingDirectory = WorkingDirectoryNode.InnerText;
                }
                var DeploymentConfigNode = FilePathsNode.SelectSingleNode(XmlFilePathNodePath + XmlDeploymentConfig);
                if (DeploymentConfigNode != null)
                {
                    this.DeployConfigPath = DeploymentConfigNode.InnerText;
                }
                var EnterpriseProxyNode = FilePathsNode.SelectSingleNode(XmlFilePathNodePath + XmlEnterpriseProxy);
                if (EnterpriseProxyNode != null)
                {
                    this.EnterpriseProxyConfigPath = EnterpriseProxyNode.InnerText;
                }
                var EncryptionToolNode = FilePathsNode.SelectSingleNode(XmlFilePathNodePath + XmlEncryptionTool);
                if (EncryptionToolNode != null)
                {
                    this.EncryptionToolPath = EncryptionToolNode.InnerText;
                }
                var ShutdownToolNode = FilePathsNode.SelectSingleNode(XmlFilePathNodePath + XmlShutdownTool);
                if (ShutdownToolNode != null)
                {
                    this.ShutdownToolPath = ShutdownToolNode.InnerText;
                }
                var LicenseFileNode = FilePathsNode.SelectSingleNode(XmlFilePathNodePath + XmlLicenseFile);
                if (LicenseFileNode != null)
                {
                    this.LicenseConfigPath = LicenseFileNode.InnerText;
                }
                var GatewayToolNode = FilePathsNode.SelectSingleNode(XmlFilePathNodePath + XmlGatewayTool);
                if (GatewayToolNode != null)
                {
                    this.GatewayToolPath = GatewayToolNode.InnerText;
                }
                var ProvisioningFileNode = FilePathsNode.SelectSingleNode(XmlFilePathNodePath + XmlProvisioningFile);
                if (ProvisioningFileNode != null)
                {
                    this.ProvisioningFilePath = ProvisioningFileNode.InnerText;
                }
                var UploadFileNode = FilePathsNode.SelectSingleNode(XmlFilePathNodePath + XmlUploadFile);
                if (UploadFileNode != null)
                {
                    this.UploadFilePath = UploadFileNode.InnerText;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    "Unable to load  Axeda Agent's File Path configuration file:  " + ex.Message);
            }

        }
    }
}
