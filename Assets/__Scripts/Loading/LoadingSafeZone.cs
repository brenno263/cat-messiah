using UnityEngine;

namespace __Scripts.Loading
{
	public class LoadingSafeZone : MonoBehaviour
	{
		#region monobehavior methods

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.CompareTag("Player"))
			{
				SceneSwapper.singleton.PlayerLeftSafeZone();
				gameObject.SetActive(false);
			}
		}

		#endregion
	}
}
