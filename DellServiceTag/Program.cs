using DellServiceTagData;
using DellServiceTagSelenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DellServiceTagLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            var servicetags = args.Any() ? args.ToList() : new[] { "16Q81M1", "1RNGTH1", "1ZMXPM1", "21YHNH1", "23SP5M1", "24J71J1", "25GC6H1", "2QMWWF1", "2SSP4L1", "3067XH1", "32FYTK1", "37DTVF1", "3G8TZK1", "3NRBTL1", "3NVVWF1", "3RFFQH1", "4ZWYZL1", "51J41M1", "532D5L1", "554C1L1", "56V9WF1", "5CD8VK1", "5GFFQJ1", "5JL73M1", "5M85XH1", "5T37NK1", "5YXHPH1", "6BG00M1", "6DC41L1", "6LK81M1", "6R4N4L1", "6SRQWF1", "70941L1", "71941L1", "7HJHPH1", "7HN5DK1", "7Q2FJK1", "7WF3NH1", "7WTVWF1", "8M85XH1", "8MM2VH1", "8MNR4L1", "8QXKSL1", "8QZ20M1", "8S980M1", "8X6NXH1", "8ZMXPM1", "94NH94J", "9HP81M1", "9JL73M1", "9MM2VH1", "9P4N4L1", "9YJLGJ1", "B27M6M1", "BYTJMJ1", "C0YHNH1", "CHLSRK1", "CHP81M1", "CKNFQJ1", "CS6TVF1", "D27M6M1", "D2L36L1", "D2YW2L1", "D4HB3M1", "DNRXZK1", "F1F8VL1", "F47XPM1", "FMRBTL1", "FRQR0L1", "FSBC6H1", "FT4TN12", "FWPHRK1", "G8CY5L1", "G9V27L1", "GFP81M1", "GHP81M1", "HMRBTL1", "HNS5WF1", "HSXW2L1", "HV63VH1", "HY2YNH1", "JDV7ZK1", "JJS86H1", "JMYPHL1", "JT6TVF1", "JTSPRK1" }.ToList();
            var db = new DatabaseWritter();
            while (servicetags.Any()) { 
                try
                {
                    if (!db.CheckIfExists(servicetags[0]))
                    {
                        DellAsset dellAsset = null;
                        using (var selenium = new SeleniumDataScrapper())
                        {
                            dellAsset = selenium.GetDellAsset(servicetags[0]) as DellAsset;
                        }

                        db.CreateDellAsset(dellAsset);
                    }

                    servicetags.Remove(servicetags[0]);
                }
                catch (Exception e)
                {
                    File.AppendAllLines("log.txt", new[] {Environment.NewLine,  DateTime.Now.ToString(), servicetags[0], e.Message, e.StackTrace });
                }
            }
        } 
    }
}
