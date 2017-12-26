using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SD.LLBLGen.Pro.ApplicationCore;

namespace LLBLGenKeygen
{
    class Program
    {
        static void Main(string[] args)
        {
            //            var privateKey =
            //                "<RSAKeyValue><Modulus>7BxBULgjzjO7b7rIjbNeJIWD291TbtAB718Odwu9YWGebfZ9ahHjE9f/imaZsfGq/tukReSxerx/MvplMIOXhvrjMXqvtZ/q+1h8+z3Og2pIvwwyCbFwTjKx0/sG8ZbBAceZmGCYfb8j5x4rU5sQMFSqf5DbpyeMOEQaHUt7ZOU=</Modulus><Exponent>AQAB</Exponent><P>8LKCVnVGAWaxmqhYdMuOeoi+YPLX4T1hiWavFqsrJNnjBoVdT6QxOPT5j7hCXLvx3iRdQrsZKEpEaQBOstFpqw==</P><Q>+x8XTLbqns9ju2WM2DRLYwv7B3K3g7yKgeJQ/rQlL6iz6YX08AQW515IDBHJEoQ0iYk4bvl29j646xruGK17rw==</Q><DP>Kxlbu01+fou765yPUkKMvaY0qLlzLHLIP0kyutVlgVC+lRFWVwdohPFgqnps75v7wDI0vNkxtQQvYbnbXaufxw==</DP><DQ>RN448JP1cgokKr9lyeFFj2s4s8k1JM6vGYGsfr1+uTxF4tQW4T/t3BPSJGU4RHi3Q8S7Ekwd4NhAtFFVXLUvBw==</DQ><InverseQ>LsUAm0azdGUuh2f8kaddgN808P6aHuYpRjzmu+CwMQB5Z4g7HaXrTcxX5WoVOUJ0Y8LOR6pn3J9lgWTKJv/dhg==</InverseQ><D>tb/KR2h3p3MLBaayWuGHxnVAWy6z2skjtC9n4xuWXC/Y1Ky5Pb0nH09V1iPEi8WL60MH4QV52RDgmU1GN2IUcMh4snmjM+3iQmv9NtZ3PuygZvb/n+Tb/BJ6DyLpNqjXg0cKrdENFzl5Qu5qFDQloH5c8WplnMXDRssXV5kttME=</D></RSAKeyValue>";
            ////                <RSAKeyValue><Modulus>xoKoGaE2eyT2ZKvEUaJyw7TM4HOrfcFcmaABM5qpzIoq8VMGBi68oqo3mPlG9zu2S1yN2AXKl3ogmrXpwRfPCO + YG7FSEr3OW / juBiuOtgkq14ebaH1ythRT / yWHVDfCphjYVh7RefjvKgtqyYpvPTZh / e8eDlU5NpKccZo7NSM =</ Modulus >< Exponent > AQAB </ Exponent ></ RSAKeyValue >
            //            var publicKey =
            //                "<RSAKeyValue><Modulus>7BxBULgjzjO7b7rIjbNeJIWD291TbtAB718Odwu9YWGebfZ9ahHjE9f/imaZsfGq/tukReSxerx/MvplMIOXhvrjMXqvtZ/q+1h8+z3Og2pIvwwyCbFwTjKx0/sG8ZbBAceZmGCYfb8j5x4rU5sQMFSqf5DbpyeMOEQaHUt7ZOU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            //            var licenseInfo = new LicenseInfo()
            //            {
            //                Company = "changer ltd",
            //                Country = "中国",
            //                SubscriptionEndDateUTC = DateTime.UtcNow.AddDays(9999),
            //                Licensee = "pedoc",
            //                LicenseNr = "1",
            //                TypeOfLicense = LicenseType.Normal,
            //                LicenseCreationDateTimeUTC = DateTime.UtcNow,
            //            };

            //            using (var rsa = new RSACryptoServiceProvider())
            //            {
            //                rsa.FromXmlString(privateKey);
            //                XmlDocument xmlDocument = new XmlDocument();
            //                xmlDocument.LoadXml($@"<LLBLGenProLicense xmlns=""""><LicenseeInfo><Licensee>{licenseInfo.Licensee}</Licensee><Company>{licenseInfo.Company}</Company><Country>{licenseInfo.Country}</Country><LicenseNr>{licenseInfo.LicenseNr}</LicenseNr></LicenseeInfo><LicenseType>{(int)licenseInfo.TypeOfLicense}</LicenseType><LicenseCreationTime>{licenseInfo.LicenseCreationDateTimeUTC.Ticks}</LicenseCreationTime><ExpirationDate><Month>{licenseInfo.SubscriptionEndDateUTC.Month}</Month><Day>{licenseInfo.SubscriptionEndDateUTC.Day}</Day><Year>{licenseInfo.SubscriptionEndDateUTC.Year}</Year></ExpirationDate><SubscriptionEndDate><Month>{licenseInfo.SubscriptionEndDateUTC.Month}</Month><Day>{licenseInfo.SubscriptionEndDateUTC.Day}</Day><Year>{licenseInfo.SubscriptionEndDateUTC.Year}</Year></SubscriptionEndDate></LLBLGenProLicense>");
            //                SignedXml signedXml = new SignedXml
            //                {
            //                    SigningKey = rsa
            //                };
            //                DataObject dataObject = new DataObject
            //                {
            //                    Data = xmlDocument.ChildNodes,
            //                    Id = "LLBLGenPro5License"
            //                };

            //                signedXml.AddObject(dataObject);
            //                Reference reference = new Reference();
            //                reference.Uri = "#LLBLGenPro5License";
            //                signedXml.AddReference(reference);
            //                signedXml.ComputeSignature();
            //                var signXml = signedXml.GetXml();

            //                LicenseVerifier.Verify(signXml.OuterXml, (str1, str2) => { }, (str1, str2) => { }, (str1, str2) => { }, GetNullDate(), publicKey);
            //                Console.WriteLine(signXml.OuterXml);
            //                Console.ReadKey();
            //            }

            ReflectionTest test = new ReflectionTest();
            test.CreateGeneric();
            Console.ReadKey();
        }

        private static DateTime GetNullDate()
        {
            string str = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LLBLGen Pro");
            if (!Directory.Exists(str))
            {
                Directory.CreateDirectory(str);
            }
            str = Path.Combine(str, ApplicationConstants.UsageStatsFileName);
            return (File.Exists(str) ? File.GetLastWriteTimeUtc(str) : DateTime.MinValue);
        }
    }
}
