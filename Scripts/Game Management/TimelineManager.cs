using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private LevelLoader.SceneName sceneToRunNext;

    private PersistentObjectSpawner _persistentObjectSpawner;
    private LevelLoader _levelLoader;
    private PlayerControls _playerControls;
    private bool _canSkip = false;

    private void Awake()
    {
        _persistentObjectSpawner = GetComponent<PersistentObjectSpawner>();
        _playerControls = new PlayerControls();
        _playerControls.Gameplay.Menu.performed += ctx => Skip();
        playableDirector.stopped += PlayableDirectorOnStopped;
        StartCoroutine(RecreatePersistentObjectSpawner());
    }

    private void PlayableDirectorOnStopped(PlayableDirector obj)
    {
        Skip();
    }

    private void Skip()
    {
        if (!_canSkip) return;
        _canSkip = false;
        playableDirector.Stop();
        _levelLoader.LoadLevel((int)sceneToRunNext);
    }
    
    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private IEnumerator RecreatePersistentObjectSpawner()
    {
        var persistentObject = GameObject.Find("Persistent Objects(Clone)");
        if (!persistentObject) yield break;
        Destroy(persistentObject);
        yield return new WaitForSeconds(0.5f);
        _persistentObjectSpawner.SpawnPersistentObjects();
        yield return new WaitForSeconds(0.5f);
        _levelLoader = FindObjectOfType<LevelLoader>();
        _canSkip = true;
    }
}