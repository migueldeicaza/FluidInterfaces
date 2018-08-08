using System;
using UIKit;
using CoreGraphics;
using Foundation;

public class CalculatorButton : UIControl {
	UILabel label = new UILabel () {
		Font = UIFont.SystemFontOfSize (36, UIFontWeight.Regular),
		TextColor = UIColor.White,
		TextAlignment = UITextAlignment.Center
	};

	UIViewPropertyAnimator animator = new UIViewPropertyAnimator ();
	UIColor normalColor = new UIColor (0.2f, 0.2f, 0.2f, 1);
	UIColor highlightedColor = new UIColor (0.45f, 0.45f, 0.45f, 1);

	int value;
	public int Value {
		get => value;
		set {
			this.value = value;
			label.Text = $"{value}";
		}
	}

	void CalculatorButton_TouchDown (object sender, EventArgs e)
	{
		animator.StopAnimation (true);
		BackgroundColor = highlightedColor;
	}

	void CalculatorButton_TouchUp (object sender, EventArgs e)
	{
		animator = new UIViewPropertyAnimator (
			duration: 0.5,
			curve: UIViewAnimationCurve.EaseOut,
			animations: () => { BackgroundColor = normalColor; });
		animator.StartAnimation ();
	}

	void SharedInit ()
	{
		BackgroundColor = normalColor;
		AddTarget (CalculatorButton_TouchDown, UIControlEvent.TouchDown | UIControlEvent.TouchDragEnter);
		AddTarget (CalculatorButton_TouchUp, UIControlEvent.TouchUpInside | UIControlEvent.TouchDragExit | UIControlEvent.TouchCancel);
		AddSubview (label);
		label.Center (this);
	}

	public CalculatorButton (CGRect frame) : base (frame) => SharedInit ();
	public CalculatorButton (NSCoder aCoder) : base (aCoder) => SharedInit ();

	public override CGSize IntrinsicContentSize => new CGSize (75, 75);

	public override void LayoutSubviews ()
	{
		base.LayoutSubviews ();
		Layer.CornerRadius = Bounds.Width / 2;
	}
}

public class CalculatorButtonInterfaceViewController : InterfaceViewController {
	public CalculatorButtonInterfaceViewController () { }

	public override void ViewDidLoad ()
	{
		base.ViewDidLoad ();
		var calculatorButton = new CalculatorButton (CGRect.Empty) {
			Value = 9
		};
		View.AddSubview (calculatorButton);
		calculatorButton.Center (inView: View);
	}
}

