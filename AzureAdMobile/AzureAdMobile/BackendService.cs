using ModernHttpClient;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AzureAdMobile
{
	public class BackendService
	{
		private const string BasicEndpoint = "/api/Basic";
		private const string ElevatedEndpoint = "/api/Elevated";
		private const string BasicGroupEndpoint = "/api/BasicGroup";
		private const string ElevatedGroupEndpoint = "/api/ElevatedGroup";
		private const string Server = "https://azureadbackend.azurewebsites.net";
		private string authHeader;

		public BackendService(string authHeader)
		{
			this.authHeader = authHeader;
		}

		public async Task<string> CallBasicGroupEndpoint()
		{
			var client = new HttpClient(new NativeMessageHandler());
			client.DefaultRequestHeaders.Add("Authorization", authHeader);
			var response = await client.GetAsync(Server + BasicGroupEndpoint);
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<string> CallElevatedGroupEndpoint()
		{
			var client = new HttpClient(new NativeMessageHandler());
			client.DefaultRequestHeaders.Add("Authorization", authHeader);
			var response = await client.GetAsync(Server + ElevatedGroupEndpoint);
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<string> CallBasicEndpoint()
		{
			var client = new HttpClient(new NativeMessageHandler());
			client.DefaultRequestHeaders.Add("Authorization", authHeader);
			var response = await client.GetAsync(Server + BasicEndpoint);
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<string> CallElevatedEndpoint()
		{
			var client = new HttpClient(new NativeMessageHandler());
			client.DefaultRequestHeaders.Add("Authorization", authHeader);
			var response = await client.GetAsync(Server + ElevatedEndpoint);
			return await response.Content.ReadAsStringAsync();
		}
	}
}

