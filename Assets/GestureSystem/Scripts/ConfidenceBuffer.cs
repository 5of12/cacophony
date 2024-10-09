using System.Collections.Generic;

public class ConfidenceBuffer 
{
    private float _confidence;
    private float _bufferLength;

    private List<float> _confidenceBuffer;

    public ConfidenceBuffer (float length)
    {
        _bufferLength = length;
        _confidenceBuffer = new List<float>();
    }

    public float SmoothConfidence(float input)
    {
        if (_confidenceBuffer.Count >= _bufferLength)
        {
            _confidenceBuffer.RemoveAt(0);
        }
        _confidenceBuffer.Add(input);

        float sum = 0;
        foreach(var sample in _confidenceBuffer)
        {
            sum += sample;
        }
        _confidence = sum / _bufferLength;

        return _confidence;
    }

    public void Reset()
    {
        _confidenceBuffer.Clear();
    }
}