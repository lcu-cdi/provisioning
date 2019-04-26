// Requires the following Azure NuGet packages and related dependencies:
// package id="Microsoft.Azure.Management.Authorization" version="2.0.0"
// package id="Microsoft.Azure.Management.ResourceManager" version="1.4.0-preview"
// package id="Microsoft.Rest.ClientRuntime.Azure.Authentication" version="2.2.8-preview"
  
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Rest.Azure.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace LCU.CDI.Provisioning
{
    /// <summary>
    /// This is a helper class for deploying an Azure Resource Manager template
    /// More info about template deployments can be found here https://go.microsoft.com/fwLink/?LinkID=733371
    /// </summary>
    public class AzureDeployment
    {
        public string SubscriptionId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }

        public AzureDeployment(string tenantId, string subscriptionId, string clientId, string clientSecret)
        {
            TenantId = tenantId;
            SubscriptionId = subscriptionId;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }     

        public async void DeployTemplate(string deploymentName, string resourceGroupName, string resourceGroupLocation, 
                            dynamic template, dynamic parameters)
        {
            // Try to obtain the service credentials
            var serviceCreds = await ApplicationTokenProvider.LoginSilentAsync(TenantId, ClientId, ClientSecret);

            // Create the resource manager client
            var resourceManagementClient = new ResourceManagementClient(serviceCreds);
            resourceManagementClient.SubscriptionId = SubscriptionId;

            // Create or check that resource group exists
            ensureResourceGroupExists(resourceManagementClient, resourceGroupName, resourceGroupLocation);

            // Start a deployment
            Console.WriteLine(string.Format("Starting template deployment '{0}' in resource group '{1}'", deploymentName, resourceGroupName));
            var deployment = new Deployment();

            deployment.Properties = new DeploymentProperties
            {
                Mode = DeploymentMode.Incremental,
                Template = template,
                Parameters = parameters
            };

            var deploymentResult = resourceManagementClient.Deployments.CreateOrUpdate(resourceGroupName, deploymentName, deployment);
            Console.WriteLine(string.Format("Deployment status: {0}", deploymentResult.Properties.ProvisioningState));
        }

        #region Helpers
        /// <summary>
        /// Reads a JSON file from the specified path
        /// </summary>
        /// <param name="pathToJson">The full path to the JSON file</param>
        /// <returns>The JSON file contents</returns>
        protected virtual JObject getJsonFileContents(string pathToJson)
        {
            JObject templatefileContent = new JObject();
            using (StreamReader file = File.OpenText(pathToJson))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    templatefileContent = (JObject)JToken.ReadFrom(reader);
                    return templatefileContent;
                }
            }
        }

        /// <summary>
        /// Ensures that a resource group with the specified name exists. If it does not, will attempt to create one.
        /// </summary>
        /// <param name="resourceManagementClient">The resource manager client.</param>
        /// <param name="resourceGroupName">The name of the resource group.</param>
        /// <param name="resourceGroupLocation">The resource group location. Required when creating a new resource group.</param>
        protected virtual void ensureResourceGroupExists(ResourceManagementClient resourceManagementClient, string resourceGroupName, string resourceGroupLocation)
        {
            if (resourceManagementClient.ResourceGroups.CheckExistence(resourceGroupName) != true)
            {
                Console.WriteLine(string.Format("Creating resource group '{0}' in location '{1}'", resourceGroupName, resourceGroupLocation));
                var resourceGroup = new ResourceGroup();
                resourceGroup.Location = resourceGroupLocation;
                resourceManagementClient.ResourceGroups.CreateOrUpdate(resourceGroupName, resourceGroup);
            }
            else
            {
                Console.WriteLine(string.Format("Using existing resource group '{0}'", resourceGroupName));
            }
        }
        #endregion
    }
}