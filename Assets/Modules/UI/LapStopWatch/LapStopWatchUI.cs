using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SETHD.UI.LapStopWatch
{
    public class LapStopWatchUI : MonoBehaviour
    {
        public Button StartButton => buttons[0];
        public Button LapButton => buttons[1];
        public Button StopButton => buttons[2];
        public Button ResetButton => buttons[3];

        public TextMeshProUGUI LapTimeText => lapTimeText;
        public TextMeshProUGUI TotalTimeText => totalTimeText;
        public TextMeshProUGUI StopButtonText => stopButtonText;
        
        private const string FORMAT = @"mm\:ss\.ff";

        [SerializeField]
        private GameObject lapTimeScroller;

        [SerializeField]
        private LapTimeItem timeItemPrefab;

        [SerializeField]
        private Transform lapScrollerContent;

        [SerializeField]
        private TextMeshProUGUI lapTimeText;
        
        [SerializeField]
        private TextMeshProUGUI totalTimeText;
        
        [SerializeField]
        private TextMeshProUGUI stopButtonText;
        
        [SerializeField]
        private Button[] buttons;

        private LapTimeItem.Factory factory;

        private List<LapTimeItem> timeItems = new List<LapTimeItem>();

        [Inject]
        private void Construct(LapTimeItem.Factory factory)
        {
            this.factory = factory;
        }

        private void Start()
        {
            ResetTime();
        }

        public void ResetTime()
        {
            lapTimeScroller.SetActive(false);
            lapTimeText.gameObject.SetActive(false);
            lapTimeText.text = totalTimeText.text = "00:00.00";
            
            for (int i = 0; i < timeItems.Count; i++)
            {
                DestroyImmediate(timeItems[i].gameObject);
            }
            
            timeItems.Clear();
        }

        public void AddLap(TimeSpan lapTime, TimeSpan totalTime)
        {
            lapTimeScroller.SetActive(true);
            var instance = factory.Create(timeItemPrefab, lapScrollerContent);
            instance.LapTime = lapTime.ToString(FORMAT);
            instance.TotalTime = totalTime.ToString(FORMAT);
            instance.Number = lapScrollerContent.childCount.ToString("D2");
            instance.transform.SetSiblingIndex(0);
            timeItems.Add(instance);
        }
    }
}
