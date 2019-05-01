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
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace LCU.CDI.Provisioning.Functions
{
	public static class SaveTemplate
	{
		[FunctionName("SaveTemplate")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("SaveTemplate function processed a request.");			

			dynamic request = req.Body.ToDynamic();			

			var response = new BaseResponse();

			try
			{
				response.Status = CommitTemplate($"templates/{request.name}", request.comment, request.template, request.parameters);
			}
			catch (Exception ex)
			{
				response.Status = Status.GeneralError.Clone(ex.Message);
			}
						
			return new JsonResult(response, new JsonSerializerSettings());
		}

		public static async Task<Status> CommitTemplate(string branchName, string comment, dynamic template, dynamic parameters)
		{
			var gitRepoPath = Environment.GetEnvironmentVariable("TEMPLATES_REPO_LOCALPATH");
			var gitUser = Environment.GetEnvironmentVariable("TEMPLATES_REPO_UNAME");
			var gitPw = Environment.GetEnvironmentVariable("TEMPLATES_REPO_PW");

			using (var repo = new Repository(gitRepoPath))
			{
				var branch = repo.Branches[branchName];

				Branch currentBranch = Commands.Checkout(repo, repo.Branches["master"]);

				if (branch == null)
					currentBranch = repo.CreateBranch(branchName); 				
				else currentBranch = Commands.Checkout(repo , branch);				
								
				File.WriteAllText(Path.Combine(repo.Info.WorkingDirectory, "template.json"), template.ToJSON());
				File.WriteAllText(Path.Combine(repo.Info.WorkingDirectory, "params.json"), parameters.ToJSON());

				Commands.Stage(repo, "*");

				var author = new Signature("AzureDevOps", "@devops", DateTime.Now);
				var committer = author;

				var commit = repo.Commit(comment ?? "updating template", author, committer);

                var remote = repo.Network.Remotes["origin"];
                var pushRefSpec = $"refs/heads/{branchName}";				

				var options = new LibGit2Sharp.PushOptions()
				{
					CredentialsProvider = new CredentialsHandler
					(
						(url, usernameFromUrl, types) => 
							new UsernamePasswordCredentials()
							{
								Username = gitUser,
								Password = gitPw
							}
					)
				};

				repo.Network.Push(remote, pushRefSpec, options);
			}

			return Status.Success;
		}

	}
}
