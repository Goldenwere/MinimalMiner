#pragma warning disable 0649
#pragma warning disable 0108

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MinimalMiner.Util
{
    public class CameraTracking : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        [SerializeField] private Camera camera;
        [SerializeField] private float dampTime;
        private Vector3 velocity = Vector3.zero;
        private GameState currState;

        private void OnEnable()
        {
            EventManager.OnUpdateGameState += UpdateGameState;
        }

        private void OnDisable()
        {
            EventManager.OnUpdateGameState -= UpdateGameState;
        }

        private void FixedUpdate()
        {
            if (currState == GameState.play)
            {
                Vector3 point = camera.WorldToViewportPoint(target.transform.position);
                Vector3 delta = target.transform.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
                Vector3 destination = transform.position + delta;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            }
        }

        private void UpdateGameState(GameState newState, GameState prevState)
        {
            currState = newState;
        }
    }
}