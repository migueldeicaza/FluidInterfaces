using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.Collections.Generic;

public class AccelerationViewController : InterfaceViewController {
	UILabel pauseLabel;
	GradientView accelerationView;
	UIPanGestureRecognizer panRecognizer = new UIPanGestureRecognizer ();
	UIImpactFeedbackGenerator feedbackGenerator = new UIImpactFeedbackGenerator (UIImpactFeedbackStyle.Light);
	nfloat verticalOffset = 180;

	public override void ViewDidLoad ()
	{
		base.ViewDidLoad ();

		pauseLabel = new UILabel (CGRect.Empty) {
			Text = "Paused",
			Font = UIFont.BoldSystemFontOfSize (16),
	    		TextColor = UIColor.White,
			TextAlignment = UITextAlignment.Center,
	    		TranslatesAutoresizingMaskIntoConstraints = true,
			Alpha = 0
		};

		accelerationView = new GradientView (CGRect.Empty) {
			TopColor = Color.FromHex (0x64ff8f),
	    		BottomColor = Color.FromHex (0x51ffea)
		};

		View.AddSubview (pauseLabel);
		pauseLabel.CenterXAnchor.ConstraintEqualTo (View.CenterXAnchor).Active = true;
		pauseLabel.TopAnchor.ConstraintEqualTo (View.SafeAreaLayoutGuide.TopAnchor, 40);

		View.AddSubview (accelerationView);
		accelerationView.Center (View, new UIOffset (0, verticalOffset));
		accelerationView.WidthAnchor.ConstraintEqualTo (160).Active = true;
		accelerationView.HeightAnchor.ConstraintEqualTo (160).Active = true;

		panRecognizer.AddTarget (Panned);
		accelerationView.AddGestureRecognizer (panRecognizer);
	}

	CGPoint originalTouchPoint = CGPoint.Empty;

	void Panned (NSObject r)
	{
		var  recognizer = r as UIPanGestureRecognizer;
		var touchPoint = recognizer.LocationInView (View);
		var velocity = recognizer.VelocityInView (View);
		switch (recognizer.State) {
		case UIGestureRecognizerState.Began:
			originalTouchPoint = touchPoint;
			break;
		case UIGestureRecognizerState.Changed:
			var offset = touchPoint.Y - originalTouchPoint.Y;
			if (offset > 0)
				offset = NMath.Pow (offset, 0.7f);
			else if (offset < -verticalOffset * 2) {
				offset = -verticalOffset * 2 - NMath.Pow (-(offset + verticalOffset * 2f), 0.7f);
			} 
			accelerationView.Transform = CGAffineTransform.MakeTranslation (0, offset);
			TrackPause (velocity.Y, offset);
			break;
		case UIGestureRecognizerState.Ended:
		case UIGestureRecognizerState.Cancelled:
			var timingParameters = UISpringTiming.MakeTimingParameters (damping: 0.8f, response: 0.3f);
			var animator = new UIViewPropertyAnimator (0, timingParameters);
			animator.AddAnimations (() => {
				accelerationView.Transform = CGAffineTransform.MakeIdentity ();
				pauseLabel.Alpha = 0;
			});
			animator.Interruptible = true;
			animator.StartAnimation ();
			hasPaused = false;
			break;
		default:
			break;
		}
	}

	/// The number of past velocities to track.
	int numberOfVelocities = 7;

	/// The array of past velocities.
	List<nfloat> velocities = new List<nfloat> ();

	/// Whether the view is considered paused
	bool hasPaused;
	void TrackPause (nfloat velocity, nfloat offset)
	{
		if (hasPaused)
			return;

		// update the array of most recent velocities

		if (velocities.Count >= numberOfVelocities) 
			velocities.RemoveAt (0);
		velocities.Add (velocity);
		// enforce minimum velocity and offset
		if (NMath.Abs (velocity) > 100 || Math.Abs (offset) < 50)
			return;
		var firstRecorded = velocities [0];
		// if the majority of the velocity has been lost recetly, we consider the motion to be paused

		if (NMath.Abs (firstRecorded - velocity) / NMath.Abs (firstRecorded) > 0.9) {
			pauseLabel.Alpha = 1;
			feedbackGenerator.ImpactOccurred ();
			hasPaused = true;
			velocities.Clear ();
		}
	}
}