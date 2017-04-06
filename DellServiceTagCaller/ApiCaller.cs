using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DellServiceTagData
{
    [Guid("EAA4976A-45C3-4BC5-BC0B-E474F4C3C83F")]
    public interface IComClassApiCaller
    {
        IComClassDellAsset GetDellAsset(string serviceTag, string apiKey);

    }

    [Guid("7BD20046-DF8C-44A6-8F6B-687FAA26FA71"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IComClassApiCallerEvents
    {
    }

    [Guid("0D53A3E8-E51A-49C7-944E-E72A2064F938"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(IComClassApiCallerEvents))]
    public class ApiCaller : IComClassApiCaller
    {
       
        public IComClassDellAsset GetDellAsset(string serviceTag, string apiKey)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    var url = $"https://api.dell.com/support/v2/assetinfo/warranty/tags?svctags={serviceTag}&apikey={apiKey}";
                    var response = client.GetStringAsync(url).Result;
                    JavaScriptSerializer json = new JavaScriptSerializer();
                    var res = json.Deserialize<DellAsset>(response);

                    return res;
                }
            }
            catch (AggregateException e)
            {
                return null;
            }
        }
    }
}
