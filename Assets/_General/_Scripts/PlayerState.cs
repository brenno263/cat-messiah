namespace _General._Scripts
{
	using static PlayerState;

	public static class PlayerStateExtensions
	{
		public static int direction(this PlayerState state)
		{
			switch (state)
			{
				case FacingRight:
				case WalkingRight:
					return 1;
				case FacingLeft:
				case WalkingLeft:
					return -1;
				default:
					return 0;
			}
		}
	}

	public enum PlayerState
	{
		Idling,
		FacingRight,
		FacingLeft,
		WalkingRight,
		WalkingLeft,
		ClimbingUp,
		ClimbingDown
	}
}