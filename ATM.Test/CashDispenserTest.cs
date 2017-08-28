namespace ATM.Test
{
    using System;
    using System.Linq;
    using System.Collections;
    using NUnit.Framework;

    [TestFixture]
    class CashDispenserTest
    {
        private Money _10by1;
        private Money _8by7;
        private Money _5by10;
        private Money _2by50;

        private CashDispenser disp1;
        private CashDispenser disp2;

        [SetUp]
        public void SetUp()
        {
            // arrange
            _10by1 = new Money(10, 1);
            _8by7 = new Money(8, 7);
            _5by10 = new Money(5, 10);
            _2by50 = new Money(2, 50);

            disp1 = new CashDispenser();
            disp2 = new CashDispenser();
        }

        [Test]
        public void TestEqualsMoney()
        {
            // [2 : 50] == [2 : 50]

            Money expected = new Money(2, 50);

            Assert.IsTrue(_2by50.Equals(expected));
            Assert.IsTrue(_2by50.Equals(_2by50));
            Assert.IsFalse(_2by50.Equals(_5by10));
            Assert.IsFalse(_2by50.Equals(null));
        }

        [Test]
        public void TestIsZeroMoney()
        {
            Money zeroMoney = new Money(0, 50);

            Assert.IsFalse(_8by7.isZero);
            Assert.IsTrue(zeroMoney.isZero);
        }

        [Test]
        public void TestHashCode()
        {
            Money expected = new Money(2, 50);

            Assert.AreEqual(_2by50.GetHashCode(), expected.GetHashCode());
        }

        [Test]
        public void TestPrint()
        {
            Money expected = new Money(2, 50);

            Assert.AreEqual(_2by50.ToString(), "[2 : 50]");
        }

        [Test]
        public void TestValidInitialFaceValues()
        {
            // sorted, unique and positive: 1, 5, 10, 50

            // arrange
            int[] factFaceValues = disp1.GetFaceValues();
            int[] checkFaceValues = new int[factFaceValues.Length];
            bool isPositive = true;

            // act
            for (int i = 0; i < factFaceValues.Length; i++)
                if (factFaceValues[i] <= 0)
                    isPositive = false;

            Array.Copy(factFaceValues, checkFaceValues, factFaceValues.Length);
            Array.Sort(checkFaceValues.Distinct().ToArray());

            // assert
            Assert.AreEqual(checkFaceValues, factFaceValues);
            Assert.IsTrue(isPositive);
        }

        [Test]
        public void TestValidSetFaceValues()
        {
            // arrange
            int[] faceValue = { 18, 6, 9, -4 };
            int[] checkFaceValue = {6, 9, 18 };

            // act
            disp2.SetFaceValues(faceValue);
            
            // assert
            Assert.AreEqual(checkFaceValue, disp2.GetFaceValues());
        }

        [Test]
        public void TestWithdrawCorrectFaceVals()
        {
            // arrange
            int[] faceValue = { 1, 5, 10, 50};
            ArrayList checkResult = new ArrayList();
            disp1.SetFaceValues(faceValue);

            checkResult.Add(new Money(1, 10));
            checkResult.Add(new Money(5, 1));
            checkResult.Add(new Money(1, 5));

            // act
            disp1.CreateBundle(20);
            ArrayList result = disp1.GetMoneyBundle();
            
            // assert
            Assert.That(result, Is.EquivalentTo(checkResult));
        }

        [Test]
        public void TestWithdrawNotCorrectFaceVals()
        {
            // arrange
            // 0, 10, 2, -4 => 2, 10
            // 0, 0, -1 => empty

            int[] faceValues1 = { 0, 10, 2, -4 };
            int[] faceValues2  = { 0, 0, -1 };
            ArrayList checkResult = new ArrayList();
            disp1.SetFaceValues(faceValues1);
            disp2.SetFaceValues(faceValues2);

            checkResult.Add(new Money(1, 10));
            checkResult.Add(new Money(5, 2));

            // act
            disp1.CreateBundle(20);
            disp2.CreateBundle(10);
            ArrayList result1 = disp1.GetMoneyBundle();
            ArrayList result2 = disp2.GetMoneyBundle();

            // assert
            Assert.That(result1, Is.EquivalentTo(checkResult));
            Assert.AreEqual(result2.Count, 0);
        }

        [Test]
        public void TestWithdrawZeroMoney()
        {
            // act
            disp1.CreateBundle(0);
            ArrayList result = disp1.GetMoneyBundle();

            // assert
            Assert.AreEqual(result.Count, 0);
        }
    }
}
