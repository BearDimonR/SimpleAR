using System;
using UnityEngine;

namespace SimpleAR.Examples.Scripts
{
    public class CubeInteraction : ActionBase
    {
        
        [SerializeField]
        private Material _materialUsual;
        [SerializeField]
        private Material _materialCollide;
        [SerializeField]
        private GameObject _spawn0bj;
        [SerializeField] private String _spawnTag = "AR";
        [SerializeField] private int _spawnLayer = 8;
        
        [SerializeField] 
        private String _handTag = SimpleARHandCollider.ColliderTag;


        private Renderer _cubeRenderer;
        private bool _isInside;

        private void Start()
        {
            _cubeRenderer = GetComponent<Renderer>();
            _cubeRenderer.sharedMaterial = _materialUsual;
            _cubeRenderer.material = _materialUsual;
            _isInside = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_handTag)) return;
            _cubeRenderer.sharedMaterial = _materialCollide;
            Handheld.Vibrate();
            _isInside = true;

        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(_handTag)) return;
            _cubeRenderer.sharedMaterial = _materialUsual;
            _isInside = false;
        }

        protected override void OnHandGrabbed()
        {
            base.OnHandGrabbed();
            if(_isInside)
                transform.parent = SimpleARHandCollider.Instance.transform;
        }

        protected override void OnHandReleased()
        {
            base.OnHandReleased();
            transform.parent = null;
        }

        protected override void OnHandClicked()
        {
            base.OnHandClicked();
            if (!_isInside) return;
            var transform1 = transform;
            var position = transform1.position;
            GameObject cube = Instantiate(_spawn0bj, new Vector3(position.x, 
                position.y + transform1.localScale.y / 1.5f, 
                position.z), Quaternion.identity);
            cube.tag = _spawnTag;
            cube.layer = _spawnLayer;
        }

        protected override void OnHandHold()
        {
            base.OnHandHold();
            if(_isInside)
                transform.Rotate(Vector3.up * (Time.deltaTime * 50), Space.World);
        }
    }
}