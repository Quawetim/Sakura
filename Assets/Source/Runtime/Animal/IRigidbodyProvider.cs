using UnityEngine;

namespace Sakura.Zoo.Animal
{
    public interface IRigidbodyProvider
    {
        public Rigidbody Rigidbody { get; }
    }
}