using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using LCU.CDI.Provisioning;

namespace LCU.CDI.Provisioning.Models
{
    public class StorageAccountParams 
    {
        #region Properties

        //DefaultValue: AccessTiers.Hot
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("storageAccount_accessTier")]
        public virtual ParamType AccessTier { get; set; }

        //Required
        [JsonProperty("storageAccount_location")]
        public virtual ParamType Location { get; set; }

        //Required
        [JsonProperty("storageAccount_name")]
        public virtual ParamType Name { get; set; }

        #endregion

        public StorageAccountParams()
        {    
            AccessTier = new ParamType(AccessTiers.Hot.ToString());
        }
    }

    public enum AccessTiers 
    {        
        Cold,
        Hot
    }

}