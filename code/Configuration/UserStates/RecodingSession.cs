using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyboordUsage.Configuration.UserStates
{
	public class RecodingSession
	{
		public DateTime FromTime;
		public readonly Dictionary<Keys, int> Records;
		public readonly RatioCalculator Ratios;

		public RecodingSession(DateTime fromTime, Dictionary<Keys, int> records, RatioCalculator ratios)
		{
			FromTime = fromTime;
			Records = records;
			Ratios = ratios;
		}

		public void Add(Keys key, KeyClassConfiguration keyClasses)
		{
			if (Records.ContainsKey(key))
				Records[key] += 1;
			else
				Records[key] = 1;

			Ratios.Add(key, keyClasses);
		}
	}
}
