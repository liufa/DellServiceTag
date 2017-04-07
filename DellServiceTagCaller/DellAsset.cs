using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DellServiceTagData
{
    [Guid("EAA4976A-45C3-4BC5-BC0B-E474F4C3C83E")]
    [ComVisible(true)]
    public interface IComClassDellAsset
    {
        string CountryLookupCode { get; set; }
        string CustomerNumber { get; set; }
        bool IsDuplicate { get; set; }
        string ItemClassCode { get; set; }
        string LocalChannel { get; set; }
        string MachineDescription { get; set; }
        string OrderNumber { get; set; }
        string ParentServiceTag { get; set; }
        string ServiceTag { get; set; }
        string ShipDate { get; set; }
        string ExpressServiceCode { get; set; }
        string RegulatoryModel { get; set; }
        string RegulatoryType { get; set; }
        List<IComClassComponent> Components { get; set; }

        List<IComClassDriver> Drivers { get; set; }
    }

    [Guid("7BD20046-DF8C-44A6-8F6B-687FAA26FA70"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [ComVisible(true)]
    public interface IComClassDellAssetEvents
    {
    }

    [Guid("0D53A3E8-E51A-49C7-944E-E72A2064F937"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(IComClassDellAssetEvents))]
    [ComVisible(true)]
    [ProgId("ProgId.DellAsset")]
    public class DellAsset : IComClassDellAsset
    {
        public string CountryLookupCode { get; set; }
        public string CustomerNumber { get; set; }
        public bool IsDuplicate { get; set; }
        public string ItemClassCode { get; set; }
        public string LocalChannel { get; set; }
        public string MachineDescription { get; set; }
        public string OrderNumber { get; set; }
        public string ParentServiceTag { get; set; }
        public string ServiceTag { get; set; }
        public string ShipDate { get; set; }
        public string ExpressServiceCode { get; set; }
        public string RegulatoryModel { get; set; }
        public string RegulatoryType { get; set; }
        public List<IComClassComponent> Components { get; set; }
        public List<IComClassDriver> Drivers { get; set; }
    }

    [Guid("EAA4976A-45C3-4BC5-BC0B-E474F4C3C73E")]
    [ComVisible(true)]
    public interface IComClassComponent
    {
        string Description { get; set; }
        List<IComClassPart> Parts { get; set; }
    }

    [Guid("7BD20046-DF8C-44A6-8F6B-687FAA26F070"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [ComVisible(true)]
    public interface IIComClassComponentEvents
    {
    }

    [Guid("0D53A3E8-E51A-49C7-944E-E72A2064F037"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(IIComClassComponentEvents))]
    [ComVisible(true)]
    [ProgId("ProgId.Component")]

    public class Component : IComClassComponent
    {
        public string Description { get; set; }

        public List<IComClassPart> Parts { get; set; }
    }

    [Guid("EAA4976A-45C3-4BC5-BC0B-E474F4C3B73E")]
    [ComVisible(true)]
    public interface IComClassPart
    {
        string PartNumber { get; set; }
        short Quantity { get; set; }
        string Description { get; set; }

    }

    [Guid("7BD20046-DF8C-44A6-8F6B-687FAA26E070"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [ComVisible(true)]
    public interface IComClassPartEvents
    {
    }

    [Guid("0D53A3E8-E51A-49C7-944E-E72A2064E037"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(IComClassPartEvents))]
    [ComVisible(true)]
    [ProgId("ProgId.Part")]
    public class Part : IComClassPart
    {
        public string PartNumber { get; set; }
        public short Quantity { get; set; }
        public string Description { get; set; }
    }


    [Guid("EAA4976A-45C3-4BC5-BC0B-E474F4C2B73E")]
    [ComVisible(true)]
    public interface IComClassDriver
    {
        string Component { get; set; }
        string Name { get; set; }
        string Filename { get; set; }
        string Description { get; set; }
        string Version { get; set; }
        string Importance { get; set; }
        DateTime ReleaseDate { get; set; }
        DateTime LastUpdated { get; set; }
        string Url { get; set; }
    }

    [Guid("7BD20046-DF8C-44A6-8F6B-687FAA25E070"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [ComVisible(true)]
    public interface IComClassDriverEvents
    {
    }

    [Guid("0D53A3E8-E51A-49C7-944E-E72A2063E037"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(IComClassDriverEvents))]
    [ComVisible(true)]
    [ProgId("ProgId.Part")]
    public class Driver : IComClassDriver
    {
        public string Component { get; set; }
        public string Name { get; set; }
        public string Filename { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Importance { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Url { get; set; }
    }
}
