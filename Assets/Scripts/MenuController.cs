using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SFXSlider;

    void Start()
    {
        AudioManager.Instance.GetSliders(this);
        AudioManager.Instance.LoadVolume();
    }

    private void OnEnable()
    {
        MasterSlider?.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
        //MasterSlider?.value = 
        MusicSlider?.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        SFXSlider?.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
    }

    private void OnDisable()
    {
        MasterSlider?.onValueChanged.RemoveAllListeners();
        MusicSlider?.onValueChanged.RemoveAllListeners();
        SFXSlider?.onValueChanged.RemoveAllListeners();
    }
}
