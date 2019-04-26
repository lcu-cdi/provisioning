using Fathym;
using Fathym.API;
using LCU.Graphs;
using LCU.Graphs.Registry.Enterprises;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LCU.CDI.Provisioning
{
	public static class ProvisionResources
	{
		[FunctionName("ProvisionResources")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("ProvisionResources function processed a request.");			

			dynamic request = req.Body.ToDynamic();

			var azureDeployment = new AzureDeployment(request.tenantId, request.subscriptionId, request.clientId, request.clientSecret);

			dynamic template = new Object().ToDynamic();

			dynamic parameters = new Object().ToDynamic();

			azureDeployment.DeployTemplate("testDeployment", "flw-tst", "West US 2", template, parameters);

			var response = new BaseResponse<Application>();

			response.Status = Status.Success;

			return new JsonResult(response, new JsonSerializerSettings());
		}
	}
}
