using UnityEngine;

namespace KID
{
    /// <summary>
    /// 技能資料
    /// </summary>
    [CreateAssetMenu(menuName = "KID/Data Skill")]
    public class DataSkill : ScriptableObject
    {
        [Header("技能名稱")]
        public string skillName;
        [Header("技能類型")]
        public TypeSkill typeSkill;
        [Header("技能說明")]
        public string skillDescription;
        [Header("技能圖示")]
        public Sprite skillSprite;
    }

    /// <summary>
    /// 技能類型
    /// </summary>
    public enum TypeSkill
    {
        addMarble1, addMarble3, addMarble5
    }
}
