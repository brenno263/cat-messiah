using UnityEngine;

namespace _Testing._Scripts
{
	public class FloatWhileNotAwake : MonoBehaviour
	{
		#region variables
		[Header("Set in Inspector")]
		public float frequency;
		public float amplitude;
		[Header("Set Dynamically")] 
		public float time;
		[Header("Fetched on Init")] 
		public Rigidbody2D rigid;
		public bool rigidUnset;
		#endregion

		#region monobehavior methods

		private void Start()
		{
			rigid = GetComponent<Rigidbody2D>();
			rigidUnset = (rigid == null);
			time = 0;
			if (!rigidUnset)
			{
				rigid.constraints = RigidbodyConstraints2D.FreezeAll;
			}
		}

		void Update()
		{
			if(rigidUnset) return;

			time += Time.deltaTime;
	    
			Vector3 pos = transform.position;
        
			pos.y += amplitude * Mathf.Cos(time * frequency);

			transform.position = pos;
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			rigid.constraints = RigidbodyConstraints2D.None;
			Destroy(this);
		}

		#endregion
	}
}
