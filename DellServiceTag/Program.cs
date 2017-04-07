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
                var serviceTag = "9QWPGT1";// args[0];// "9QWPGT1";
                var dellAsset = new ApiCaller().GetDellAsset(serviceTag, "asdadadasd");
                if (dellAsset == null)
                {
                    using (var selenium = new SeleniumDataScrapper())
                    {
                        dellAsset = selenium.GetDellAsset(serviceTag);
                    }
                }
                new DatabaseWritter().CreateDellAsset(dellAsset);
            }
            catch (Exception e)
            {
                File.AppendAllLines("log.txt", new[] { Environment.NewLine, DateTime.Now.ToString(), e.Message, e.StackTrace });
            }
        }
    }
}
