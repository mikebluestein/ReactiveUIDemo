// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace RXUIDemo
{
	[Register ("SearchResultView")]
	partial class SearchResultView
	{
		[Outlet]
		UIKit.UILabel description { get; set; }

		[Outlet]
		UIKit.UILabel title { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (title != null) {
				title.Dispose ();
				title = null;
			}

			if (description != null) {
				description.Dispose ();
				description = null;
			}
		}
	}
}
