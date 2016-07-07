using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Android.Text;
using System.Linq;
using System.Collections.Generic;
using Android.Support.V4.View;
using Android.Support.V4.App;

namespace AzureAdMobile.Droid
{
	[Activity (Label = "AzureAdMobile.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : FragmentActivity
	{
		public const string AzureAdRedirectUri = "https://azuread-mobile-app";
		public const string AzureTenantId = "dff22dde-cb4d-470d-9645-153dec0c6e71";
		public const string AzureClientId = "cea9a049-b37e-44a7-9d5c-9d0770159a83";
		public const string AzureHost = "https://login.windows.net";
		public const string AzureAuthority = "https://login.windows.net/{0}";
		public const string AzureDomainHint = "domain_hint=ilmservicedemo.onmicrosoft.com";
		public const string BackendResourceUri = "https://ilmservicedemo.onmicrosoft.com/AzureADBackend";
		public const string AppResourceUri = "https://graph.microsoft.com";

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			ViewPager pager = FindViewById<ViewPager>(Resource.Id.pager);

			PagerAdapter adapter = new FragmentPagerAdapter(this.SupportFragmentManager);
			pager.Adapter = adapter;
		}

		protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			AuthenticationAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(requestCode, resultCode, data);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
		}
	}
}


