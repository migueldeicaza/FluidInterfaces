using System;
using Foundation;
using MonoTouch.Dialog;
using UIKit;


[Register ("AppDelegate")]
public class AppDelegate : UIApplicationDelegate {
	public override UIWindow Window {
		get;
		set;
	}
	UINavigationController navigation;

	void Push (string title, UIViewController uivc)
	{
		navigation.PushViewController(uivc, true);
		uivc.Title = title;
	}

	Element MakeButton (string title, UIViewController uvc)
	{
		return new StringElement (title, () => Push (title, uvc));
	}

	public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
	{
		Window = new UIWindow (UIScreen.MainScreen.Bounds);
		Window.MakeKeyAndVisible ();
		navigation = new UINavigationController ();

		var menu = new RootElement ("Fluid Interfaces") {
			new Section ("Fluid Interfaces"){
				MakeButton ("Calculator Button", new CalculatorButtonInterfaceViewController ()),
				MakeButton ("Spring animations", new SpringInterfaceViewController ()),
				MakeButton ("Flashlight button",  new FlashlightButtonInterfaceViewController ()),
				MakeButton ("Rubberbanding", new RubberbandingViewController ()),
				MakeButton ("Acceleration pausing", new AccelerationViewController ()),
				MakeButton ("Rewarding momentum",  new MomentumInterfaceViewController ()),
				// new StringElement ("Facetime PiP", () => Push (new  ())),
				// new StringElement ("Rotation", () => Push (new  ())),
			}
		};
		var dv = new DialogViewController (menu);
		navigation.PushViewController (dv, true);
		Window.RootViewController = navigation;
		return true;
	}

	// This is the main entry point of the application.
	static void Main (string [] args)
	{
		UIApplication.Main (args, null, "AppDelegate");
	}
}