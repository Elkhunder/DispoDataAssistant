using Dapper;
using DispoDataAssistant.Models;
using System.Data.SQLite;
using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;

namespace DispoDataAssistant.Services
{
    public class DbService
    {
        public static ServiceNowAsset? GetServiceNowAssetById(string Id)
        {
            using (SQLiteConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                if (dbConnection.State != ConnectionState.Open)
                {
                    dbConnection.Open();
                }

                ServiceNowAsset? serviceNowAsset = dbConnection.QuerySingleOrDefault<ServiceNowAsset?>($"SELECT * FROM Asset WHERE Id = @Id", new { Id });

                if (dbConnection.State != ConnectionState.Closed)
                {
                    dbConnection.Close();
                }

                return serviceNowAsset;
            }
        }

        public async static Task<IEnumerable<ServiceNowAsset>> LoadServiceNowAssetsByTable(string tableName)
        {
            using (SQLiteConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                if (dbConnection.State != ConnectionState.Open)
                {
                    dbConnection.Open();
                }

                return await dbConnection.QueryAsync<ServiceNowAsset>($"SELECT * FROM {tableName}");
            }
        }

        public static ServiceNowAsset? GetServiceNowAssetByProperty(string propertyName, string propertyValue)
        {
            using (SQLiteConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                if (dbConnection.State != ConnectionState.Open)
                {
                    dbConnection.Open();
                }

                ServiceNowAsset? serviceNowAsset = dbConnection.QuerySingleOrDefault<ServiceNowAsset?>($"SELECT * FROM Asset WHERE {propertyName} = {propertyValue}");

                if (dbConnection.State != ConnectionState.Closed)
                {
                    dbConnection.Close();
                }

                return serviceNowAsset;
            }
        }

        public static void SaveAsset(ServiceNowAsset serviceNowAsset)
        {
            using (SQLiteConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                if (dbConnection.State != ConnectionState.Open)
                {
                    dbConnection.Open();
                }

                dbConnection.Execute("Insert into Asset (Id, AssetTag, Manufacturer, Model, Category, SerialNumber, OperationalStatus, InstallStatus, LastUpdated)" +
                    "values (@Id, @AssetTag, @Manufacturer, @Model, @Category, @SerialNumber, @OperationalStatus, @InstallStatus, @LastUpdated)", serviceNowAsset);

                if (dbConnection.State != ConnectionState.Closed)
                {
                    dbConnection.Close();
                }
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

                        using (SQLiteConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
                        {
                            if (dbConnection.State != ConnectionState.Open)
                            {
                                dbConnection.Open();
                            }

                            dbConnection.Execute(updateString, serviceNowAsset);

                            if (dbConnection.State != ConnectionState.Closed)
                            {
                                dbConnection.Close();
                            }
                        }
                        return serviceNowAsset;
                    }
                    return existingAsset;
                }
                else
                {
                    using (SQLiteConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
                    {
                        if (dbConnection.State != ConnectionState.Open)
                        {
                            dbConnection.Open();
                        }

                        dbConnection.Execute("Insert into Asset (Id, AssetTag, Manufacturer, Model, Category, SerialNumber, OperationalStatus, InstallStatus, LastUpdated)" +
                            "values (@Id, @AssetTag, @Manufacturer, @Model, @Category, @SerialNumber, @OperationalStatus, @InstallStatus, @LastUpdated)", serviceNowAsset);

                        if (dbConnection.State != ConnectionState.Closed)
                        {
                            dbConnection.Close();
                        }
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

                    using (SQLiteConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
                    {
                        if (dbConnection.State != ConnectionState.Open)
                        {
                            dbConnection.Open();
                        }

                        dbConnection.Execute(updateString, serviceNowAsset);

                        if (dbConnection.State != ConnectionState.Closed)
                        {
                            dbConnection.Close();
                        }
                    }
                    return serviceNowAsset;
                }
                return existingAsset;
            }
            else
            {
                using (SQLiteConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
                {
                    if (dbConnection.State != ConnectionState.Open)
                    {
                        dbConnection.Open();
                    }

                    dbConnection.Execute("Insert into Asset (Id, AssetTag, Manufacturer, Model, Category, SerialNumber, OperationalStatus, InstallStatus, LastUpdated)" +
                            "values (@Id, @AssetTag, @Manufacturer, @Model, @Category, @SerialNumber, @OperationalStatus, @InstallStatus, @LastUpdated)", serviceNowAsset);

                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
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

        public static async void DropTable(string tableName)
        {
            using (SQLiteConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                if (dbConnection.State is not ConnectionState.Open)
                {
                    dbConnection.Open();
                }

                string sqlCommand = $"DROP TABLE {tableName}";
                SQLiteCommand command = new SQLiteCommand(sqlCommand, dbConnection);
                var result = await command.ExecuteNonQueryAsync();
                Console.WriteLine(result);
            }
        }

        public static void CreateNewTable(string? tableName)
        {
            using (SQLiteConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                if (dbConnection.State is not ConnectionState.Open)
                {
                    dbConnection.Open();
                }

                if(tableName is null)
                {
                    var date = DateTime.Now;
                    var year = date.Year;
                    var month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month);

                    tableName = $"{year}_{month} Assets";
                }

                //Check if table name alread exists before creating it
                var tableNames = GetTableNames(dbConnection);
                if (tableNames != null && tableNames.Contains(tableName)) { return; }

                string sqlCommand = $"Create Table {tableName} (Id TEXT PRIMARY KEY NOT NULL UNIQUE, AssetTag TEXT NOT NULL, Manufacturer TEXT NOT NULL, Model TEXT NOT NULL, Category TEXT NOT NULL, SerialNumber TEXT NOT NULL, OperationalStatus TEXT NOT NULL, InstallStatus TEXT NOT NULL, LastUpdated TEXT NOT NULL)";
                SQLiteCommand command = new SQLiteCommand(sqlCommand, dbConnection);
                command.ExecuteNonQuery();

                if (dbConnection.State != ConnectionState.Closed)
                {
                    dbConnection.Close();
                }
            }
        }

        public static List<string>? GetTableNames()
        {
            var tableNames = new List<string>();
            using (SQLiteConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                if (dbConnection.State is not ConnectionState.Open)
                {
                    dbConnection.Open();
                }

                using (SQLiteCommand command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type ='table' AND name NOT LIKE 'sqlite_%'", dbConnection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string? name = reader["name"].ToString();
                            if (!string.IsNullOrEmpty(name))
                            {
                                tableNames.Add(name);
                            }
                        }
                    }
                }

                if (dbConnection.State is not ConnectionState.Closed)
                {
                    dbConnection.Close();
                }
            }
            return tableNames;
        }
        private static List<string>? GetTableNames(IDbConnection dbConnection)
        {
            var tableNames = new List<string>();

            var readaer = dbConnection.ExecuteReader("SELECT name FROM sqlite_master WHERE type ='table' AND name NOT LIKE 'sqlite_%'");
            while (readaer.Read())
            {
                string? name = readaer["name"].ToString();

                if (!string.IsNullOrEmpty(name))
                {
                    tableNames.Add($"{name}");
                }
            }
            return tableNames;
        }
    }
}
