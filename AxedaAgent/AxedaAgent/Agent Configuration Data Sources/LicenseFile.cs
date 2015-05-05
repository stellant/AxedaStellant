// -----------------------------------------------------------------------------
//           $Copyright: (C) Copyright by Bayer Healthcare 2013. All rights reserved. $
// -----------------------------------------------------------------------------
// 
//  $Source: AxedaAgent/Agent Configuration Data Sources/LicenseFile.cs $
//  $Revision: 1.1 $
//  $Date: 2012/09/12 14:43:39EDT $
//  $Author: MLHAY $
//  $ProjectName: g:/MKS_Files/Service_Engineering/ApplicationFramework/AxedaAgent/project.pj $
// 
// -----------------------------------------------------------------------------

namespace Medrad.ServiceEngineering.ApplicationFramework.Axeda.Agent
{
	using Medrad.ServiceEngineering.ApplicationFramework.Axeda.Agent;
    using System.Collections.Specialized;
    using System.Xml;

    /// <summary>
    /// This class will read all of the values in an Axeda license file
    /// </summary>
    public class LicenseFile
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseFile" /> class.
        /// </summary>
        /// <remarks>
        /// Class Based on Solution Number 2038 from Axeda
        /// </remarks>
        /// <param name="licenseFilePath">
        /// Holds the path to the license file
        /// </param>
        public LicenseFile(string licenseFilePath = "")
        {
            // This we'll initialize a collection of strings, which will be our de-facto "xml" parsing.  
            // Axeda Files are not strong XML.  They use XML more or less as raw text file storage.  We'll do a two part
            // decode of these files, first, we'll read all of the values, then, we'll order based assign them to an object
            // to make manipulation easier.  
            var values = new StringCollection();
            if (string.IsNullOrEmpty(licenseFilePath))
            {
                licenseFilePath = AxedaFilePathConfiguration.GetFilePaths().LicenseConfigPath;
            }
            using (XmlReader rdr = XmlReader.Create(licenseFilePath))
            {
                while (rdr.Read())
                {
                    // We're only concerned with text data.  If we find text, we'll just add it to our list.  
                    if (rdr.NodeType == XmlNodeType.Text)
                    {
                        // Axeda uses quotes to delimit strings, so we'll strip them before we add them to the collection.  
                        values.Add(rdr.Value.Replace("\"", string.Empty).Trim());
                    }
                }
            }

            // If we aren't able to read all of the elements, we will fail safe and not set any element values.  If everything 
            // works out though, we will be setting the values accordingly based on the index positions.  
            if (values.Count == (int)ElementPos.TotalElements)
            {
                this.FileFormat = ConversionUtils.StrToInt(values[(int)ElementPos.FileFormat]);
                this.ReservedValue1 = values[(int)ElementPos.ReservedValue1];
                this.ModelName = values[(int)ElementPos.ModelName];
                this.SerialNumber = values[(int)ElementPos.SerialNumber];
                this.Owner = values[(int)ElementPos.Owner];
                this.IdentificationLibraryName = values[(int)ElementPos.IdentificationLibaryName];
                this.IdentificationConfigurationString = values[(int)ElementPos.IdentificationConfigurationString];
            }
        }

        #endregion

        #region Nested type: ElementPos

        /// <summary>
        /// Holds the positions in the license file
        /// </summary>
        private enum ElementPos
        {
            /// <summary>
            /// File Format Identifier Position
            /// </summary>
            FileFormat = 0,

            /// <summary>
            /// Axeda Reserved Value #1 Position
            /// </summary>
            ReservedValue1,

            /// <summary>
            /// Model Name Position
            /// </summary>
            ModelName,

            /// <summary>
            /// Serial Number Position
            /// </summary>
            SerialNumber,

            /// <summary>
            /// <see cref="Owner"/> Name Position
            /// </summary>
            Owner,

            /// <summary>
            /// Identification Library Name Position
            /// </summary>
            IdentificationLibaryName,

            /// <summary>
            /// Identification Configuration String Position
            /// </summary>
            IdentificationConfigurationString,

            /// <summary>
            /// Holds the total number of elements that we expect in the license
            /// file.
            /// </summary>
            TotalElements
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the file format version number.
        /// </summary>
        public int? FileFormat { get; private set; }

        /// <summary>
        /// Gets value is explicitly no longer used by Axeda but remains in the
        /// file.
        /// </summary>
        public string ReservedValue1 { get; private set; }

        /// <summary>
        /// Gets the model name for the Axeda device.
        /// </summary>
        public string ModelName { get; private set; }

        /// <summary>
        /// Gets the serial number for the Axeda device.
        /// </summary>
        public string SerialNumber { get; private set; }

        /// <summary>
        /// Gets the owner of the Axeda device.
        /// </summary>
        public string Owner { get; private set; }

        /// <summary>
        /// Gets the name of the identification library.
        /// </summary>
        /// <value>
        /// The name of the identification library.
        /// </value>
        public string IdentificationLibraryName { get; private set; }

        /// <summary>
        /// Gets the identification configuration string.
        /// </summary>
        public string IdentificationConfigurationString { get; private set; }

        #endregion

        #region Public Methods and Operators
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this
        /// instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "License File Contents: \n" +
                   "\tFile Format = " + this.FileFormat + "\n" +
                   "\tReserved Value 1 = " + this.ReservedValue1 + "\n" +
                   "\tModel Name = " + this.ModelName + "\n" +
                   "\tSerial Number = " + this.SerialNumber + "\n" +
                   "\tOwner = " + this.Owner + "\n" +
                   "\tIdentification Library Name = " + this.IdentificationLibraryName + "\n" +
                   "\tIdentification Configuration String = " + this.IdentificationConfigurationString + "\n";
        }

        #endregion
    }
}