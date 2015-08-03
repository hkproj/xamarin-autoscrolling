using System;
using System.Linq;
using Foundation;
using System.Collections.Generic;
using UIKit;

namespace TestScrolling
{
	public static class ExtensionMethods
	{

		/// <summary>
		/// Initializes navigation on a list of fields
		/// </summary>
		/// <param name="fields">the list of fields</param>
		/// <param name="setKeyboardReturnKeys">If set to <c>true</c> sets the correct return key on the keyboard based on the currently selected field.</param>
		public static void InitializeNavigationOnFields(this IEnumerable<UITextField> fields, KeyboardViewScroller viewScroller = null, bool setKeyboardReturnKeys = true)
		{
			var fieldsList = fields.ToList ();
			var navigator = new NavigationalTextFieldDelegate (fieldsList, viewScroller);

			for (int i = 0; i < fieldsList.Count; i++) {
				if (setKeyboardReturnKeys)
					fieldsList [i].ReturnKeyType = i == fieldsList.Count - 1 ? UIReturnKeyType.Done : UIReturnKeyType.Next;
				fieldsList [i].Delegate = navigator;
			}
		}

	}
}

