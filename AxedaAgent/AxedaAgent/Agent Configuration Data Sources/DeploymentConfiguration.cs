// -----------------------------------------------------------------------------
//           $Copyright: (C) Copyright by Bayer Healthcare 2013. All rights reserved. $
// -----------------------------------------------------------------------------
// 
//  $Source: AxedaAgent/Agent Configuration Data Sources/DeploymentConfiguration.cs $
//  $Revision: 1.1 $
//  $Date: 2012/09/12 14:43:38EDT $
//  $Author: MLHAY $
//  $ProjectName: g:/MKS_Files/Service_Engineering/ApplicationFramework/AxedaAgent/project.pj $
// 
// -----------------------------------------------------------------------------

namespace Medrad.ServiceEngineering.ApplicationFramework.Axeda.Agent
{
	using Medrad.ServiceEngineering.ApplicationFramework.Axeda.Agent;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// This class is used to read the Axeda deployment files and hold the accessible data.  
    /// </summary>
    internal class DeploymentConfiguration
    {
        #region Constructors and Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DeploymentConfiguration"/> class.
        /// </summary>
        /// <param name="deploymentFilePath">The deployment file path.</param>
        public DeploymentConfiguration(string deploymentFilePath="")
        {
            if (string.IsNullOrEmpty(deploymentFilePath))
            {
                deploymentFilePath = AxedaFilePathConfiguration.GetFilePaths().DeployConfigPath;
            }

            // We will set a tracking variable to make sure a deployment file exists. 
            this.DeploymentConfigurationExists = File.Exists(deploymentFilePath);

            // If the deployment file exists, initialize all of our variables 
            if (this.DeploymentConfigurationExists)
            {
                this.InitializeFromFile(deploymentFilePath);
            }
            else
            {
                SslEncryptionLevel = SslEncryptionLevel.Unknown;
                EncryptionType = EncryptionType.Unknown;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether [deployment configuration exists].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [deployment configuration exists]; otherwise, <c>false</c>.
        /// </value>
        public bool DeploymentConfigurationExists { get; private set; }

        /// <summary>
        /// Gets/sets the auto proxy script url.  
        /// </summary>
        public string AutoProxyUrl { get; private set; }

        /// <summary>
        /// Gets the DRM server IP address or hostname 
        /// </summary>
        public string ServerAddress { get; private set; }

        /// <summary>
        /// Gets the port number on DRM server
        /// </summary>
        public int? ServerPortNumber { get; private set; }

        /// <summary>
        /// Gets the encryption type
        /// </summary>
        public EncryptionType EncryptionType { get; private set; }

        /// <summary>
        /// Gets the HTTP proxy used status
        /// </summary>
        public bool? ProxyHttpUsed { get; private set; }

        /// <summary>
        /// Gets the HTTP proxy IP address or hostname
        /// </summary>
        public string ProxyHttpAddress { get; private set; }

        /// <summary>
        /// Gets the HTTP proxy port number 
        /// </summary>
        public int? ProxyHttpPortNumber { get; private set; }

        /// <summary>
        /// Gets the HTTP proxy user name 
        /// </summary>
        public string ProxyHttpUserName { get; private set; }

        /// <summary>
        /// Gets the HTTP proxy password (encrypted) 
        /// </summary>
        public string ProxyHttpEncryptedPassword { get; private set; }

        /// <summary>
        /// Gets the SOCKS proxy used status
        /// </summary>
        public bool? ProxySocksUsed { get; private set; }

        /// <summary>
        /// Gets the SOCKS proxy IP address or hostname
        /// </summary>
        public string ProxySocksAddress { get; private set; }

        /// <summary>
        /// Gets the SOCKS proxy port number 
        /// </summary>
        public int? ProxySocksPortNumber { get; private set; }

        /// <summary>
        /// Gets the SOCKS proxy user name 
        /// </summary>
        public string ProxySocksUserName { get; private set; }

        /// <summary>
        /// Gets the SOCKS proxy password (encrypted) 
        /// </summary>
        public string ProxySocksEncryptedPassword { get; private set; }

        /// <summary>
        /// Gets the Ssl encryption level.
        /// </summary>
        public SslEncryptionLevel SslEncryptionLevel { get; private set; }

        /// <summary>
        /// Gets the Ssl server authentication flag.
        /// </summary>
        public bool? SslAuthenticationFlag { get; private set; }

        /// <summary>
        /// Gets the name of the database.
        /// </summary>
        public string DatabaseName { get; private set; }

        /// <summary>
        /// Gets the "enable Ssl" flag.
        /// </summary>
        public bool? EnableSslFlag { get; private set; }

        /// <summary>
        /// Gets the model name for the Axeda device.
        /// </summary>
        public string ModelName { get; private set; }

        /// <summary>
        /// Gets the serial number for the Axeda device.
        /// </summary>
        public string SerialNumber { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Deployment Configuration: \n" +
                   "\tServerAddress= " + this.ServerAddress + "\n" +
                   "\tServerPortNumber= " + this.ServerPortNumber + "\n" +
                   "\tEncryptionType= " + this.EncryptionType + "\n" +
                   "\tProxyHttpUsed= " + this.ProxyHttpUsed + "\n" +
                   "\tProxyAddress= " + this.ProxyHttpAddress + "\n" +
                   "\tProxyPortNumber= " + this.ProxyHttpPortNumber + "\n" +
                   "\tProxyUserName= " + this.ProxyHttpUserName + "\n" +
                   "\tProxyEncryptedPassword= " + this.ProxyHttpEncryptedPassword + "\n" +
                   "\tProxySocksUsed= " + this.ProxySocksUsed + "\n" +
                   "\tProxySocksAddress= " + this.ProxySocksAddress + "\n" +
                   "\tProxySocksPortNumber= " + this.ProxySocksPortNumber + "\n" +
                   "\tProxySocksUserName= " + this.ProxySocksUserName + "\n" +
                   "\tProxySocksEncryptedPassword= " + this.ProxySocksEncryptedPassword + "\n" +
                   "\tProxyPAC Address= " + this.AutoProxyUrl + "\n" +
                   "\tSslEncryptionLevel= " + this.SslEncryptionLevel + "\n" +
                   "\tSslAuthenticationFlag= " + this.SslAuthenticationFlag + "\n" +
                   "\tEnableSSLFlag= " + this.EnableSslFlag + "\n" +
                   "\tDatabaseName= " + this.DatabaseName + "\n" +
                   "\tModelName = " + this.ModelName + "\n" +
                   "\tSerialNumber = " + this.SerialNumber + "\n";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Query the provided XML element using the provided XPath expression
        /// for a stored value, returning a default value if the query fails.
        /// </summary>
        /// <param name="xmlNode">The XML element to query.</param>
        /// <param name="xpathExpression">
        /// The XPath expression to query with.
        /// </param>
        /// <returns>
        /// The value stored in the first element matching the XPath expression,
        /// or an empty string if no elements match the XPath expression.
        /// </returns>
        private static string TryGetAttributeValueFromXml(XmlNode xmlNode, string xpathExpression)
        {
            XmlNode valueNode = xmlNode.SelectSingleNode(xpathExpression);
            return valueNode != null ? valueNode.InnerText.Trim() : string.Empty;
        }

        /// <summary>
        /// Initializes from file.
        /// </summary>
        /// <param name="deploymentFilePath">The deployment file path.</param>
        private void InitializeFromFile(string deploymentFilePath)
        {
            var deploymentConfig = new XmlDocument();
            deploymentConfig.Load(deploymentFilePath);
            XmlElement root = deploymentConfig.DocumentElement;

            // We'll do our string values first
            this.ProxyHttpAddress = TryGetAttributeValueFromXml(root, AxedaXPathQueries.ProxyHttpAddress);
            this.ProxyHttpUserName = TryGetAttributeValueFromXml(root, AxedaXPathQueries.ProxyHttpUserName);
            this.ProxyHttpEncryptedPassword = TryGetAttributeValueFromXml(root, AxedaXPathQueries.ProxyHttpPassword);
            this.ProxySocksAddress = TryGetAttributeValueFromXml(root, AxedaXPathQueries.ProxySocksAddress);
            this.ProxySocksUserName = TryGetAttributeValueFromXml(root, AxedaXPathQueries.ProxySocksUserName);
            this.ProxySocksEncryptedPassword = TryGetAttributeValueFromXml(root, AxedaXPathQueries.ProxySocksPassword);
            this.ServerAddress = TryGetAttributeValueFromXml(root, AxedaXPathQueries.EnterprisePrimaryHost);
            this.DatabaseName = TryGetAttributeValueFromXml(root, AxedaXPathQueries.EnterprisePrimaryDb);
            this.ModelName = TryGetAttributeValueFromXml(root, AxedaXPathQueries.IdModelName);
            this.SerialNumber = TryGetAttributeValueFromXml(root, AxedaXPathQueries.IdSerialNumber);

            // TODO:  In the future this should be converted to a URI
            this.AutoProxyUrl = TryGetAttributeValueFromXml(root, AxedaXPathQueries.ProxyAutoConfigurationUrl);

            // Next we'll do our integer values
            this.ProxyHttpPortNumber =
                ConversionUtils.StrToInt(TryGetAttributeValueFromXml(root, AxedaXPathQueries.ProxyHttpPort));
            //this.ProxySocksPortNumber =
            //    ConversionUtils.StrToInt(TryGetAttributeValueFromXml(root, AxedaXPathQueries.ProxySocksPort));
            this.ServerPortNumber =
                ConversionUtils.StrToInt(TryGetAttributeValueFromXml(root, AxedaXPathQueries.EnterprisePrimaryPort));

            // Now we'll convert our Boolean values
            this.ProxyHttpUsed =
                ConversionUtils.StrToBool(TryGetAttributeValueFromXml(root, AxedaXPathQueries.ProxyHttpEnabled));
            this.ProxySocksUsed =
                ConversionUtils.StrToBool(TryGetAttributeValueFromXml(root, AxedaXPathQueries.ProxySocksEnabled));
            //this.EnableSslFlag = ConversionUtils.StrToBool(TryGetAttributeValueFromXml(root, AxedaXPathQueries.SslEnabled));
            //this.SslAuthenticationFlag =
            //    ConversionUtils.StrToBool(TryGetAttributeValueFromXml(root, AxedaXPathQueries.SslCertificate));

            // Finally we'll handle the "special" cases
            //this.SslEncryptionLevel = ConversionUtils.StrToSslLevel(TryGetAttributeValueFromXml(root, AxedaXPathQueries.SslStrength));
            //this.EncryptionType = ConversionUtils.StrToEncryptionType(TryGetAttributeValueFromXml(root, AxedaXPathQueries.EnterprisePrimaryEncryption));
        }

        #endregion

        #region Nested type: AxedaXPathQueries

        /// <summary>
        /// This class adds a layer of abstraction to the Axeda XPath Queries.  This is purely an attempt to 
        /// simply the code to perform the functions.  
        /// </summary>
        private static class AxedaXPathQueries
        {
            /// <summary>
            ///  XPath Query String for the HTTP Proxy Server User Name
            /// </summary>
            public const string ProxyHttpUserName =
                RootPathProxyServerHttp + PartialPathAuthentication + AttributeUserName;

            /// <summary>
            ///  XPath Query String for the HTTP Proxy Server Password
            /// </summary>
            public const string ProxyHttpPassword =
                RootPathProxyServerHttp + PartialPathAuthentication + AttributePassword;

            /// <summary>
            ///  XPath Query String for the HTTP Proxy Server Address
            /// </summary>
            public const string ProxyHttpAddress = RootPathProxyServerHttp + AttributeHost;

            /// <summary>
            ///  XPath Query String for the HTTP Proxy Server Port
            /// </summary>
            public const string ProxyHttpPort = RootPathProxyServerHttp + AttributePort;

            /// <summary>
            ///  XPath Query String for the HTTP Proxy Server Enabled
            /// </summary>
            public const string ProxyHttpEnabled = RootPathProxyServerHttp + AttributeEnabled;

            /// <summary>
            ///  XPath Query String for the SOCKS Proxy User Name
            /// </summary>
            public const string ProxySocksUserName =
                RootPathProxyServerSocks + PartialPathAuthentication + AttributeUserName;

            /// <summary>
            ///  XPath Query String for the SOCKS Proxy Password
            /// </summary>
            public const string ProxySocksPassword =
                RootPathProxyServerSocks + PartialPathAuthentication + AttributePassword;

            /// <summary>
            ///  XPath Query String for the SOCKS Proxy Address
            /// </summary>
            public const string ProxySocksAddress = RootPathProxyServerSocks + AttributeHost;

            /// <summary>
            ///  XPath Query String for the SOCKS Proxy Port
            /// </summary>
            public const string ProxySocksPort = RootPathProxyServerSocks + AttributePort;

            /// <summary>
            ///  XPath Query String for the SOCKS Proxy Enabled Setting
            /// </summary>
            public const string ProxySocksEnabled = RootPathProxyServerSocks + AttributeEnabled;

            /// <summary>
            ///  XPath Query String for the Auto Configuration URL
            /// </summary>
            public const string ProxyAutoConfigurationUrl = RootPathProxyServerPac + AttributeUrl;

            /// <summary>
            ///  XPath Query String for the Ssl Enabled Setting
            /// </summary>
            public const string SslEnabled = RootPathSslSettings + AttributeEnabled;

            /// <summary>
            ///  XPath Query String for the Ssl Strength Setting
            /// </summary>
            public const string SslStrength = RootPathSslSettings + AttributeStrength;

            /// <summary>
            ///  XPath Query String for the Ssl Certificate Enabled Setting
            /// </summary>
            public const string SslCertificate = RootPathSslSettings + AttributeCertificate;

            /// <summary>
            ///  XPath Query String for the Enterprise Primary Host Address
            /// </summary>
            public const string EnterprisePrimaryHost = RootPathServerPrimary + AttributeHost;

            /// <summary>
            ///  XPath Query String for the Enterprise Primary Host Port
            /// </summary>
            public const string EnterprisePrimaryPort = RootPathServerPrimary + AttributePort;

            /// <summary>
            ///  XPath Query String for the Enterprise DB
            /// </summary>
            public const string EnterprisePrimaryDb = RootPathServerPrimary + AttributeDb;

            /// <summary>
            ///  XPath Query String for the Enterprise Communication Encryption
            /// </summary>
            public const string EnterprisePrimaryEncryption = RootPathServerPrimary + AttributeEncryption;

            /// <summary>
            ///  XPath Query String for the Axeda Model Name
            /// </summary>
            public const string IdModelName = RootPathIdentification + AttributeModel;

            /// <summary>
            ///  XPath Query String for the Axeda Serial Number
            /// </summary>
            public const string IdSerialNumber = RootPathIdentification + AttributeSerialNumber;

            /// <summary>
            /// The common Root XML Path for the Axeda deployment file.  
            /// </summary>
            private const string RootPath = @"/AgentRequest/UpdateConfiguration/Configurations/Config/";

            /// <summary>
            /// The root path of the HTTP Proxy Server Properties
            /// </summary>
            private const string RootPathProxyServerHttp = RootPath + @"Network/ProxyServer/HTTP/";

            /// <summary>
            /// The root path of the SOCKS Proxy Server Properties
            /// </summary>
            private const string RootPathProxyServerSocks = RootPath + @"Network/ProxyServer/SOCKS/";

            /// <summary>
            /// The root path of the Automatic Proxy Server Properties
            /// </summary>
            private const string RootPathProxyServerPac = RootPath + @"Network/ProxyServer/PAC/";

            /// <summary>
            /// The root path of the Ssl Properties
            /// </summary>
            private const string RootPathSslSettings = RootPath + @"ProjectSettings/EnterpriseServers/Ssl/";

            /// <summary>
            /// The root path of the Enterprise Server
            /// </summary>
            private const string RootPathServerPrimary = RootPath + @"ProjectSettings/EnterpriseServers/Primary/";

            /// <summary>
            /// The root path of the Identification portion of settings
            /// </summary>
            private const string RootPathIdentification = RootPath + @"/ProjectSettings/Identification/";

            /// <summary>
            /// Partial path for the common authentication portion of the path
            /// </summary>
            private const string PartialPathAuthentication = @"Authentication/";

            /// <summary>
            /// Attribute tag for user
            /// </summary>
            private const string AttributeUserName = "@user";

            /// <summary>
            /// Attribute tag for encrypted passwords
            /// </summary>
            private const string AttributePassword = "@password";

            /// <summary>
            /// Attribute tag for ports
            /// </summary>
            private const string AttributePort = "@port";

            /// <summary>
            /// Attribute tag for enabled settings
            /// </summary>
            private const string AttributeEnabled = "@enabled";

            /// <summary>
            /// Attribute tag for a URL
            /// </summary>
            private const string AttributeUrl = "@url";

            /// <summary>
            /// Attribute tag for strength
            /// </summary>
            private const string AttributeStrength = "@strength";

            /// <summary>
            /// Attribute tag for certificate
            /// </summary>
            private const string AttributeCertificate = "@certificate";

            /// <summary>
            /// Attribute tag for host
            /// </summary>
            private const string AttributeHost = "@host";

            /// <summary>
            /// Attribute tag for the database
            /// </summary>
            private const string AttributeDb = "@db";

            /// <summary>
            /// Attribute tag for encryption type
            /// </summary>
            private const string AttributeEncryption = "@encryption";

            /// <summary>
            /// Attribute tag for model name
            /// </summary>
            private const string AttributeModel = "@mn";

            /// <summary>
            /// Attribute tag for serial number
            /// </summary>
            private const string AttributeSerialNumber = "@sn";
        }

        #endregion
    }
}