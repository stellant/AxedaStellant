// -----------------------------------------------------------------------------
//           $Copyright: (C) Copyright by Bayer Healthcare 2013. All rights reserved. $
// -----------------------------------------------------------------------------
// 
//  $Source: AxedaAgent/Utils/Constants.cs $
//  $Revision: 1.1 $
//  $Date: 2012/09/12 14:43:42EDT $
//  $Author: MLHAY $
//  $ProjectName: g:/MKS_Files/Service_Engineering/ApplicationFramework/AxedaAgent/project.pj $
// 
// -----------------------------------------------------------------------------

namespace Medrad.ServiceEngineering.ApplicationFramework.Axeda.Agent
{
    #region EncryptionType enum

    /// <summary>
    /// Possible Ssl Encryption Levels for Axeda
    /// </summary>
    public enum EncryptionType
    {
        /// <summary>
        /// Unknown Encryption Type Seen
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// No Encryption Needed
        /// </summary>
        None = 0,

        /// <summary>
        /// <see cref="Blowfish"/> Encryption
        /// </summary>
        Blowfish = 1,

        /// <summary>
        /// <see cref="Ssl"/> Encryption
        /// </summary>
        Ssl = 2,

        /// <summary>
        /// <see cref="Aes"/> Encryption
        /// </summary>
        Aes = 3
    }

    #endregion

    #region ProxyServerType enum

    /// <summary>
    /// Holds the Possible Proxy Server Types
    /// </summary>
    public enum ProxyServerType
    {
        /// <summary>
        /// No Proxy Server is Used
        /// </summary>
        None = 0,

        /// <summary>
        /// HTTP Proxy Server is Used
        /// </summary>
        Http,

        /// <summary>
        /// SOCKS Proxy Server is Used
        /// </summary>
        Socks,

        /// <summary>
        /// An <see cref="Auto"/> Discovery Proxy Server is Used
        /// </summary>
        Auto
    }

    #endregion

    #region SslEncryptionLevel enum

    /// <summary>
    /// Possible Ssl Encryption Levels for Axeda
    /// </summary>
    public enum SslEncryptionLevel
    {
        /// <summary>
        /// Unknown Encryption Level Requested
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// No SLL (This should never be used from Axeda)
        /// </summary>
        None = 0,

        /// <summary>
        /// <list type="number">
        /// <item><description>Bit Encryption</description></item>
        /// </list>
        /// </summary>
        Bit40 = 1,

        /// <summary>
        /// <list type="number">
        /// <item><description>Bit Encryption</description></item>
        /// </list>
        /// </summary>
        Bit128 = 2,

        /// <summary>
        /// <list type="number">
        /// <item><description>Bit Encryption</description></item>
        /// </list>
        /// </summary>
        Bit168 = 3
    }

    #endregion

    #region AgentStatus enum

    /// <summary>
    /// Possible Communication Status
    /// </summary>
    public enum AgentStatus
    {
        /// <summary>
        /// The Agent is not installed.
        /// </summary>
        NotInstalled = 0,

        /// <summary>
        /// The Agent is not running.
        /// </summary>
        NotRunning,

        /// <summary>
        /// The Agent is not reachable due to web services not enabled.
        /// </summary>
        WebServicesNotSupported,

        /// <summary>
        /// The system does not have a network cable attached.
        /// </summary>
        NoNetworkConnectivity,

        /// <summary>
        /// The Enterprise is not reachable, but there is a network cable attached.  
        /// </summary>
        EnterpriseUnreachable,

        /// <summary>
        /// The system is communicating to the Enterprise Server.
        /// </summary>
        ConnectedToEnterprise,

        /// <summary>
        /// An unexpected error occurred
        /// </summary>
        Unknown
    }

    #endregion

    #region AgentDataWriteStatus enum

    /// <summary>
    /// Possible Data Write Status Variables
    /// </summary>
    public enum AgentDataWriteStatus
    {
        /// <summary>
        /// The Data was written successfully.
        /// </summary>
        Ok = 0,

        /// <summary>
        /// The Agent was unreachable (either not online or not present).  
        /// </summary>
        AgentUnreachable,

        /// <summary>
        /// The data item requested to be written was invalid.
        /// </summary>
        InvalidDataItem,

        /// <summary>
        /// An unexpected error occurred during the data write.
        /// </summary>
        UnknownError
    }

    #endregion

    #region AgentDataWriteStatus enum

    /// <summary>
    /// Possible Data Write Status Variables
    /// </summary>
    public enum AgentDataReadStatus
    {
        /// <summary>
        /// The Data was written successfully.
        /// </summary>
        Ok = 0,

        /// <summary>
        /// The Agent was unreachable (either not online or not present).  
        /// </summary>
        AgentUnreachable,

        /// <summary>
        /// The data item requested to be read was invalid.
        /// </summary>
        InvalidDataItem,

        /// <summary>
        /// The data item requested to be read was invalid.
        /// </summary>
        InvalidDataType,

        /// <summary>
        /// An unexpected error occurred during the data write.
        /// </summary>
        UnknownError
    }

    #endregion
}