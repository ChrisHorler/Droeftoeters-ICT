using UnityEngine;

[CreateAssetMenu(fileName = "NewReward", menuName = "Rewards/Reward")]
public class RewardData : ScriptableObject
{
    public string rewardID;
    public Sprite icon;
    public string displayName;
    [TextArea] public string description;
}
