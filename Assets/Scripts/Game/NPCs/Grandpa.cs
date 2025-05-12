using System.Collections;
using UnityEngine;

public class Grandpa : NPC
{
    [SerializeField] private AudioClip m_GameStart;
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        PlayAudioClip(m_GameStart);
    }
}
