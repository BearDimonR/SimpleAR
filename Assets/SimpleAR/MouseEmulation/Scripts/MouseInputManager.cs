using System;
using System.Collections;
using UnityEngine;

namespace SimpleAR.MouseEmulation.Scripts
{
    public class MouseInputManager : MonoBehaviour
    {
        #region Events

        public static Action OnEventOccured;

        #endregion

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(TimeSeconds);
            _isCoroutines = false;
        }

        #region Singleton

        public static MouseInputManager Instance { get; private set; }

        #endregion

        #region Variables

        public static float TimeSeconds = 0.5f;

        public GestureAction action;
        public GestureContinuous continuous;
        public Vector2 cursorPosition;
        public int width = 50;
        public int height = 75;
        public float depth = 2f;

        private bool _isCoroutines;
        private bool _isGrab;

        #endregion

        #region UnityEvents

        protected void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                gameObject.SetActive(false);
        }

        protected void Update()
        {
            cursorPosition = Input.mousePosition;

            var down = Input.GetMouseButtonDown(0);
            var up = Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.F);
            var hold = Input.GetMouseButton(0);
            var grab = Input.GetKeyDown(KeyCode.F);

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
                action = GestureAction.GrabAction;
            }
            else if (up && _isCoroutines)
            {
                continuous = GestureContinuous.NoContinuous;
                action = GestureAction.ClickAction;
            }
            else if (up && !_isCoroutines)
            {
                if (_isGrab)
                {
                    action = GestureAction.ReleaseAction;
                    _isGrab = false;
                }
                else
                {
                    action = GestureAction.NoAction;
                }

                continuous = GestureContinuous.NoContinuous;
            }
            else if (hold && !_isCoroutines)
            {
                continuous = GestureContinuous.Hold;
            }
            else
            {
                continuous = GestureContinuous.NoContinuous;
                action = GestureAction.NoAction;
            }

            OnEventOccured.Invoke();
        }

        #endregion
    }
}