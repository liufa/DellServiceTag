using DellServiceTagData;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DellServiceTagSelenium
{
    public class SeleniumDataScrapper
    {
        public SeleniumDataScrapper() {
            this.Driver = new ChromeDriver(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Driver\"));
        }

        public ChromeDriver Driver { get; private set; }

        public IComClassDellAsset GetDellAsset(string assetId)
        {
            this.Driver.Url = $"http://www.dell.com/support/home/us/en/19/product-support/servicetag/{assetId}/diagnose";
            this.Driver.Navigate();
            var feedbackOverlay = this.Driver.FindElementsByCssSelector("[onclick='ipe120994.clWin();']'");
            if(feedbackOverlay.Any())
                feedbackOverlay.First().Click();

            var configurationTab = this.Driver.FindElementsById("tab-configuration");
            configurationTab.First().Click();

            return null;
        }
    }
}
