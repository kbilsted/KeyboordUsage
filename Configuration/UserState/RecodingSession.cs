using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyboordUsage.Configuration.UserState
{
	public class RecodingSession
	{
		public DateTime UpToTime;
		public Dictionary<Keys, int> Records;

		public RecodingSession(DateTime upToTime, Dictionary<Keys, int> records)
		{
			UpToTime = upToTime;
			Records = records;
		}

		public void Add(Keys key)

{
			if (Records.ContainsKey(key))
			{
				Records[key] += 1;
			}
			else
			{
				Records[key] = 1;
			}

		}

	}
}
