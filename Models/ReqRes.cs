using Fathym;
using Fathym.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LCU.CDI.Provisioning.Models
{
    public class GetResourceTemplateResponse
    {

        [JsonProperty("params")]
        public virtual dynamic Parameters { get; set; }        

        [JsonProperty("templateURL")]
        public virtual string TemplateURL { get; set; }

        public GetResourceTemplateResponse(string templateURL, dynamic parameters)
        {
            TemplateURL = templateURL;

            Parameters = parameters;
        }
    }
}
