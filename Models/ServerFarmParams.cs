using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using LCU.CDI.Provisioning;

namespace LCU.CDI.Provisioning.Models
{
    public class ServerFarmParams
    {
        #region Properties

        //Required
        [JsonProperty("serverFarm_location")]
        public virtual ParamType Location { get; set; }

        //Required
        [JsonProperty("serverFarm_name")]
        public virtual ParamType Name { get; set; }

        #endregion

        public ServerFarmParams()
        {    
            
        }
    }

}