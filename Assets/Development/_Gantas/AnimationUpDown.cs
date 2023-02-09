using UnityEngine;

namespace MoonGale
{
    public class AnimationUpDown : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 1)]
        private float speed = 0.4f;

        [SerializeField]
        [Range(0, 100)]
        private float range = 0.8f;

        [SerializeField]
        [Range(0, 100)]
        private float smoothingSpeed = 2f;

        private float targetYOffset;
        private float timeOffset;

        private Vector3 initialPos;
        private Vector3 targetPos;

        private void Awake()
        {
            // Add slight time offset to make up, down animation more varied.
            timeOffset = Random.Range(0, range);

            initialPos = transform.position;
        }

        private void Update()
        {
            UpdateTargetPos();
            UpdatePos();
        }

        private void UpdateTargetPos()
        {
            targetYOffset = Mathf.PingPong(Time.time * speed + timeOffset, 1f) * range;
            targetPos = new Vector3(initialPos.x, initialPos.y + targetYOffset, initialPos.z);
        }

        private void UpdatePos()
        {
            var currentPos = transform.position;
            transform.position = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * smoothingSpeed);
        }
    }
}
