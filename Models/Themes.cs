using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
