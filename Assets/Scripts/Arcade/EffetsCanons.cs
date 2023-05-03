using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffetsCanons : MonoBehaviour
{
    [SerializeField] AudioClip[] effetsSoundEffects;
    const float ICE_EFFECT_DURATION = 5.0f;
    const float INK_EFFECT_DURATION = 5.0f;
    const int SCORE_TO_REMOVE = 1000;

    private Color iceShipColor = Color.blue;
    private Color inkShipColor = Color.black;
    private HoverMotor hoverMotor;

    private bool iceEffectActive;
    private float iceEffectStartTime;
    private bool inkEffectActive;
    private float inkEffectStartTime;


    private List<AudioSource> audioSourceList;
    
    // Start is called before the first frame update
    void Awake()
    {
        audioSourceList = new List<AudioSource>();
        hoverMotor = gameObject.GetComponent<HoverMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        //delete les audioSource inactifs
        if (audioSourceList.Count > 0)
        {
            foreach (AudioSource source in audioSourceList)
            {
                if (!source.isPlaying)
                {
                    Destroy(source);
                    audioSourceList.Remove(source);
                }
            }
        }
        if (Time.time >= iceEffectStartTime + ICE_EFFECT_DURATION)
        {
            iceEffectActive = false;
            hoverMotor.ModifyIceNerf(1f);
        }
        if (Time.time >= inkEffectStartTime + INK_EFFECT_DURATION)
        {
            inkEffectActive = false;
            hoverMotor.gameEventManager.ApplyInk(false);
        }
        if (!iceEffectActive && !inkEffectActive)
            hoverMotor.ModifyShipColor();
    }

    public void GiveIceEffect()
    {
        iceEffectActive = true;
        iceEffectStartTime = Time.time;
        hoverMotor.ModifyIceNerf(0.5f);
        hoverMotor.ModifyShipColor(iceShipColor);
        PlaySound(effetsSoundEffects[0]);
    }

    public void RemoveScoreEffect()
    {
        hoverMotor.gameEventManager.RemoveScore(SCORE_TO_REMOVE);
        PlaySound(effetsSoundEffects[1]);
    }

    public void GiveInkEffect()
    {
        inkEffectActive = true;
        inkEffectStartTime = Time.time;
        hoverMotor.ModifyShipColor(inkShipColor);
        hoverMotor.gameEventManager.ApplyInk(true);
        PlaySound(effetsSoundEffects[2]);
    }
    private void PlaySound (AudioClip sound)
    {
        AudioSource audioSourceTemp = gameObject.AddComponent<AudioSource>();
        audioSourceTemp.clip = sound;
        audioSourceTemp.Play();
        audioSourceList.Add(audioSourceTemp);
    }
    
}
