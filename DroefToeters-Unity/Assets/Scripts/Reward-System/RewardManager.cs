using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour {
    public static RewardManager Instance { get; private set; }
    
    private ApiConnecter apiConnecter;
    private string userId;

    [Tooltip("A database of all badges/rewards if you want to iterate them in the UI")]
    public BadgeDatabase badgeDatabase;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
    
    void Start() {
        apiConnecter = FindFirstObjectByType<ApiConnecter>();
        StartCoroutine(GetUserId());
        
    }

    private IEnumerator GetUserId() {
        yield return apiConnecter.SendRequest("account/id", HttpMethod.GET, true,
            (string userIdResponse, string error) => {
                if (error == null) {
                    userId = userIdResponse;
                    Debug.Log($"User ID: {userId}");

                    CheckRewards(userId);
                }
                else {
                    Debug.LogError($"Error whilst getting user id: {error}");
                }
            });
    }

    private void CheckRewards(string userId) {
        Debug.Log("Checking rewards for: " + userId);
    }

    /// <summary>
    /// Unlocks a reward (badge/sticker) for this user locally
    /// </summary>
    public void UnlockReward(string rewardID) {
        if (string.IsNullOrEmpty(userId)) {
            Debug.LogWarning("[RewardManager] Can't unlock a reward - userId is null or empty");
            return;
        }

        if (!isRewardUnlocked(rewardID)) {
            string prefsKey = $"{userId}_{rewardID}";
            PlayerPrefs.SetInt(prefsKey, 1);
            PlayerPrefs.Save();

            Debug.Log($"[RewardManager] Unlocked '{rewardID}' for user '{userId}'.");
        }
        else {
            Debug.Log($"[RewardManager] '{rewardID}' already unlocked for user '{userId}'.");
        }
    }


    public bool isRewardUnlocked(string rewardID) {
        if (string.IsNullOrEmpty(userId)) {
            Debug.LogWarning("[RewardManager] userId not set - returning false.");
            return false;
        }
        
        string prefsKey = $"{userId}_{rewardID}";
        return PlayerPrefs.GetInt(prefsKey, 0) == 1;
    }

    public List<RewardData> GetUnlockedRewards() {
        List<RewardData> unlocked = new List<RewardData>();

        if (badgeDatabase == null) {
            Debug.LogWarning("[RewardManager] No badgeDatabase assigned; returning empty list.");
            return unlocked;
        }

        foreach (var data in badgeDatabase.allBadges) {
            if (isRewardUnlocked(data.rewardID)) {
                unlocked.Add(data);
            }
        }
        return unlocked;
    }
}
