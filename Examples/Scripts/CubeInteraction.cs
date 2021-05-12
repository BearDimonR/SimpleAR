using UnityEngine;

namespace SimpleAR.Examples.Scripts
{
    public class CubeInteraction : ActionBase
    {
        [SerializeField] private Material materialUsual;

        [SerializeField] private Material materialCollide;

        [SerializeField] private GameObject spawn0Bj;

        [SerializeField] private string spawnTag = "AR";
        [SerializeField] private int spawnLayer = 8;

        [SerializeField] private string handTag = SimpleARHandCollider.ColliderTag;


        private Renderer _cubeRenderer;
        private bool _isInside;

        private void Start()
        {
            _cubeRenderer = GetComponent<Renderer>();
            _cubeRenderer.sharedMaterial = materialUsual;
            _cubeRenderer.material = materialUsual;
            _isInside = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(handTag)) return;
            _cubeRenderer.sharedMaterial = materialCollide;
            Handheld.Vibrate();
            _isInside = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(handTag)) return;
            _cubeRenderer.sharedMaterial = materialUsual;
            _isInside = false;
        }

        protected override void OnHandGrabbed()
        {
            if (_isInside)
                transform.parent = SimpleARHandCollider.Instance.transform;
        }

        protected override void OnHandReleased()
        {
            transform.parent = null;
        }

        protected override void OnHandClicked()
        {
            if (!_isInside) return;
            var transform1 = transform;
            var position = transform1.position;
            var cube = Instantiate(spawn0Bj, new Vector3(position.x,
                position.y + transform1.localScale.y / 1.5f,
                position.z), Quaternion.identity);
            cube.tag = spawnTag;
            cube.layer = spawnLayer;
        }

        protected override void OnHandHold()
        {
            if (_isInside)
                transform.Rotate(Vector3.up * (Time.deltaTime * 50), Space.World);
        }
    }
}