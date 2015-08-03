using System;
using Foundation;
using UIKit;
using CoreGraphics;

namespace TestScrolling
{

	/// <summary>
	/// Helper class to scroll the view when the keyboard appears
	/// </summary>
	public class KeyboardViewScroller
	{

		CGSize keyboardSize;
		UITextField selectedField;

		NSObject keyboardShownObserver;
		NSObject keyboardHiddenObserver;
		NSObject keyboardChangedFrameObserver;

		/// <summary>
		/// Indicates whether the keyboard is currently visible to the user.
		/// </summary>
		public bool KeyboardVisible { get; private set; }

		/// <summary>
		/// The minimum gap to keep between keyboard and the current textfield
		/// </summary>
		public float Gap { get; set; }

		/// <summary>
		/// The scrollview to scroll automatically
		/// </summary>
		public UIScrollView ScrollView { get; set; }

		public event Action<CGSize> KeyboardDidShow;
		public event Action<CGSize> KeyboardDidHide;

		public KeyboardViewScroller(UIScrollView scrollView, float gap = 10.0f)
		{
			selectedField = null;
			Gap = gap;
			ScrollView = scrollView;
			KeyboardVisible = false;
		}

		void KeyboardNotificationWasShown(NSNotification notification)
		{
			keyboardSize = ((NSValue)notification.UserInfo.ObjectForKey (UIKeyboard.FrameEndUserInfoKey)).CGRectValue.Size;
			KeyboardVisible = true;

			if (KeyboardDidShow != null)
				KeyboardDidShow (keyboardSize);
		}

		void KeyboardNotificationWasHidden(NSNotification notification)
		{
			if (!KeyboardVisible)
				return;
			KeyboardVisible = false;

			if (KeyboardDidHide != null)
				KeyboardDidHide (keyboardSize);
		}

		void KeyboardNotificationChangeFrame(NSNotification notification)
		{
			keyboardSize = ((NSValue)notification.UserInfo.ObjectForKey (UIKeyboard.FrameEndUserInfoKey)).CGRectValue.Size;
			MoveScrollViewContent (true);
		}

		public void RegisterForKeyboardNotification()
		{
			keyboardShownObserver = NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.DidShowNotification, new Action<NSNotification> (KeyboardNotificationWasShown));
			keyboardHiddenObserver = NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.DidHideNotification, new Action<NSNotification> (KeyboardNotificationWasHidden));
			keyboardChangedFrameObserver = NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.DidChangeFrameNotification, new Action<NSNotification> (KeyboardNotificationChangeFrame));
		}

		public void UnregisterKeyboardNotifications()
		{
			NSNotificationCenter.DefaultCenter.RemoveObserver (keyboardShownObserver);
			NSNotificationCenter.DefaultCenter.RemoveObserver (keyboardHiddenObserver);
			NSNotificationCenter.DefaultCenter.RemoveObserver (keyboardChangedFrameObserver);
		}

		public void SetSelectedField(UITextField field)
		{
			selectedField = field;
			if (KeyboardVisible)
				MoveScrollViewContent (true);
		}

		public void Handle()
		{
			KeyboardDidShow += DoHandle;
			KeyboardDidHide += DoHandle;
		}

		void DoHandle(CGSize size)
		{
			if (KeyboardVisible)
				MoveScrollViewContent (true);
			else
				MoveScrollViewContent (false);
		}

		/// <summary>
		/// Moves the content of the scroll view up/down depending on the presence of the keyboard.
		/// </summary>
		void MoveScrollViewContent(bool moveUp)
		{
			nfloat delta = 0;

			if (selectedField != null) {
				// Gap between textfield and the keyboard based on the UIViewController's frame size
				var keyboardFieldGap = ScrollView.Superview.Frame.Height - keyboardSize.Height - selectedField.Frame.Bottom;

				if (keyboardFieldGap < Gap) {
					delta = Gap - keyboardFieldGap;
				}
			}

			if (moveUp)
				ScrollView.SetContentOffset (new CGPoint (0, delta), true);
			else
				ScrollView.SetContentOffset (new CGPoint (0, 0), true);
		}

	}

}

