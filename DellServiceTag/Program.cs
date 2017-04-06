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
            try
            {
                var assetId = "9QWPGT1";
                var dellAsset = new ApiCaller().GetDellAsset(assetId, "asdadadasd");
                if (dellAsset == null)
                {
                    var selenium = new SeleniumDataScrapper();
                     dellAsset = selenium.GetDellAsset(assetId);
                }
                new DatabaseWritter().CreateDellAsset(dellAsset);
            }
            catch (Exception e) {
                File.AppendAllLines("log.txt", new[] { Environment.NewLine, DateTime.Now.ToString(), e.Message, e.StackTrace });
            }
        }
    }
}
