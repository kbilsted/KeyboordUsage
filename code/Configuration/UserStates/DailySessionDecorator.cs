using System;

namespace KeyboordUsage.Configuration.UserStates
{
	public class DailySessionDecorator : IUserState
	{
		private readonly IUserState state;
		private int lastKeyStroke = DateTime.Now.Day;

		public DailySessionDecorator(IUserState state)
		{
			this.state = state;
		}

		public RecodingSession GetAccumulated()
		{
			return state.GetAccumulated();
		}

		public RecodingSession GetCurrentSession()
		{
			if (lastKeyStroke != DateTime.Now.Day)
				state.NewSession();

			lastKeyStroke = DateTime.Now.Day;

			return state.GetCurrentSession();
		}

		public void Clear()
		{
			state.Clear();
		}

		public void NewSession()
		{
			state.NewSession();
		}

		public KeyClassConfiguration GetKeyClasses()
		{
			return state.GetKeyClasses();
		}

		public GuiConfiguration GetGuiConfiguration()
		{
			return state.GetGuiConfiguration();
		}
	}
}
