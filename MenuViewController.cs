using Foundation;
using System;
using UIKit;

namespace FluidInterfaces {
	//
	// This class is references from the Main.storyboard
	//
	public partial class MenuViewController : UIViewController {
		public MenuViewController (IntPtr handle) : base (handle)
		{
		}

		public MenuViewController ()
		{
			Delegate = this;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// reduces screen tearing on iPhone X
			View.BackgroundColor = UIColor.FromWhiteAlpha (0.05f, 1);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			foreach (var cell in collectionView.VisibleCells)
				cell.Highlighted = true;

		}

		class CollectionViewDelegate : UICollectionViewDelegate {
			[Weak] MenuViewController parent;
			public CollectionViewDelegate (MenuViewController parent) => this.parent = parent;

			public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
			{
				parent.collectionView.CellForItem (indexPath).Highlighted = true;

			}
		}
	}
}