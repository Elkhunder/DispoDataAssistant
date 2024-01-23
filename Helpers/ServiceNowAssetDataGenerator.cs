﻿using DispoDataAssistant.Data.Models;
using System;
using System.Collections.Generic;

namespace DispoDataAssistant.Helpers
{
    public static class ServiceNowAssetDataGenerator
    {
        private static readonly Random random = new();
        private static readonly string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static ICollection<ServiceNowAsset> GenerateData(int recordCount, TabModel tab)
        {
            ICollection<ServiceNowAsset> assets = [];


            for (int i = 0; i < recordCount; i++)
            {
                assets.Add(new ServiceNowAsset
                {
                    SysId = Guid.NewGuid().ToString(),
                    AssetTag = random.Next(100000, 999999).ToString(),
                    Manufacturer = random.Next(0, 2) == 0 ? "Dell" : "HP",
                    Model = $"Ultra Notebook {i + 1}",
                    Category = "Laptop",
                    SerialNumber = GenerateRandomSerialNumber(),
                    OperationalStatus = random.Next(0, 2) == 0 ? "Working" : "Non-working",
                    InstallStatus = random.Next(0, 2) == 0 ? "Installed" : "Not Installed",
                    LastUpdated = DateTime.UtcNow.ToString(),
                    Tab = tab
                });
            }

            return assets;
        }

        private static string GenerateRandomSerialNumber()
        {
            char[] chars = new char[7];

            // Place 3 letters
            for (int i = 0; i < 3; ++i)
            {
                int index;
                do { index = random.Next(0, 7); }
                while (chars[index] != '\0');

                chars[index] = alphabet[random.Next(alphabet.Length)];
            }

            for (int i = 0; i < 7; i++)
            {
                if (chars[i] == '\0') // If the spot is empty, place a number
                    chars[i] = Convert.ToChar(random.Next(0, 10) + '0');
            }

            return new string(chars);
        }




    }
}