using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SD.LLBLGen.Pro.ApplicationCore;
using SD.LLBLGen.Pro.ApplicationCore.ProjectClasses;
using SD.LLBLGen.Pro.Core;

namespace LLBLGenKeygen
{
    internal static class LicenseVerifier
    {
        internal static LicenseInfo Verify(string signedXml, Action<string, string> messageReporterFunc, Action<string, string> errorDisplayFunc, Action<string, string> noLicenseFoundReporterFunc, DateTime nullDate,string publicKey)
        {
            LicenseInfo licenseInfo;
            SignedXml signedXml1 = new SignedXml();
            using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
            {
                rSACryptoServiceProvider.FromXmlString(publicKey);
                if (!string.IsNullOrEmpty(signedXml))
                {
                    XmlDocument xmlDocument = new XmlDocument()
                    {
                        PreserveWhitespace = true
                    };
                    xmlDocument.LoadXml(signedXml);
                    XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Signature");
                    signedXml1.LoadXml((XmlElement)elementsByTagName[0]);
                    if (signedXml1.CheckSignature(rSACryptoServiceProvider))
                    {
                        LicenseInfo licenseInfo1 = LicenseInfo.CreateLicenseInfo(xmlDocument.SelectSingleNode(".//LLBLGenProLicense"));
                        switch (licenseInfo1.TypeOfLicense)
                        {
                            case LicenseType.Trial:
                            case LicenseType.Beta:
                                {
                                    if (!licenseInfo1.Expires)
                                    {
                                        if (errorDisplayFunc != null)
                                        {
                                            errorDisplayFunc("The license file is invalid", "Invalid license file");
                                        }
                                        licenseInfo1 = null;
                                        goto case LicenseType.Lite;
                                    }
                                    else
                                    {
                                        if (!(licenseInfo1.LicenseCreationDateTimeUTC > DateTime.UtcNow) && !(licenseInfo1.ExpirationDateUTC < DateTime.UtcNow.ToUniversalDate()) && !(nullDate > DateTime.UtcNow))
                                        {
                                            goto case LicenseType.Lite;
                                        }
                                        if (messageReporterFunc != null)
                                        {
                                            LicenseType typeOfLicense = licenseInfo1.TypeOfLicense;
                                            messageReporterFunc(string.Format("The {0} period has ended as your license has expired.", typeOfLicense.ToString().ToLowerInvariant()), "License expired");
                                        }
                                        licenseInfo1 = null;
                                        goto case LicenseType.Lite;
                                    }
                                }
                            case LicenseType.Normal:
                                {
                                    DateTime linkerTimeUTC = typeof(Project).Assembly.GetLinkerTimeUTC();
                                    if (linkerTimeUTC <= licenseInfo1.SubscriptionEndDateUTC)
                                    {
                                        goto case LicenseType.Lite;
                                    }
                                    if (messageReporterFunc != null)
                                    {
                                        string str = linkerTimeUTC.ToString("dd-MMM-yyyy");
                                        DateTime subscriptionEndDateUTC = licenseInfo1.SubscriptionEndDateUTC;
                                        messageReporterFunc(string.Format("Sorry, but this build isn't allowed to be used with your license as it was released after your subscription expired (Build is from {0}, your subscription expired on {1}). Please renew your subscription to use this build and newer builds, or go to the LLBLGen Pro website and download a build released before {1}.", str, subscriptionEndDateUTC.ToString("dd-MMM-yyyy")), "Build is incompatible with expired subscription");
                                    }
                                    licenseInfo1 = null;
                                    goto case LicenseType.Lite;
                                }
                            case LicenseType.Lite:
                                {
                                    licenseInfo = licenseInfo1;
                                    break;
                                }
                            default:
                                {
                                    goto case LicenseType.Lite;
                                }
                        }
                    }
                    else
                    {
                        if (errorDisplayFunc != null)
                        {
                            errorDisplayFunc("The license file signature is invalid", "Invalid license file");
                        }
                        licenseInfo = null;
                    }
                }
                else
                {
                    if (noLicenseFoundReporterFunc != null)
                    {
                        noLicenseFoundReporterFunc("No license files found. Please install your LLBLGen Pro license file in the application's folder and restart the application. If you downloaded the trial version, be sure to request a trial license. You can request one on the LLBLGen Pro website or by clicking the 'Request Trial License' button below.", "No license file found");
                    }
                    licenseInfo = null;
                }
            }
            return licenseInfo;
        }
    }
}
