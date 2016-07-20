using System;
using AzureAdMobile.iOS.Auth;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;
using UIKit;

namespace AzureAdMobile.iOS
{
	public class AuthUtility
	{
		public static AuthenticationResult currentBackendAuthResult = null;
		public static AuthenticationResult currentAppAuthResult = null;
		private PersistentTokenCache tokenCache = null;

		public AuthUtility()
		{
			this.tokenCache = new PersistentTokenCache();
		}

		public JObject ParseAuthResult(AuthenticationResult authResult)
		{
			JObject authData = new JObject();
			authData.Add("AccessToken", authResult.AccessToken);
			authData.Add("IdToken", authResult.IdToken);
			authData.Add("TenantId", authResult.TenantId);
			authData.Add("ExpiresOn", authResult.ExpiresOn.ToString());
			authData.Add("UserInfo.FamilyName", authResult.UserInfo.FamilyName);
			authData.Add("UserInfo.GivenName", authResult.UserInfo.GivenName);
			authData.Add("UserInfo.DisplayableId", authResult.UserInfo.DisplayableId);
			return authData;
		}

		public void Clear()
		{
			this.tokenCache.Clear();
		}

		public async void PerformAuth(UIViewController currentViewController, UITextView console, PlatformParameters platformParameters, string resourceUri, bool isBackend)
		{
			var bounds = UIScreen.MainScreen.Bounds;

			// show the loading overlay on the UI thread using the correct orientation sizin
			LoadingOverlay loadingOverlay = new LoadingOverlay(bounds);
			currentViewController.View.Add(loadingOverlay);

			try
			{
				AuthenticationResult authResult = null;
				if (isBackend)
				{
					authResult = currentBackendAuthResult;
				}
				else
				{
					authResult = currentAppAuthResult;
				}

				var authContext = new AuthenticationContext(string.Format(AzureConstants.AzureAuthority, AzureConstants.AzureTenantId), this.tokenCache);

				try
				{
					UserIdentifier userIdentifier = UserIdentifier.AnyUser;
					if (authResult != null)
					{
						// force it to only consider the current user
						userIdentifier = new UserIdentifier(authResult.UserInfo.UniqueId, UserIdentifierType.UniqueId);
					}
					else
					{
						// there is no current user so force UI authentication instead of using UserIdentifier.AnyUser
						authResult = await authContext.AcquireTokenAsync(resourceUri, AzureConstants.AzureClientId, new Uri(AzureConstants.AzureAdRedirectUri), platformParameters, userIdentifier, AzureConstants.AzureDomainHint);
					}

					authResult = await authContext.AcquireTokenSilentAsync(resourceUri, AzureConstants.AzureClientId, userIdentifier, platformParameters);
				}
				catch (AdalSilentTokenAcquisitionException)
				{
					authResult = await authContext.AcquireTokenAsync(resourceUri, AzureConstants.AzureClientId, new Uri(AzureConstants.AzureAdRedirectUri), platformParameters, UserIdentifier.AnyUser, AzureConstants.AzureDomainHint);
				}


				console.Text = ParseAuthResult(authResult).ToString();

				if (isBackend)
				{
					currentBackendAuthResult = authResult;
				}
				else
				{
					currentAppAuthResult = authResult;
				}
			}
			catch (Exception ex)
			{
				console.Text = ex.Message;
			}

			loadingOverlay.Hide();
		}
	}
}

