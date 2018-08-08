using System;
using UIKit;
using CoreGraphics;

public static class UIViewExtensions {

	public static void Center (this UIView view, UIView inView) => view.Center (inView, UIOffset.Zero);

	public static void Center (this UIView view, UIView inView, UIOffset offset)
	{
		view.TranslatesAutoresizingMaskIntoConstraints = false;
		NSLayoutConstraint.ActivateConstraints (new [] {
			view.CenterXAnchor.ConstraintEqualTo (inView.CenterXAnchor, offset.Horizontal),
			view.CenterYAnchor.ConstraintEqualTo (inView.CenterYAnchor, offset.Vertical)});

	}

	public static void Pin (this UIView view, UIView inView) => view.Pin (inView, UIEdgeInsets.Zero);

	public static void Pin (this UIView view, UIView inView, UIEdgeInsets insets)
	{
		view.TranslatesAutoresizingMaskIntoConstraints = false;
		NSLayoutConstraint.ActivateConstraints (new [] {
			view.TopAnchor.ConstraintEqualTo (inView.TopAnchor, insets.Top),
			view.LeftAnchor.ConstraintEqualTo (inView.LeftAnchor, insets.Left),
			view.BottomAnchor.ConstraintEqualTo (inView.BottomAnchor, -insets.Bottom),
			view.RightAnchor.ConstraintEqualTo (inView.RightAnchor, -insets.Right)
		});
	}


}
