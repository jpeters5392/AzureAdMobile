// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AzureAdMobile.iOS
{
    [Register ("LoginViewController")]
    partial class LoginViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView JsonText { get; set; }
        [Action ("OnAppLoginTouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnAppLoginTouchUpInside (UIKit.UIButton sender);

        [Action ("OnBackendLoginTouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnBackendLoginTouchUpInside (UIKit.UIButton sender);

        [Action ("OnCompareTouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnCompareTouchUpInside (UIKit.UIButton sender);

        [Action ("OnLogoutTouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnLogoutTouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (JsonText != null) {
                JsonText.Dispose ();
                JsonText = null;
            }
        }
    }
}