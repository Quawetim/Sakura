using UnityEngine;
using Zenject;

namespace Sakura.Zoo.Helper
{
    public sealed class ScreenBoundsChecker : IInitializable
    {
        private (float min, float max) _dx;
        private (float min, float max) _dz;

        public void Initialize()
        {
            var mainCamera = Camera.main;
            if (!mainCamera)
                return;

            var distanceToGround = mainCamera.transform.position.y;
            var bottomLeft = mainCamera.ViewportToWorldPoint(new(0, 0, distanceToGround));
            var topRight = mainCamera.ViewportToWorldPoint(new(1, 1, distanceToGround));

            _dx = (bottomLeft.x, topRight.x);
            _dz = (bottomLeft.z, topRight.z);
        }

        public bool IsInBounds(Vector3 position)
        {
            return position.x > _dx.min && position.x < _dx.max && position.z > _dz.min && position.z < _dz.max;
        }
    }
}
