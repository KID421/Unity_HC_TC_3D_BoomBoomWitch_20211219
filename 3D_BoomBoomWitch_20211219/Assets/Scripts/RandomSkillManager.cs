using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace KID
{
    /// <summary>
    /// 隨機技能
    /// </summary>
    public class RandomSkillManager : MonoBehaviour
    {
        public static RandomSkillManager instance;

        [SerializeField, Header("所有技能")]
        private List<DataSkill> dataSkills = new List<DataSkill>();
        [SerializeField, Header("技能物件")]
        private GameObject goSkill;
        [SerializeField, Header("隨機技能格子")]
        private Transform[] traRandomSkillBox;
        [SerializeField, Header("技能物件尺寸")]
        private float sizeSkillObject = 100;
        [SerializeField, Header("技能每次移動單位")]
        private float unitSkillMove = 20;
        [SerializeField, Header("移動間隔")]
        private float intervalMove = 0.1f;
        [SerializeField, Header("移動次數")]
        private float countMove = 30;
        [Header("音效")]
        [SerializeField]private AudioClip soundRandom;
        [SerializeField]private AudioClip soundGetSkill;

        private List<GameObject>[] listSkillInBox = { new List<GameObject>(), new List<GameObject>(), new List<GameObject>() };
        private List<RectTransform>[] listSkillRectInBox = { new List<RectTransform>(), new List<RectTransform>(), new List<RectTransform>() };

        /// <summary>
        /// 隨機技能
        /// </summary>
        private CanvasGroup groupRandomSkill;
        private AudioSource aud;

        private void Awake()
        {
            instance = this;

            aud = GetComponent<AudioSource>();
            groupRandomSkill = GameObject.Find("隨機技能").GetComponent<CanvasGroup>();

            InitializeSkill();
            StartCoroutine(ShowRandomSkill());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) StartCoroutine(ShowRandomSkill());
        }

        public IEnumerator ShowRandomSkill()
        {
            yield return StartCoroutine(FadeRandomSkillGroup(true));

            RandomSkill();

            for (int i = 0; i < listSkillRectInBox.Length; i++)
            {
                yield return new WaitForSeconds(0.1f);

                StartCoroutine(MoveSkill(listSkillRectInBox[i]));
            }
        }

        /// <summary>
        /// 初始化技能物件
        /// </summary>
        private void InitializeSkill()
        {
            for (int i = 0; i < listSkillInBox.Length; i++)
            {
                int index = 0;

                foreach (var item in dataSkills)
                {
                    GameObject tempSkill = Instantiate(goSkill, traRandomSkillBox[i]);
                    tempSkill.GetComponent<Image>().sprite = item.skillSprite;
                    tempSkill.GetComponent<SkillUIObject>().dataSkill = item;
                    listSkillRectInBox[i].Add(tempSkill.GetComponent<RectTransform>());
                    listSkillInBox[i].Add(tempSkill);
                    index++;
                }
            }
        }

        /// <summary>
        /// 隨機技能
        /// </summary>
        private void RandomSkill()
        {
            for (int i = 0; i < listSkillInBox.Length; i++)
            {
                int randomSeed = Random.Range(0, 100);
                var random = new System.Random(randomSeed);
                listSkillInBox[i] = listSkillInBox[i].OrderBy(item => random.Next()).ToList();
                listSkillRectInBox[i] = listSkillRectInBox[i].OrderBy(item => random.Next()).ToList();

                for (int j = 0; j < listSkillRectInBox[i].Count; j++)
                {
                    print(j);
                    listSkillRectInBox[i][j].anchoredPosition = Vector2.up * j * sizeSkillObject;
                }
            }
        }

        /// <summary>
        /// 移動技能
        /// </summary>
        private IEnumerator MoveSkill(List<RectTransform> listSkillRect)
        {
            for (int i = 0; i < countMove; i++)
            {
                MovePerSKillRect(listSkillRect, -unitSkillMove, soundRandom, new Vector2(0.1f, 0.25f));

                for (int j = 0; j < listSkillRect.Count; j++)
                {
                    //listSkillRect[j].anchoredPosition += Vector2.up * -unitSkillMove;
                    //aud.PlayOneShot(soundRandom, Random.Range(0.1f, 0.3f));

                    if (listSkillRect[j].anchoredPosition.y == -sizeSkillObject)
                    {
                        listSkillRect[j].anchoredPosition += Vector2.up * listSkillRect.Count * sizeSkillObject;
                    }
                }
                yield return new WaitForSeconds(intervalMove);
            }

            yield return new WaitForSeconds(0.1f);

            MovePerSKillRect(listSkillRect, -20, soundRandom, new Vector2(0.1f, 0.25f));

            yield return new WaitForSeconds(0.3f);

            MovePerSKillRect(listSkillRect, 25, soundRandom, new Vector2(0.1f, 0.25f));

            yield return new WaitForSeconds(0.3f);

            MovePerSKillRect(listSkillRect, -5, soundGetSkill, new Vector2(0.5f, 0.7f));
        }

        private void MovePerSKillRect(List<RectTransform> listSkillRect, float moveDistance, AudioClip sound, Vector2 volumeRange)
        {
            for (int i = 0; i < listSkillRect.Count; i++)
            {
                listSkillRect[i].anchoredPosition += Vector2.up * moveDistance;
                aud.PlayOneShot(sound, Random.Range(volumeRange.x, volumeRange.y));
            }
        }

        /// <summary>
        /// 淡入淡出隨機技能群組
        /// </summary>
        /// <param name="fadeIn">淡入</param>
        public IEnumerator FadeRandomSkillGroup(bool fadeIn)
        {
            ControlSystem.instance.enabled = !fadeIn;

            float increase = fadeIn ? 0.1f : -0.1f;

            for (int i = 0; i < 10; i++)
            {
                groupRandomSkill.alpha += increase;
                yield return new WaitForSeconds(0.01f);
            }

            groupRandomSkill.interactable = fadeIn;
            groupRandomSkill.blocksRaycasts = fadeIn;
        }
    }
}
