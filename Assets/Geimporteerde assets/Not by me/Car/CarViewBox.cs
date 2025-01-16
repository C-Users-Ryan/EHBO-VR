using EHBOVR.PlayerCharacter;
using UnityEngine;
using UnityEngine.Events;

namespace EHBOVR.Car
{
    public class CarViewBox : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent playerInViewEvent;
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<Player>() != null)
            {
                playerInViewEvent.Invoke();
            }
        }
    }
}
