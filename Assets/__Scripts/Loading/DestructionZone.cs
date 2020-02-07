using System;
using UnityEngine;

namespace __Scripts.Loading
{
    public class DestructionZone : MonoBehaviour
    {

    	//This requires a collider with some width (greater than 10 probably),
    	//but it is a lot more reliable than trigger enter when entities are falling fast
        private void OnTriggerStay2D(Collider2D other)
        {
            Destroy(other.gameObject);
        }
    }
}
