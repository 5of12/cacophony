using Cacophony;

public class MockDetectionSource : IDetectionSource
{
    public void Init()
    {
        OnStart = new();
        OnHold = new();
        OnEnd = new();
        OnCancel = new();
    }
}