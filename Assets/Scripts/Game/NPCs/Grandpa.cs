using System.Collections;
using UnityEngine;

public class Grandpa : NPC
{
    [SerializeField] private AudioClip m_GameStart;

#if UNITY_EDITOR
#else
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        PlayAudioClip(m_GameStart);
    }
#endif
}
