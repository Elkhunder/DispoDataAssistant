using System;

namespace DispoDataAssistant.Models
{
    public class Themes
    {
        public string[] Names { get; set; } 
        public Themes()
        {
            Console.WriteLine("Themes: Instance Created");
            Names = new string[]
            {
                "Light",
                "Dark",
                "Neutral"
            };
        }
    }
}
