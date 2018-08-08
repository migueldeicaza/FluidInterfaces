using System;
using CoreGraphics;
using UIKit;
using Foundation;
using System.Linq;

public class SliderView : UIView {
	void HandleEventHandler (object sender, EventArgs e)
	{
	}


	UILabel titleLabel = new UILabel (CGRect.Empty) {
		TranslatesAutoresizingMaskIntoConstraints = false,
		TextColor = UIColor.White,
		Font = UIFont.SystemFontOfSize (16, UIFontWeight.Bold)
	};
	UILabel valueLabel = new UILabel (CGRect.Empty) {
		TranslatesAutoresizingMaskIntoConstraints = false,
		Font = UIFont.SystemFontOfSize (16, UIFontWeight.Bold),
		TextColor = UIColor.White,
		Text = "0"
	};
	UISlider slider;

	string title;
	public string Title {
		get => title;
		set {
			title = value;
			titleLabel.Text = title;
		}
	}

	nfloat value;
	public nfloat Value {
		get => value;
		set {
			this.value = value;
			slider.Value = (float)value;
			valueLabel.Text = $"{value:f2}";
		}
	}

	float minValue = 0, maxValue = 1;
	public float MinValue {
		get => minValue;
		set {
			minValue = value;
			slider.MinValue = value;
		}
	}

	public float MaxValue {
		get => maxValue;
		set {
			maxValue = value;
			slider.MaxValue = value;
		}
	}

	public Action<float> SliderMoved;
	public Action SliderFinishedMoving;

	void SharedInit ()
	{
		AddSubview (titleLabel);
		titleLabel.LeadingAnchor.ConstraintEqualTo (LeadingAnchor).Active = true;
		titleLabel.TopAnchor.ConstraintEqualTo (TopAnchor).Active = true;

		AddSubview (valueLabel);
		valueLabel.TrailingAnchor.ConstraintEqualTo (TrailingAnchor).Active = true;
		valueLabel.LastBaselineAnchor.ConstraintEqualTo (LastBaselineAnchor).Active = true;

		AddSubview (slider);
		slider.LeadingAnchor.ConstraintEqualTo (LeadingAnchor).Active = true;
		slider.TrailingAnchor.ConstraintEqualTo (TrailingAnchor).Active = true;
		slider.BottomAnchor.ConstraintEqualTo (BottomAnchor).Active = true;
		slider.TopAnchor.ConstraintEqualTo (TopAnchor, 20).Active = true;

		slider.AddTarget (this, new ObjCRuntime.Selector ("slider:event:"), UIControlEvent.ValueChanged);
	}

	public SliderView (CGRect rect) : base (rect) => SharedInit ();
	public SliderView (NSCoder aCoder) : base (aCoder) => SharedInit ();

	[Export ("slider:event:")]
	void SliderMovedEvent (UISlider senderSlider, UIEvent uievent)
	{
		valueLabel.Text = $"{senderSlider.Value:f2}";
		if (SliderMoved != null)
			SliderMoved (senderSlider.Value);

		if ((uievent.AllTouches?.FirstOrDefault<NSObject> () as UITouch)?.Phase == UITouchPhase.Ended)
			if (SliderFinishedMoving != null)
				SliderFinishedMoving ();

	}
}
