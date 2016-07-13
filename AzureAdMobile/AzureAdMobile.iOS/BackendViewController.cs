using Foundation;
using System;
using UIKit;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace AzureAdMobile.iOS
{
    public partial class BackendViewController : UIViewController
    {
		private PlatformParameters platformParameters = null;

        public BackendViewController (IntPtr handle) : base (handle)
        {
			this.platformParameters = new PlatformParameters(this);
        }

		partial void OnAppLoginTouchUpInside(UIButton sender)
		{
			AuthUtility authUtility = new AuthUtility();
			authUtility.PerformAuth(this, this.JsonText, this.platformParameters, AzureConstants.AppResourceUri, false);
		}

		partial void OnBackendLoginTouchUpInside(UIButton sender)
		{
			AuthUtility authUtility = new AuthUtility();
			authUtility.PerformAuth(this, this.JsonText, this.platformParameters, AzureConstants.BackendResourceUri, true);
		}

		partial void OnBasicGroupTouchUpInside(UIButton sender)
		{
			if (AuthUtility.currentBackendAuthResult == null)
			{
				this.JsonText.Text = "You must be authenticated to the backend";
			}
			else
			{
				this.CallBasicGroupEndpoint();
			}
		}

		partial void OnBasicRoleTouchUpInside(UIButton sender)
		{
			if (AuthUtility.currentBackendAuthResult == null)
			{
				this.JsonText.Text = "You must be authenticated to the backend";
			}
			else
			{
				this.CallBasicRoleEndpoint();
			}
		}

		partial void OnElevatedGroupTouchUpInside(UIButton sender)
		{
			if (AuthUtility.currentBackendAuthResult == null)
			{
				this.JsonText.Text = "You must be authenticated to the backend";
			}
			else
			{
				this.CallElevatedGroupEndpoint();
			}
		}

		partial void OnElevatedRoleTouchUpInside(UIButton sender)
		{
			if (AuthUtility.currentBackendAuthResult == null)
			{
				this.JsonText.Text = "You must be authenticated to the backend";
			}
			else
			{
				this.CallElevatedRoleEndpoint();
			}
		}

		private async void CallBasicRoleEndpoint()
		{
			var bounds = UIScreen.MainScreen.Bounds;

			// show the loading overlay on the UI thread using the correct orientation sizin
			LoadingOverlay loadingOverlay = new LoadingOverlay(bounds);
			this.View.Add(loadingOverlay);

			try
			{
				var backendService = new BackendService(AuthUtility.currentBackendAuthResult.CreateAuthorizationHeader());
				this.JsonText.Text = await backendService.CallBasicEndpoint();
			}
			catch (Exception ex)
			{
				this.JsonText.Text = ex.Message;
			}

			loadingOverlay.Hide();
		}

		private async void CallBasicGroupEndpoint()
		{
			var bounds = UIScreen.MainScreen.Bounds;

			// show the loading overlay on the UI thread using the correct orientation sizi
			LoadingOverlay loadingOverlay = new LoadingOverlay(bounds);
			this.View.Add(loadingOverlay);

			try
			{
				var backendService = new BackendService(AuthUtility.currentBackendAuthResult.CreateAuthorizationHeader());
				this.JsonText.Text = await backendService.CallBasicGroupEndpoint();
			}
			catch (Exception ex)
			{
				this.JsonText.Text = ex.Message;
			}

			loadingOverlay.Hide();
		}

		private async void CallElevatedRoleEndpoint()
		{
			var bounds = UIScreen.MainScreen.Bounds;

			// show the loading overlay on the UI thread using the correct orientation sizi
			LoadingOverlay loadingOverlay = new LoadingOverlay(bounds);
			this.View.Add(loadingOverlay);

			try
			{
				var backendService = new BackendService(AuthUtility.currentBackendAuthResult.CreateAuthorizationHeader());
				this.JsonText.Text = await backendService.CallElevatedEndpoint();
			}
			catch (Exception ex)
			{
				this.JsonText.Text = ex.Message;
			}

			loadingOverlay.Hide();
		}

		private async void CallElevatedGroupEndpoint()
		{
			var bounds = UIScreen.MainScreen.Bounds;

			// show the loading overlay on the UI thread using the correct orientation sizi
			LoadingOverlay loadingOverlay = new LoadingOverlay(bounds);
			this.View.Add(loadingOverlay);

			try
			{
				var backendService = new BackendService(AuthUtility.currentBackendAuthResult.CreateAuthorizationHeader());
				this.JsonText.Text = await backendService.CallElevatedGroupEndpoint();
			}
			catch (Exception ex)
			{
				this.JsonText.Text = ex.Message;
			}

			loadingOverlay.Hide();
		}

		partial void OnLogoutTouchUpInside(UIButton sender)
		{
			TokenCache.DefaultShared.Clear();
			AuthUtility.currentAppAuthResult = null;
			AuthUtility.currentBackendAuthResult = null;

			this.JsonText.Text = "You are logged out";
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			this.platformParameters = null;
		}
	}
}