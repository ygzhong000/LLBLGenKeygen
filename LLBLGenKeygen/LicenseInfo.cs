using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SD.LLBLGen.Pro.ApplicationCore;
using SD.LLBLGen.Pro.Core;

namespace LLBLGenKeygen
{
    public class LicenseInfo
    {
        public string Company
        {
            get;
            set;
        }

        public string Country
        {
            get;
            set;
        }

        public DateTime ExpirationDateUTC
        {
            get;
            set;
        }

        public bool Expires
        {
            get
            {
                return ((IEnumerable<LicenseType>)(new LicenseType[] { LicenseType.Beta, LicenseType.Trial })).Contains<LicenseType>(this.TypeOfLicense);
            }
        }

        public DateTime LicenseCreationDateTimeUTC
        {
            get;
            set;
        }

        public string Licensee
        {
            get;
            set;
        }

        public string LicenseNr
        {
            get;
            set;
        }

        public DateTime SubscriptionEndDateUTC
        {
            get;
            set;
        }

        public bool SubscriptionHasExpired
        {
            get
            {
                if (this.TypeOfLicense != LicenseType.Normal)
                {
                    return false;
                }
                return this.SubscriptionEndDateUTC < DateTime.UtcNow.ToUniversalDate();
            }
        }

        public LicenseType TypeOfLicense
        {
            get;
            set;
        }

        public LicenseInfo()
        {
        }

        public static LicenseInfo CreateLicenseInfo(XmlNode licenseeInfoBlock)
        {
            LicenseInfo licenseInfo = new LicenseInfo()
            {
                Licensee = licenseeInfoBlock.SelectSingleNode("LicenseeInfo/Licensee").InnerText,
                Company = licenseeInfoBlock.SelectSingleNode("LicenseeInfo/Company").InnerText,
                Country = licenseeInfoBlock.SelectSingleNode("LicenseeInfo/Country").InnerText,
                LicenseNr = licenseeInfoBlock.SelectSingleNode("LicenseeInfo/LicenseNr").InnerText
            };
            XmlNode xmlNodes = licenseeInfoBlock.SelectSingleNode("LicenseCreationTime");
            long num = XmlConvert.ToInt64(xmlNodes.InnerText);
            licenseInfo.LicenseCreationDateTimeUTC = new DateTime(num, DateTimeKind.Utc);
            xmlNodes = licenseeInfoBlock.SelectSingleNode("LicenseType");
            licenseInfo.TypeOfLicense = (LicenseType)XmlConvert.ToInt32(xmlNodes.InnerText);
            if (licenseeInfoBlock.SelectSingleNode("ExpirationDate") != null)
            {
                xmlNodes = licenseeInfoBlock.SelectSingleNode("ExpirationDate/Month");
                int num1 = Convert.ToInt32(xmlNodes.InnerText);
                xmlNodes = licenseeInfoBlock.SelectSingleNode("ExpirationDate/Day");
                int num2 = Convert.ToInt32(xmlNodes.InnerText);
                xmlNodes = licenseeInfoBlock.SelectSingleNode("ExpirationDate/Year");
                int num3 = Convert.ToInt32(xmlNodes.InnerText);
                licenseInfo.ExpirationDateUTC = new DateTime(num3, num1, num2, 0, 0, 0, DateTimeKind.Utc);
            }
            if (licenseeInfoBlock.SelectSingleNode("SubscriptionEndDate") != null)
            {
                xmlNodes = licenseeInfoBlock.SelectSingleNode("SubscriptionEndDate/Month");
                int num4 = Convert.ToInt32(xmlNodes.InnerText);
                xmlNodes = licenseeInfoBlock.SelectSingleNode("SubscriptionEndDate/Day");
                int num5 = Convert.ToInt32(xmlNodes.InnerText);
                xmlNodes = licenseeInfoBlock.SelectSingleNode("SubscriptionEndDate/Year");
                int num6 = Convert.ToInt32(xmlNodes.InnerText);
                licenseInfo.SubscriptionEndDateUTC = new DateTime(num6, num4, num5, 0, 0, 0, DateTimeKind.Utc);
            }
            else
            {
                licenseInfo.SubscriptionEndDateUTC = DateTime.MinValue;
            }
            return licenseInfo;
        }

        public string GetSubscriptionStateDescription()
        {
            DateTime subscriptionEndDateUTC;
            if (this.TypeOfLicense != LicenseType.Normal)
            {
                return string.Empty;
            }
            if (this.SubscriptionHasExpired)
            {
                subscriptionEndDateUTC = this.SubscriptionEndDateUTC;
                return string.Format("Expired. (Expired on {0})", subscriptionEndDateUTC.ToString("dd-MMM-yyyy"));
            }
            subscriptionEndDateUTC = this.SubscriptionEndDateUTC;
            return string.Format("Active. Expires on {0}.", subscriptionEndDateUTC.ToString("dd-MMM-yyyy"));
        }

        public string GetTimeLeftDescription()
        {
            if (!this.Expires)
            {
                return string.Empty;
            }
            TimeSpan expirationDateUTC = this.ExpirationDateUTC - DateTime.UtcNow.ToUniversalDate();
            object days = expirationDateUTC.Days;
            LicenseType typeOfLicense = this.TypeOfLicense;
            return string.Format(" ({0} day(s) left in {1} period)", days, typeOfLicense.ToString().ToLowerInvariant());
        }
    }
}
