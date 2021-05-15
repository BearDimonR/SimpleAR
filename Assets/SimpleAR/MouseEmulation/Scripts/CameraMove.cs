using UnityEngine;

namespace Eccentric {

    public class CameraMove : MonoBehaviour {

        [SerializeField]
        private float _mouseSensitivity = 0.4f;
        [SerializeField]
        private float _moveSpeed = 2f;

        private Vector3 _mousePreveousePos;
        private float _rotationX;
        private float _rotationY;


        void Update() {
            Move();
            Rotate();
        }

        void Move() {

            float shiftMult = 1f;
            if (Input.GetKey(KeyCode.LeftShift)) {
                shiftMult = 3f;
            }

            float right = Input.GetAxis("Horizontal");
            float forward = Input.GetAxis("Vertical");
            float up = 0;
            if (Input.GetKey(KeyCode.E)) {
                up = 1f;
            } else if (Input.GetKey(KeyCode.C)) {
                up = -1f;
            }

            Vector3 offset = new Vector3(right, up, forward) * _moveSpeed * shiftMult * Time.unscaledDeltaTime;
            transform.Translate(offset);
        }

        void Rotate() {

            Vector3 _mouseDelta;

            if (Input.GetMouseButtonDown(1)) {
                _mousePreveousePos = Input.mousePosition;
            }

            if (Input.GetMouseButton(1)) {
                _mouseDelta = Input.mousePosition - _mousePreveousePos;
                _mousePreveousePos = Input.mousePosition;

                _rotationX -= _mouseDelta.y * _mouseSensitivity;
                _rotationY += _mouseDelta.x * _mouseSensitivity;

                transform.localEulerAngles = new Vector3(_rotationX, _rotationY, 0f);
            }
        }

    }

}

