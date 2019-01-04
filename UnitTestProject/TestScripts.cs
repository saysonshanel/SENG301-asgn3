using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Frontend2.Hardware;
using Frontend2;


namespace UnitTestProject
{
    [TestClass]
    public class TestScripts
    {
        int[] coinKinds;
        int selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity;
        VendingMachine vm;
        VendingMachineStoredContents teardown;
        List<IDeliverable> extraction;

        /// Test method for T01-good-insert-and-press-exact-change
        [TestMethod]
        public void InsertWithExactChange()
        {

            coinKinds = new int[] { 5, 10, 25, 100 };
            selectionButtonCount = 3;
            coinRackCapacity = 10;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            List<string> name = new List<string>() { "Coke", "water", "stuff" };
            List<int> cost = new List<int>() { 250, 250, 205 };
            int[] coinCounts = new int[] { 1, 1, 2, 0 };
            int[] popCanCounts = new int[] { 1, 1, 1 };

            vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            new VendingMachineLogic(vm);

            vm.Configure(name, cost);
            vm.LoadCoins(coinCounts);
            vm.LoadPopCans(popCanCounts);
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(25));
            vm.CoinSlot.AddCoin(new Coin(25));
            vm.SelectionButtons[0].Press();

            var items = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items);

            Assert.AreEqual(true, CheckDelivery(0, new List<PopCan>() { new PopCan("Coke") }));

            teardown = UnloadVendingMachine(vm);

            Assert.AreEqual(true, CheckTeardown(315, 0, new List<PopCan>() { new PopCan("water"), new PopCan("stuff") }));

        }

        /// Test method for T02-good-insert-and-press-change-expected
        [TestMethod]
        public void InsertWithExpectedChange()
        {

            coinKinds = new int[] { 5, 10, 25, 100 };
            selectionButtonCount = 3;
            coinRackCapacity = 10;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            List<string> name = new List<string>() { "Coke", "water", "stuff" };
            List<int> cost = new List<int>() { 250, 250, 205 };
            int[] coinCounts = new int[] { 1, 1, 2, 0 };
            int[] popCanCounts = new int[] { 1, 1, 1 };

            vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            new VendingMachineLogic(vm);

            vm.Configure(name, cost);
            vm.LoadCoins(coinCounts);
            vm.LoadPopCans(popCanCounts);
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.SelectionButtons[0].Press();

            var items = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items);

            Assert.AreEqual(true, CheckDelivery(50, new List<PopCan>() { new PopCan("Coke") }));

            teardown = UnloadVendingMachine(vm);

            Assert.AreEqual(true, CheckTeardown(315, 0, new List<PopCan>() { new PopCan("water"), new PopCan("stuff") }));

        }

        /// Test method for T03-good-teardown-without-configure-or-load
        [TestMethod]
        public void TeardownWithoutConfigureLoad()
        {

            coinKinds = new int[] { 5, 10, 25, 100 };
            selectionButtonCount = 3;
            coinRackCapacity = 10;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            new VendingMachineLogic(vm);

            var items = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items);

            Assert.AreEqual(true, CheckDelivery(0, new List<PopCan>()));

            teardown = UnloadVendingMachine(vm);

            Assert.AreEqual(true, CheckTeardown(0, 0, new List<PopCan>()));
        }

        /// Test method for T04-good-press-without-insert
        [TestMethod]
        public void PressWithoutInsert()
        {

            coinKinds = new int[] { 5, 10, 25, 100 };
            selectionButtonCount = 3;
            coinRackCapacity = 10;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            List<string> name = new List<string>() { "Coke", "water", "stuff" };
            List<int> cost = new List<int>() { 250, 250, 205 };
            int[] coinCounts = new int[] { 1, 1, 2, 0 };
            int[] popCanCounts = new int[] { 1, 1, 1 };

            vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            new VendingMachineLogic(vm);

            vm.Configure(name, cost);
            vm.LoadCoins(coinCounts);
            vm.LoadPopCans(popCanCounts);
            vm.SelectionButtons[0].Press();

            var items = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items);

            Assert.AreEqual(true, CheckDelivery(0, new List<PopCan>()));

            teardown = UnloadVendingMachine(vm);

            Assert.AreEqual(true, CheckTeardown(65, 0, new List<PopCan>() { new PopCan("Coke"), new PopCan("water"), new PopCan("stuff") }));
        }

        /// Test method for T05-good-scrambled-coin-kinds
        [TestMethod]
        public void ScrambledCoinKinds()
        {

            coinKinds = new int[] { 100, 5, 25, 10 };
            selectionButtonCount = 3;
            coinRackCapacity = 2;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            List<string> name = new List<string>() { "Coke", "water", "stuff" };
            List<int> cost = new List<int>() { 250, 250, 205 };
            int[] coinCounts = new int[] { 0, 1, 2, 1 };
            int[] popCanCounts = new int[] { 1, 1, 1 };

            vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            new VendingMachineLogic(vm);

            vm.Configure(name, cost);
            vm.LoadCoins(coinCounts);
            vm.LoadPopCans(popCanCounts);
            vm.SelectionButtons[0].Press();

            var items = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items);

            Assert.AreEqual(true, CheckDelivery(0, new List<PopCan>()));

            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));

            vm.SelectionButtons[0].Press();

            var items1 = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items1);

            Assert.AreEqual(true, CheckDelivery(50, new List<PopCan>() { new PopCan("Coke") }));

            teardown = UnloadVendingMachine(vm);

            Assert.AreEqual(true, CheckTeardown(215, 100, new List<PopCan>() { new PopCan("water"), new PopCan("stuff") }));

        }

        /// Test method for T06-good-extract-before-sale
        [TestMethod]
        public void ExtractBeforeSale()
        {

            coinKinds = new int[] { 100, 5, 25, 10 };
            selectionButtonCount = 3;
            coinRackCapacity = 10;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            List<string> name = new List<string>() { "Coke", "water", "stuff" };
            List<int> cost = new List<int>() { 250, 250, 205 };
            int[] coinCounts = new int[] { 0, 1, 2, 1 };
            int[] popCanCounts = new int[] { 1, 1, 1 };

            vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            new VendingMachineLogic(vm);

            vm.Configure(name, cost);
            vm.LoadCoins(coinCounts);
            vm.LoadPopCans(popCanCounts);
            vm.SelectionButtons[0].Press();

            var items = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items);

            Assert.AreEqual(true, CheckDelivery(0, new List<PopCan>()));

            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));

            var items1 = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items1);

            Assert.AreEqual(true, CheckDelivery(0, new List<PopCan>()));

            teardown = UnloadVendingMachine(vm);

            Assert.AreEqual(true, CheckTeardown(65, 0, new List<PopCan>() { new PopCan("Coke"), new PopCan("water"), new PopCan("stuff") }));

        }

        /// Test method for T07-good-changing-configuration
        [TestMethod]
        public void ChangingConfig()
        {
            coinKinds = new int[] { 5, 10, 25, 100 };
            selectionButtonCount = 3;
            coinRackCapacity = 10;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            List<string> name1 = new List<string>() { "A", "B", "C" };
            List<int> cost1 = new List<int>() { 5, 10, 25 };
            List<string> name2 = new List<string>() { "Coke", "water", "stuff" };
            List<int> cost2 = new List<int>() { 250, 250, 205 };
            int[] coinCounts = new int[] { 1, 1, 2, 0 };
            int[] popCanCounts = new int[] { 1, 1, 1 };

            vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            new VendingMachineLogic(vm);

            vm.Configure(name1, cost1);
            vm.LoadCoins(coinCounts);
            vm.LoadPopCans(popCanCounts);
            vm.Configure(name2, cost2);
            vm.SelectionButtons[0].Press();

            var items1 = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items1);

            Assert.AreEqual(true, CheckDelivery(0, new List<PopCan>()));

            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.SelectionButtons[0].Press();

            var items2 = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items2);

            Assert.AreEqual(true, CheckDelivery(50, new List<PopCan>() { new PopCan("A") }));

            teardown = UnloadVendingMachine(vm);

            Assert.AreEqual(true, CheckTeardown(315, 0, new List<PopCan>() { new PopCan("B"), new PopCan("C") }));

            vm.LoadCoins(coinCounts);
            vm.LoadPopCans(popCanCounts);
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.SelectionButtons[0].Press();

            var items3 = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items3);

            Assert.AreEqual(true, CheckDelivery(50, new List<PopCan>() { new PopCan("Coke") }));

            teardown = UnloadVendingMachine(vm);

            Assert.AreEqual(true, CheckTeardown(315, 0, new List<PopCan>() { new PopCan("water"), new PopCan("stuff") }));
        }

        /// Test method for T08-good-approximate-change
        [TestMethod]
        public void ApproximateChange()
        {
            coinKinds = new int[] { 5, 10, 25, 100 };
            selectionButtonCount = 1;
            coinRackCapacity = 10;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            List<string> name = new List<string>() { "stuff" };
            List<int> cost = new List<int>() { 140 };
            int[] coinCounts = new int[] { 0, 5, 1, 1 };
            int[] popCanCounts = new int[] { 1 };

            vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            new VendingMachineLogic(vm);

            vm.Configure(name, cost);
            vm.LoadCoins(coinCounts);
            vm.LoadPopCans(popCanCounts);
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.SelectionButtons[0].Press();

            var items = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items);

            Assert.AreEqual(true, CheckDelivery(155, new List<PopCan>() { new PopCan("stuff") }));

            teardown = UnloadVendingMachine(vm);

            Assert.AreEqual(true, CheckTeardown(320, 0, new List<PopCan>()));
        }

        /// Test method for T09-good-hard-for-change
        [TestMethod]
        public void OverLoadChange()
        {
            coinKinds = new int[] { 5, 10, 25, 100 };
            selectionButtonCount = 1;
            coinRackCapacity = 10;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            List<string> name = new List<string>() { "stuff" };
            List<int> cost = new List<int>() { 140 };
            int[] coinCounts = new int[] { 1, 6, 1, 1 };
            int[] popCanCounts = new int[] { 1 };

            vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            new VendingMachineLogic(vm);

            vm.Configure(name, cost);
            vm.LoadCoins(coinCounts);
            vm.LoadPopCans(popCanCounts);
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.SelectionButtons[0].Press();

            var items = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items);

            Assert.AreEqual(true, CheckDelivery(160, new List<PopCan>() { new PopCan("stuff") }));

            teardown = UnloadVendingMachine(vm);

            Assert.AreEqual(true, CheckTeardown(330, 0, new List<PopCan>()));

        }

        /// Test method for T10-good-invalid-coin
        [TestMethod]
        public void InvalidCoin()
        {
            coinKinds = new int[] { 5, 10, 25, 100 };
            selectionButtonCount = 1;
            coinRackCapacity = 10;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            List<string> name = new List<string>() { "stuff" };
            List<int> cost = new List<int>() { 140 };
            int[] coinCounts = new int[] { 1, 6, 1, 1 };
            int[] popCanCounts = new int[] { 1 };

            vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            new VendingMachineLogic(vm);

            vm.Configure(name, cost);
            vm.LoadCoins(coinCounts);
            vm.LoadPopCans(popCanCounts);
            vm.CoinSlot.AddCoin(new Coin(1));
            vm.CoinSlot.AddCoin(new Coin(139));
            vm.SelectionButtons[0].Press();

            var items = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items);

            Assert.AreEqual(true, CheckDelivery(140, new List<PopCan>()));

            teardown = UnloadVendingMachine(vm);

            Assert.AreEqual(true, CheckTeardown(190, 0, new List<PopCan>() { new PopCan("stuff") }));
        }

        /// Test method for T11-good-extract-before-sale-complex
        [TestMethod]
        public void ComplexExtractionBeforeSale()
        {
            coinKinds = new int[] { 100, 5, 25, 10 };
            selectionButtonCount = 3;
            coinRackCapacity = 10;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            List<string> name = new List<string>() { "Coke", "water", "stuff" };
            List<int> cost = new List<int>() { 250, 250, 205 };
            int[] coinCounts = new int[] { 0, 1, 2, 1 };
            int[] popCanCounts = new int[] { 1, 1, 1 };

            vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            new VendingMachineLogic(vm);

            vm.Configure(name, cost);
            vm.LoadCoins(coinCounts);
            vm.LoadPopCans(popCanCounts);
            vm.SelectionButtons[0].Press();

            var items1 = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items1);

            Assert.AreEqual(true, CheckDelivery(0, new List<PopCan>()));

            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));

            var items2 = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items2);

            Assert.AreEqual(true, CheckDelivery(0, new List<PopCan>()));

            teardown = UnloadVendingMachine(vm);

            Assert.AreEqual(true, CheckTeardown(65, 0, new List<PopCan>() { new PopCan("Coke"), new PopCan("water"), new PopCan("stuff") }));

            vm.LoadCoins(coinCounts);
            vm.LoadPopCans(popCanCounts);
            vm.SelectionButtons[0].Press();

            var items3 = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items3);

            Assert.AreEqual(true, CheckDelivery(50, new List<PopCan>() { new PopCan("Coke") }));

            teardown = UnloadVendingMachine(vm);

            Assert.AreEqual(true, CheckTeardown(315, 0, new List<PopCan>() { new PopCan("water"), new PopCan("stuff") }));

            List<string> name1 = new List<string>() { "A", "B", "C" };
            List<int> cost1 = new List<int>() { 5, 10, 25 };

            VendingMachine vm1 = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            new VendingMachineLogic(vm1);
            vm1.Configure(name, cost);
            vm1.Configure(name1, cost1);

            teardown = UnloadVendingMachine(vm1);

            Assert.AreEqual(true, CheckTeardown(0, 0, new List<PopCan>()));

            vm1.LoadCoins(coinCounts);
            vm1.LoadPopCans(popCanCounts);

            vm1.CoinSlot.AddCoin(new Coin(10));
            vm1.CoinSlot.AddCoin(new Coin(5));
            vm1.CoinSlot.AddCoin(new Coin(10));

            vm1.SelectionButtons[2].Press();

            var items4 = vm1.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items4);

            Assert.AreEqual(true, CheckDelivery(0, new List<PopCan>() { new PopCan("C") }));

            teardown = UnloadVendingMachine(vm1);

            Assert.AreEqual(true, CheckTeardown(90, 0, new List<PopCan>() { new PopCan("A"), new PopCan("B") }));

        }

        /// Test method for T12-good-approximate-change-with-credit
        [TestMethod]
        public void ChangeWithCredit()
        {
            coinKinds = new int[] { 5, 10, 25, 100 };
            selectionButtonCount = 1;
            coinRackCapacity = 10;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            List<string> name = new List<string>() { "stuff" };
            List<int> cost = new List<int>() { 140 };
            int[] coinCounts = new int[] { 0, 5, 1, 1 };
            int[] popCanCounts = new int[] { 1 };

            vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            new VendingMachineLogic(vm);
            vm.Configure(name, cost);
            vm.LoadCoins(coinCounts);
            vm.LoadPopCans(popCanCounts);
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));

            vm.SelectionButtons[0].Press();

            var items1 = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items1);

            Assert.AreEqual(true, CheckDelivery(155, new List<PopCan>() { new PopCan("stuff") }));

            teardown = UnloadVendingMachine(vm);

            Assert.AreEqual(true, CheckTeardown(320, 0, new List<PopCan>()));

            int[] coinCounts1 = new int[] { 10, 10, 10, 10 };

            vm.LoadCoins(coinCounts1);
            vm.LoadPopCans(popCanCounts);
            vm.CoinSlot.AddCoin(new Coin(25));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(10));
            vm.SelectionButtons[0].Press();

            var items2 = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items2);

            Assert.AreEqual(true, CheckDelivery(0, new List<PopCan>() { new PopCan("stuff") }));

            teardown = UnloadVendingMachine(vm);

            Assert.AreEqual(true, CheckTeardown(1400, 135, new List<PopCan>()));

        }

        /// Test method for T13-good-need-to-store-payment
        [TestMethod]
        public void OverloadStorePayment()
        {
            coinKinds = new int[] { 5, 10, 25, 100 };
            selectionButtonCount = 1;
            coinRackCapacity = 10;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            List<string> name = new List<string>() { "stuff" };
            List<int> cost = new List<int>() { 135 };
            int[] coinCounts = new int[] { 10, 10, 10, 10 };
            int[] popCanCounts = new int[] { 1 };

            vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            new VendingMachineLogic(vm);

            vm.Configure(name, cost);
            vm.LoadCoins(coinCounts);
            vm.LoadPopCans(popCanCounts);

            vm.CoinSlot.AddCoin(new Coin(25));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(10));
            vm.SelectionButtons[0].Press();

            var items = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items);

            Assert.AreEqual(true, CheckDelivery(0, new List<PopCan>() { new PopCan("stuff") }));

            teardown = UnloadVendingMachine(vm);

            Assert.AreEqual(true, CheckTeardown(1400, 135, new List<PopCan>()));
        }

        /// Test method for U01-bad-configure-before-construct
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CongifureThenConstructFail()
        {
            coinKinds = new int[] { 5, 10, 25, 100 };
            selectionButtonCount = 3;
            coinRackCapacity = 10;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            List<string> name = new List<string>() { "Coke", "water", "stuff" };
            List<int> cost = new List<int>() { 250, 250, 205 };
            int[] coinCounts = new int[] { 1, 1, 2, 0 };
            int[] popCanCounts = new int[] { 1, 1, 1 };
            var pop1 = new PopCan("Coke");
            var pop2 = new PopCan("water");
            var pop3 = new PopCan("stuff");

            vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            new VendingMachineLogic(null);
            vm.Configure(name, cost);
            vm.LoadCoins(coinCounts);
            vm.LoadPopCans(popCanCounts);

            // var items = vm.DeliveryChute.RemoveItems();
            // extraction = new List<IDeliverable>(items);

            teardown = UnloadVendingMachine(vm);

            CheckTeardown(65, 0, new List<PopCan>() { pop1, pop2, pop3 });
            Assert.Fail("Fail!");
        }

        /// Test method for U02-bad-cost-list
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void PopCostListFail()
        {
            coinKinds = new int[] { 5, 10, 25, 100 };
            selectionButtonCount = 3;
            coinRackCapacity = 10;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            List<string> name = new List<string>() { "Coke", "water", "stuff" };
            List<int> cost = new List<int>() { 250, 250, 0 };

            vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            vm.Configure(name, cost);   //fail here since "stuff" costs 0
            Assert.Fail("FAil!");
        }

        /// Test method for U03-bad-names-list
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void PopCanNamesListFail()
        {
            coinKinds = new int[] { 5, 10, 25, 100 };
            selectionButtonCount = 3;
            coinRackCapacity = 10;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            List<string> name = new List<string>() { "Coke", "water" };
            List<int> cost = new List<int>() { 250, 250 };

            VendingMachine vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            vm.Configure(name, cost);	//Should fail here since there is more selection button counts than pops
            Assert.Fail("Fail!");
        }

        /// Test method for U04-bad-non-unique-denomination
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void NonUniqueCoinKinds()
        {
            coinKinds = new int[] { 1, 1 };
            selectionButtonCount = 1;
            coinRackCapacity = 10;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            VendingMachine vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            Assert.Fail("Fail!"); //Fails since the coinKinds are non unique when creating vm
        }

        /// Test method for U05-bad-coin-kind
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CoinKindFail()
        {
            coinKinds = new int[] { 0 };
            selectionButtonCount = 1;
            coinRackCapacity = 10;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            VendingMachine vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            Assert.Fail("Fail!");	//Fails since coin cannot be the value of 0

        }
        /// Test method for U06-bad-button-number
        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void ButtonNumberFail()
        {
            //Assume the line CREATE(5, 10, 25, 100) is successful

            SelectionButton[] sb = new SelectionButton[3];
            sb[3].Press();
            //coinKinds = new int[] { 5, 10, 25, 100 };
            // selectionButtonCount = 3;
            // coinRackCapacity = 0;
            //  popCanRackCapacity = 0;
            //  receptacleCapacity = 0;

            //  VendingMachine vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            //  vm.SelectionButtons[3].Press();
            // Assert.Fail("Fail!");	//Fail since pressing 3 before any pop is loaded(meaning nothing is in there right now)
        }

        /// Test method for U07-bad-button-number-2
        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void NegativeButtonNumberFail()
        {
            //Assume the line CREATE(5, 10, 25, 100) is successful

            SelectionButton[] sb = new SelectionButton[3];
            sb[-1].Press();
            // coinKinds = new int[] { 5, 10, 25, 100 };
            // selectionButtonCount = 3;
            //  coinRackCapacity = 0;
            // popCanRackCapacity = 0;
            //  receptacleCapacity = 0;

            //  VendingMachine vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            //  vm.SelectionButtons[-1].Press();
            Assert.Fail("Fail!");	//Fail since pressing a non positive number
        }

        /// Test method for U08-bad-button-number-3
        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void GreaterThanButtonCountFail()
        {
            //Assume the line CREATE(5, 10, 25, 100) is successful
            // coinKinds = new int[] { 5, 10, 25, 100 };
            // selectionButtonCount = 3;
            //coinRackCapacity = 0;
            // popCanRackCapacity = 0;
            // receptacleCapacity = 0;
            SelectionButton[] sb = new SelectionButton[3];
            sb[4].Press();

            //  VendingMachine vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            // new VendingMachineLogic(vm);
            // vm.SelectionButtons[4].Press();
            Assert.Fail("Fail!");	//Fail since pressing a button that is non existent
        }

        /// Following methods used to help check delivery, teardown, and to help unload the vending machine

        private bool CheckDelivery(int totalCoinValue, List<PopCan> popsDelivered)
        {
            var result = true;
            foreach (var item in this.extraction)
            {
                if (item is Coin)
                {
                    totalCoinValue -= ((Coin)item).Value;
                }
                else if (item is PopCan)
                {
                    if (popsDelivered.Contains((PopCan)item))
                    {
                        popsDelivered.Remove((PopCan)item);
                    }
                    else
                    {
                        result = false;
                        break;
                    }
                }
            }
            if (!((totalCoinValue == 0) && (popsDelivered.Count == 0)))
            {
                result = false;
            }
            return result;
        }

        private bool CheckTeardown(int totalChangeRemaining, int totalCoinsUsed, List<PopCan> popsRemaining)
        {
            var result = true;
            var coinsInCoinRacks = this.teardown.CoinsInCoinRacks;
            var coinsUsedForPayment = this.teardown.PaymentCoinsInStorageBin;
            var unsoldPopCanRacks = this.teardown.PopCansInPopCanRacks;

            foreach (var rack in coinsInCoinRacks)
            {
                foreach (var coin in rack)
                {
                    totalChangeRemaining -= ((Coin)coin).Value;
                }
            }
            foreach (var coin in coinsUsedForPayment)
            {
                totalCoinsUsed -= ((Coin)coin).Value;
            }
            if (!((totalChangeRemaining == 0) && (totalCoinsUsed == 0)))
            {
                result = false;
            }
            else
            {
                foreach (var popCanRack in unsoldPopCanRacks)
                {
                    foreach (var popCan in popCanRack)
                    {
                        if (popsRemaining.Contains((PopCan)popCan))
                        {
                            popsRemaining.Remove((PopCan)popCan);
                        }
                        else
                        {
                            result = false;
                            break;
                        }
                    }
                }
                if (popsRemaining.Count > 0)
                {
                    result = false;
                }
            }
            return result;
        }

        public VendingMachineStoredContents UnloadVendingMachine(VendingMachine vm)
        {

            var storedContents = new VendingMachineStoredContents();

            foreach (var coinRack in vm.CoinRacks)
            {
                storedContents.CoinsInCoinRacks.Add(coinRack.Unload());
            }
            storedContents.PaymentCoinsInStorageBin.AddRange(vm.StorageBin.Unload());
            foreach (var popCanRack in vm.PopCanRacks)
            {
                storedContents.PopCansInPopCanRacks.Add(popCanRack.Unload());
            }

            return storedContents;
        }

        //BONUS TEST SCRIPT

        /// <summary>
        /// This test script tests to see if the customer will get their change back if the technician forgot to load the coins in the vending machine
        /// expected => no they will not get change, as the vending machine will eat it and wont give the pop
        /// actual => no change or pop is dispensed
        /// </summary>
        [TestMethod]
        public void TechFail()
        {

            coinKinds = new int[] { 5, 10, 25, 100 };
            selectionButtonCount = 3;
            coinRackCapacity = 10;
            popCanRackCapacity = 10;
            receptacleCapacity = 10;

            List<string> name = new List<string>() { "Coke", "water", "stuff" };
            List<int> cost = new List<int>() { 250, 250, 205 };
            int[] coinCounts = new int[] { 0, 0, 0, 0 };    //techy forgot to load the coins!
            int[] popCanCounts = new int[] { 1, 1, 1 };

            vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popCanRackCapacity, receptacleCapacity);
            vm.Configure(name, cost);
            vm.LoadCoins(coinCounts);
            vm.LoadPopCans(popCanCounts);

            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.SelectionButtons[0].Press();

            var items = vm.DeliveryChute.RemoveItems();
            extraction = new List<IDeliverable>(items);

            Assert.AreEqual(true, CheckDelivery(0, new List<PopCan>()));
        }
    }

}

