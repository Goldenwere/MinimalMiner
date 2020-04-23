#pragma warning disable 0649

using System;
using UnityEngine;

namespace MinimalMiner.Util
{
    /// <summary>
    /// EventManager passes event related information to managers that depend on them. This primarily consists of UI and Input calls.
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        #region Fields & Properties
        #region Events
        public delegate void OnSelectThemeDelegate(int themeIndex);
        /// <summary>
        /// Informs subscribed objects that a Theme was selected
        /// </summary>
        public static event OnSelectThemeDelegate OnSelectTheme = delegate { };

        public delegate void OnTargetLockForceValueDelegate(float force);
        /// <summary>
        /// Informs subscribed objects that the TargetLockForce value has changed
        /// </summary>
        public static event OnTargetLockForceValueDelegate OnTargetLockForceValueChanged = delegate { };

        public delegate void OnUpdateGameStateDelegate(GameState newState, GameState prevState);
        /// <summary>
        /// Informs subscribed objects that the current GameState was changed
        /// </summary>
        public static event OnUpdateGameStateDelegate OnUpdateGameState = delegate { };

        public delegate void OnUpdateHUDElementDelegate(HUDElement element, string content);
        /// <summary>
        /// Informs subscribed objects that a HUD element needs updated
        /// </summary>
        public static event OnUpdateHUDElementDelegate OnUpdateHUDElement = delegate { };

        public delegate void OnUpdateTargetDelegate(bool isEnabled, Transform target, Transform source, Rigidbody2D rigidbody);
        /// <summary>
        /// Informs subscribed objects that the target element needs updated
        /// </summary>
        public static event OnUpdateTargetDelegate OnUpdateTarget = delegate { };

        public delegate void OnUpdatePlayModeDelegate(PlayMode mode);
        /// <summary>
        /// Informs subscribed objects that the play mode was changed
        /// </summary>
        public static event OnUpdatePlayModeDelegate OnUpdatePlayMode = delegate { };
        #endregion

        /// <summary>
        /// The current GameState that the game is in
        /// </summary>
        public GameState CurrState
        {
            get; private set;
        }

        /// <summary>
        /// The current PlayMode that the game is in
        /// </summary>
        public PlayMode CurrMode
        {
            get; private set;
        }

        /// <summary>
        /// The current control preferences
        /// </summary>
        public InputDefinitions Controls
        {
            get; private set;
        }

        public static EventManager Instance
        {
            get; private set;
        }

        // Refers to the current player preferences
        [SerializeField] private PreferencesManager playerPrefs;
        #endregion

        #region Methods
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        /// <summary>
        /// Handles the start of the object before the first frame
        /// </summary>
        private void Start()
        {
            Controls = playerPrefs.Controls;
            UpdateGameState(GameState.main);
        }

        /// <summary>
        /// Updates once per frame
        /// </summary>
        private void Update()
        {
            if (CurrState == GameState.play)
            {
                if (Input.GetKeyDown(Controls.Menu_Pause))
                {
                    UpdateGameState(GameState.pause);
                }
            }

            else if (CurrState == GameState.pause)
            {
                if (Input.GetKeyDown(Controls.Menu_Pause))
                {
                    UpdateGameState(GameState.play);
                }
            }
        }

        /// <summary>
        /// Updates the current GameState and passes this update to relevant objects
        /// </summary>
        /// <param name="newState">The new state to update to</param>
        public void UpdateGameState(GameState newState)
        {
            GameState prevState = CurrState;
            CurrState = newState;
            OnUpdateGameState(newState, prevState);
            UpdateTimeState();
        }

        /// <summary>
        /// Updates the current GameState and passes this update to relevant objects
        /// </summary>
        /// <param name="desiredState">The desired state to update to</param>
        public void UpdateGameState(string desiredState)
        {
            Enum.TryParse<GameState>(desiredState, out GameState newState);
            GameState prevState = CurrState;
            CurrState = newState;
            OnUpdateGameState(newState, prevState);
            UpdateTimeState();
        }

        /// <summary>
        /// Updates the current PlayMode and passes this update to relevant objects
        /// </summary>
        /// <param name="newMode">The new PlayMode</param>
        public void UpdatePlayMode(PlayMode newMode)
        {
            CurrMode = newMode;
            OnUpdatePlayMode(newMode);
        }

        /// <summary>
        /// Updates the current PlayMode and passes this update to relevant objects
        /// </summary>
        /// <param name="newMode">The new PlayMode</param>
        public void UpdatePlayMode(string desiredMode)
        {
            Enum.TryParse<PlayMode>(desiredMode, out PlayMode newMode);
            CurrMode = newMode;
            OnUpdatePlayMode(newMode);
        }

        /// <summary>
        /// Updates the time scale of the game based on incoming GameState
        /// </summary>
        private void UpdateTimeState()
        {
            if (CurrState == GameState.pause)
            {
                Time.timeScale = 0;
            }

            else
            {
                Time.timeScale = 1;
            }
        }

        /// <summary>
        /// Handles a theme selection in the settings menu
        /// </summary>
        /// <param name="themeIndex">The index of the theme selected in the settings menu</param>
        public void UpdateTheme(int themeIndex)
        {
            OnSelectTheme(themeIndex);
        }

        /// <summary>
        /// Updates the TargetLockForce value
        /// </summary>
        /// <param name="val">The value that it was updated to</param>
        public void UpdateTargetLockForceValue(float val)
        {
            OnTargetLockForceValueChanged(val);
        }

        /// <summary>
        /// Quits the game
        /// </summary>
        public void CallQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Can be used to notify the HUD to update an element
        /// </summary>
        /// <param name="element">The element to update</param>
        /// <param name="content">The content to update the element with</param>
        public void UpdateHUDElement(HUDElement element, string content)
        {
            OnUpdateHUDElement(element, content);
        }

        /// <summary>
        /// Can be used to notify the HUD to update the target HUD element
        /// <para>Note: All references are required if isEnabled is true; otherwise, null is sufficient</para>
        /// </summary>
        /// <param name="isEnabled">Whether the element is enabled or disabled</param>
        /// <param name="target">The target transform being locked on to</param>
        /// <param name="source">The source transform (i.e. the player in most circumstances)</param>
        /// <param name="rigidbody">The target's rigidbody</param>
        public void UpdateTargetElement(bool isEnabled, Transform target, Transform source, Rigidbody2D rigidbody)
        {
            OnUpdateTarget(isEnabled, target, source, rigidbody);
        }
        #endregion
    }
}