using System;
using CoreGraphics;
using UIKit;

public class SpringInterfaceViewController : InterfaceViewController {
	nfloat dampingRatio = 0.5f, frequencyResponse = 1, margin = 30;
	GradientView springView;
	SliderView dampingSliderView, frequencySliderView;
	UIViewPropertyAnimator animator = new UIViewPropertyAnimator ();

	public SpringInterfaceViewController ()
	{
		springView = new GradientView (CGRect.Empty) {
			TranslatesAutoresizingMaskIntoConstraints = false,
			TopColor = new UIColor (.39f, .8f, .96f, 1),
			BottomColor = new UIColor (.20f, .61f, .92f, 1)
		};
		dampingSliderView = new SliderView (CGRect.Empty) {
			TranslatesAutoresizingMaskIntoConstraints = false,
			Title = "DAMPING (BOUNCINESS)",
			MinValue = 0.1f,
			MaxValue = 1,
			Value = dampingRatio,
			SliderMoved = (x) => dampingRatio = x,
			SliderFinishedMoving = () => ResetAnimation ()
		};

		frequencySliderView = new SliderView (CGRect.Empty) {
			TranslatesAutoresizingMaskIntoConstraints = false,
			Title = "RESPONSE (SPEED)",
			MinValue = 0.1f,
			MaxValue = 2f,
			Value = frequencyResponse,
			SliderMoved = (x) => frequencyResponse = x,
			SliderFinishedMoving = () => ResetAnimation ()
		};
	}

	void AnchorEqual (NSLayoutXAxisAnchor target, NSLayoutXAxisAnchor host, nfloat margin) => target.ConstraintEqualTo (host, margin).Active = true;
	void AnchorEqual (NSLayoutYAxisAnchor target, NSLayoutYAxisAnchor host, nfloat margin) => target.ConstraintEqualTo (host, margin).Active = true;

	public override void ViewDidLoad ()
	{
		base.ViewDidLoad ();
		View.AddSubview (dampingSliderView);
		AnchorEqual (dampingSliderView.LeadingAnchor, View.LeadingAnchor, margin);
		AnchorEqual (dampingSliderView.TrailingAnchor, View.TrailingAnchor, -margin);
		AnchorEqual (dampingSliderView.CenterYAnchor, View.CenterYAnchor, 0);

		View.AddSubview (frequencySliderView);
		AnchorEqual (frequencySliderView.LeadingAnchor, View.LeadingAnchor, margin);
		AnchorEqual (frequencySliderView.TrailingAnchor, View.TrailingAnchor, -margin);
		AnchorEqual (frequencySliderView.CenterYAnchor, View.CenterYAnchor, 0);

		View.AddSubview (springView);
		springView.HeightAnchor.ConstraintEqualTo (80).Active = true;
		springView.WidthAnchor.ConstraintEqualTo (80).Active = true;
		AnchorEqual (springView.LeadingAnchor, View.LeadingAnchor, margin);
		AnchorEqual (springView.BottomAnchor, View.BottomAnchor, -80);
	}

	void AnimateView ()
	{
		var timingParameters = UISpringTiming.MakeTimingParameters (damping: dampingRatio, response: frequencyResponse);
		animator = new UIViewPropertyAnimator (duration: 0, parameters: timingParameters);
		animator.AddAnimations (() => {
			var translation = View.Bounds.Width - 2 * margin - 80;

			springView.Transform = CGAffineTransform.MakeTranslation (translation, 0);
		});
		animator.AddCompletion ((x) => {
			springView.Transform = CGAffineTransform.MakeIdentity ();
			AnimateView ();
		});
		animator.StartAnimation ();
	}

	void ResetAnimation ()
	{
		animator.StopAnimation (true);
		springView.Transform = CGAffineTransform.MakeIdentity ();
		AnimateView ();
	}
}