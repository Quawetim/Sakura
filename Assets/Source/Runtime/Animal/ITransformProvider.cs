using UnityEngine;

namespace Sakura.Zoo.Animal
{
    public interface ITransformProvider
    {
        public Transform Transform { get; }
    }
}