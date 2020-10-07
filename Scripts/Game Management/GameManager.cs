using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text fpsText;
    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private Button firstButton;
    [SerializeField] private Reticle reticle;
    private SavingWrapper _savingWrapper;
    private PlayerControls _playerControls;
    private LevelLoader _levelLoader;

    private bool _isInGameMenuEnabled = false;
    private bool _cursorHidden = false;

    private void Awake()
    {
        _playerControls = new PlayerControls();

        _playerControls.Gameplay.Menu.performed += ctx =>
        {
            _isInGameMenuEnabled = !_isInGameMenuEnabled;
            if (!_isInGameMenuEnabled)
            {
                Time.timeScale = 0;
                reticle.IsReticleEnabled = false;
                ShowCursor();
                firstButton.Select();
                DOVirtual.Float(menuCanvasGroup.alpha, 1, 1f, x => menuCanvasGroup.alpha = x).SetUpdate(true);
            }
            else
            {
                ResumeGame();
            }
        };
    }

    private void Start()
    {
        _savingWrapper = FindObjectOfType<SavingWrapper>();
        _levelLoader = FindObjectOfType<LevelLoader>();
        _savingWrapper.Save();
    }

    private void Update()
    {
        if (Cursor.visible && !_cursorHidden)
        {
            _cursorHidden = true;
           HideCursor();
        }

        if (!fpsText) return;
        fpsText.text = $"FPS: {(1 / Time.unscaledDeltaTime):0}";
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
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

    public void ResumeGame()
    {
        Time.timeScale = 1;
        reticle.IsReticleEnabled = true;
        _cursorHidden = false;
        DOVirtual.Float(menuCanvasGroup.alpha, 0, 1f, x => menuCanvasGroup.alpha = x);
    }

    public void GoToMainMenu()
    {
        ResumeGame();
        _levelLoader.LoadLevel((int)LevelLoader.SceneName.MainMenu);
    }

    public void Respawn()
    {
        _savingWrapper.Load();
        ResumeGame();
    }
}
