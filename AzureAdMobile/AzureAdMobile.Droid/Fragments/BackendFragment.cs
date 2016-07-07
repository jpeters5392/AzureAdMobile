using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AzureAdMobile.Droid.Fragments
{
	public class BackendFragment : BaseLoginFragment
	{
		private Button callBasic;
		private Button callBasicGroup;
		private Button callElevated;
		private Button callElevatedGroup;

		public override int LayoutId
		{
			get
			{
				return Resource.Layout.backendData;
			}
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = base.OnCreateView(inflater, container, savedInstanceState);

			this.callBasic = view.FindViewById<Button>(Resource.Id.callBasic);
			this.callBasicGroup = view.FindViewById<Button>(Resource.Id.callBasicGroup);
			this.callElevated = view.FindViewById<Button>(Resource.Id.callElevated);
			this.callElevatedGroup = view.FindViewById<Button>(Resource.Id.callElevatedGroup);

			this.callBasic.Click += CallBasic;
			this.callBasicGroup.Click += CallBasicGroup;
			this.callElevated.Click += CallElevated;
			this.callElevatedGroup.Click += CallElevatedGroup;

			var title = view.FindViewById<TextView>(Resource.Id.title);
			title.Text = "Backend Data";

			return view;
		}

		private async void CallBasic(object sender, EventArgs e)
		{
			var progressDialog = new ProgressDialog(this.Activity);
			progressDialog.SetMessage("Executing");
			progressDialog.Show();
			try
			{
				this.consoleLayout.Visibility = ViewStates.Gone;
				var service = new BackendService(this.currentBackendAuthResult.CreateAuthorizationHeader());
				var response = await service.CallBasicEndpoint();
				this.console.Text = response;
			}
			catch(Exception ex)
			{
				Toast.MakeText(this.Activity, ex.Message, ToastLength.Long);
			}


			progressDialog.Dismiss();
		}

		private async void CallBasicGroup(object sender, EventArgs e)
		{
			var progressDialog = new ProgressDialog(this.Activity);
			progressDialog.SetMessage("Executing");
			progressDialog.Show();

			try
			{
				this.consoleLayout.Visibility = ViewStates.Gone;
				var service = new BackendService(this.currentBackendAuthResult.CreateAuthorizationHeader());
				var response = await service.CallBasicGroupEndpoint();
				this.console.Text = response;
			}
			catch (Exception ex)
			{
				Toast.MakeText(this.Activity, ex.Message, ToastLength.Long);
			}

			progressDialog.Dismiss();
		}

		private async void CallElevated(object sender, EventArgs e)
		{
			var progressDialog = new ProgressDialog(this.Activity);
			progressDialog.SetMessage("Executing");
			progressDialog.Show();

			try
			{
				this.consoleLayout.Visibility = ViewStates.Gone;
				var service = new BackendService(this.currentBackendAuthResult.CreateAuthorizationHeader());
				var response = await service.CallElevatedEndpoint();
				this.console.Text = response;
			}
			catch (Exception ex)
			{
				Toast.MakeText(this.Activity, ex.Message, ToastLength.Long);
			}

			progressDialog.Dismiss();
		}

		private async void CallElevatedGroup(object sender, EventArgs e)
		{
			var progressDialog = new ProgressDialog(this.Activity);
			progressDialog.SetMessage("Executing");
			progressDialog.Show();

			try
			{
				this.consoleLayout.Visibility = ViewStates.Gone;
				var service = new BackendService(this.currentBackendAuthResult.CreateAuthorizationHeader());
				var response = await service.CallElevatedGroupEndpoint();
				this.console.Text = response;
			}
			catch (Exception ex)
			{
				Toast.MakeText(this.Activity, ex.Message, ToastLength.Long);
			}

			progressDialog.Dismiss();
		}

		public override void OnDestroy()
		{
			if (this.callBasic != null)
			{
				this.callBasic.Click -= CallBasic;
				this.callBasic.Dispose();
				this.callBasic = null;
			}

			if (this.callBasicGroup != null)
			{
				this.callBasicGroup.Click -= CallBasicGroup;
				this.callBasicGroup.Dispose();
				this.callBasicGroup = null;
			}

			if (this.callElevated != null)
			{
				this.callElevated.Click -= CallElevated;
				this.callElevated.Dispose();
				this.callElevated = null;
			}

			if (this.callElevatedGroup != null)
			{
				this.callElevatedGroup.Click -= CallElevatedGroup;
				this.callElevatedGroup.Dispose();
				this.callElevatedGroup = null;
			}

			base.OnDestroy();
		}
	}
}