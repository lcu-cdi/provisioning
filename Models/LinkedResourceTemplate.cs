using System;
using Newtonsoft.Json;

namespace LCU.CDI.Provisioning.Models
{
    public class LinkedResourceTemplate
    {
        #region Properties

        [JsonProperty("apiVersion")]
        public virtual string APIVersion { get; set; }


        [JsonProperty("name")]
        public virtual string Name { get; set; }


        [JsonProperty("properties")]
        public virtual dynamic Properties { get; set; }    

        [JsonProperty("type")]
        public virtual string Type { get; set; }          

        #endregion

        public LinkedResourceTemplate(string templateLinkURI, dynamic parameters)
        {    
            APIVersion = "2017-05-10";

            Name = "linkedTemplate";

            Type = "Microsoft.Resources/deployments";

            Properties = new Properties()
            {
                Mode = "incremental",
                TemplateLink = new TemplateLink()
                {
                    ContentVersion = "1.0.0.0",
                    URI = templateLinkURI
                },
                Parameters = parameters
            };
        }
    }

    public class Properties
    {
        [JsonProperty("mode")]
        public virtual string Mode { get; set; }  

        [JsonProperty("parameters")]
        public virtual dynamic Parameters { get; set; }    

        [JsonProperty("templateLink")]
        public virtual TemplateLink TemplateLink { get; set; }                                
    }
   
    public class TemplateLink
    {
        [JsonProperty("contentVersion")]
        public virtual string ContentVersion { get; set; } 

        [JsonProperty("uri")]
        public virtual string URI { get; set; }         
    }
}