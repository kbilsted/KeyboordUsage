using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace KeyboordUsage.Configuration.UserStates
{
	public class UserState
	{
		public RecodingSession accumulated;
		public List<RecodingSession> sessions;
		RecodingSession currentSession = new RecodingSession(DateTime.Now, new Dictionary<Keys, int>());
		public GuiConfiguration GuiConfiguration;

		/// <summary>
		/// called from json
		/// </summary>
		public UserState(RecodingSession accumulated, List<RecodingSession> sessions, GuiConfiguration guiConfiguration)
		{
			if (accumulated == null)
				accumulated = new RecodingSession(DateTime.Now, new Dictionary<Keys, int>());
			this.accumulated = accumulated;

			if (sessions == null)
				sessions = new List<RecodingSession>();
			this.sessions = sessions;

			if (guiConfiguration == null)
				guiConfiguration = CreateGuiConfiguration();
			this.GuiConfiguration = guiConfiguration;

			NewInstance();
		}

		public static GuiConfiguration CreateGuiConfiguration()
		{
			return new GuiConfiguration(10, 10, 1125, 550, 1);
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
			accumulated = new RecodingSession(DateTime.Now, new Dictionary<Keys, int>());
			sessions.Clear();
			NewInstance();
		}

		private void NewInstance()
		{
			currentSession = new RecodingSession(DateTime.Now, new Dictionary<Keys, int>());
			sessions.Add(currentSession);
		}
	}
}