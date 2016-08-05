using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using KeyboordUsage.Configuration.UserStates;

namespace KeyboordUsage
{
	public class KeysCounter
	{
		private readonly IUserState state;

		public KeysCounter(IUserState state)
		{
			this.state = state;
		}

		public void Add(Keys key)
		{
			var keyClassConfiguration = state.GetKeyClasses();

			state.GetAccumulated().Add(key, keyClassConfiguration);
			state.GetCurrentSession().Add(key, keyClassConfiguration);
		}

		public Dictionary<Keys, int> GetRecords()
		{
			return state.GetAccumulated().Records;
		}

		public void Clear()
		{
			state.Clear();
		}

		public IEnumerable<KeyValuePair<Keys, int>> GetAccumulatedKeyPopularity()
		{
			return GetRecords().OrderByDescending(x => x.Value);
		}

		public double GetProductiveRatio()
		{
			return state.GetAccumulated().Ratios.CalculateProductiveRatio();
		}

		public double GetNavigationRatio()
		{
			return state.GetAccumulated().Ratios.CalculateNavigationRatio();
		}

		public double GetDestructiveRatio()
		{
			return state.GetAccumulated().Ratios.CalculateDestructiveRatio();
		}

		public double GetMetaRatio()
		{
			return state.GetAccumulated().Ratios.CalculateMetaRatio();
		}
	}
}	
