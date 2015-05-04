// -----------------------------------------------------------------------------
//           $Copyright: (C) Copyright by Bayer Healthcare 2013. All rights reserved. $
// -----------------------------------------------------------------------------
// 
//  $Source: AxedaAgent/Provisioning Data/SetProvisioning.cs $
//  $Revision: 1.1 $
//  $Date: 2012/09/12 14:43:41EDT $
//  $Author: MLHAY $
//  $ProjectName: g:/MKS_Files/Service_Engineering/ApplicationFramework/AxedaAgent/project.pj $
// 
// -----------------------------------------------------------------------------
namespace Medrad.ServiceEngineering.ApplicationFramework.Axeda.Agent
{
    using System;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    #region CLASS Location

    /// <summary>
    /// Provides a holder for the provisioning location data
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the address line 1
        /// </summary>
        public string Line1 { get; set; }

        /// <summary>
        /// Gets or sets the address line 2
        /// </summary>
        public string Line2 { get; set; }

        /// <summary>
        /// Gets or sets the city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the zip code
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the region
        /// </summary>
        public string Region { get; set; }
    }

    #endregion

    #region CLASS: Property

    /// <summary>
    /// Provides a holder for the provisioning properties
    /// </summary>
    [XmlType(TypeName = "Property")]
    public class ProvisioningProperty
    {
        /// <summary>
        /// Gets or sets the property name
        /// </summary> 
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the property value
        /// </summary>
        public string Value { get; set; }
    }

    #endregion

    #region CLASS: SetProvisioning

    /// <summary>
    /// Provides access to all the provisioning data
    /// </summary>
    [Serializable]
    public class SetProvisioning
    {
        /// <summary>
        /// Gets or sets the customer
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// Gets or sets the location information
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Gets or sets the provisioning properties
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "This class acts a data holder and is actually populated from class ProvisioningData by design.")]
        public Collection<ProvisioningProperty> Properties { get; set;  }
    }

    #endregion
}
