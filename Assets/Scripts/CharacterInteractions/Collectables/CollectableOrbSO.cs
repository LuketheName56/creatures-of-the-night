using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "Collectables/CollectableOrbSO", fileName = "Orb")]
public class CollectableOrbSO : CollectableSOBase
{
    public int OrbAmount;
}
