using Microsoft.Xna.Framework.Input;
using MonoApp.Generic;
using MonoApp.Input;
using System.Geometry;
using System.Numerics;

namespace MonoApp.Audio;

public class AudioManager : IAudioManager
{
    private IInputManager _inputManager;
    public List<AudioStream> AudioStreams { get; set; } = [];

    public AudioManager(IInputManager inputManager)
    {
        _inputManager = inputManager;
        _inputManager
            .RegisterEvent(Keys.P, Input.KeyTrigger.Pressed, () =>
            {
                foreach (var stream in AudioStreams)
                {
                    stream.Toggle();
                }
            })
            .RegisterEvent(Keys.Z, KeyTrigger.Pressed, () =>
            {
                foreach (var source in AudioStreams[0].Sources)
                {
                    source.Trigger();
                }
            })
            .RegisterEvent(Keys.Z, KeyTrigger.Released, () =>
            {
                foreach (var source in AudioStreams[0].Sources)
                {
                    source.Release();
                }
            });
    }

    public static double Harmonics(double t, double frequency)
    {
        double sum = 0;

        foreach (var (amp, freq) in Enumerable.Zip(
            Iterators.Decimal.GeometricSeries(1.0, 0.6, 5),
            Iterators.Decimal.GeometricSeries(frequency, 2, 5)
        ))
        {
            sum += amp * Math.Sin(freq * t * Math.PI * 2);
        }

        return sum;
    }

    public static double Multitone(IEnumerable<double> times, IEnumerable<double> frequencies, Func<double, double, double> toneFunction)
    {
        double sum = 0;

        foreach (var (time, frequency) in Enumerable.Zip(times, frequencies))
        {
            sum += toneFunction(time, frequency);
        }

        return sum;
    }

    public static double ExponentialDistribution(double lambda, double t) =>
        lambda * Math.Pow(Math.E, -lambda * t);

    public void Setup()
    {
        Vector2 vec = new(0, 0);
        var curve = new Bezier([new(0, 0), new(0.2f, 1), new(2, 0)]);
        AudioStreams.Add(new());
        AudioStreams[^1].Sources.AddRange([
            new TriggerRelaseTone()
            {
                FrequencyFunction = (t) => 100,
                SampleFunction = (t, f) => Harmonics(t, f)
            },
            new TriggerRelaseTone()
        ]);

        foreach (var stream in AudioStreams)
        {
            stream.Play();
        }
    }

    public void Update()
    {
        foreach (var stream in AudioStreams)
        {
            stream.Update();
        }
    }
}