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

			var response = new BaseResponse<Application>();

			response.Status = Status.Success;

			return new JsonResult(response, new JsonSerializerSettings());
		}
	}
}
