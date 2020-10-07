using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private AudioClip mouseClickClip = default;
    [SerializeField] private AudioClip startGameClip = default;
    [SerializeField] private List<UIObject> uiObjects = new List<UIObject>();
    [SerializeField] private List<UIButtonObject> uiButtons = new List<UIButtonObject>();
    [SerializeField] private Image specificControlsImage;
    [SerializeField] private List<Sprite> controlsSprites = new List<Sprite>();

    private Dictionary<UIObjectName, GameObject> _uiDictionary = new Dictionary<UIObjectName, GameObject>();
    private Dictionary<UIButtonName, Button> _uiButtons = new Dictionary<UIButtonName, Button>();
    private Fader _fader;
    private LevelLoader _levelLoader;
    private UIButtonName _lastControls;

    private void Awake()
    {
        _uiDictionary = uiObjects.ToDictionary(k => k.name, v => v.gameObject);
        _uiButtons = uiButtons.ToDictionary(k => k.buttonName, v => v.button);
    }

    private void Start()
    {
        ShowCursor();
        _levelLoader = GameObject.FindWithTag("Level Loader").GetComponent<LevelLoader>();
    }

    private void PlayClip(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(clip);
    }

    public void GoToCredits()
    {
        PlayClip(mouseClickClip);
        _uiDictionary[UIObjectName.MainPlate].SetActive(false);
        _uiDictionary[UIObjectName.Credits].SetActive(true);
        _uiButtons[UIButtonName.Back].Select();
    }
    
    public void GoToControls()
    {
        PlayClip(mouseClickClip);
        _uiDictionary[UIObjectName.MainPlate].SetActive(false);
        _uiDictionary[UIObjectName.Controls].SetActive(true);
        _uiButtons[UIButtonName.Pc].Select();
    }
    
    public void GoToPcControls()
    {
        _lastControls = UIButtonName.Pc;
        specificControlsImage.sprite = controlsSprites[0];
        GoToSpecificControls();
    }
    
    public void GoToPsControls()
    {
        _lastControls = UIButtonName.Ps;
        specificControlsImage.sprite = controlsSprites[1];
        GoToSpecificControls();
    }
    
    public void GoToXboxControls()
    {
        _lastControls = UIButtonName.Xbox;
        specificControlsImage.sprite = controlsSprites[2];
        GoToSpecificControls();
        
    }

    public void GoToSpecificControls()
    {
        PlayClip(mouseClickClip);
        _uiDictionary[UIObjectName.Controls].SetActive(false);
        _uiDictionary[UIObjectName.SpecificControls].SetActive(true);
        _uiButtons[UIButtonName.ControlsBack].Select();
    }

    public void GoToQuitWindow()
    {
        PlayClip(mouseClickClip);
        _uiDictionary[UIObjectName.MainPlate].SetActive(false);
        _uiDictionary[UIObjectName.QuitWindow].SetActive(true);
        _uiButtons[UIButtonName.No].Select();
    }

    public void GoBackToMainPlate()
    {
        PlayClip(mouseClickClip);
        _uiDictionary.Values.ToList().ForEach(obj => obj.SetActive(false));
        _uiDictionary[UIObjectName.MainPlate].SetActive(true);
        _uiButtons[UIButtonName.Play].Select();
    }
    
    public void GoBackToMainControls()
    {
        PlayClip(mouseClickClip);
        _uiDictionary[UIObjectName.SpecificControls].SetActive(false);
        _uiDictionary[UIObjectName.Controls].SetActive(true);
        _uiButtons[_lastControls].Select();
    }

    public void PlayGame()
    {
        HideCursor();
        PlayClip(startGameClip);
        _levelLoader.LoadLevel((int)LevelLoader.SceneName.ComicStart);
    }

    public void Quit()
    {
        PlayClip(mouseClickClip);
        Application.Quit();
    }
    
    private static void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private static void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
