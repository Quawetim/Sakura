using System.Threading;

namespace Sakura.Zoo.Animal.Movement
{
    public interface IMovementController
    {
        public void Start(CancellationToken ct);
    }
}