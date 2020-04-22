namespace _General._Scripts.Player
{
	using static PlayerState;

	public static class PlayerStateExtensions
	{
		public static int MoveDirection(this PlayerState state)
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

		public static int Direction(this PlayerState state)
		{
			switch (state)
			{
				case WalkingRight:
				case FacingRight:
					return 1;
				case WalkingLeft:
				case FacingLeft:
					return -1;
				default:
					return 0;
			}
		}
	}

	public enum PlayerState
	{
		FacingRight = 0,
		FacingLeft = 1,
		WalkingRight = 2,
		WalkingLeft = 3,
		ClimbingUp = 4,
		ClimbingDown = 5
	}
}