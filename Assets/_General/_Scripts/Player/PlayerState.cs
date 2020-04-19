namespace _General._Scripts.Player
{
	using static PlayerState;

	public static class PlayerStateExtensions
	{
		public static int Direction(this PlayerState state)
		{
			switch (state)
			{
				case WalkingRight:
					return 1;
				case WalkingLeft:
					return -1;
				default:
					return 0;
			}
		}
	}

	public enum PlayerState
	{
		Idling = 0,
		WalkingRight = 1,
		WalkingLeft = 2,
		ClimbingUp = 3,
		ClimbingDown = 4
	}
}