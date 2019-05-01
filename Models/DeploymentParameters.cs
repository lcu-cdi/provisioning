using System;
using Newtonsoft.Json;

namespace LCU.CDI.Provisioning.Models
{
    public class DeploymentParameters
    {
        #region Properties

        [JsonProperty("contentVersion")]
        public virtual string ContentVersion { get; set; }


        [JsonProperty("parameters")]
        public virtual dynamic Parameters { get; set; }      


        [JsonProperty("$schema")]
        public virtual string Schema { get; set; }

        #endregion

        public DeploymentParameters(dynamic parameters)
        {    
            ContentVersion = "1.0.0.0";

            Schema = "https://schema.management.azure.com/schemas/2015-01-01/deploymentParameters.json#";            

            Parameters = parameters;
        }
    }

    public class ParamType
    {
        public ParamType(string value)
        {
            Value = value;
        }

        [JsonProperty("value")]
        public virtual string Value { get; set; }
    }    

}