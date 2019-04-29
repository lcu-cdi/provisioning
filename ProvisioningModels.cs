using Fathym;
using Fathym.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCU.CDI.Provisioning
{
    public enum FathymResourceTypes
    {
        ClassicStorage,
        DataLakeAnalytics,
        DataLakeStore,
        Device,
        IotHub,
        CosmosDB,        
        EventHub,
        EventHubNamespace,
        ApplicationInsights,
        StorageAccount,
        StorageContainer,
        ServiceBusNamespace,
        StreamAnalytics,
        AppServicePlan,
        FunctionApp,
        Topic
    }    
    public enum LocationZones
    {
        USEast,
        USCentral,
        USWest
    }    
    public enum ProvisioningEnvironment
    {
        @int,
        stg,
        pro
    }    
    
    public class ResourceGroupConfigurationResponse : BaseResponse
    {
        public virtual string Location { get; set; }

        public virtual string ResourceGroupName { get; set; }

        public virtual IDictionary<string, string> Tags { get; set; }
    }

    public class ResourceConfigurationResponse : BaseResponse
    {
        public virtual string Location { get; set; }

        public virtual string ResourceGroupName { get; set; }

        public virtual string ResourceName { get; set; }

        public virtual IDictionary<string, string> Tags { get; set; }
    }

    public class ResourceGroupResponse : BaseResponse
    {
        public virtual ResourceGroupModel ResourceGroup { get; set; }
    }

    public class ResourceGroupModel
    {
        public virtual string ID { get; set; }
        
        public virtual string Name { get; set; }        
        
        public virtual string Location { get; set; }
        
        public virtual string ManagedBy { get; set; }
        
        public virtual IDictionary<string, string> Tags { get; set; }        
    }    
}
