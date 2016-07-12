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
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView JsonText { get; set; }
        [Action ("OnLoginBackendTouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnLoginBackendTouchUpInside (UIKit.UIButton sender);

        [Action ("OnLoginAppTouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnLoginAppTouchUpInside (UIKit.UIButton sender);

        [Action ("OnCompareTokensTouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnCompareTokensTouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (JsonText != null) {
                JsonText.Dispose ();
                JsonText = null;
            }
        }
    }
}