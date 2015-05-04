// -----------------------------------------------------------------------------
//           $Copyright: (C) Copyright by Bayer Healthcare 2013. All rights reserved. $
// -----------------------------------------------------------------------------
// 
//  $Source: AxedaAgent/Utils/ConversionUtils.cs $
//  $Revision: 1.1 $
//  $Date: 2012/09/12 14:43:42EDT $
//  $Author: MLHAY $
//  $ProjectName: g:/MKS_Files/Service_Engineering/ApplicationFramework/AxedaAgent/project.pj $
// 
// -----------------------------------------------------------------------------

namespace Medrad.ServiceEngineering.ApplicationFramework.Axeda.Agent
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Common utilities to ease conversion of data types
    /// </summary>
    internal class ConversionUtils
    {
        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="ConversionUtils" />
        /// class from being created.
        /// </summary>
        private ConversionUtils()
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Converts the string to an int. If it fails, it will return
        /// null, which would be invalid.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns>
        /// Positive integer if Successful, otherwise null
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
                                                         "CA1031:DoNotCatchGeneralExceptionTypes",
                                                         Justification = "Conversions will fail to a specific value to notify the caller.")]
        public static int? StrToInt(string inputValue)
        {
            try
            {
                return int.Parse(inputValue, CultureInfo.CurrentCulture);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Converts the string to a nullible boolean.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns>
        /// <see cref="bool"/> if Successful, otherwise
        /// <see langword="null"/>
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
                                                         "CA1031:DoNotCatchGeneralExceptionTypes",
                                                         Justification = "Conversions will fail to a specific value to notify the caller.")]

        public static bool? StrToBool(string inputValue)
        {
            if (inputValue.Equals("0") || inputValue.Equals("-1"))
            {
                return false;
            }

            if (inputValue.Equals("1"))
            {
                return true;
            }

            try
            {
                return Convert.ToBoolean(inputValue, CultureInfo.CurrentCulture);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Converts <see cref="string"/> Values to Encryption Types.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns>
        /// The new Encryption or Unknown
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
                                                         "CA1031:DoNotCatchGeneralExceptionTypes",
                                                         Justification = "Conversions will fail to a specific value to notify the caller.")]

        public static EncryptionType StrToEncryptionType(string inputValue)
        {
            try
            {
                return (EncryptionType)Enum.Parse(typeof(EncryptionType), inputValue);
            }
            catch (Exception)
            {
                // Encryption types can actually be stored 2 ways, the first is as an enum, the second, is as a string.  
                // We obviously failed the enum portion, so we'll try the string parsing now.
                inputValue = inputValue.ToUpperInvariant();

                if (inputValue.Equals("NONE"))
                {
                    return EncryptionType.None;
                }

                if (inputValue.Equals("BLOWFISH"))
                {
                    return EncryptionType.Blowfish;
                }

                if (inputValue.Equals("AES"))
                {
                    return EncryptionType.Aes;
                }

                if (inputValue.Equals("SSL"))
                {
                    return EncryptionType.Ssl;
                }

                // Note we are expecting this to catch all exceptions and just return back unknown.  This will
                // allow the calling application to handle this gracefully when the decision to use the viable occurs.
                return EncryptionType.Unknown;
            }
        }

        /// <summary>
        /// Converts <see cref="string"/> Values to Ssl levels.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns>
        /// The new Ssl Level or Unknown
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
                                                         "CA1031:DoNotCatchGeneralExceptionTypes",
                                                         Justification = "Conversions will fail to a specific value to notify the caller.")]

        public static SslEncryptionLevel StrToSslLevel(string inputValue)
        {
            // There are 2 ways to convert Strings to SSL levels.   One way would be to see if the SSL level is an 
            // integer.  If it is, we'll just return the representation of it.   
            int newValue;
            if (int.TryParse(inputValue, out newValue))
            {
                switch (newValue)
                {
                    case (int)SslEncryptionLevel.Bit128:
                        return SslEncryptionLevel.Bit128;

                    case (int)SslEncryptionLevel.Bit168:
                        return SslEncryptionLevel.Bit168;

                    case (int)SslEncryptionLevel.Bit40:
                        return SslEncryptionLevel.Bit40;

                    case (int)SslEncryptionLevel.None:
                        return SslEncryptionLevel.None;
                    default:
                        return SslEncryptionLevel.Unknown;
                } 
            }

            // Otherwise, we'll try to parse the string.  Axeda does not standardize.  
            try
            {
                return (SslEncryptionLevel)Enum.Parse(typeof(SslEncryptionLevel), inputValue);
            }
            catch (Exception)
            {
                // Note we are expecting this to catch all exceptions and just return back unknown.  This will
                // allow the calling application to handle this gracefully when the decision to use the viable occurs.  
                return SslEncryptionLevel.Unknown;
            }
        }

        /// <summary>
        /// Converts <see cref="string"/> Values to Ssl levels.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns>
        /// The new Ssl Level or Unknown
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
                                                         "CA1031:DoNotCatchGeneralExceptionTypes",
                                                         Justification = "Conversions will fail to a specific value to notify the caller.")]

        public static string SslLevelToString(SslEncryptionLevel inputValue)
        {
            if (inputValue == SslEncryptionLevel.None)
            {
                return "none";
            }
            
            if (inputValue == SslEncryptionLevel.Bit40)
            {
                return "40";
            }
            
            if (inputValue == SslEncryptionLevel.Bit128)
            {
                return "128";
            }

            if (inputValue == SslEncryptionLevel.Bit168)
            {
                return "168";
            }

            // If we made it this far, just return null.  
            return null; 
        }

        /// <summary>
        /// Converts a string to an agent status.  
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns>
        /// The new Agent Status or Unknown
        /// </returns>
        public static AgentStatus StrToAgentStatus(string inputValue)
        {
            if (inputValue.Equals("Offline") || inputValue.Equals("Starting"))
            {
                return AgentStatus.EnterpriseUnreachable;
            }

            if (inputValue.Equals("Online"))
            {
                return AgentStatus.ConnectedToEnterprise;
            }

            return AgentStatus.Unknown;
        }
        
        #endregion
    }
}