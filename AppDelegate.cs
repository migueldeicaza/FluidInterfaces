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

	void Push (UIViewController uivc) => navigation.PushViewController (uivc, true);

	public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
	{
		navigation = new UINavigationController ();

		var menu = new RootElement ("Fluid Interfaces") {
			new Section ("Fluid Interfaces"){
				new StringElement ("Calculator Button", () => Push (new CalculatorButtonInterfaceViewController ())),
				new StringElement ("Spring animations", () => Push (new SpringInterfaceViewController ())),
				new StringElement ("Flashlight button", () => Push (new FlashlightButtonInterfaceViewController ())),
				new StringElement ("Rubberbanding", () => Push (new RubberbandingViewController ())),
				new StringElement ("Acceleration pausing", () => Push (new AccelerationViewController ())),
				// new StringElement ("Rewarding momentum", () => Push (new  ())),
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