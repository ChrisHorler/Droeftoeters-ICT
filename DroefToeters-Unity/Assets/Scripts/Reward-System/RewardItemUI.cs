using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardItemUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text rewardName;

    public void SetData(RewardData data)
    {
        if (iconImage != null)
            iconImage.sprite = data.icon;
        
        if (rewardName != null)
            rewardName.text = data.displayName;
    }
}
