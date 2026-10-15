namespace BadPractice.Cursoriam.Helper.Pool
{
    public interface ICursoriamPoolableBase
    {
        public void OnDespawned() { }
    }

    public interface ICursoriamPoolable : ICursoriamPoolableBase
    {
        public void OnSpawned() { }
    }

    public interface ICursoriamPoolable<in TParam1> : ICursoriamPoolableBase
    {
        public void OnSpawned(TParam1 p1) { }
    }
}