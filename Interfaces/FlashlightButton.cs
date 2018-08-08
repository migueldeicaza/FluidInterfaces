using System;
using CoreGraphics;
using UIKit;
using Foundation;
using System.Linq;

class FlashlightButtonInterfaceViewController : InterfaceViewController {
	public override void ViewDidLoad ()
	{
		base.ViewDidLoad ();
		var flashlightButton = new FlashlightButton ();
		View.AddSubview (flashlightButton);
		flashlightButton.Center (View);
	}
}

class FlashlightButton : UIControl {
	enum ForceState {
		// The button is ready to be activiated. Default state.
		Reset,
		// The button has been pressed with enough force.
		Activated,
		// The button has recently switched on/off.
		Confirmed
	}
	
	UIImage onImage, offImage;
	UIImageView imageView;
	bool isOn = false;
	ForceState forceState = ForceState.Reset;
	bool touchExited = false;
	UIImpactFeedbackGenerator activationFeedbackGenerator, confirmationFeedbackGenerator;
	nfloat minWidth = 50;
	nfloat maxWidth = 92;
	nfloat activationForce = 0.5f;
	nfloat confirmationForce = 0.49f;
	nfloat resetForce = 0.4f;
	UIColor offColor = UIColor.FromWhiteAlpha (0.2f, 1);
	UIColor onColor = UIColor.FromWhiteAlpha (0.95f, 1);

	public FlashlightButton (CGRect frame) : base (frame) => Init ();
	public FlashlightButton () : base (CGRect.Empty) => Init ();
	public FlashlightButton (NSCoder aCoder) : base (aCoder) => Init ();

	void Init ()
	{
		onImage = UIImage.FromBundle ("flaslight_on");
		offImage = UIImage.FromBundle ("flaslight_off");
		BackgroundColor = offColor;

		imageView = new UIImageView () {
			Image = offImage,
			TintColor = UIColor.White
		};
		AddSubview (imageView);
		imageView.Pin (this);
	}

	public override CGSize IntrinsicContentSize => new CGSize (minWidth, minWidth);

	public override void LayoutSubviews ()
	{
		base.LayoutSubviews ();
		Layer.CornerRadius = Bounds.Width / 2;
	}

	void TouchMoved (UITouch touch)
	{
		if (touch == null)
			return;
		if (touchExited)
			return;
		var cancelDistance = minWidth / 2 + 20;
		if (touch.LocationInView (this).Distance (new CGPoint (Bounds.GetMidX (), Bounds.GetMidY ())) < cancelDistance) {
			// the touch has moved outside of the bounds of the button
			touchExited = true;
			forceState = ForceState.Reset;
			AnimateToRest ();
			return;
		}
		var force = touch.Force / touch.MaximumPossibleForce;
		var scale = 1 + (maxWidth / minWidth - 1) * force;

		// update the button's size and color
		Transform = CGAffineTransform.MakeScale (scale, scale);
		if (!isOn) {
			BackgroundColor = UIColor.FromWhiteAlpha (0.2f - force * 0.2f, 1);
		}
		switch (forceState) {
		case ForceState.Reset:
			if (force > activationForce) {
				forceState = ForceState.Activated;
				activationFeedbackGenerator.ImpactOccurred ();
			}
			break;
		case ForceState.Activated:
			if (force <= confirmationForce) {
				forceState = ForceState.Confirmed;
				Activate ();
			}
			break;
		case ForceState.Confirmed:
			if (force <= resetForce)
				forceState = ForceState.Reset;
			
			break;
		}
	}

	void TouchEnded (UITouch touch)
	{
		if (touch == null)
			return;
		if (touchExited)
			return;
		forceState = ForceState.Reset;
		AnimateToRest ();
	}

	void Activate ()
	{
		isOn = !isOn;
		imageView.Image = isOn ? onImage : offImage;
		imageView.TintColor = isOn ? UIColor.Black : UIColor.White;
		BackgroundColor = isOn ? onColor : offColor;
		confirmationFeedbackGenerator.ImpactOccurred ();
	}

	void AnimateToRest ()
	{
		var timingParameters = UISpringTiming.MakeTimingParameters (damping: 0.4f, response: 0.2f);
		var animator = new UIViewPropertyAnimator (duration: 0, parameters: timingParameters);
		animator.AddAnimations (() => {
			Transform = CGAffineTransform.MakeScale (1, 1);
			BackgroundColor = isOn ? onColor : offColor;
		});
		animator.Interruptible = true;
		animator.StartAnimation ();
	}

	public override void TouchesBegan (NSSet touches, UIEvent evt)
	{
		base.TouchesBegan (touches, evt);
		touchExited = false;
		TouchMoved (touches.FirstOrDefault () as UITouch);
	}

	public override void TouchesMoved (NSSet touches, UIEvent evt)
	{
		base.TouchesMoved (touches, evt);
		TouchMoved (touches.FirstOrDefault () as UITouch);
	}

	public override void TouchesEnded (NSSet touches, UIEvent evt)
	{
		base.TouchesEnded (touches, evt);
		TouchEnded (touches.FirstOrDefault () as UITouch);
	}

	public override void TouchesCancelled (NSSet touches, UIEvent evt)
	{
		base.TouchesCancelled (touches, evt);
		TouchEnded (touches.FirstOrDefault () as UITouch);
	}
}