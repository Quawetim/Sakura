using System;

namespace Sakura.Zoo.Manager
{
    public interface IAnimalManager
    {
        public int GetDeaths(Type type);

        public event Action<Type> onAnimalDeath;
    }
}