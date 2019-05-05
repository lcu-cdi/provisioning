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
	public static class GetServerFarm
	{
		public const string TemplateURL = "https://lcuintbfb9.blob.core.windows.net/arm-templates/ServerFarms.json?sp=r&st=2019-05-05T22:47:24Z&se=2023-05-06T06:47:24Z&spr=https&sv=2018-03-28&sig=XJeRxbUhFtA%2BSu7IglJuugzaGJqfYBy9QjvU2v7W28c%3D&sr=b";

		[FunctionName("GetServerFarm")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("GetServerFarm function processing a request.");			

			var json = await new StreamReader(req.Body).ReadToEndAsync();

			log.LogInformation($"Request: {json}");		

			dynamic request = JsonConvert.DeserializeObject<dynamic>(json);

            var svfParams = new ServerFarmParams()
            {
                Location = new ParamType(Convert.ToString(request.location)),
                Name = new ParamType(Convert.ToString(request.name))
            };

			var response = new BaseResponse<dynamic>()
			{
				Status = Status.Success,

				Model = new LinkedResourceTemplate<ServerFarmParams>(TemplateURL, svfParams).ToDynamic()
			};

			log.LogInformation($"GetServerFarm function processed a request: {JsonConvert.SerializeObject(response)}");	

			return new JsonResult(response, new JsonSerializerSettings());
		}
	}
}
