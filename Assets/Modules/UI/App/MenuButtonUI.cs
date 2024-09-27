using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SETHD.UI.App
{
    public class MenuButtonUI : MonoBehaviour
    {
        public Color ActiveColor => activeColor;
        public Color NormalColor => normalColor;
        public Button TimerButton => menuButtons[0];
        public Button StopWatchButton => menuButtons[1];
        public TextMeshProUGUI TimerButtonText => timerButtonText;

        public TextMeshProUGUI StopWatchButtonText => stopWatchButtonText;

        [SerializeField]
        private Color activeColor;

        [SerializeField]
        private Color normalColor;

        [SerializeField]
        private TextMeshProUGUI timerButtonText;
        
        [SerializeField]
        private TextMeshProUGUI stopWatchButtonText;
        
        [SerializeField]
        private Button[] menuButtons;
    }
}
