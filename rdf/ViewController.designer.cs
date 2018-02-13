// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace rdf
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSPopUpButton objectMenu { get; set; }

		[Outlet]
		AppKit.NSPopUpButton predicateMenu { get; set; }

		[Outlet]
		AppKit.NSPopUpButton subjectMenu { get; set; }

		[Outlet]
		AppKit.NSTextField textBox { get; set; }

		[Action ("showAll:")]
		partial void showAll (Foundation.NSObject sender);

		[Action ("showInstances:")]
		partial void showInstances (Foundation.NSObject sender);

		[Action ("showObjects:")]
		partial void showObjects (Foundation.NSObject sender);

		[Action ("showProperties:")]
		partial void showProperties (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (textBox != null) {
				textBox.Dispose ();
				textBox = null;
			}

			if (subjectMenu != null) {
				subjectMenu.Dispose ();
				subjectMenu = null;
			}

			if (objectMenu != null) {
				objectMenu.Dispose ();
				objectMenu = null;
			}

			if (predicateMenu != null) {
				predicateMenu.Dispose ();
				predicateMenu = null;
			}
		}
	}
}
