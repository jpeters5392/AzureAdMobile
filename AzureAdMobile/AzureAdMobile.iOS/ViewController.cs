using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;
using UIKit;

namespace AzureAdMobile.iOS
{
	public partial class ViewController : UIViewController
	{
		private LoadingOverlay loader;
		private AuthenticationResult currentAppAuthResult = null;
		private AuthenticationResult currentBackendAuthResult = null;
		private PlatformParameters platformParameters = null;

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			this.platformParameters = new PlatformParameters(this);
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		partial void OnCompareTokensTouchUpInside(UIButton sender)
		{
			throw new NotImplementedException();
		}

		partial void OnLoginAppTouchUpInside(UIButton sender)
		{
			PerformAuth(false);
		}

		partial void OnLoginBackendTouchUpInside(UIButton sender)
		{
			PerformAuth(true);
		}

		protected async void PerformAuth(bool isBackend)
		{
			var bounds = UIScreen.MainScreen.Bounds;

			// show the loading overlay on the UI thread using the correct orientation sizing
			loader = new LoadingOverlay(bounds);
			View.Add(loader);

			try
			{
				var authContext = new AuthenticationContext(string.Format(AzureConstants.AzureAuthority, AzureConstants.AzureTenantId));

				try
				{
					UserIdentifier userIdentifier = null;
					if (this.currentAppAuthResult != null)
					{
						// force it to only consider the current user
						userIdentifier = new UserIdentifier(this.currentAppAuthResult.UserInfo.UniqueId, UserIdentifierType.UniqueId);
					}
					else if (this.currentBackendAuthResult != null)
					{
						// force it to only consider the current user
						userIdentifier = new UserIdentifier(this.currentBackendAuthResult.UserInfo.UniqueId, UserIdentifierType.UniqueId);
					}
					else
					{
						// there is no current user so force UI authentication instead of using UserIdentifier.AnyUser
						if (isBackend)
						{
							this.currentBackendAuthResult = await authContext.AcquireTokenAsync(AzureConstants.BackendResourceUri, AzureConstants.AzureClientId, new Uri(AzureConstants.AzureAdRedirectUri), platformParameters, UserIdentifier.AnyUser, AzureConstants.AzureDomainHint);
						}
						else
						{
							this.currentAppAuthResult = await authContext.AcquireTokenAsync(AzureConstants.AppResourceUri, AzureConstants.AzureClientId, new Uri(AzureConstants.AzureAdRedirectUri), platformParameters, UserIdentifier.AnyUser, AzureConstants.AzureDomainHint);
						}
					}

					if (isBackend)
					{
						// we know what user is logged in so try to acquire a silent token for them
						this.currentBackendAuthResult = await authContext.AcquireTokenSilentAsync(AzureConstants.BackendResourceUri, AzureConstants.AzureClientId, userIdentifier, this.platformParameters);
					}
					else
					{
						// we know what user is logged in so try to acquire a silent token for them
						this.currentAppAuthResult = await authContext.AcquireTokenSilentAsync(AzureConstants.AppResourceUri, AzureConstants.AzureClientId, userIdentifier, this.platformParameters);
					}
				}
				catch (AdalSilentTokenAcquisitionException)
				{
					if (isBackend)
					{
						// perform UI based authentication
						this.currentBackendAuthResult = await authContext.AcquireTokenAsync(AzureConstants.BackendResourceUri, AzureConstants.AzureClientId, new Uri(AzureConstants.AzureAdRedirectUri), platformParameters, UserIdentifier.AnyUser, AzureConstants.AzureDomainHint);
					}
					else
					{
						// perform UI based authentication
						this.currentAppAuthResult = await authContext.AcquireTokenAsync(AzureConstants.AppResourceUri, AzureConstants.AzureClientId, new Uri(AzureConstants.AzureAdRedirectUri), platformParameters, UserIdentifier.AnyUser, AzureConstants.AzureDomainHint);
					}
				}

				if (isBackend)
				{
					this.JsonText.Text = ParseAuthResult(this.currentBackendAuthResult).ToString();
				}
				else
				{
					this.JsonText.Text = ParseAuthResult(this.currentAppAuthResult).ToString();
				}
			}
			catch (Exception)
			{
			}

			loader.Hide();
		}

		private JObject ParseAuthResult(AuthenticationResult authResult)
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

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			this.platformParameters = null;
			this.currentAppAuthResult = null;
			this.currentBackendAuthResult = null;
		}
	}
}

