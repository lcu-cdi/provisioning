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
		public const string TemplateURL = "https://lcuintbfb9.blob.core.windows.net/arm-templates/StorageAccount.json?sp=r&st=2019-05-01T21:54:49Z&se=2025-05-02T05:54:49Z&spr=https&sv=2018-03-28&sig=ewYbPL3OLB44pwTUl6vt18rr%2F%2F3T4bq2ohdIz5TQBIU%3D&sr=b";

		[FunctionName("GetStorageAccount")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("GetStorageAccount function processing a request.");			

			var json = await new StreamReader(req.Body).ReadToEndAsync();

			log.LogInformation($"Request: {json}");		

			dynamic request = JsonConvert.DeserializeObject<dynamic>(json);

            var stgParams = new StorageAccountParams()
            {
                AccessTier = new ParamType(Convert.ToString(request.accessTier)),
                Location = new ParamType(Convert.ToString(request.location)),
                Name = new ParamType(Convert.ToString(request.name))
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
