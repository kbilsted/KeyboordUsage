namespace KeyboordUsage.Configuration.UserStates
{
	public class GuiConfiguration
	{
		public double X;
		public double Y;
		public double Width;
		public double Height;
		public int SelectedKeyboardIndex;

		public GuiConfiguration(double x, double y, double width, double height, int selectedKeyboardIndex)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
			SelectedKeyboardIndex = selectedKeyboardIndex;
		}
	}
}
