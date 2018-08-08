using System;
using UIKit;

public class InterfaceViewController : UIViewController {
	public InterfaceViewController ()
	{		
	}

	public override void ViewDidLoad ()
	{
		base.ViewDidLoad ();

		// reduces screen tearing on iPhone X
		View.BackgroundColor = UIColor.FromWhiteAlpha (0.05f, 1);

		NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Never;
	}
}

