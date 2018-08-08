using System;
using CoreGraphics;
using UIKit;
using Foundation;
using System.Linq;
using CoreAnimation;

public class GradientView : UIView {
	UIColor topColor = UIColor.White, bottomColor = UIColor.Black;
	nfloat? cornerRadius;
	CAGradientLayer gradientLayer;

	public UIColor TopColor {
		get => topColor;
		set {
			topColor = value;
			UpdateGradientColors ();
		}
	}

	public UIColor BottomColor {
		get => bottomColor;
		set {
			bottomColor = value;
			UpdateGradientColors ();
		}
	}

	/// The corner radius of the view.
	/// If no value is provided, the default is 20% of the view's width.
	public nfloat? CornerRadius {
		get => cornerRadius;
		set {
			cornerRadius = value;
			LayoutSubviews ();
		}
	}

	public GradientView (CGRect rect) : base (rect) => Init ();
	public GradientView (NSCoder aCoder) : base (aCoder) => Init ();

	void Init ()
	{
		gradientLayer = new CAGradientLayer () {
			Colors = new [] { topColor.CGColor, bottomColor.CGColor },
			StartPoint = new CGPoint (0, 0),
			EndPoint = new CGPoint (0, 1)
		};
		Layer.AddSublayer (gradientLayer);
	}

	public override void LayoutSubviews ()
	{
		base.LayoutSubviews ();

		gradientLayer.Frame = Bounds;

		var maskLayer = new CAShapeLayer () {
			Path = UIBezierPath.FromRoundedRect (Bounds, cornerRadius ?? Bounds.Width * 0.2f).CGPath
		};
		Layer.Mask = maskLayer;
	}

	void UpdateGradientColors ()
	{
		gradientLayer.Colors = new [] { topColor.CGColor, bottomColor.CGColor };
	}
}