using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;

namespace IgiCore.Server
{
    public partial class Server
    {
        //public new static void TriggerClientEvent(Player player, string eventName, params object[] args)
        //{
        //    var serialised = new List<string>();
        //    JsonSerializerSettings settings = new JsonSerializerSettings
        //    {
        //        TypeNameHandling = TypeNameHandling.Objects,
        //        Formatting = Formatting.None
        //    };

        //    foreach (object arg in args)
        //    {
        //        serialised.Add(JsonConvert.SerializeObject(args, settings));
        //    }
        //    BaseScript.TriggerClientEvent(player, eventName, serialised.ToArray());
        //}

        //public new static void TriggerClientEvent(string eventName, params object[] args)
        //{
        //    var serialised = new List<string>();
        //    JsonSerializerSettings settings = new JsonSerializerSettings
        //    {
        //        TypeNameHandling = TypeNameHandling.Objects,
        //        Formatting = Formatting.None
        //    };

        //    foreach (object arg in args)
        //    {
        //        serialised.Add(JsonConvert.SerializeObject(args, settings));
        //    }
        //    BaseScript.TriggerClientEvent(eventName, serialised.ToArray());
        //}
    }
}
