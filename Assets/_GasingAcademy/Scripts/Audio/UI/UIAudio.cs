namespace DGE.Audio
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class UIAudio : MonoBehaviour
    {
        public Slider sliderBGM;
        public Slider sliderSFX;

        public float volumeBGM;
        public float volumeSFX;

        public UnityEvent<float> OnChangeBGM;
        public UnityEvent<float> OnChangeSFX;

        void Start()
        {
            sliderBGM.value = volumeBGM;
            sliderSFX.value = volumeSFX;
        }

        void OnEnable()
        {
            sliderBGM.onValueChanged.AddListener(delegate { OnValueChangeBGM(); } );
            sliderSFX.onValueChanged.AddListener(delegate { OnValueChangeSFX(); });
        }

        void OnDisable()
        {
            sliderBGM.onValueChanged.RemoveAllListeners();
            sliderSFX.onValueChanged.RemoveAllListeners();
        }

        void OnValueChangeBGM() => OnChangeBGM?.Invoke(sliderBGM.value);
        void OnValueChangeSFX() => OnChangeSFX?.Invoke(sliderSFX.value);
    }
}
