using Foundation;
using UIKit;


[Register ("AppDelegate")]
public class AppDelegate : UIApplicationDelegate {
	public override UIWindow Window {
		get;
		set;
	}

	public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
	{
		return true;
	}

	// This is the main entry point of the application.
	static void Main (string [] args)
	{
		UIApplication.Main (args, null, "AppDelegate");
	}
}