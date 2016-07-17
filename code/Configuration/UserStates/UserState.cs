using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyboordUsage.Configuration.UserStates
{
	public interface IUserState
	{
		RecodingSession GetAccumulated();
		RecodingSession GetCurrentSession();
		void Clear();
		void NewSession();
		KeyClassConfiguration GetKeyClasses();
		GuiConfiguration GetGuiConfiguration();
	}


	public class UserState : IUserState
	{
		/// <summary>must be public for json serialization</summary>
		public string ConfigurationVersion;

		/// <summary>must be public for json serialization</summary>
		public RecodingSession accumulated;

		/// <summary>must be public for json serialization</summary>
		public readonly List<RecodingSession> pastSessions;

		public readonly GuiConfiguration GuiConfiguration;

		public readonly KeyClassConfiguration KeyClasses;

		RecodingSession currentSession;

		/// <summary>
		/// called from json
		/// </summary>
		public UserState(string configurationVersion, RecodingSession accumulated, List<RecodingSession> pastSessions, GuiConfiguration guiConfiguration, KeyClassConfiguration keyclasses)
		{
			ConfigurationVersion = configurationVersion;

			if (keyclasses == null)
				keyclasses = UserStateStandardConfiguraion.CreateStdKeyClassConfiguration();
			KeyClasses = keyclasses;

			if (accumulated == null)
				accumulated = new RecodingSession(DateTime.Now, new Dictionary<Keys, int>(), new RatioCalculator());
			this.accumulated = accumulated;

			if (pastSessions == null)
				pastSessions = new List<RecodingSession>();
			this.pastSessions = pastSessions;

			if (guiConfiguration == null)
				guiConfiguration = UserStateStandardConfiguraion.CreateGuiConfiguration();
			GuiConfiguration = guiConfiguration;

			NewSession();
		}

		public RecodingSession GetAccumulated()
		{
			return accumulated;
		}

		public RecodingSession GetCurrentSession()
		{
			return currentSession;
		}

		public void Clear()
		{
			accumulated = new RecodingSession(DateTime.Now, new Dictionary<Keys, int>(), new RatioCalculator());

			pastSessions.Clear();

			NewSession();
		}

		public void NewSession()
		{
			currentSession = new RecodingSession(DateTime.Now, new Dictionary<Keys, int>(), new RatioCalculator());

			pastSessions.Add(currentSession);
		}

		public KeyClassConfiguration GetKeyClasses()
		{
			return KeyClasses;
		}

		public GuiConfiguration GetGuiConfiguration()
		{
			return GuiConfiguration;
		}
	}
}
