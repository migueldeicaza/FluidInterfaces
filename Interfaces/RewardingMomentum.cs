using System;
using UIKit;
using CoreGraphics;
using Foundation;

public class MomentumInterfaceViewController : InterfaceViewController {
	public MomentumInterfaceViewController () { }
	GradientView momentumView;
	bool isOpen;
	nfloat animationProgress = 0;
	CGAffineTransform closedTransform = CGAffineTransform.MakeIdentity ();
	UIViewPropertyAnimator animator = new UIViewPropertyAnimator ();

	public override void ViewDidLoad ()
	{
		base.ViewDidLoad ();
		momentumView = new GradientView (CGRect.Empty) {
			TranslatesAutoresizingMaskIntoConstraints = false,
			BackgroundColor = UIColor.FromWhiteAlpha (white: 0.3f, alpha: 1),
	    		TopColor = Color.FromHex (0x61A8FF),
			BottomColor = Color.FromHex (hex: 0x243BD1),
	    		CornerRadius = 30f,
		};
		View.AddSubview (momentumView);

		var handleView = new UIView () {
			TranslatesAutoresizingMaskIntoConstraints = false,
			BackgroundColor = UIColor.FromWhiteAlpha (white: 1, alpha: 0.5f),
		};
		handleView.Layer.CornerRadius = 3;

		momentumView.LeadingAnchor.ConstraintEqualTo (View.LeadingAnchor).Active = true;
		momentumView.TrailingAnchor.ConstraintEqualTo (View.TrailingAnchor, constant: -8).Active = true;
		momentumView.BottomAnchor.ConstraintEqualTo (View.BottomAnchor, constant: 80).Active = true;
		momentumView.TopAnchor.ConstraintEqualTo (View.SafeAreaLayoutGuide.TopAnchor, constant: 80).Active = true;


		momentumView.AddSubview (handleView);
		handleView.TopAnchor.ConstraintEqualTo (momentumView.TopAnchor, constant: 10).Active = true;
		handleView.WidthAnchor.ConstraintEqualTo (50).Active = true;
		handleView.HeightAnchor.ConstraintEqualTo (5).Active = true;
		handleView.CenterXAnchor.ConstraintEqualTo (momentumView.CenterXAnchor).Active = true;

		var panRecognizer = new InstantPanGestureRecognizer ();

		closedTransform = CGAffineTransform.MakeTranslation (0, View.Bounds.Height * 0.6f);
		momentumView.Transform = closedTransform;

		panRecognizer.AddTarget (Panned);
		momentumView.AddGestureRecognizer (panRecognizer);
	}

	void Panned (object r)
	{
		var recognizer = r as UIPanGestureRecognizer;
		switch (recognizer.State) {
		case UIGestureRecognizerState.Began:
			StartAnimationIfNeeded ();
			animator.PauseAnimation ();
			animationProgress = animator.FractionComplete;
			break;
		case UIGestureRecognizerState.Changed:
			var fraction = -recognizer.TranslationInView (momentumView).Y / closedTransform.y0;
			if (isOpen)
				fraction *= -1;

			if (animator.Reversed)
				fraction *= -1;

			animator.FractionComplete = fraction + animationProgress;
			break;
		case UIGestureRecognizerState.Ended:
		case UIGestureRecognizerState.Cancelled:
			var yVelocity = recognizer.VelocityInView (momentumView).Y;
			var shouldClose = yVelocity > 0; // todo: should use projection instead

			if (yVelocity == 0) {
				animator.ContinueAnimation (parameters: null, durationFactor: 0);
				break;
			}
			if (isOpen) {
				if (!shouldClose && !animator.Reversed) animator.Reversed = !animator.Reversed;
				if (shouldClose && animator.Reversed) animator.Reversed = !animator.Reversed;
			} else {
				if (shouldClose && !animator.Reversed) animator.Reversed = !animator.Reversed;
				if (!shouldClose && animator.Reversed) animator.Reversed = !animator.Reversed;
			}
			var fractionRemaining = 1 - animator.FractionComplete;

	    		var distanceRemaining = fractionRemaining * closedTransform.y0;

			if (distanceRemaining == 0) {
				animator.ContinueAnimation (null, 0);
				break;
			}

			var relativeVelocity = NMath.Min (NMath.Abs (yVelocity) / distanceRemaining, 30);
			var timingParameters = UISpringTiming.MakeTimingParameters (damping: 0.8f, response: 0.3f, initialVelocity: new CGVector (dx: relativeVelocity, dy: relativeVelocity));
			var preferredDuration = new UIViewPropertyAnimator (duration: 0, parameters: timingParameters).Duration;
			var durationFactor = preferredDuration / animator.Duration;
			animator.ContinueAnimation (parameters: timingParameters, durationFactor: (nfloat) durationFactor);
			break;
		default:
			break;
		}
	}

	private void StartAnimationIfNeeded ()
	{
		if (animator.Running)
			return;

		var timingParameters = UISpringTiming.MakeTimingParameters (damping: 1, response: 0.4f);
		animator = new UIViewPropertyAnimator (0, timingParameters);
		animator.AddAnimations (() => {
			momentumView.Transform = isOpen ? closedTransform : CGAffineTransform.MakeIdentity ();
		});
		animator.AddCompletion ((position) => {
			if (position == UIViewAnimatingPosition.End) {
				isOpen = !isOpen;
			}
		});
		
		animator.StartAnimation ();
	}
}

class InstantPanGestureRecognizer : UIPanGestureRecognizer {
	public override void TouchesBegan (NSSet touches, UIEvent evt)
	{
		base.TouchesBegan (touches, evt);
		State = UIGestureRecognizerState.Began;
	}
}