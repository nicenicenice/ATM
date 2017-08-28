namespace ATM
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Linq;

    public class CashDispenser
    {

        public class ImpossibleToWithdrawAllFundsException : ApplicationException
        {
        }

        private ArrayList moneyBundle;
        private int[] faceValues = { 1, 5, 10, 50 };

        public CashDispenser()
        {
            moneyBundle = new ArrayList();
            MakeValidFaceValues();
        }

        public void SetFaceValues(int[] listFaceValues)
        {
            if (listFaceValues.Length == 0)
                return;

            faceValues = new int[listFaceValues.Length];

            for (int i = 0; i < listFaceValues.Length; i++)
            {
                if (listFaceValues[i] > 0)
                    faceValues[i] = listFaceValues[i];
            }

            MakeValidFaceValues();
        }

        public int[] GetFaceValues()
        {
            return faceValues;
        }

        private void MakeValidFaceValues()
        {
            var positiveNumbers = faceValues.Where(val => val > 0);
            faceValues = positiveNumbers.Distinct().ToArray();
            Array.Sort(faceValues);
        }

        private void AddMoneyToBundle(Money added)
        {
            if (added.isZero)
                return;

            Money foundMoney = FindMoney(added.FaceValue);
            if (foundMoney == null)
            {
                moneyBundle.Add(added);
                return;
            }

            moneyBundle.Remove(foundMoney);

            int resultAmount = foundMoney.Amount + added.Amount;
            moneyBundle.Add(new Money(resultAmount, added.FaceValue));
        }

        private Money FindMoney(int faceValue)
        {
            foreach (Money money in moneyBundle)
                if (money.FaceValue == faceValue)
                    return money;
            return null;
        }

        private bool IsSuccessTransaction(int totalSum, int minFaceValue)
        {
            bool success = false;
            try
            {
                if (minFaceValue != 0 && totalSum % minFaceValue != 0)
                    throw new ImpossibleToWithdrawAllFundsException();
                return true;
            }
            catch (ImpossibleToWithdrawAllFundsException e)
            {
                while (true)
                {
                    Console.WriteLine("Impossible to withdraw all of funds using these face values.");
                    Console.WriteLine("Still withdraw money? (yes/no)");

                    String answer = Console.ReadLine().ToLower();
                    if ("yes".Equals(answer)) {
                        success = true;
                        break;
                    } else if ("no".Equals(answer)) {
                        success = false;
                        break;
                    }
                }
                return success;
            }
        }

        public void CreateBundle(int totalSum)
        {
            if (totalSum <= 0 || faceValues.Length <= 0)
                return;

            moneyBundle.Clear();
            int minFaceValue = faceValues[0];
            int remainingTotalSumm = 0;

            for (int i = 0; i < faceValues.Length; i++)
            {
                remainingTotalSumm = totalSum - faceValues[i];
                if (remainingTotalSumm < minFaceValue)
                    break;

                totalSum = remainingTotalSumm;
                AddMoneyToBundle(new Money(1, faceValues[i]));
            }

            int numCurrentFaceValue = 0;

            for (int i = faceValues.Length - 1; i >= 0 ; i--)
            {
                numCurrentFaceValue = totalSum / faceValues[i];
                if (numCurrentFaceValue < 1)
                    continue;
                AddMoneyToBundle(new Money(numCurrentFaceValue, faceValues[i]));

                totalSum = totalSum % faceValues[i];

                if (totalSum <= 0)
                    break;
            }

            if (!IsSuccessTransaction(totalSum, minFaceValue))
            {
                moneyBundle.Clear();
                return;
            }
        }

        public ArrayList GetMoneyBundle()
        {
            return moneyBundle;
        }

        public void GiveMonies(int totalSum)
        {
            CreateBundle(totalSum);
            PrintCash();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            foreach (Money money in moneyBundle)
                builder.Append(money);
            builder.Append("}");
            return builder.ToString();
        }

        private void PrintCash()
        {
            if (moneyBundle.Count == 0)
            {
                Console.WriteLine("No money to withdraw!");
                return;
            }
                
            foreach (Money money in moneyBundle)
                Console.WriteLine(money.Amount + " pieces for " + money.FaceValue);
        }
    }
}