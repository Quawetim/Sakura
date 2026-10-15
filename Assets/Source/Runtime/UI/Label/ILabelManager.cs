using Sakura.Zoo.Animal;

namespace Sakura.Zoo.UI.Label
{
    public interface ILabelManager
    {
        public void Show(ITransformProvider transformProvider, float offset = -0.5f);
    }
}