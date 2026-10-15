using Sakura.Zoo.Helper;
using System.Threading;

namespace Sakura.Zoo.Animal.Movement
{
    public abstract class MovementController : IMovementController
    {
        protected readonly ScreenBoundsChecker _screenBoundsChecker;

        protected MovementController(ScreenBoundsChecker screenBoundsChecker)
        {
            _screenBoundsChecker = screenBoundsChecker;
        }

        public abstract void Start(CancellationToken ct);
    }
}