using Foundation;
using MonoTouch.Dialog;
using UIKit;


[Register ("AppDelegate")]
public class AppDelegate : UIApplicationDelegate {
	public override UIWindow Window {
		get;
		set;
	}

	public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
	{
		var navigation = new UINavigationController ();

		var menu = new RootElement ("Fluid Interfaces") {
			new Section ("Fluid Interfaces"){
				new StringElement ("Calculator Button", Calculator)
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