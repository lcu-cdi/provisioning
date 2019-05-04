using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using LCU.CDI.Provisioning;

namespace LCU.CDI.Provisioning.Models
{
    public class StorageAccountParams : ResourceParams
    {
        #region Properties

        //DefaultValue: AccessTiers.Hot
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("storageAccount_accessTier")]
        public virtual string AccessTier { get; set; }

        //Required
        [JsonProperty("storageAccount_location")]
        public virtual string Location { get; set; }

        //Required
        [JsonProperty("storageAccount_name")]
        public virtual string Name { get; set; }

        #endregion

        public StorageAccountParams()
        {    
            AccessTier = AccessTiers.Hot.ToString();
        }
    }

    public abstract class ResourceParams
    {

    }

    public enum AccessTiers 
    {        
        Cold,
        Hot
    }

}