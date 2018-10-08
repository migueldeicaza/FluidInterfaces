using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.Collections.Generic;

public class RubberbandingViewController : InterfaceViewController {
	GradientView rubberView = new GradientView (CGRect.Empty) {
		TopColor = Color.FromHex (0xff5b50),
		BottomColor = Color.FromHex (0xffc950)
	};
	UIPanGestureRecognizer panRecognizer = new UIPanGestureRecognizer ();

	public override void ViewDidLoad ()
	{
		base.ViewDidLoad ();

		View.AddSubview (rubberView);
		rubberView.Center (View);
		rubberView.WidthAnchor.ConstraintEqualTo (140).Active = true;
		rubberView.HeightAnchor.ConstraintEqualTo (160).Active = true;

		panRecognizer.AddTarget (Panned);
		rubberView.AddGestureRecognizer (panRecognizer);
	}

	CGPoint originalTouchPoint = CGPoint.Empty;

	void Panned (NSObject r)
	{
		var recognizer = r as UIPanGestureRecognizer;
		var touchPoint = recognizer.LocationInView (View);
		switch (recognizer.State) {
		case UIGestureRecognizerState.Began:
			originalTouchPoint = touchPoint;
			break;
		case UIGestureRecognizerState.Changed:
			var offset = touchPoint.Y - originalTouchPoint.Y;
			offset = offset > 0 ? NMath.Pow (offset, 0.7f) : -NMath.Pow (-offset, 0.7f);
			rubberView.Transform = CGAffineTransform.MakeTranslation (0, offset);
			break;
		case UIGestureRecognizerState.Ended:
		case UIGestureRecognizerState.Cancelled:
			var timingParameters = UISpringTiming.MakeTimingParameters (damping: 0.6f, response: .3f);
			var animator = new UIViewPropertyAnimator (duration: 0, parameters: timingParameters);
			animator.AddAnimations (() => rubberView.Transform = CGAffineTransform.MakeIdentity ());
			animator.Interruptible = true;
			animator.StartAnimation ();
			break;
		default:
			break;
		}
	}
}