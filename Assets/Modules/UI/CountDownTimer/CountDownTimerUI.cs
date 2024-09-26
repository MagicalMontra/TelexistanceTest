using TMPro;
using UniRx;
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TheraBytes.BetterUi;
using Cysharp.Threading.Tasks;
using DanielLochner.Assets.SimpleScrollSnap;

namespace SETHD.UI.CountDownTimer
{
    public class CountDownTimerUI : MonoBehaviour
    {
        public IReactiveProperty<int> TargetHours { get; private set; }
        public IReactiveProperty<int> TargetMinutes { get; private set; }
        public IReactiveProperty<int> TargetSeconds { get; private set; }
        public Button StartButton => buttons[0];
        public Button PauseButton => buttons[1];
        public Button StopButton => buttons[2];
        
        private CanvasGroup StartButtonFader => buttonFaders[0];
        private CanvasGroup PauseButtonFader => buttonFaders[1];
        private CanvasGroup StopButtonFader => buttonFaders[2];
        
        private const int INDEX_OFFSET = 99;

        [SerializeField]
        private float stopSize;

        [SerializeField]
        private float startSize;

        [SerializeField]
        private TextMeshProUGUI timeItemPrefab;

        [SerializeField]
        private SimpleScrollSnap hoursScroller;
        
        [SerializeField]
        private SimpleScrollSnap minutesScroller;
        
        [SerializeField]
        private SimpleScrollSnap secondsScroller;

        [SerializeField]
        private BetterLocator circleMaskLocator;
        
        [SerializeField]
        private TextMeshProUGUI timeText;

        [SerializeField]
        private RectTransform circleMask;

        [SerializeField]
        private Button[] buttons;

        [SerializeField]
        private CanvasGroup[] buttonFaders;

        private Tween circleSizeTween;
        private Sequence buttonFaderSequence;

        private void Awake()
        {
            TargetHours = new ReactiveProperty<int>(0);
            TargetMinutes = new ReactiveProperty<int>(0);
            TargetSeconds = new ReactiveProperty<int>(0);
            
            for (int i = 0; i < 100; i++)
            {
                timeItemPrefab.text = i.ToString("D2");
                hoursScroller.Add(timeItemPrefab.gameObject, 0);
                minutesScroller.Add(timeItemPrefab.gameObject, 0);
                secondsScroller.Add(timeItemPrefab.gameObject, 0);
            }
            
            hoursScroller.OnPanelCentered.AddListener(OnHoursCentered);
            minutesScroller.OnPanelCentered.AddListener(OnMinutesCentered);
            secondsScroller.OnPanelCentered.AddListener(OnSecondsCentered);
        }

        private async UniTaskVoid Start()
        {
            PauseButtonFader.alpha = StopButtonFader.alpha = 0;
            await UniTask.DelayFrame(1);
            hoursScroller.GoToPanel(INDEX_OFFSET);
            minutesScroller.GoToPanel(INDEX_OFFSET);
            secondsScroller.GoToPanel(INDEX_OFFSET);
        }

        private void OnDestroy()
        {
            hoursScroller.OnPanelCentered.RemoveListener(OnHoursCentered);
            minutesScroller.OnPanelCentered.RemoveListener(OnMinutesCentered);
            secondsScroller.OnPanelCentered.RemoveListener(OnSecondsCentered);
        }

        public async UniTask StartMask()
        {
            circleMaskLocator.enabled = false;
            StartButtonFader.interactable = false;
            circleSizeTween = DOVirtual.Float(stopSize, startSize, 0.4f, value => circleMask.sizeDelta = Vector2.one * value);
            circleSizeTween.SetEase(Ease.InOutQuart);
            buttonFaderSequence = DOTween.Sequence();
            buttonFaderSequence.Append(StartButtonFader.DOFade(0f, 0.4f).SetEase(Ease.InOutQuart));
            buttonFaderSequence.Join(circleSizeTween);
            buttonFaderSequence.Append(PauseButtonFader.DOFade(1f, 0.4f).SetEase(Ease.InOutQuart));
            buttonFaderSequence.Join(StopButtonFader.DOFade(1f, 0.4f).SetEase(Ease.InOutQuart));
            buttonFaderSequence.Play();
            await buttonFaderSequence.AsyncWaitForCompletion();
            StopButtonFader.interactable = true;
            PauseButtonFader.interactable = true;
            StartButtonFader.blocksRaycasts = false;
            circleSizeTween?.Kill();
            buttonFaderSequence?.Kill();
            circleSizeTween = null;
            buttonFaderSequence = null;
        }
        
        public async UniTask EndMask()
        {
            circleMaskLocator.enabled = false;
            StopButtonFader.interactable = false;
            PauseButtonFader.interactable = false;
            circleSizeTween = DOVirtual.Float(startSize, stopSize, 0.4f, value => circleMask.sizeDelta = Vector2.one * value);
            buttonFaderSequence = DOTween.Sequence();
            buttonFaderSequence.Append(StopButtonFader.DOFade(0f, 0.4f).SetEase(Ease.InOutQuart));
            buttonFaderSequence.Join(PauseButtonFader.DOFade(0f, 0.4f).SetEase(Ease.InOutQuart));
            buttonFaderSequence.Append(circleSizeTween);
            buttonFaderSequence.Append(StartButtonFader.DOFade(1f, 0.4f).SetEase(Ease.InOutQuart));
            buttonFaderSequence.Play();
            await buttonFaderSequence.AsyncWaitForCompletion();
            circleMaskLocator.enabled = true;
            StartButtonFader.interactable = true;
            StartButtonFader.blocksRaycasts = true;
            circleSizeTween?.Kill();
            buttonFaderSequence?.Kill();
            circleSizeTween = null;
            buttonFaderSequence = null;
        }

        public void SetTimeText(float time)
        {
            var span = TimeSpan.FromSeconds(time);
            timeText.SetText(span.ToString(@"hh\:mm\:ss"));
        }

        private void OnHoursCentered(int centeredIndex, int selectedIndex)
        {
            TargetHours.Value = Math.Clamp(INDEX_OFFSET - centeredIndex, 0, INDEX_OFFSET);
        }
        
        private void OnMinutesCentered(int centeredIndex, int selectedIndex)
        {
            TargetMinutes.Value = Math.Clamp(INDEX_OFFSET - centeredIndex, 0, INDEX_OFFSET);
        }
        
        private void OnSecondsCentered(int centeredIndex, int selectedIndex)
        {
            TargetSeconds.Value = Math.Clamp(INDEX_OFFSET - centeredIndex, 0, INDEX_OFFSET);
        }
    }
}
