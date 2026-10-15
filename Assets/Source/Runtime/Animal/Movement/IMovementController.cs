using Sakura.Zoo.Helper;
using System.Threading;

namespace Sakura.Zoo.Animal.Movement
{
    public interface IMovementController
    {
        public void Start(CancellationToken ct);
    }

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