// -----------------------------------------------------------------------------
//           $Copyright: (C) Copyright by Bayer Healthcare 2013. All rights reserved. $
// -----------------------------------------------------------------------------
// 
//  $Source: AxedaAgent/Provisioning Data/ProvisioningData.cs $
//  $Revision: 1.1 $
//  $Date: 2012/09/12 14:43:41EDT $
//  $Author: MLHAY $
//  $ProjectName: g:/MKS_Files/Service_Engineering/ApplicationFramework/AxedaAgent/project.pj $
// 
// -----------------------------------------------------------------------------
namespace Medrad.ServiceEngineering.ApplicationFramework.Axeda.Agent
{
	using Medrad.ServiceEngineering.ApplicationFramework.Axeda.Agent;
    using System.ComponentModel;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Provides access to the provisioning data elements
    /// </summary>
    public class ProvisioningData : INotifyPropertyChanged
    {
        #region Constants

        /// <summary>
        /// Expected path of the provisioning file
        /// </summary>
        private string ProvisionFilePath = AxedaFilePathConfiguration.GetFilePaths().ProvisioningFilePath;
        
        /// <summary>
        /// Organization property name
        /// </summary>
        private const string PropertyNameOrganization = "Organization";
        
        /// <summary>
        /// Site name property name
        /// </summary>
        private const string PropertyNameSiteName = "SiteName";
        
        /// <summary>
        /// Site location property name
        /// </summary>
        private const string PropertyNameSiteLocation = "SiteLocation";

        /// <summary>
        /// Contact Name property name
        /// </summary>
        private const string PropertyNameClinicalContactName = "ClinicalContactName";

        /// <summary>
        /// Phone number property name
        /// </summary>
        private const string PropertyNameClinicalContactPhoneNumber = "ClinicalContactPhoneNumber";
        
        /// <summary>
        /// Extension property name
        /// </summary>
        private const string PropertyNameClinicalContactExtension = "ClinicalContactExtension";

        /// <summary>
        /// Combination phone and extension property name
        /// </summary>
        private const string PropertyNameClinicalPhoneAndExtension = "ClinicalPhoneAndExtension";

        #endregion

        #region Private Members

        /// <summary>
        /// The current provision data
        /// </summary>
        private SetProvisioning provisioningData;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the ProvisioningData class
        /// </summary>
        public ProvisioningData()
        {
            // Load the provision data from the XML file
            this.LoadProvisioningFile();
        }

        #endregion

        #region Events

        /// <summary>
        /// Event fired when a property value is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the organization
        /// </summary>
        public string Organization
        {
            get
            {
                return this.provisioningData.Customer;
            }

            set
            {
                if (value != this.Organization)
                {
                    this.provisioningData.Customer = value;
                    this.NotifyPropertyChanged(PropertyNameOrganization);
                }
            }
        }

        /// <summary>
        /// Gets or sets the site name
        /// </summary>
        public string SiteName
        {
            get
            {
                return this.provisioningData.Location.Name;
            }

            set
            {
                if (value != this.SiteName)
                {
                    this.provisioningData.Location.Name = value;
                    this.NotifyPropertyChanged(PropertyNameSiteName);
                }
            }
        }

        /// <summary>
        /// Gets or sets the site location
        /// </summary>
        public string SiteLocation
        {
            get
            {
                // If the property is not found then create new one with empty value
                string returnValue = string.Empty;
                int index = this.GetPropertyIndex(PropertyNameSiteLocation);
                if (index == -1)
                {
                    this.CreateProperty(PropertyNameSiteLocation, string.Empty);
                }
                else
                {
                    returnValue = ((ProvisioningProperty)this.provisioningData.Properties[index]).Value;
                }

                return returnValue;
            }

            set
            {
                // If value has changed go ahead and set new value
                if (value != this.SiteLocation)
                {
                    // If the property is not found then create a new one with the passed in value
                    int index = this.GetPropertyIndex(PropertyNameSiteLocation);
                    if (index == -1)
                    {
                        this.CreateProperty(PropertyNameSiteLocation, value);
                    }
                    else
                    {
                        ProvisioningProperty newProperty = new ProvisioningProperty();
                        newProperty.Name = ((ProvisioningProperty)this.provisioningData.Properties[index]).Name;
                        newProperty.Value = value;
                        this.provisioningData.Properties[index] = newProperty;
                    }

                    this.NotifyPropertyChanged(PropertyNameSiteLocation);
                }
            }
        }

        /// <summary>
        /// Gets or sets the contact name
        /// </summary>
        public string ClinicalContactName
        {
            get
            {
                // If the property is not found then create new one with empty value
                string returnValue = string.Empty;
                int index = this.GetPropertyIndex(PropertyNameClinicalContactName);
                if (index == -1)
                {
                    this.CreateProperty(PropertyNameClinicalContactName, string.Empty);
                }
                else
                {
                    returnValue = ((ProvisioningProperty)this.provisioningData.Properties[index]).Value;
                }

                return returnValue;
            }

            set
            {
                // If value has changed go ahead and set new value
                if (value != this.ClinicalContactName)
                {
                    // If the property is not found then create a new one with the passed in value
                    int index = this.GetPropertyIndex(PropertyNameClinicalContactName);
                    if (index == -1)
                    {
                        this.CreateProperty(PropertyNameClinicalContactName, value);
                    }
                    else
                    {
                        ProvisioningProperty newProperty = new ProvisioningProperty();
                        newProperty.Name = ((ProvisioningProperty)this.provisioningData.Properties[index]).Name;
                        newProperty.Value = value;
                        this.provisioningData.Properties[index] = newProperty;
                    }

                    this.NotifyPropertyChanged(PropertyNameClinicalContactName);
                }
            }
        }        

        /// <summary>
        /// Gets or sets the phone number
        /// </summary>
        public string ClinicalContactPhoneNumber
        {
            get
            {
                // If the property is not found then create new one with empty value
                string returnValue = string.Empty;
                int index = this.GetPropertyIndex(PropertyNameClinicalContactPhoneNumber);
                if (index == -1)
                {
                    this.CreateProperty(PropertyNameClinicalContactPhoneNumber, string.Empty);
                }
                else
                {
                    returnValue = ((ProvisioningProperty)this.provisioningData.Properties[index]).Value;
                }

                return returnValue;
            }

            set
            {
                // If value has changed go ahead and set new value
                if (value != this.ClinicalContactPhoneNumber)
                {
                    // If the property is not found then create a new one with the passed in value
                    int index = this.GetPropertyIndex(PropertyNameClinicalContactPhoneNumber);
                    if (index == -1)
                    {
                        this.CreateProperty(PropertyNameClinicalContactPhoneNumber, value);
                    }
                    else
                    {
                        ProvisioningProperty newProperty = new ProvisioningProperty();
                        newProperty.Name = ((ProvisioningProperty)this.provisioningData.Properties[index]).Name;
                        newProperty.Value = value;
                        this.provisioningData.Properties[index] = newProperty;
                    }

                    this.NotifyPropertyChanged(PropertyNameClinicalContactPhoneNumber);
                    this.NotifyPropertyChanged(PropertyNameClinicalPhoneAndExtension);
                }
            }
        }

        /// <summary>
        /// Gets or sets the extension
        /// </summary>
        public string ClinicalContactExtension
        {
            get
            {
                // If the property is not found then create new one with empty value
                string returnValue = string.Empty;
                int index = this.GetPropertyIndex(PropertyNameClinicalContactExtension);
                if (index == -1)
                {
                    this.CreateProperty(PropertyNameClinicalContactExtension, string.Empty);
                }
                else
                {
                    returnValue = ((ProvisioningProperty)this.provisioningData.Properties[index]).Value;
                }

                return returnValue;
            }

            set
            {
                // If value has changed go ahead and set new value
                if (value != this.ClinicalContactExtension)
                {
                    // If the property is not found then create a new one with the passed in value
                    int index = this.GetPropertyIndex(PropertyNameClinicalContactExtension);
                    if (index == -1)
                    {
                        this.CreateProperty(PropertyNameClinicalContactExtension, value);
                    }
                    else
                    {
                        ProvisioningProperty newProperty = new ProvisioningProperty();
                        newProperty.Name = ((ProvisioningProperty)this.provisioningData.Properties[index]).Name;
                        newProperty.Value = value;
                        this.provisioningData.Properties[index] = newProperty;
                    }

                    this.NotifyPropertyChanged(PropertyNameClinicalContactExtension);
                    this.NotifyPropertyChanged(PropertyNameClinicalPhoneAndExtension);
                }
            }
        }

        /// <summary>
        /// Gets the phone and extension. This is a convenience property to display
        /// the phone and extension together. It is not stored in the provisioning file.
        /// </summary>
        public string ClinicalPhoneAndExtension
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.ClinicalContactExtension) == true)
                {
                    return this.ClinicalContactPhoneNumber;
                }
                else
                {
                    return this.ClinicalContactPhoneNumber + " ext. " + this.ClinicalContactExtension;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves the current values in the class to the provisioning xml file
        /// </summary>
        public void SaveData()
        {
            this.SaveProvisioningFile();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Notify caller that the named property has been updated
        /// </summary>
        /// <param name="propertyName">name of changed property </param>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Gets the index within the Properties list of the property with the passed in name
        /// </summary>
        /// <param name="propertyName">Name of property</param>
        /// <returns>Index of property if found, -1 if not found.</returns>
        private int GetPropertyIndex(string propertyName)
        {
            int index = -1;

            for (int i = 0; i < this.provisioningData.Properties.Count; i++)
            {
                if (this.provisioningData.Properties[i].Name == propertyName)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        /// <summary>
        /// Creates a new new property and adds it to the Properties list
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="value">Property value</param>
        private void CreateProperty(string name, string value)
        {
            ProvisioningProperty newProperty = new ProvisioningProperty();
            newProperty.Name = name;
            newProperty.Value = value;
            this.provisioningData.Properties.Add(newProperty);
        }

        /// <summary>
        /// Reads the provisioning XML File and populates the provisioning data
        /// </summary>
        private void LoadProvisioningFile()
        {
            // First create provisioning file if it isn't found
            this.CreateProvisioningFileIfNotFound();

            XmlSerializer serializer = new XmlSerializer(typeof(SetProvisioning));

            using (TextReader tr = new StreamReader(ProvisionFilePath))
            {
                this.provisioningData = (SetProvisioning)serializer.Deserialize(tr);
            }
        }

        /// <summary>
        /// Saves the current values of the provisioning data to the provisioning XML file.
        /// </summary>
        private void SaveProvisioningFile()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SetProvisioning));

            using (TextWriter tw = new StreamWriter(ProvisionFilePath))
            {
                serializer.Serialize(tw, this.provisioningData);
            }
        }

        /// <summary>
        /// Create an empty provisioning file if one is not found in the expected location
        /// </summary>
        private void CreateProvisioningFileIfNotFound()
        {
            if (File.Exists(ProvisionFilePath) == false)
            {
                // Create new SetProvisioning class and populate the customer and Location with empty data
                this.provisioningData = new SetProvisioning();
                this.provisioningData.Customer = string.Empty;
                this.provisioningData.Location = new Location();

                // Save it to create starting provision file
                this.SaveProvisioningFile();
            }
        }

        #endregion
    }
}
