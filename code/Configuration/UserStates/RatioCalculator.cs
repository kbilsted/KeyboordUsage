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

		public double CalculateProductiveRatio()
		{
			double sum = Records[KeyClass.Productive] + Records[KeyClass.Destructive];
			if (sum == 0d)
				return 0;

			var ratio = Records[KeyClass.Productive] / sum;

			return ToTwoDidigts(ratio * 100d);
		}

		public double CalculateNavigationRatio()
		{
			double sum = Records[KeyClass.Productive] + Records[KeyClass.Destructive] + Records[KeyClass.Navigation];
			if (sum == 0d)
				return 0;

			var ratio = Records[KeyClass.Navigation] / sum;

			return ToTwoDidigts(ratio * 100d);
		}

		double ToTwoDidigts(double d)
		{
			return Math.Round(d*100d) / 100d;
		}
	}
}