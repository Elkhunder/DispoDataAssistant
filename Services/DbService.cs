using Dapper;
using DispoDataAssistant.Models;
using System.Data.SQLite;
using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace DispoDataAssistant.Services
{
    public class DbService
    {
        public static ServiceNowAsset? GetServiceNowAssetById(string Id)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                ServiceNowAsset? serviceNowAsset = conn.QuerySingleOrDefault<ServiceNowAsset?>($"SELECT * FROM Asset WHERE Id = @Id", new { Id });
                return serviceNowAsset;
            }
        }

        public static ServiceNowAsset? GetServiceNowAssetByProperty(string propertyName, string propertyValue)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                ServiceNowAsset? serviceNowAsset = conn.QuerySingleOrDefault<ServiceNowAsset?>($"SELECT * FROM Asset WHERE {propertyName} = {propertyValue}");
                return serviceNowAsset;
            }
        }

        public static void SaveAsset(ServiceNowAsset serviceNowAsset)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                conn.Execute("Insert into Asset (Id, AssetTag, Manufacturer, Model, Category, SerialNumber, OperationalStatus, InstallStatus, LastUpdated)" +
                    "values (@Id, @AssetTag, @Manufacturer, @Model, @Category, @SerialNumber, @OperationalStatus, @InstallStatus, @LastUpdated)", serviceNowAsset);
            }
        }

        public static ServiceNowAsset? SaveAsset(ServiceNowAsset serviceNowAsset, bool upsert)
        {
            if (upsert && serviceNowAsset is not null && serviceNowAsset.Id is not null)
            {
                ServiceNowAsset? existingAsset = GetServiceNowAssetById(serviceNowAsset.Id);
                if (existingAsset is not null)
                {
                    var changedProperties = CompareExistingAssetToNewAsset(existingAsset, serviceNowAsset);

                    if (changedProperties is not null)
                    {
                        var updateString = GenerateUpdateString(changedProperties);

                        using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
                        {
                            connection.Execute(updateString, serviceNowAsset);
                        }
                        return serviceNowAsset;
                    }
                    return existingAsset;
                }
                else
                {
                    using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
                    {
                        conn.Execute("Insert into Asset (Id, AssetTag, Manufacturer, Model, Category, SerialNumber, OperationalStatus, InstallStatus, LastUpdated)" +
                            "values (@Id, @AssetTag, @Manufacturer, @Model, @Category, @SerialNumber, @OperationalStatus, @InstallStatus, @LastUpdated)", serviceNowAsset);
                    }
                    return serviceNowAsset;
                }
            }
            return null;
        }

        public static ServiceNowAsset? SaveAsset(ServiceNowAsset serviceNowAsset, ServiceNowAsset existingAsset,  bool upsert)
        {
            if (upsert && serviceNowAsset is not null && existingAsset is not null)
            {
                var changedProperties = CompareExistingAssetToNewAsset(existingAsset, serviceNowAsset);

                if (changedProperties is not null)
                {
                    var updateString = GenerateUpdateString(changedProperties);

                    using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
                    {
                        connection.Execute(updateString, serviceNowAsset);
                    }
                    return serviceNowAsset;
                }
                return existingAsset;
            }
            else
            {
                using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
                {
                    connection.Execute("Insert into Asset (Id, AssetTag, Manufacturer, Model, Category, SerialNumber, OperationalStatus, InstallStatus, LastUpdated)" +
                            "values (@Id, @AssetTag, @Manufacturer, @Model, @Category, @SerialNumber, @OperationalStatus, @InstallStatus, @LastUpdated)", serviceNowAsset);
                }
                return serviceNowAsset;
            }
        }

        private static string GenerateUpdateString(Dictionary<string, string> changes)
        {
            var queryParams = changes.Select(kvp => $"{kvp.Key} = @{kvp.Key}");
            string queryString = $"UPDATE Asset SET {string.Join(", ", queryParams)}";
            return queryString;
        }

        private static Dictionary<string, string>? CompareExistingAssetToNewAsset(ServiceNowAsset? existingAsset, ServiceNowAsset newAsset)
        {
            Dictionary<string, string> changes = new Dictionary<string, string>();

            var properties = typeof(ServiceNowAsset).GetProperties();

            foreach (var prop in properties)
            {
                string currentValue = (string)prop.GetValue(existingAsset)!;
                string newValue = (string)prop.GetValue(newAsset)!;

                if (currentValue != newValue)
                {
                    changes.Add(prop.Name, newValue);
                }
            }
            if (changes.Count > 0)
            {
                return changes;
            }
            return null;
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
