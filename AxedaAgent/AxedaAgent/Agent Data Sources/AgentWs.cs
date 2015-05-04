// -----------------------------------------------------------------------------
//           $Copyright: (C) Copyright by Bayer Healthcare 2013. All rights reserved. $
// -----------------------------------------------------------------------------
// 
//  $Source: AxedaAgent/Agent Data Sources/AgentWs.cs $
//  $Revision: 1.2 $
//  $Date: 2012/09/17 08:59:48EDT $
//  $Author: MLHAY $
//  $ProjectName: g:/MKS_Files/Service_Engineering/ApplicationFramework/AxedaAgent/project.pj $
// 
// -----------------------------------------------------------------------------
namespace Medrad.ServiceEngineering.ApplicationFramework.Axeda.Agent
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Security.Permissions;
    using System.Web.Services.Protocols;

    using Medrad.ServiceEngineering.ApplicationFramework.Axeda.Agent.AgentWebService;
    
    using File = System.IO.File;
    using ValueType = Medrad.ServiceEngineering.ApplicationFramework.Axeda.Agent.AgentWebService.ValueType;
	using Medrad.ServiceEngineering.ApplicationFramework.Axeda.Agent;

    /// <summary>
    /// Holds a generic Axeda Agent Web Service instance that will allow real time
    /// interaction with the device.
    /// </summary>
    internal class AgentWs : IDisposable
    {
        #region Static Fields

        /// <summary>
        /// Holds the the singleton for the Axeda agent
        /// </summary>
        private static AgentWs instance;

        #endregion

        #region Constants and Fields

        /// <summary>
        /// Holds the web <see cref="service"/> reference for the Axeda Agent.
        /// </summary>
        private readonly Service service;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentWs"/> class. 
        /// Initializes a new <see cref="instance"/> of the
        /// <see cref="AgentWs"/> class.
        /// </summary>
        /// <param name="url">
        /// The URL.
        /// </param>
        private AgentWs(string url = @"http://localhost:8080")
        {
            this.service = new Service();
            this.Url = url;
            this.service.Timeout = 500;
            this.Refresh();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the agent extension version.
        /// </summary>
        public string AgentExtensionVersion { get; private set; }

        /// <summary>
        /// Gets the model of the Axeda Agent.
        /// </summary>
        public string Model { get; private set; }

        /// <summary>
        /// Gets the serial number.
        /// </summary>
        public string SerialNumber { get; private set; }

        /// <summary>
        /// Gets the Connection status.
        /// </summary>
        public AgentStatus Status { get; private set; }

        /// <summary>
        /// Sets the URL of the Axeda Agent Web service.
        /// </summary>
        /// <value>
        /// The URL of the Agent.
        /// </value>
        public string Url
        {
            set
            {
                this.service.Url = value;
            }
        }

        /// <summary>
        /// Gets the version number of the Axeda Agent.
        /// </summary>
        public string Version { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns>
        /// The current <see cref="instance"/> of the Axeda Agent.
        /// </returns>
        public static AgentWs GetInstance()
        {
            return instance ?? (instance = new AgentWs());
        }

        /// <summary>
        /// Indicates whether any network connection is available Filter
        /// connections below a specified speed, as well as
        /// <see langword="virtual"/> network cards.
        /// </summary>
        /// <returns>
        /// <c>true</c> if a network connection is available; otherwise, 
        /// <c>false</c> .
        /// </returns>
        public static bool IsNetworkAvailable()
        {
            return IsNetworkAvailable(0);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.service.Dispose();
        }

        /// <summary>
        /// Refreshes all data within this instance.
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Refresh()
        {
            // We'll assume everything is inaccessible 
            this.Version = null;
            this.Model = null;
            this.SerialNumber = null;
            this.AgentExtensionVersion = null;

            // At this point, we can assume the agent is running, so we'll try to read our data.  
            try
            {
                // Start with the Version of the Agent
                this.Version = this.service.GetVersion();

                // We'll update our device ID Information
                DeviceIdentification deviceIdentification = this.service.GetDeviceIds()[0];
                this.Model = deviceIdentification.model;
                this.SerialNumber = deviceIdentification.serialNumber;
                //this.AgentExtensionVersion =
                //    this.service.GetDataitem("CE_Description", deviceIdentification).value;

                // Refresh any info we have about the device.  We'll treat this a little bit special here.  There may be instances 
                // where we can talk to the agent web services, but cannot go outbound.  We'll try to report a more valid status.
            //    this.Status =
            //        ConversionUtils.StrToAgentStatus(
            //            this.service.GetDataitem("ConnectivityStatus", deviceIdentification).value);
            //    if ((this.Status != AgentStatus.ConnectedToEnterprise) && !IsNetworkAvailable())
            //    {
            //        this.Status = AgentStatus.NoNetworkConnectivity;
            //    }

            }
            catch (WebException)
            {
                // If there is no agent installed, just set our status and leave.  
                if (!File.Exists(AxedaFilePathConfiguration.GetFilePaths().GatewayToolPath))
                {
                    this.Status = AgentStatus.NotInstalled;
                    return;
                }

                // If the agent isn't running, there is no point in going on.  We can't get an info anyway.  
                if (Process.GetProcessesByName("xgate").Length == 0)
                {
                    this.Status = AgentStatus.NotRunning;
                    return;
                }

                // If we made it this far, the agent is probably unreachable.  
                this.Status = AgentStatus.WebServicesNotSupported;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this
        /// instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Agent Information: \n" + "\tStatus= " + this.Status + "\n" + "\tSerialNumber= " + this.SerialNumber
                   + "\n" + "\tModel= " + this.Model + "\n" + "\tVersion= " + this.Version + "\n"
                   + "\tAgentExtensionVersion= " + this.AgentExtensionVersion + "\n";
        }

        /// <summary>
        /// Writes the string data property.
        /// </summary>
        /// <remarks>
        /// We are suppressing the specific warning CA1031 because we want our
        /// conversions to fail with an explicit value. We are not concerned as
        /// to the specific reason why it failed.
        /// </remarks>
        /// <param name="propertyName">
        /// Name of the property.
        /// </param>
        /// <param name="dataValue">
        /// The data value.
        /// </param>
        /// <returns>
        /// The status of the data write.
        /// </returns>
        [SuppressMessage("Microsoft.Design",
                         "CA1031:DoNotCatchGeneralExceptionTypes",
                         Justification = "Generic Failure Response is preferred to ease interface development")]
        public AgentDataWriteStatus WriteStringDataProperty(string propertyName, string dataValue)
        {
            try
            {
                // We'll update our device ID Information so we can update the right device.
                DeviceIdentification deviceIdentification = this.service.GetDeviceIds()[0];

                // We'll create our new data item now
                var dataItem = new DataitemValue
                    {
                        name = propertyName,
                        quality = Quality.Good,
                        timestamp = DateTime.Now,
                        type = ValueType.String,
                        value = dataValue
                    };

                // Finally send it over
                this.service.SetDataitem(dataItem, deviceIdentification);
            }
            catch (WebException)
            {
                return AgentDataWriteStatus.AgentUnreachable;
            }
            catch (SoapHeaderException)
            {
                return AgentDataWriteStatus.InvalidDataItem;
            }
            catch (Exception)
            {
                return AgentDataWriteStatus.UnknownError;
            }

            // If we've made it this far, everything is OK
            return AgentDataWriteStatus.Ok;
        }

        /// <summary>
        /// Writes the string data property.
        /// </summary>
        /// <remarks>
        /// We are suppressing the specific warning CA1031 because we want our
        /// conversions to fail with an explicit value. We are not concerned as
        /// to the specific reason why it failed.
        /// </remarks>
        /// <param name="propertyName">
        /// Name of the property.
        /// </param>
        /// <returns>
        /// The status of the data write.
        /// </returns>
        [SuppressMessage("Microsoft.Design",
                         "CA1031:DoNotCatchGeneralExceptionTypes",
                         Justification = "Generic Failure Response is preferred to ease interface development")]
        public AgentReadResponse ReadStringDataProperty(string propertyName)
        {
            AgentReadResponse response = new AgentReadResponse { Value = string.Empty };

            try
            {
                // We'll update our device ID Information so we can read the right device.
                DeviceIdentification deviceIdentification = this.service.GetDeviceIds()[0];

                // We'll read our data item
                var dataItem = this.service.GetDataitem(propertyName, deviceIdentification);

                // Finally we'll set our output.  
                if (dataItem != null)
                {
                    if (dataItem.type != ValueType.String)
                    {
                        response.Status = AgentDataReadStatus.InvalidDataType;
                        return response; 
                    }

                    response.Value = dataItem.value; 
                }
            }
            catch (WebException)
            {
                response.Status = AgentDataReadStatus.AgentUnreachable;
                return response; 
            }
            catch (SoapHeaderException)
            {
                response.Status = AgentDataReadStatus.InvalidDataItem;
                return response; 
            }
            catch (Exception)
            {
                response.Status = AgentDataReadStatus.UnknownError;
                return response; 
            }

            // If we've made it this far, everything is OK
            response.Status = AgentDataReadStatus.Ok;
            return response; 
        }

        /// <summary>
        /// Attempts to stop the service agent through its own web service.
        /// </summary>
        /// <returns>
        /// Service Agent stop request
        /// </returns>
        /// <remarks>
        /// We are suppressing the specific warning CA1031 because we want our
        /// conversions to fail with an explicit value. We are not concerned as
        /// to the specific reason why it failed.
        /// </remarks>
        [SuppressMessage("Microsoft.Design",
                         "CA1031:DoNotCatchGeneralExceptionTypes",
                         Justification = "Generic Failure Response is preferred to ease interface development")]
        public bool StopServiceAgent()
        {
            try
            {
                this.service.Shutdown(ShutdownMode.StopAgent);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Indicates whether any network connection is available. Filter
        /// connections below a specified speed, as well as
        /// <see langword="virtual"/> network cards.
        /// </summary>
        /// <param name="minimumSpeed">
        /// The minimum speed required. Passing 0 will not filter connection
        /// using speed.
        /// </param>
        /// <returns>
        /// <c>true</c> if a network connection is available; otherwise, 
        /// <c>false</c> .
        /// </returns>
        private static bool IsNetworkAvailable(long minimumSpeed)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                return false;
            }

            return
                NetworkInterface.GetAllNetworkInterfaces().Where(
                    ni => (ni.OperationalStatus == OperationalStatus.Up) &&
                          (ni.Name == "LAN1-RIS") &&
                          (ni.NetworkInterfaceType != NetworkInterfaceType.Loopback) &&
                          (ni.NetworkInterfaceType != NetworkInterfaceType.Tunnel)).Where(
                            ni => ni.Speed >= minimumSpeed).Any(
                                ni => (ni.Description.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) < 0) && 
                                      (ni.Name.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) < 0));
        }

        #endregion
    }
}