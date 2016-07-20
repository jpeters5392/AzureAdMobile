using Foundation;
using System;
using UIKit;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace AzureAdMobile.iOS
{
    public partial class LoginViewController : UIViewController
    {
		private PlatformParameters platformParameters = null;

        public LoginViewController (IntPtr handle) : base (handle)
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

		partial void OnCompareTouchUpInside(UIButton sender)
		{
			if (AuthUtility.currentAppAuthResult != null && AuthUtility.currentBackendAuthResult != null)
			{
				if (AuthUtility.currentAppAuthResult.AccessToken == AuthUtility.currentBackendAuthResult.AccessToken)
				{
					this.JsonText.Text = "The tokens are identical";
				}
				else
				{
					this.JsonText.Text = "The tokens are different";
				}
			}
			else
			{
				this.JsonText.Text = "You are not logged in to both systems";
			}
		}

		partial void OnLogoutTouchUpInside(UIButton sender)
		{
			AuthUtility authUtility = new AuthUtility();
			authUtility.Clear();
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