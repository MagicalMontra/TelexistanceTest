using TMPro;
using UnityEngine;
using Zenject;

namespace SETHD.UI.LapStopWatch
{
    public class LapTimeItem : MonoBehaviour
    {
        public string Number
        {
            get => number.text;
            set => number.text = value;
        }
        
        public string LapTime
        {
            get => lapTime.text;
            set => lapTime.text = value;
        }
        
        public string TotalTime
        {
            get => totalTime.text;
            set => totalTime.text = value;
        }
        
        [SerializeField]
        private TextMeshProUGUI number;
        
        [SerializeField]
        private TextMeshProUGUI lapTime;
        
        [SerializeField]
        private TextMeshProUGUI totalTime;

        public class Factory : PlaceholderFactory<Object, Transform, LapTimeItem>
        {
            
        }
    }

    public class LapTimeItemFactory : IFactory<Object, Transform, LapTimeItem>
    {
        private readonly DiContainer container;

        public LapTimeItemFactory(DiContainer container)
        {
            this.container = container;
        }

        public LapTimeItem Create(Object prefab, Transform parent)
        {
            return container.InstantiatePrefabForComponent<LapTimeItem>(prefab, parent);
        }
    }
}