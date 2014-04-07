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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Expenses.Model.Fakes;
using Expenses.Model;
using System.Threading.Tasks;
using Expenses.ViewModel.Fakes;

namespace Expenses.ViewModel.Tests
{
    [TestClass]
    public class ChargeViewModelTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            ServiceLocator.Current.SetService<INavigationService>(new StubINavigationService());

            StubIViewService viewService =
                new StubIViewService()
                {
                    ExecuteBusyActionAsyncFuncOfTask =
                        async (Func<Task> func) =>
                        {
                            await func();
                        }
                };
            ServiceLocator.Current.SetService<IViewService>(viewService);
        }

        [TestMethod]
        public async Task Load_Single()
        {
            StubIExpenseRepository repository =
                new StubIExpenseRepository()
                {
                    GetChargeAsyncInt32 =
                        (chargeId) =>
                        {
                            return Task.FromResult(
                                new Charge()
                                {
                                    ChargeId = chargeId,
                                });
                        }
                };

            ServiceLocator.Current.SetService<IExpenseRepository>(repository);

            ChargeViewModel chargeViewModel = new ChargeViewModel();
            Assert.AreEqual(0, chargeViewModel.ChargeId);
            await chargeViewModel.LoadAsync(1);
            Assert.AreEqual(1, chargeViewModel.ChargeId);
        }
    }
}
