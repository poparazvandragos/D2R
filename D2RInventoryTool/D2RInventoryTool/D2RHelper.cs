using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2RInventoryTool
{
    class D2RHelper
    {
        public static JObject settings;

        public static List<JToken> PlayerSpecs {
            get
            {
                var playerSpecs = new List<JToken>() {
                D2RHelper.settings["d2rScreenSpecs"]["inventory"],
                D2RHelper.settings["d2rScreenSpecs"]["stash"],
                D2RHelper.settings["d2rScreenSpecs"]["equipment"]["head"],
                D2RHelper.settings["d2rScreenSpecs"]["equipment"]["armor"],
                D2RHelper.settings["d2rScreenSpecs"]["equipment"]["belt"],
                D2RHelper.settings["d2rScreenSpecs"]["equipment"]["amulet"],
                D2RHelper.settings["d2rScreenSpecs"]["equipment"]["ring1"],
                D2RHelper.settings["d2rScreenSpecs"]["equipment"]["ring2"],
                D2RHelper.settings["d2rScreenSpecs"]["equipment"]["gloves"],
                D2RHelper.settings["d2rScreenSpecs"]["equipment"]["boots"],
                D2RHelper.settings["d2rScreenSpecs"]["equipment"]["weapon"],
                D2RHelper.settings["d2rScreenSpecs"]["equipment"]["shield"],
                };
                return playerSpecs;
            }
        }

        public static List<JToken> MercSpecs
        {
            get
            {
                var playerSpecs = new List<JToken>() {
                D2RHelper.settings["d2rScreenSpecs"]["merc"]["head"],
                D2RHelper.settings["d2rScreenSpecs"]["merc"]["armor"],
                D2RHelper.settings["d2rScreenSpecs"]["merc"]["weapon"],
                D2RHelper.settings["d2rScreenSpecs"]["merc"]["shield"],
                };
                return playerSpecs;
            }
        }


    }
}
