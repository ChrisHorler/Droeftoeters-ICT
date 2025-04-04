using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BadgeDatabase", menuName = "Rewards/Badge Database")]
public class BadgeDatabase : ScriptableObject
{
    public List<RewardData> allBadges;
}
