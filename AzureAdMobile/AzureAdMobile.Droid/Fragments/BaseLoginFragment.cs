using System;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Android.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Android.Util;
using Android.Webkit;

namespace AzureAdMobile.Droid.Fragments
{
	public abstract class BaseLoginFragment : Fragment
	{
		protected Button loginBackendButton;
		protected Button loginAppButton;
		protected Button logout;
		protected TextView console;
		protected View consoleLayout;
		protected TextView accessToken;
		protected TextView accessTokenType;
		protected TextView expiresOn;
		protected TextView tenantId;
		protected TextView idToken;
		protected TextView userUniqueId;
		protected TextView userDisplayableId;
		protected TextView userGivenName;
		protected TextView userFamilyName;
		protected TextView userIdentityProvider;

		protected AuthenticationResult currentAppAuthResult;
		protected AuthenticationResult currentBackendAuthResult;
		protected IPlatformParameters platformParameters;
		protected PersistentTokenCache tokenCache;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			this.platformParameters = new PlatformParameters(this.Activity);
			this.tokenCache = new PersistentTokenCache();

			// Create your fragment here
		}

		public virtual int LayoutId
		{
			get
			{
				return Resource.Layout.fragmentLayout;
			}
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(this.LayoutId, container, false);

			this.loginBackendButton = view.FindViewById<Button>(Resource.Id.loginBackend);
			this.loginAppButton = view.FindViewById<Button>(Resource.Id.loginApp);
			this.console = view.FindViewById<TextView>(Resource.Id.console);
			this.consoleLayout = view.FindViewById<View>(Resource.Id.consoleLayout);
			this.accessToken = view.FindViewById<TextView>(Resource.Id.accessToken);
			this.accessTokenType = view.FindViewById<TextView>(Resource.Id.accessTokenType);
			this.expiresOn = view.FindViewById<TextView>(Resource.Id.expiresOn);
			this.tenantId = view.FindViewById<TextView>(Resource.Id.tenantId);
			this.idToken = view.FindViewById<TextView>(Resource.Id.idToken);
			this.userUniqueId = view.FindViewById<TextView>(Resource.Id.userUniqueId);
			this.userDisplayableId = view.FindViewById<TextView>(Resource.Id.userDisplayableId);
			this.userGivenName = view.FindViewById<TextView>(Resource.Id.userGivenName);
			this.userFamilyName = view.FindViewById<TextView>(Resource.Id.userFamilyName);
			this.userIdentityProvider = view.FindViewById<TextView>(Resource.Id.userIdentityProvider);
			this.logout = view.FindViewById<Button>(Resource.Id.logout);
			

			this.consoleLayout.Visibility = ViewStates.Gone;
			this.loginBackendButton.Click += OnLoginBackend;
			this.loginAppButton.Click += OnLoginApp;
			this.logout.Click += OnLogout;

			this.logout.Enabled = false;
			this.loginBackendButton.Enabled = true;
			this.loginAppButton.Enabled = true;

			return view;
		}

		protected async void OnLoginBackend(object sender, EventArgs e)
		{
			try
			{
				if (await Login(AzureConstants.BackendResourceUri, false))
				{
					this.logout.Enabled = true;
					this.loginBackendButton.Enabled = false;
				}
			}
			catch (Exception)
			{
			}
		}

		protected async void OnLoginApp(object sender, EventArgs e)
		{
			try
			{
				if (await Login(AzureConstants.AppResourceUri, true))
				{
					this.logout.Enabled = true;
					this.loginAppButton.Enabled = false;
				}
			}
			catch (Exception)
			{
			}
		}

		protected void OnLogout(object sender, EventArgs e)
		{
			// clear all cookies to remove any auth cookies
			CookieManager.Instance.RemoveAllCookie();

			// now clear the local token cache
			this.tokenCache.Clear();

			this.currentBackendAuthResult = null;
			this.currentAppAuthResult = null;
			this.logout.Enabled = false;
			this.loginAppButton.Enabled = true;
			this.loginBackendButton.Enabled = true;
			this.consoleLayout.Visibility = ViewStates.Gone;
			this.console.Text = "Logged out";
		}

		protected async Task<AuthenticationResult> PerformAuth(string resourceId)
		{
			var authContext = new AuthenticationContext(string.Format(AzureConstants.AzureAuthority, AzureConstants.AzureTenantId), this.tokenCache);

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
					return await authContext.AcquireTokenAsync(resourceId, AzureConstants.AzureClientId, new Uri(AzureConstants.AzureAdRedirectUri), platformParameters, UserIdentifier.AnyUser, AzureConstants.AzureDomainHint);
				}

				// we know what user is logged in so try to acquire a silent token for them
				return await authContext.AcquireTokenSilentAsync(resourceId, AzureConstants.AzureClientId, userIdentifier, this.platformParameters);
			}
			catch (AdalSilentTokenAcquisitionException)
			{
				// perform UI based authentication
				return await authContext.AcquireTokenAsync(resourceId, AzureConstants.AzureClientId, new Uri(AzureConstants.AzureAdRedirectUri), platformParameters, UserIdentifier.AnyUser, AzureConstants.AzureDomainHint);
			}
		}

		protected async Task<bool> Login(string resourceId, bool isAppLogin)
		{
			try
			{
				AuthenticationResult authResult = await PerformAuth(resourceId);

				ProcessAuthResult(authResult);

				if (isAppLogin)
				{
					this.currentAppAuthResult = authResult;
				}
				else
				{
					this.currentBackendAuthResult = authResult;
				}

				return true;
			}
			catch (Exception ex)
			{
				this.consoleLayout.Visibility = ViewStates.Gone;
				ISpanned htmlText = Html.FromHtml("<strong>Error</strong><br/>" + ex.Message.Replace("\r\n", "<br/>"));
				this.console.SetText(htmlText, TextView.BufferType.Spannable);

				return false;
			}
		}

		protected void ProcessAuthResult(AuthenticationResult authResult)
		{
			this.consoleLayout.Visibility = ViewStates.Visible;
			SetHtmlTextOnTextView(this.accessToken, "<strong>AccessToken:</strong> " + authResult.AccessToken);
			SetHtmlTextOnTextView(this.accessTokenType, "<strong>AccessTokenType:</strong> " + authResult.AccessTokenType);
			SetHtmlTextOnTextView(this.expiresOn, "<strong>ExpiresOn:</strong> " + authResult.ExpiresOn.ToString());
			SetHtmlTextOnTextView(this.tenantId, "<strong>TenantId:</strong> " + authResult.TenantId);
			SetHtmlTextOnTextView(this.idToken, "<strong>IdToken:</strong> " + authResult.IdToken);
			SetHtmlTextOnTextView(this.userUniqueId, "<strong>UserUniqueId:</strong> " + authResult.UserInfo.UniqueId);
			SetHtmlTextOnTextView(this.userDisplayableId, "<strong>UserDisplayableId:</strong> " + authResult.UserInfo.DisplayableId);
			SetHtmlTextOnTextView(this.userGivenName, "<strong>UserGivenName:</strong> " + authResult.UserInfo.GivenName);
			SetHtmlTextOnTextView(this.userFamilyName, "<strong>UserFamilyName:</strong> " + authResult.UserInfo.FamilyName);
			SetHtmlTextOnTextView(this.userIdentityProvider, "<strong>UserIdentityProvider:</strong> " + authResult.UserInfo.IdentityProvider);
			this.console.Text = string.Empty;
		}

		protected void SetHtmlTextOnTextView(TextView tv, string htmlText)
		{
			ISpanned html = Html.FromHtml(htmlText);
			tv.SetText(html, TextView.BufferType.Spannable);
		}

		public override void OnDestroy()
		{
			this.currentAppAuthResult = null;
			this.currentBackendAuthResult = null;
			this.platformParameters = null;

			if (this.loginBackendButton != null)
			{
				this.loginBackendButton.Click -= OnLoginBackend;
				this.loginBackendButton.Dispose();
				this.loginBackendButton = null;
			}

			if (this.loginAppButton != null)
			{
				this.loginAppButton.Click -= OnLoginApp;
				this.loginAppButton.Dispose();
				this.loginAppButton = null;
			}

			if (this.logout != null)
			{
				this.logout.Click -= OnLogout;
				this.logout.Dispose();
				this.logout = null;
			}

			if (this.console != null)
			{
				this.console.Dispose();
				this.console = null;
			}

			if (this.accessToken != null)
			{
				this.accessToken.Dispose();
				this.accessToken = null;
			}

			if (this.accessTokenType != null)
			{
				this.accessTokenType.Dispose();
				this.accessTokenType = null;
			}

			if (this.expiresOn != null)
			{
				this.expiresOn.Dispose();
				this.expiresOn = null;
			}

			if (this.tenantId != null)
			{
				this.tenantId.Dispose();
				this.tenantId = null;
			}

			if (this.idToken != null)
			{
				this.idToken.Dispose();
				this.idToken = null;
			}

			if (this.userUniqueId != null)
			{
				this.userUniqueId.Dispose();
				this.userUniqueId = null;
			}

			if (this.userDisplayableId != null)
			{
				this.userDisplayableId.Dispose();
				this.userDisplayableId = null;
			}

			if (this.userGivenName != null)
			{
				this.userGivenName.Dispose();
				this.userGivenName = null;
			}

			if (this.userIdentityProvider != null)
			{
				this.userIdentityProvider.Dispose();
				this.userIdentityProvider = null;
			}

			if (this.userFamilyName != null)
			{
				this.userFamilyName.Dispose();
				this.userFamilyName = null;
			}

			if (this.consoleLayout != null)
			{
				this.consoleLayout.Dispose();
				this.consoleLayout = null;
			}

			base.OnDestroy();
		}
	}
}