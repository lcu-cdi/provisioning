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
using LCU.CDI.Provisioning.Models;
using LCU.CDI.Provisioning;

namespace LCU.CDI.Provisioning.Functions
{	
	public static class GetStorageAccount
	{
		public const string TemplateURL = "https://lcuintbfb9.blob.core.windows.net/arm-templates/StorageAccount.json";

		[FunctionName("GetStorageAccount")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("GetStorageAccount function processing a request.");			

			dynamic request = req.Body.ToDynamic();

            var stgParams = new StorageAccountParams()
            {
                AccessTier = new ParamType(request.accessTier),
                Location = new ParamType(request.location),
                Name = new ParamType(request.name)
            };

			var templateParams = new DeploymentParameters(stgParams.ToDynamic()).ToDynamic();

			var response = new BaseResponse<GetResourceTemplateResponse>()
			{
				Status = Status.Success,

				Model = new GetResourceTemplateResponse(TemplateURL, templateParams)
			};

			log.LogInformation("GetStorageAccount function processed a request.");	

			return new JsonResult(response, new JsonSerializerSettings());
		}
	}
}
