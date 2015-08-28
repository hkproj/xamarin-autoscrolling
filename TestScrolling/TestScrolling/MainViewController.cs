using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TestScrolling
{
	partial class MainViewController : UIViewController
	{

		KeyboardViewScroller kbScroller;
		
		public MainViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var fields = new [] {
				nameField, // tab index = 0
				emailField, // tab index = 1
				passwordField // tab index = 2
			};

			// Initialize scrolling mechanism
			kbScroller = new KeyboardViewScroller (scrollView);
			kbScroller.Handle ();

			// Automatically handle navigation between text fields
			fields.InitializeNavigationOnFields(kbScroller);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			kbScroller.RegisterForKeyboardNotification ();
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			kbScroller.UnregisterKeyboardNotifications ();
		}

		partial void SendData (UIButton sender)
		{
			var alert = UIAlertController.Create("LOL", "I am the most useless button in the whole Git-sphera", UIAlertControllerStyle.Alert);
			alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, null));

			PresentViewController(alert, true, null);
		}

	}
}
