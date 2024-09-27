using TMPro;
using UnityEngine;

namespace SETHD.UI.Time
{
    public class TimeUI : MonoBehaviour
    {
        public TextMeshProUGUI TimeText => timeText;

        public TextMeshProUGUI TimeZoneText => timeZoneText;
        
        [SerializeField]
        private TextMeshProUGUI timeText;
        
        [SerializeField]
        private TextMeshProUGUI timeZoneText;
    }
}
