// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

using Expenses.WindowsStore.Common;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Expenses.ExpenseRepository.WcfClient;
using Expenses.Model;
using Expenses.ViewModel;

// The Hub App template is documented at http://go.microsoft.com/fwlink/?LinkId=321221

namespace Expenses.WindowsStore
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        const string ExpensesServiceUrl = "http://YOURWEBSITE.azurewebsites.net/ExpenseService.svc";
        const string AzureActiveDirectoryClientID = "YOURCLIENTID";
        const string AzureActiveDirectoryDomainName = "YOURNAMESPACE.onmicrosoft.com";
        const string AzureActiveDirectoryRedirectUri = "http://YOURWEBSITE.azurewebsites.net";
        const string AzureActiveDirectoryResource = "http://YOURWEBSITE.azurewebsites.net";

        static public Frame RootFrame = Window.Current.Content as Frame;

        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            WinRtServiceAuthorizer winRtServiceAuthorizer =
                new WinRtServiceAuthorizer()
                {
                    ClientID = App.AzureActiveDirectoryClientID,
                    DomainName = App.AzureActiveDirectoryDomainName,
                    RedirectUri = App.AzureActiveDirectoryRedirectUri,
                    Resource = App.AzureActiveDirectoryResource,
                };

            ServiceLocator.Current.SetService<WinRtServiceAuthorizer>(winRtServiceAuthorizer);
            ServiceLocator.Current.SetService<IExpenseRepository>(new WcfClientExpenseRepository(App.ExpensesServiceUrl, winRtServiceAuthorizer));
            ServiceLocator.Current.SetService<IViewService>(new ViewService());
            ServiceLocator.Current.SetService<INavigationService>(new NavigationService());
            ServiceLocator.Current.SetService<EmployeeViewModel>(new EmployeeViewModel());

            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
//#if DEBUG
//            if (System.Diagnostics.Debugger.IsAttached)
//            {
//                this.DebugSettings.EnableFrameRateCounter = true;
//            }
//#endif

            App.RootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active

            if (App.RootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                App.RootFrame = new Frame();
                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(App.RootFrame, "AppFrame");
                // Set the default language
                App.RootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                App.RootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }

                // Place the frame in the current Window
                Window.Current.Content = App.RootFrame;
            }
            if (App.RootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                App.RootFrame.Navigate(typeof(HubPage), e.Arguments);
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}
