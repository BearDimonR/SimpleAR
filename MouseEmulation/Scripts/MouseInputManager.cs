using System;
using System.Collections;
using UnityEngine;

namespace SimpleAR.MouseEmulation.Scripts
{
    public class MouseInputManager : MonoBehaviour
    {
        #region Singleton

        protected static MouseInputManager _instance;

        public static MouseInputManager Instance => _instance;

        #endregion

        #region Events

        public static Action OnEventOccured;

        #endregion

        #region Variables

        public static float TimeSeconds = 0.5f;

        public GestureAction Action;
        public GestureContinuous Continuous;
        public Vector2 CursorPosition;
        public int Width = 50;
        public int Height = 75;
        public float Depth = 2f;

        private bool _isCoroutines = false;
        private bool _isGrab = false;
        
        #endregion

        #region UnityEvents

        protected void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
                gameObject.SetActive(false);
        }

        protected void Update()
        {

            CursorPosition = Input.mousePosition;
            
            bool down = Input.GetMouseButtonDown(0);
            bool up = Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.F);
            bool hold = Input.GetMouseButton(0);
            bool grab = Input.GetKeyDown(KeyCode.F);

            if (down)
            {
                StopAllCoroutines();
                StartCoroutine(Wait());
                _isCoroutines = true;
            }
            else if (grab)
            {
                StopAllCoroutines();
                _isCoroutines = false;
                _isGrab = true;
                Action = GestureAction.GrabAction;
            }
            else if (up && _isCoroutines)
            {
                Continuous = GestureContinuous.NoContinuous;
                Action = GestureAction.ClickAction;
            }
            else if (up && !_isCoroutines)
            {
                if (_isGrab)
                {
                    Action = GestureAction.ReleaseAction;
                    _isGrab = false;
                }
                else
                    Action = GestureAction.NoAction;
                Continuous = GestureContinuous.NoContinuous;
            }
            else if (hold && !_isCoroutines)
            {
                Continuous = GestureContinuous.Hold;
            }
            else
            {
                Continuous = GestureContinuous.NoContinuous;
                Action = GestureAction.NoAction;
            }
            
            OnEventOccured.Invoke();
        }

        #endregion

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(TimeSeconds);
            _isCoroutines = false;
        }

    }
}