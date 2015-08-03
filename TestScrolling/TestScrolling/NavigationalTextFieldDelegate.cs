using System;
using System.Linq;
using UIKit;
using System.Collections.Generic;

namespace TestScrolling
{
	/// <summary>
	/// Navigates between UITextFields when the "Done" key is tapped
	/// </summary>
	public class NavigationalTextFieldDelegate : UITextFieldDelegate
	{

		/// <summary>
		/// Indicates whether to use the tag property to identify the next control
		/// </summary>
		readonly bool useTagProperty;

		public List<UITextField> Fields { get; private set; }
		public KeyboardViewScroller ViewScroller { get; private set; }

		public NavigationalTextFieldDelegate(IEnumerable<UITextField> fields, KeyboardViewScroller viewScroller = null)
		{
			Fields = fields.ToList();
			useTagProperty = Fields.Count == 0;
			ViewScroller = viewScroller;
		}

		public override void EditingStarted (UITextField textField)
		{
			if (ViewScroller != null)
				ViewScroller.SetSelectedField (textField);
		}

		public override bool ShouldReturn (UITextField textField)
		{
			if (useTagProperty) {
				var nextTag = textField.Tag + 1;
				var nextResponder = textField.Superview.ViewWithTag (nextTag);

				// If there is a responder, set focus on it.
				if (nextResponder != null)
					nextResponder.BecomeFirstResponder ();
				else
					// There is no control to focus, so hide the keyboard.
					textField.ResignFirstResponder ();
			} else {
				var currentIndex = Fields.IndexOf (textField);
				if (currentIndex == Fields.Count - 1)
					// This is the last field, so hide the keyboard.
					textField.ResignFirstResponder ();
				else
					// Set focus to the next field.
					Fields [currentIndex + 1].BecomeFirstResponder ();
			}

			return false; // Don't insert new lines :)
		}

	}
}

