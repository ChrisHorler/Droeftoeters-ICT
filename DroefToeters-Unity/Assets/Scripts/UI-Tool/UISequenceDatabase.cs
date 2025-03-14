using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UISequenceDatabase", menuName = "UI/UISequence Database")]
public class UISequenceDatabase : ScriptableObject
{
    [System.Serializable]
    public class UISequence
    {
        public string buttonName;
        public List<UISequencePanel> panelData;
    }
    
    public List<UISequence> sequences = new List<UISequence>();
}