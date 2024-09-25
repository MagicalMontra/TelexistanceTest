using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SETHD.UI.CountDownTimer
{
    public class CountDownTimerUI : MonoBehaviour
    {
        public Button StartButton => buttons[0];
        public Button PauseButton => buttons[1];
        public Button StopButton => buttons[2];
        public TextMeshProUGUI TimeText => timeText;
        
        [SerializeField]
        private TextMeshProUGUI timeText;

        [SerializeField]
        private Button[] buttons;
    }
}
