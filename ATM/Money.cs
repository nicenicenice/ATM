namespace ATM
{
    using System;
    using System.Text;

    public class Money
    {
        private int amount;
        private int faceValue;

        public int Amount
        {
            get { return amount; }
        }

        public int FaceValue
        {
            get { return faceValue; }
        }

        /// <summary>Constructs a money from the given amount and
		/// face value.</summary>
        public Money(int amount, int faceValue)
        {
            this.amount = amount;
            this.faceValue = faceValue;
        }

        public bool isZero
        {
            get { return this.Amount <= 0; }
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;
         
            if (isZero)
                return ((Money)obj).isZero;

            Money money = (Money)obj;
            return money.FaceValue == this.FaceValue 
                && money.Amount == this.Amount;
        }

        public override int GetHashCode()
        {
            return FaceValue.GetHashCode() + Amount;
        }

        public override String ToString()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append("[" + Amount + " : " + FaceValue + "]");
            return buffer.ToString();
        }
    }
}