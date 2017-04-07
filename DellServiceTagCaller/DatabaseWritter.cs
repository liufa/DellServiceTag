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
        private string connString = "Server=12.41.72.28;Port=3306;Database=gpdb;Uid=allgreen2;Pwd=7Ld4S8d4TaDWApVW;Allow User Variables=True";
        public void CreateDellAsset(IComClassDellAsset asset)
        {

            using (var connection = new MySqlConnection(connString))
            {
                connection.Open();
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText =
                    @"INSERT INTO tbldellservicetags(
country_lookup_code,machine_description,service_tag,ship_date,express_Service_Code,regulatory_Model, Regulatory_Type) 
                    VALUES(@country_lookup_code,  @machine_description, @service_tag, @ship_date,@expressServiceCode,@regulatoryModel,@RegulatoryType)";

                comm.Parameters.AddWithValue("@country_lookup_code", asset.CountryLookupCode);
                comm.Parameters.AddWithValue("@expressServiceCode", asset.ExpressServiceCode);
                comm.Parameters.AddWithValue("@regulatoryModel", asset.RegulatoryModel);
                comm.Parameters.AddWithValue("@RegulatoryType", asset.RegulatoryType);
                //    comm.Parameters.AddWithValue("@RegulatoryType", asset.LocalChannel);
                comm.Parameters.AddWithValue("@machine_description", asset.MachineDescription);
                //     comm.Parameters.AddWithValue("@order_number", asset.OrderNumber);
                //     comm.Parameters.AddWithValue("@parent_service_tag", asset.ParentServiceTag);
                comm.Parameters.AddWithValue("@service_tag", asset.ServiceTag);
                comm.Parameters.AddWithValue("@ship_date", asset.ShipDate);
                comm.ExecuteNonQuery();
                var id = comm.LastInsertedId;

                foreach (var component in asset.Components)
                {
                    var componentCom = connection.CreateCommand();
                    componentCom.CommandText = @"INSERT INTO tbldellservicetagcomponents(
                        parent_id,description) 
                    VALUES(@parent_id,  @description)";
                    componentCom.Parameters.AddWithValue("@parent_id", id);
                    componentCom.Parameters.AddWithValue("@description", component.Description);
                    componentCom.ExecuteNonQuery();
                    var componentid = componentCom.LastInsertedId;
                    foreach (var part in component.Parts)
                    {
                        var partCom = connection.CreateCommand();
                        partCom.CommandText = @"INSERT INTO tbldellservicetagcomponentparts(
                            parent_id,partnumber,quantity,description) 
                        VALUES(@parent_id,  @partnumber, @quantity, @description)";
                        partCom.Parameters.AddWithValue("@parent_id", componentid);
                        partCom.Parameters.AddWithValue("@partnumber", part.PartNumber);
                        partCom.Parameters.AddWithValue("@quantity", part.Quantity);
                        partCom.Parameters.AddWithValue("@description", part.Description);
                        partCom.ExecuteNonQuery();
                    }
                }

                foreach (var driver in asset.Drivers)
                {
                    var componentCom = connection.CreateCommand();
                    componentCom.CommandText = @"INSERT INTO tbldellservicetagdrivers(
                        parent_id,description,component,name,file_name,version,importance,release_date,last_updated,download_link) 
                    VALUES(@parent_id,  @description,@component,@name,@file_name,@version,@importance,@release_date,@last_updated,@download_link)";
                    componentCom.Parameters.AddWithValue("@parent_id", id);
                    componentCom.Parameters.AddWithValue("@description", driver.Description);
                    componentCom.Parameters.AddWithValue("@component", driver.Component);
                    componentCom.Parameters.AddWithValue("@name", driver.Name);
                    componentCom.Parameters.AddWithValue("@file_name", driver.Filename);
                    componentCom.Parameters.AddWithValue("@version", driver.Version);
                    componentCom.Parameters.AddWithValue("@importance", driver.Importance);
                    componentCom.Parameters.AddWithValue("@release_date", driver.ReleaseDate);
                    componentCom.Parameters.AddWithValue("@last_updated", driver.LastUpdated);
                    componentCom.Parameters.AddWithValue("@download_link", driver.Url);
                    componentCom.ExecuteNonQuery();

                }

                connection.Close();
            }
        }

        public bool CheckIfExists(string serviceTag)
        {
            using (var connection = new MySqlConnection(connString))
            {
                connection.Open();
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText =
                    @"Select 1 from tbldellservicetags Where service_tag = @service_tag";
                comm.Parameters.AddWithValue("@service_tag", serviceTag);
                return comm.ExecuteReader().HasRows;
            }
        }
    }
}
