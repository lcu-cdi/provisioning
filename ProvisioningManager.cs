using Fathym;
using Fathym.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCU.CDI.Provisioning
{
    public interface IProvisioningManager 
    {
        ResourceGroupConfigurationResponse LoadResourceGroupConfiguration(string clientName, string clientLookup,
            ProvisioningEnvironment environment, LocationZones desiredLocation, string clientFlowName, string clientFlowLookup);

        ResourceConfigurationResponse LoadResourceConfiguration(FathymResourceTypes resourceType, string clientName, string clientLookup,
            ProvisioningEnvironment environment, LocationZones desiredLocation, string clientFlowName, string clientFlowLookup,
            string uniqueLookup);
    }

	public class ProvisioningManager : IProvisioningManager
    {
        #region Fields
        public const string ApplicationLookup = "flw";
        #endregion

        #region Constructors

        #endregion

        #region API Methods
        public virtual ResourceGroupConfigurationResponse LoadResourceGroupConfiguration(string clientName, string clientLookup,
            ProvisioningEnvironment environment, LocationZones desiredLocation, string clientFlowName, string clientFlowLookup)
        {
            var resGrp = $"{ApplicationLookup}-{clientLookup}-{clientFlowLookup}-{environment}";

            return new ResourceGroupConfigurationResponse()
            {
                ResourceGroupName = resGrp,
                Tags = getTags(clientName, environment, clientFlowName),
                Location = getLocation(desiredLocation),
                Status = Status.Success
            };
        }

        public virtual ResourceConfigurationResponse LoadResourceConfiguration(FathymResourceTypes resourceType, string clientName, string clientLookup,
            ProvisioningEnvironment environment, LocationZones desiredLocation, string clientFlowName,
            string clientFlowLookup, string uniqueLookup)
        {
            var resGrp = $"{ApplicationLookup}-{clientLookup}-{clientFlowLookup}-{environment}";

            var resName = resGrp.Replace("-","");

            if (!uniqueLookup.IsNullOrEmpty())
                resName += $"{uniqueLookup}";

            resName = getResourceName(resourceType, resName);

            return new ResourceConfigurationResponse()
            {
                ResourceGroupName = resGrp,
                ResourceName = resName,
                Tags = getTags(clientName, environment, clientFlowName),
                Location = getLocation(resourceType, desiredLocation),
                Status = Status.Success
            };
        }
        #endregion

        #region Helpers
        protected virtual string getEnvironmentName(ProvisioningEnvironment environment)
        {
            switch (environment)
            {
                default:
                case ProvisioningEnvironment.@int:
                    return "Integration";
                case ProvisioningEnvironment.stg:
                    return "Staging";
                case ProvisioningEnvironment.pro:
                    return "Production";
            }
        }

        protected virtual string getLocation(FathymResourceTypes resourceType, LocationZones desiredLocation)
        {
            switch (resourceType)
            {
                case FathymResourceTypes.ApplicationInsights:
                    return getLocation(LocationZones.USEast);

                case FathymResourceTypes.DataLakeAnalytics :
                case FathymResourceTypes.DataLakeStore :
                    return getLocation(LocationZones.USCentral);

                default:
                    return getLocation(desiredLocation);
            }            
        }

        protected virtual string getLocation(LocationZones location)
        {
            switch (location)
            {
                case LocationZones.USCentral:
                    return "centralus";
                case LocationZones.USEast:
                    return "eastus";
                default:
                case LocationZones.USWest:
                    return "westus";
            }
        }

        protected virtual string getResourceName(FathymResourceTypes resourceType, string desiredName)
        {
            string rscName = null;

            switch (resourceType)
            {
                case FathymResourceTypes.EventHub:
                case FathymResourceTypes.EventHubNamespace:
                case FathymResourceTypes.ApplicationInsights:
                case FathymResourceTypes.StreamAnalytics:
                case FathymResourceTypes.AppServicePlan:
                    rscName = desiredName.ToLower();
                    break;
                case FathymResourceTypes.ClassicStorage:
                case FathymResourceTypes.DataLakeAnalytics:
                case FathymResourceTypes.DataLakeStore:
                case FathymResourceTypes.IotHub:
                case FathymResourceTypes.CosmosDB:
                case FathymResourceTypes.ServiceBusNamespace:
                case FathymResourceTypes.StorageAccount:
                default:
                    rscName = desiredName.Replace("-", "").ToLower();
                    break;
            }

            return rscName;
        }

        protected virtual IDictionary<string, string> getTags(string clientName, ProvisioningEnvironment environment, string clientFlowName)
        {           
            var tags = new Dictionary<string, string>();

            tags.Add("Client", clientName);
            tags.Add("Client Flow", clientFlowName);
            tags.Add("Environment", getEnvironmentName(environment));            

            return tags;
        }
        
        #endregion
    }
    
}
