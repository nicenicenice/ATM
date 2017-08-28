using System;
using System.Collections;

namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            CashDispenser atm = new CashDispenser();
            atm.SetFaceValues(new int[] { 1, 5, 10, 50 });
            atm.GiveMonies(20);

            Console.ReadLine();
        }
    }
}
