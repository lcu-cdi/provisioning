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
			ILogger log, ExecutionContext context)
		{
			log.LogInformation("SaveTemplate function processed a request.");			

			var json = await new StreamReader(req.Body).ReadToEndAsync();

			log.LogInformation($"Request: {json}");	

			dynamic request = JsonConvert.DeserializeObject<dynamic>(json);	

			var response = new BaseResponse();

			try
			{
				response.Status = CommitTemplate($"templates/{Convert.ToString(request.name)}", Path.Combine(context.FunctionDirectory, "repo"),
										Convert.ToString(request.comment), request.template);
			}
			catch (Exception ex)
			{
				response.Status = Status.GeneralError.Clone(ex.Message);
			}
						
			return new JsonResult(response, new JsonSerializerSettings());
		}

		public static async Task<Status> CommitTemplate(string branchName, string repoPath, string comment, dynamic template)
		{
			var gitURL = Environment.GetEnvironmentVariable("TEMPLATES_REPO_URL");
			var gitUser = Environment.GetEnvironmentVariable("TEMPLATES_REPO_UNAME");
			var gitPw = Environment.GetEnvironmentVariable("TEMPLATES_REPO_PW");

            var cloneOptions = new CloneOptions();
            cloneOptions.CredentialsProvider = (url, user, cred) => new UsernamePasswordCredentials { Username = gitUser, Password = gitPw };
            Repository.Clone(gitURL, repoPath, cloneOptions);			

			using (var repo = new Repository(repoPath))
			{
				var branch = repo.Branches[branchName];

				var currentBranch = Commands.Checkout(repo, repo.Branches["master"]);

				if (branch == null)
					currentBranch = repo.CreateBranch(branchName); 				
				else currentBranch = Commands.Checkout(repo , branch);				
								
				File.WriteAllText(Path.Combine(repo.Info.WorkingDirectory, "template.json"), template.ToJSON());

				Commands.Stage(repo, "*");

				var author = new Signature("AzureDevOps", "@devops", DateTime.Now);
				var committer = author;

				var commit = repo.Commit(comment ?? "updating template", author, committer);

                var remote = repo.Network.Remotes["origin"];
                var pushRefSpec = $"refs/heads/{branchName}";				

				var options = new PushOptions()
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
