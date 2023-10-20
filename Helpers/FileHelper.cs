

using System;
using System.IO;

namespace DispoDataAssistant.Helpers
{
    public static class FileHelper
    {
        public static byte[] ReadBytes(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    // Read the contents of the file as bytes
                    return File.ReadAllBytes(filePath);
                }
                else
                {
                    Console.WriteLine("File not found: " + filePath);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
                return null;
            }
        }

        public static string ReadText(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    // Read the contents of the file and return as a string
                    return File.ReadAllText(filePath);
                }
                else
                {
                    Console.WriteLine("File not found: " + filePath);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
                return null;
            }
        }
    }
}
