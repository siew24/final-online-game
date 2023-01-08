using UnityEngine;

[CreateAssetMenu(fileName = "OnGunAmmoChange", menuName = "Events/OnGunAmmoChange", order = 0)]
public class OnGunAmmoChange : BaseEvent<(int, int)>
{ }