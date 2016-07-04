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

		/// <summary>
		/// called from json
		/// </summary>
		public UserState(RecodingSession accumulated, List<RecodingSession> sessions)
		{
			if (accumulated == null)
				accumulated = new RecodingSession(DateTime.Now, new Dictionary<Keys, int>());
			this.accumulated = accumulated;

			if (sessions == null)
				sessions = new List<RecodingSession>();
			this.sessions = sessions;

			NewInstance();
		}

		public static UserState LoadFromJson(string path)
		{
			if (File.Exists(path))
			{
				var json = File.ReadAllText(path);
				return JsonConvert.DeserializeObject<UserState>(json);
			}

			return new UserState(new RecodingSession(DateTime.Now, new Dictionary<Keys, int>()), new List<RecodingSession>());
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