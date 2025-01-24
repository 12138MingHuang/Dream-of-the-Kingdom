using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    public PlayableDirector director;
    public ObjectEventSO loadMenuEvent;
    public Button skipButton;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.stopped += OnPlayableDirectorStopped;
        
        skipButton.onClick.AddListener(() =>
        {
            if(director.state == PlayState.Playing)
                director.Stop();
        });
    }

    private void OnPlayableDirectorStopped(PlayableDirector obj)
    {
        loadMenuEvent?.RaiseEvent(null, this);
    }
}
