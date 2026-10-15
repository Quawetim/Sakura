using System;
using System.Threading;

namespace Sakura.Zoo.Animal.Behaviour
{
    public interface IAnimalBehaviour
    {
        public void Start(CancellationToken ct);

        public Type InterfaceType { get; }
    }
}