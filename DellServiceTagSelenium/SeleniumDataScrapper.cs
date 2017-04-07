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

            this.Driver.FindElementById("tab-manuals").Click();
            Thread.Sleep(3000);
            var regulatory = this.Driver.FindElements(By.CssSelector(".top-offset-mini.bottom-offset-mini"));
            var regulatoryModel = regulatory.Where(o => o.Text.Contains("Regulatory Model")).FirstOrDefault()?.Text?.Replace("Regulatory Model ","");
            var regulatoryType = regulatory.Where(o => o.Text.Contains("Regulatory Type")).FirstOrDefault()?.Text?.Replace("Regulatory Type ", "");

            this.Driver.FindElementById("tab-configuration").Click();
            Thread.Sleep(5000);
            var configurationDivContainer = this.Driver.FindElementsById("subSectionA").First();

            var rows = configurationDivContainer.FindElements(By.CssSelector("tr"));
            var computerModelRow = rows.Where(o => o.Text.Contains("Computer Model")).FirstOrDefault();
            var computerModel = computerModelRow.FindElements(By.CssSelector("td"))[1].Text;
            var shippingDateRow = rows.Where(o => o.Text.Contains("Shipping Date")).FirstOrDefault();
            var shippingDateCell = shippingDateRow.FindElements(By.CssSelector("td"))[1].Text;

            var shippingDate = DateTime.ParseExact(shippingDateCell, new[] { "M/d/yyyy", "M/dd/yyyy", "MM/dd/yyyy" }, null, System.Globalization.DateTimeStyles.RoundtripKind);
            var expressServiceCodeSpan = this.Driver.FindElementByClassName("beforeCaptcha");
            var expressServiceCode = expressServiceCodeSpan.Text.Replace("Express Service Code: ", "");
            var countryRow = rows.Where(o => o.Text.Contains("Country")).FirstOrDefault();
            var country = countryRow.FindElements(By.CssSelector("td"))[1].Text;
            var dellAsset = new DellAsset
            {
                ServiceTag = serviceTag,
                MachineDescription = computerModel,
                ShipDate = shippingDate.ToString("yyyy-MM-dd"),
                CountryLookupCode = country,
                Components = new List<IComClassComponent>(),
                Drivers = new List<IComClassDriver>(),
                ExpressServiceCode = expressServiceCode,
                RegulatoryModel = regulatoryModel,
                RegulatoryType = regulatoryType
            };
            var componentsLink = this.Driver.FindElementById("hrefsubSectionB");
            componentsLink.Click();
            Thread.Sleep(1000);
            var componentSection = this.Driver.FindElementById("subSectionB");
            var containers = componentSection.FindElements(By.CssSelector(".top-offset-20"));

            foreach (var container in containers)
            {
                if (!string.IsNullOrWhiteSpace(container.Text))
                {
                    var heading = container.FindElement(By.CssSelector("[onclick='enableExpColBtn();']"));
                    heading.Click();
                    Thread.Sleep(500);
                    var component = new Component { Description = heading.Text, Parts = new List<IComClassPart>() };
                    var tdCells = container.FindElements(By.CssSelector("td"));
                    foreach (var tdCell in tdCells)
                    {
                        if (!tdCell.Text.Contains("Part Number") && !string.IsNullOrWhiteSpace(tdCell.Text))
                        {
                            var divCells = tdCell.FindElements(By.CssSelector("div"));
                            component.Parts.Add(
                                new Part
                                {
                                    PartNumber = divCells[0].Text,
                                    Quantity = short.Parse(divCells[1].Text),
                                    Description = divCells[2].Text
                                });
                        }
                    }

                    dellAsset.Components.Add(component);
                }
            }

            this.Driver.FindElementById("tab-drivers").Click();
            Thread.Sleep(5000);
            this.Driver.FindElement(By.Id("DndAdvTabLink")).Click();
            Thread.Sleep(500);
            var driversSection = this.Driver.FindElementById("divDriversSection");

            var driverContainrs = driversSection.FindElements(By.CssSelector("[ng-repeat='driverData in driversBaseData track by $index']"));


            foreach (var driverContainer in driverContainrs)
            {
                var toggle = driverContainer.FindElement(By.CssSelector(@"[ng-attr-id=""{{'header' + driverData.key }}""]"));
                if (toggle != null && toggle.Displayed)
                {
                    toggle.Click();
                    Thread.Sleep(500);
                    var component = toggle.Text.Split(new[] { " (" }, StringSplitOptions.None)[0];
                    var driverMains = driverContainer.FindElements(By.CssSelector(@"[ng-repeat=""drivers in driverData.value | orderBy:'-ReleaseDateValue' track by drivers.DriverId ""]"));

                    foreach (var drivermain in driverMains)
                    {
                        var name = drivermain.FindElement(By.TagName("h4")).Text;
                        var driverRows = drivermain.FindElements(By.CssSelector("[class^='col-lg-']"));
                        var filename = driverRows.Where(o => o.Text.Contains("File Name")).FirstOrDefault()?.Text?.Replace("File Name: ", "");
                        var description = driverRows.Where(o => o.Text.Contains("Description")).FirstOrDefault()?.Text?.Replace("Description: ", "");
                        var version = driverRows.Where(o => o.Text.Contains("Version: ")).FirstOrDefault()?.Text?.Replace("Version: ", "");
                        var importance = driverRows.Where(o => o.Text.Contains("Importance: ")).FirstOrDefault()?.Text?.Replace("Importance: ", "");
                        var releaseDate = driverRows.Where(o => o.Text.Contains("Release Date: ")).FirstOrDefault()?.Text?.Replace("Release Date: ", "");
                        var lastUpdated = driverRows.Where(o => o.Text.Contains("Last Updated: ")).FirstOrDefault()?.Text?.Replace("Last Updated: ", "");
                        var driverUrl = drivermain.FindElement(By.CssSelector("a.text-blue.dellmetrics-driverdownloads.driverHomeDownload.ng-binding"))?.GetAttribute("href");

                        dellAsset.Drivers.Add(
                            new Driver
                            {
                                Component = component,
                                Description = description,
                                Filename = filename,
                                Importance = importance,
                                LastUpdated = DateTime.ParseExact(lastUpdated, new[] { "dd MMM yyyy" }, null, System.Globalization.DateTimeStyles.RoundtripKind),
                                ReleaseDate = DateTime.ParseExact(releaseDate, new[] { "dd MMM yyyy" }, null, System.Globalization.DateTimeStyles.RoundtripKind),
                                Name = name,
                                Url = driverUrl,
                                Version = version
                            });
                    }
                }
            }

            return dellAsset;
        }
    }
}
