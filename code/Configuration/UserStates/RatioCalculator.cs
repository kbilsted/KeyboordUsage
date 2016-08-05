using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyboordUsage.Configuration.UserStates
{
	public class RatioCalculator
	{
		public Dictionary<KeyClass, int> Records = new Dictionary<KeyClass, int>();

		/// <summary> for json deserialize/// </summary>
		public RatioCalculator(Dictionary<KeyClass, int> records)
		{
			Records = records;
		}

		public RatioCalculator()
		{
			Records[KeyClass.Productive] = 0;
			Records[KeyClass.Destructive] = 0;
			Records[KeyClass.Navigation] = 0;
			Records[KeyClass.Meta] = 0;
		}

		public void Add(Keys key, KeyClassConfiguration keyClasses)
		{
			var keyClass = keyClasses.GetKeyClass(key);
			Records[keyClass] += 1;
		}

		private double Calc(KeyClass selector)
		{
			double sum = Records[KeyClass.Productive] + Records[KeyClass.Destructive] + Records[KeyClass.Navigation] + Records[KeyClass.Meta];
			if (sum == 0d)
				return 0;

			var ratio = Records[selector] / sum;

			return ToTwoDidigts(ratio * 100d);
		}

		public double CalculateProductiveRatio()
		{
			return Calc(KeyClass.Productive);
		}

		public double CalculateDestructiveRatio()
		{
			return Calc(KeyClass.Destructive);
		}

		public double CalculateNavigationRatio()
		{
			return Calc(KeyClass.Navigation);
		}

		public double CalculateMetaRatio()
		{
			return Calc(KeyClass.Meta);
		}

		double ToTwoDidigts(double d)
		{
			return Math.Round(d*100d) / 100d;
		}
	}
}