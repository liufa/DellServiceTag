using DellServiceTagData;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DellServiceTagSelenium
{
    [Guid("EAA4976A-45C3-4BC5-BC0B-E474F4C3C82D")]
    [ComVisible(true)]
    public interface IComClassSeleniumDataScrapper
    {
        ChromeDriver Driver { get; }
        IComClassDellAsset GetDellAsset(string serviceTag);

    }

    [Guid("7BD20046-DF8C-44A6-8F6B-687FAA26FA59"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [ComVisible(true)]
    public interface IComClassSeleniumDataScrapperEvents
    {
    }

    [Guid("0D53A3E8-E51A-49C7-944E-E72A2064F926"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(IComClassSeleniumDataScrapperEvents))]
    [ComVisible(true)]
    [ProgId("ProgId.DatabaseWritter")]
    public class SeleniumDataScrapper : IComClassSeleniumDataScrapper, IDisposable
    {
        public SeleniumDataScrapper() {
            this.Driver = new ChromeDriver(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Driver\"));
        }

        public ChromeDriver Driver { get; private set; }

        public void Dispose()
        {
            this.Driver.Dispose();
        }

        public IComClassDellAsset GetDellAsset(string serviceTag)
        {
            this.Driver.Url = $"http://www.dell.com/support/home/us/en/19/product-support/servicetag/{serviceTag}/diagnose";
            this.Driver.Navigate();
            var feedbackOverlay = this.Driver.FindElementsByCssSelector("[onclick='ipe120994.clWin();']");
            if (feedbackOverlay.Any())
                feedbackOverlay.First().Click();

            var configurationTab = this.Driver.FindElementsById("tab-configuration");
            configurationTab.First().Click();
            Thread.Sleep(3000);
            var configurationDivContainer = this.Driver.FindElementsById("subSectionA").First();

            var rows = configurationDivContainer.FindElements(By.CssSelector("tr"));
            var computerModelRow = rows.Where(o => o.Text.Contains("Computer Model")).FirstOrDefault();
            var computerModel = computerModelRow.FindElements(By.CssSelector("td"))[1].Text;
            var shippingDateRow = rows.Where(o => o.Text.Contains("Shipping Date")).FirstOrDefault();
            var shippingDateCell = shippingDateRow.FindElements(By.CssSelector("td"))[1].Text;

            var shippingDate = DateTime.ParseExact(shippingDateCell, new[] { "M/d/yyyy", "M/dd/yyyy", "MM/dd/yyyy" }, null, System.Globalization.DateTimeStyles.RoundtripKind);

            var countryRow = rows.Where(o => o.Text.Contains("Country")).FirstOrDefault();
            var country = countryRow.FindElements(By.CssSelector("td"))[1].Text;
            var dellAsset = new DellAsset
            {
                ServiceTag = serviceTag,
                MachineDescription = computerModel,
                ShipDate = shippingDate.ToString("yyyy-MM-dd"),
                CountryLookupCode = country,
                Components = new List<IComClassComponent>()
            };
            var componentsLink = this.Driver.FindElementById("hrefsubSectionB");
            componentsLink.Click();
            Thread.Sleep(1000);
            var componentSection = this.Driver.FindElementById("subSectionB");
            var containers = componentSection.FindElements(By.CssSelector(".top-offset-20"));

            foreach (var container in containers)
            {
                var heading = componentSection.FindElement(By.CssSelector("[onclick='enableExpColBtn();']"));
                heading.Click();
                Thread.Sleep(500);
                var component = new Component { Description = heading.Text, Parts = new List<IComClassPart>() };
                var tdCells = container.FindElements(By.CssSelector("td"));
                foreach (var tdCell in tdCells) {
                    if (!tdCell.Text.Contains("Part Number"))
                    {
                        var divCells = tdCell.FindElements(By.CssSelector("div"));
                        component.Parts.Add(
                            new Part {
                                PartNumber = divCells[0].Text,
                                Quantity = short.Parse(divCells[1].Text),
                                Description = divCells[2].Text
                            });
                    }
                }

                dellAsset.Components.Add(component);
            }


            return dellAsset;
        }
    }
}
