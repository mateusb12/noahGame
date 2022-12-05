using UnityEngine;

namespace Core
{
    public class LimitFps: MonoBehaviour
    {
        private void Start()
        {
            // Set the target frame rate to 60 FPS
            Application.targetFrameRate = 60;
        }
    }
}
