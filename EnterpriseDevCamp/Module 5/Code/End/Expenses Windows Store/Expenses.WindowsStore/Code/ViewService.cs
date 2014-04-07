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

using Expenses.Model;
using Expenses.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Popups;

namespace Expenses.WindowsStore
{
    public class ViewService : ViewModelBase, IViewService
    {
        private bool _isBusy;

        public event EventHandler<EventArgs<bool>> BusyChanged;

        public void ShowBusy(bool isBusy)
        {
            if (this._isBusy == isBusy) { return; }

            this._isBusy = isBusy;

            EventHandler<EventArgs<bool>> handler = this.BusyChanged;
            if (handler != null)
            {
                handler(this, new EventArgs<bool>(this._isBusy));
            }
        }

        async public void ShowError(string message)
        {
            MessageDialog dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }

        async public Task<bool> ConfirmAsync(string title, string message)
        {
            bool result = false;

            MessageDialog dialog = new MessageDialog(message, title);
            dialog.Commands.Add(new UICommand("OK", (_) => { result = true; }));
            dialog.Commands.Add(new UICommand("Cancel", (_) => { result = false; }));
            
            await dialog.ShowAsync();

            return result;
        }

        async Task IViewService.ExecuteBusyActionAsync(Func<Task> func)
        {
            this.ShowBusy(true);
            try
            {
                await func();
            }
            catch (Exception e)
            {
                this.ShowError(e.ToString());
            }
            finally
            {
                this.ShowBusy(false);
            }
        }
    }
}
