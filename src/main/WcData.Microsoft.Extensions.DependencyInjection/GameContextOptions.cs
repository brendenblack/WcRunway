using System;
using System.Collections.Generic;
using System.Text;
using WcData.GameContext;

namespace WcData.Microsoft.Extensions.DependencyInjection
{
    public class GameContextOptions
    {
        public string Url { get; set; } 
        public string ConnectionString
        {
            get
            {
                return $"server={Url};database={Name};uid={Username};pwd={Password};ssl-mode=none";
            }
        }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
