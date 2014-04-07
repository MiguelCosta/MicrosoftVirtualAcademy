using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Expenses.Model;
using Expenses.Model.Fakes;
using Expenses.ViewModel;
using Expenses.ViewModel.Fakes;
using System.Threading.Tasks;

namespace Expenses.LabTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        async public Task TestMethod1()
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




        }
    }
}
