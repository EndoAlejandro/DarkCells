using UnityEngine;

namespace PlayerComponents
{
    [CreateAssetMenu(menuName = "Custom/PlayerStats", fileName = "NewPlayerStats")]
    public class PlayerStats : ScriptableObject
    {
        [SerializeField] private float speed;
    }
}