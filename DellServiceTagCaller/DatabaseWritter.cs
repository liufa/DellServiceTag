using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;

namespace DellServiceTagData
{
    [Guid("EAA4976A-45C3-4BC5-BC0B-E474F4C3C83D")]
    [ComVisible(true)]
    public interface IComClassDatabaseWritter
    {
        void CreateDellAsset(IComClassDellAsset asset);

    }

    [Guid("7BD20046-DF8C-44A6-8F6B-687FAA26FA69"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [ComVisible(true)]
    public interface IComClassDatabaseWritterEvents
    {
    }

    [Guid("0D53A3E8-E51A-49C7-944E-E72A2064F936"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(IComClassDatabaseWritterEvents))]
    [ComVisible(true)]
    [ProgId("ProgId.DatabaseWritter")]
    public class DatabaseWritter : IComClassDatabaseWritter
    {
        public void CreateDellAsset(IComClassDellAsset asset)
        {
            string connString = "Server=12.41.72.28;Port=3306;Database=gpdb;Uid=allgreen2;Pwd=6B^94J@BZ4$p@bwZ;Allow User Variables=True";
            using (var conn = new MySqlConnection(connString))
            {
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText =
                    @"INSERT INTO tbldellservicetags(country_lookup_code,customer_number,is_duplicate,item_class_code,local_channel,machine_description,order_number,parent_service_tag,service_tag,ship_date) 
                    VALUES(@country_lookup_code, @customer_number, @is_duplicate, @item_class_code, @local_channel, @machine_description, @order_number, @parent_service_tag, @service_tag, @ship_date)";
                comm.Parameters.AddWithValue("@country_lookup_code", asset.CountryLookupCode);
                comm.Parameters.AddWithValue("@customer_number", asset.CustomerNumber);
                comm.Parameters.AddWithValue("@is_duplicate", asset.IsDuplicate);
                comm.Parameters.AddWithValue("@item_class_code", asset.ItemClassCode);
                comm.Parameters.AddWithValue("@local_channel", asset.LocalChannel);
                comm.Parameters.AddWithValue("@machine_description", asset.MachineDescription);
                comm.Parameters.AddWithValue("@order_number", asset.OrderNumber);
                comm.Parameters.AddWithValue("@parent_service_tag", asset.ParentServiceTag);
                comm.Parameters.AddWithValue("@service_tag", asset.ServiceTag);
                comm.Parameters.AddWithValue("@ship_date", asset.ShipDate);
                comm.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
