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
        public virtual string storageAccount_accessTier { get; set; }

        //Required
        [JsonProperty("storageAccount_location")]
        public virtual string storageAccount_location { get; set; }

        //Required
        [JsonProperty("storageAccount_name")]
        public virtual string storageAccount_name { get; set; }

        #endregion

        public StorageAccountParams()
        {    
            storageAccount_accessTier = AccessTiers.Hot.ToString();
        }
    }

    public class ResourceParams
    {

    }

    public enum AccessTiers 
    {        
        Cold,
        Hot
    }

}