using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using ReactiveUI;

namespace RXUIDemo
{
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        UIWindow window;

        SearchViewController viewController;
        //ClassicSearchViewController viewController;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        { 
            window = new UIWindow(UIScreen.MainScreen.Bounds);

            viewController = new SearchViewController();
            //viewController = new ClassicSearchViewController ();

            window.RootViewController = viewController;

            window.MakeKeyAndVisible();

            return true;
        }
    }
}