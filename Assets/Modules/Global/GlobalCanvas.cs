using UnityEngine;
using UnityEngine.EventSystems;

namespace SETHD.Global
{
    public class GlobalCanvas
    {
        public EventSystem EventSystem { get; }
        public RectTransform RectTransform { get; }

        public GlobalCanvas(EventSystem eventSystem, RectTransform rectTransform)
        {
            EventSystem = eventSystem;
            RectTransform = rectTransform;
        }
    }
}