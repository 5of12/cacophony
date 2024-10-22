namespace Cacophony
{
    public abstract class IGestureDetector<T> : IDetectionSource
    {
        public abstract void Evaluate(T input);

        public abstract void EnableDetector();
        public abstract void DisableDetector();
    }
}