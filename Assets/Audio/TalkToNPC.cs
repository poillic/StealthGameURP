using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TalkToNPC : MonoBehaviour
{
    public List<AudioClip> voiceLines;
    AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray r = Camera.main.ScreenPointToRay( Input.mousePosition );

        Debug.DrawRay( r.origin, r.direction * 1000f, Color.magenta );
        if( Input.GetMouseButtonDown( 0 ) )
        {
            if (  Physics.Raycast(r, out RaycastHit hit ) )
            {
                if( hit.collider.transform == transform )
                {
                    _audio.PlayOneShot( voiceLines[ Random.Range( 0, voiceLines.Count ) ] );
                }
            }
        }
    }
}
