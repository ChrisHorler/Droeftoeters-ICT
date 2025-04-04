using UnityEngine;
using UnityEngine.UI;

public class RouteStarUI : MonoBehaviour
{
    [Header("Default Buttons")] 
    public Button button_Level1;
    public Button button_Level2;
    public Button button_Level3;
    public Button button_Level4;
    
    [Header("Route A Buttons")] 
    public Button routeA_Level5;
    public Button routeA_Level6;
    public Button routeA_Level7;
    public Button routeA_Level8;
    
    [Header("Route B Buttons")]
    public Button routeB_Level5;
    public Button routeB_Level6;
    public Button routeB_Level7;
    public Button routeB_Level8;

    private void OnEnable() {
        UpdateAllStars();
    }

    public void UpdateAllStars() {
        UpdateDefaultStars();
        UpdateRouteAStars();
        UpdateRouteBStars();
    }
    
    private void UpdateDefaultStars() {
        UpdateStar(button_Level1, "Reward-Level1");
        UpdateStar(button_Level2, "Reward-Level2");
        UpdateStar(button_Level3, "Reward-Level3");
        UpdateStar(button_Level4, "Reward-Level4");
    }
    
    private void UpdateRouteAStars() {
        UpdateStar(routeA_Level5, "Reward-Level5A");
        UpdateStar(routeA_Level6, "Reward-Level6A");
        UpdateStar(routeA_Level7, "Reward-Level7A");
        UpdateStar(routeA_Level8, "Reward-Level8A");
    }

    private void UpdateRouteBStars() {
        UpdateStar(routeB_Level5, "Reward-Level5B");
        UpdateStar(routeB_Level6, "Reward-Level6B");
        UpdateStar(routeB_Level7, "Reward-Level7B");
        UpdateStar(routeB_Level8, "Reward-Level8B");
    }

    private void UpdateStar(Button starButton, string rewardID) {
        bool isUnlocked = RewardManager.Instance.isRewardUnlocked(rewardID);
        
        Transform emptyStar = starButton.transform.Find("StarEmpty");
        Transform fullStar = starButton.transform.Find("StarFull");
        
        if (emptyStar) emptyStar.gameObject.SetActive(!isUnlocked);
        if (fullStar) fullStar.gameObject.SetActive(isUnlocked);
    }
}
