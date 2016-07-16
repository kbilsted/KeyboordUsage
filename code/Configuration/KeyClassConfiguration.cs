using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyboordUsage.Configuration
{
	public class KeyClassConfiguration
	{
		public List<string> ProductiveKeyData = new List<string>();
		public List<string> DestructiveKeyData = new List<string>();
		public List<string> NaviationKeyData = new List<string>();
		public List<string> MetaKeyData = new List<string>();

		private readonly Dictionary<string, KeyClass> resolution = new Dictionary<string, KeyClass>();

		public KeyClass GetKeyClass(Keys key)
		{
			string keydata = key.ToString();

			KeyClass result;
			if (resolution.TryGetValue(keydata, out result))
				return result;

			if (DestructiveKeyData.Contains(keydata))
				result = KeyClass.Destructive;
			else if (NaviationKeyData.Contains(keydata))
				result = KeyClass.Navigation;
			else if (MetaKeyData.Contains(keydata))
				result = KeyClass.Meta;
			else result = KeyClass.Productive;

			resolution.Add(keydata, result);

			return result;
		}
	}
}
