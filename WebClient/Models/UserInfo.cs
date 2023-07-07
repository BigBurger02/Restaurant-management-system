using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using System.Text.Json;

namespace WebClient.Models;

public class UserInfo
{
	public UserInfo(AuthenticateResult result)
	{
		AuthenticateResult = result;

		foreach (var item in AuthenticateResult.Principal.Claims)
		{
			Claims += item.Type + "-" + item.Value + "<->";
		}

		if (result.Properties.Items.ContainsKey("client_list"))
		{
			var encoded = result.Properties.Items["client_list"];
			var bytes = Base64Url.Decode(encoded);
			var value = Encoding.UTF8.GetString(bytes);

			Clients = JsonSerializer.Deserialize<string[]>(value);
		}
	}

	public AuthenticateResult AuthenticateResult { get; }
	public IEnumerable<string> Clients { get; } = new List<string>();

	public string Claims { get; } = string.Empty;
}

