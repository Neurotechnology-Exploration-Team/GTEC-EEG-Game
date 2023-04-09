using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioMixer audioMixer;
    public Slider slider;
    void Start()
    {
        float volume;
        audioMixer.GetFloat("MasterVolume", out volume);
        slider.value = -((volume / -40f)-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AdjustVolume()
    {
        float volume = (1 - slider.value) * -40f;
        audioMixer.SetFloat("MasterVolume", volume);
    }
    
    public void KeyRebind(GameObject text)
    {
        
    }
}
