using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audios : MonoBehaviour
{
    public AudioSource source { get { return GetComponent<AudioSource>(); } }
    public Button btn { get { return GetComponent<Button>(); } }
    public AudioClip clip;

    private void Start()
    {
        gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        btn.onClick.AddListener(reproducir);
    }
    public void reproducir()
    {
        source.PlayOneShot(clip);
    }
}
