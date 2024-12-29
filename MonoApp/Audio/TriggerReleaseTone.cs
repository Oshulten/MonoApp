using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoApp.Generic;

namespace MonoApp.Audio;

public class Envelope
{
    public enum ToneState
    {
        PreTrigger,
        Triggered,
        Released
    }

    private Func<double, double> _ads;
    private Func<double, double> _release;
    private double _suspensionTime;
    private ToneState _state = ToneState.PreTrigger;
    private double _time = 0;

    public record TimeAmplitude(double Time, double Amplitude);

    public Envelope(TimeAmplitude triggerPoint, TimeAmplitude suspensionPoint, double releaseTime)
    {
        _ads = MetaFunctions.LinearMultipartFunction([
            new(0, 0), 
            new(triggerPoint.Time, triggerPoint.Amplitude),
            new(suspensionPoint.Time, suspensionPoint.Amplitude)]);

        _release = MetaFunctions.LinearMultipartFunction([
            new(0, suspensionPoint.Amplitude),
            new(releaseTime, 0)
        ]);

        _suspensionTime = suspensionPoint.Time;        
    }

    public void Trigger()
    {
        _time = 0;
        _state = ToneState.Triggered;
    }

    public void Release()
    {
        _time = 0;
        _state = ToneState.Released;
    }

    public double GetAmplitude(double delta)
    {
        _time += delta;

        switch (_state)
        {
            case ToneState.Triggered:
                return _ads(_time);
            case ToneState.Released:
                return _release(_time);
        }

        return 0;
    }
}

public class TriggerRelaseTone : IAudioSource
{
    public Func<double, double> FrequencyFunction { get; set; }
    public Func<double, double, double> SampleFunction { get; set; }
    public Envelope Envelope { get; set; }

    public TriggerRelaseTone()
    {
        Envelope = new(new(1, 0.5), new(3, 0.25), 2);
        FrequencyFunction = (t) => 220 + t;
        SampleFunction = (t, frequency) => Math.Sin(t * Math.PI * frequency * 2);
    }

    public void Trigger() => Envelope.Trigger();

    public void Release() => Envelope.Release();

    public virtual double Sample(double t)
    {
        return Envelope.GetAmplitude(t) * SampleFunction(t, FrequencyFunction(t));
    }
}