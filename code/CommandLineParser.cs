using System;

namespace KeyboordUsage
{
	public class CommandLineParser
	{
		public CommandLineArgs ParseCommandLine()
		{
			string[] args = Environment.GetCommandLineArgs();

			bool useVsNavigation = args.Length > 1 && args[1] == "--useVisualStudioNavigation";

			var commandLine = new CommandLineArgs(useVsNavigation);

			return commandLine;
		}
	}

	public class CommandLineArgs
	{
		public readonly bool UseVisualStudioNavigation;

		public CommandLineArgs(bool useVisualStudioNavigation)
		{
			UseVisualStudioNavigation = useVisualStudioNavigation;
		}
	}
}
