using System;
using UnityEngine;

public class RewardUI : MonoBehaviour {
    public GameObject rewardItemPrefab;
    public Transform rewardListCOntainer;

    void OnEnable() {
        if (RewardManager.Instance != null) 
            RewardManager.Instance.OnRewardDataReady -= RefreshUI;
        
        if (RewardManager.Instance != null)
            RewardManager.Instance.OnRewardDataReady += RefreshUI;
    }
    
    void OnDisable() {
        if (RewardManager.Instance != null)
            RewardManager.Instance.OnRewardDataReady -= RefreshUI;
    }

    private void Start() {
        if (RewardManager.Instance != null && RewardManager.Instance.HasUserId()) {
            RefreshUI();
        }
    }

    public void RefreshUI() {
        for (int i = rewardListCOntainer.childCount - 1; i >= 0; i--) {
            Destroy(rewardListCOntainer.GetChild(i).gameObject);
        }

        var unlocked = RewardManager.Instance.GetUnlockedRewards();
        foreach (var reward in unlocked)
        {
            GameObject itemGO = Instantiate(rewardItemPrefab, rewardListCOntainer);
            var itemUI = itemGO.GetComponent<RewardItemUI>();
            itemUI.SetData(reward);
        }
        
        Debug.Log("Refreshing Reward UI");
    }
}
