using UnityEngine;

public class UnlockBadge : MonoBehaviour
{
    [Tooltip("Which reward to unlock for this level")]
    public RewardData rewardToUnlock;

    public void UnlockReward() {
        if (rewardToUnlock != null) {
            RewardManager.Instance.UnlockReward(rewardToUnlock.rewardID);
        }
        else {
            Debug.LogWarning("No rewardToUnlock assigned");
        }
    }
}
