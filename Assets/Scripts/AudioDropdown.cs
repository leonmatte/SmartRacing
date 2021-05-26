using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioDropdown : MonoBehaviour
{
    public AudioSource source { get { return GetComponent<AudioSource>(); } }
    public TMP_Dropdown drop { get { return GetComponent<TMP_Dropdown>(); } }
    public AudioClip clip;

    private void Start()
    {
        gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        //drop.onClick.AddListener(reproducir);
    }
    public void reproducir()
    {
        source.PlayOneShot(clip);
    }
}
