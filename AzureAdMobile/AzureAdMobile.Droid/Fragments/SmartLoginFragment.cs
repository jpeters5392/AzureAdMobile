using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace AzureAdMobile.Droid.Fragments
{
	public class SmartLoginFragment : BaseLoginFragment
	{
		protected Button compareTokens;

		public override int LayoutId
		{
			get
			{
				return Resource.Layout.fragmentLayout;
			}
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = base.OnCreateView(inflater, container, savedInstanceState);

			this.compareTokens = view.FindViewById<Button>(Resource.Id.compareTokens);
			this.compareTokens.Click += OnCompareTokens;

			var title = view.FindViewById<TextView>(Resource.Id.title);
			title.Text = "Smart Login";

			return view;
		}

		protected void OnCompareTokens(object sender, EventArgs e)
		{
			if (this.currentAppAuthResult != null && this.currentBackendAuthResult != null)
			{
				if (this.currentAppAuthResult.AccessToken == this.currentBackendAuthResult.AccessToken)
				{
					this.console.Text = "The access tokens are identical";
				}
				else
				{
					this.console.Text = "The access tokens are NOT identical";
				}

				if (this.currentAppAuthResult.IdToken == this.currentBackendAuthResult.IdToken)
				{
					this.console.Text += ", and the id tokens are identical";
				}
				else
				{
					this.console.Text += ", and the id tokens are NOT identical";
				}
			}
			else
			{
				this.console.Text = "You must be logged in to both app and graph api";
			}
		}

		public override void OnDestroy()
		{
			if (this.compareTokens != null)
			{
				this.compareTokens.Click -= OnCompareTokens;
				this.compareTokens.Dispose();
				this.compareTokens = null;
			}

			base.OnDestroy();
		}
	}
}