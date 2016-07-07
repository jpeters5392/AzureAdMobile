using System;
using Android.Support.V4.App;
using System.Collections.Generic;
using AzureAdMobile.Droid.Fragments;

namespace AzureAdMobile.Droid
{
	public class FragmentPagerAdapter : FragmentStatePagerAdapter
	{
		private IList<Fragment> fragments;
		public FragmentPagerAdapter(FragmentManager fm) : base(fm)
		{
			fragments = new List<Fragment>();
			fragments.Add(new SmartLoginFragment());
			fragments.Add(new BackendFragment());
		}

		public override int Count
		{
			get
			{
				return fragments.Count;
			}
		}

		public override Fragment GetItem(int position)
		{
			return fragments[position];
		}
	}
}