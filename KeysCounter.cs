using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace KeyboordUsage
{
	public class KeysCounter
	{
		public Tuple<DateTime, DateTime> Interval;
		public Dictionary<Keys, int> records = new Dictionary<Keys, int>();

		private KeysCounter()
		{
			Interval = new Tuple<DateTime, DateTime>(DateTime.Now, DateTime.Now);
		}

		public KeysCounter(Tuple<DateTime,DateTime> interval)
		{
			this.Interval = interval;
		}

		private Keys previous;

		public void Add(Keys key)
		{
			if (records.ContainsKey(key))
			{
				records[key] += 1;
			}
			else
			{
				records[key] = 1;
			}
		}

		public void Clear()
		{
			records.Clear();
		}

		public IEnumerable<KeyValuePair<Keys, int>> GetTopxx()
		{
			return records.OrderByDescending(x => x.Value);
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		public static KeysCounter ReadJson(string json)
		{
			return JsonConvert.DeserializeObject<KeysCounter>(json);
		}
	}
}	