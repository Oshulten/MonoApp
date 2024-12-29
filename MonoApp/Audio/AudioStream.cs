using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoApp.Generic;

namespace MonoApp.Audio;

public class AudioStream
{
    private DynamicSoundEffectInstance _soundEffectInstance;
    private TimeSpan _bufferDuration = TimeSpan.FromMilliseconds(20);
    private AudioChannels _audioChannels;
    private int _sampleRate;
    public double Time { get; set; } = 0;
    public Func<double, double> FrequencyFunction { get; set; }
    public Func<double, double> AmplitudeFunction { get; set; }
    public Func<double, double, double> SampleFunction { get; set; }

    public List<IAudioSource> Sources { get; set; } = [];

    public AudioStream(
        int sampleRate = 44100,
        AudioChannels audioChannels = AudioChannels.Stereo)
    {
        _sampleRate = sampleRate;
        _audioChannels = audioChannels;
        _soundEffectInstance = new DynamicSoundEffectInstance(sampleRate, audioChannels);

        FrequencyFunction = (t) => 220 + t;
        AmplitudeFunction = (t) => 0.5 + Math.Sin(t * 5) * 0.5;
        SampleFunction = (t, frequency) => Math.Sin(t * Math.PI * frequency * 2);
    }

    public double[,] GenerateBuffer()
    {
        var duration = _bufferDuration.TotalSeconds;
        var samples = (int)(_sampleRate * _bufferDuration.TotalSeconds);
        double sampleDuration = duration / samples;
        var buffer = new double[1, samples];

        double t = Time;
        double maxAmp = 0;
        foreach (var i in Iterators.Integral.Range(0, samples))
        {
            buffer[0, i] = 0;

            foreach (var source in Sources)
            {
                buffer[0, i] += source.Sample(t);
            }

            if (buffer[0, i] > maxAmp)
                maxAmp = buffer[0, i];

            t += sampleDuration;
        }

        foreach (var i in Iterators.Integral.Range(0, samples))
        {
            // buffer[0, i] /= maxAmp; //(1.0 / Math.PI) * Math.Atan(buffer[0, i]);
        }

        Time += duration;

        return buffer;
    }

    private void SubmitBuffer()
    {
        var buffer = GenerateBuffer();
        var monoBuffer = ConvertBuffer(buffer);
        _soundEffectInstance.SubmitBuffer(monoBuffer);
    }

    private static byte[] ConvertBuffer(double[,] buffer)
    {
        const int bytesPerSample = 2;
        int channels = buffer.GetLength(0);
        int samplesPerBuffer = buffer.GetLength(1);
        byte[] monoBuffer = new byte[channels * samplesPerBuffer * 2];

        for (int i = 0; i < samplesPerBuffer; i++)
        {
            for (int c = 0; c < channels; c++)
            {
                float floatSample = MathHelper.Clamp((float)buffer[c, i], -1.0f, 1.0f);
                short shortSample = (short)(floatSample >= 0.0f ? floatSample * short.MaxValue : floatSample * short.MinValue * -1);
                int index = i * channels * bytesPerSample + c * bytesPerSample;

                if (!BitConverter.IsLittleEndian)
                {
                    monoBuffer[index] = (byte)(shortSample >> 8);
                    monoBuffer[index + 1] = (byte)shortSample;
                }
                else
                {
                    monoBuffer[index] = (byte)shortSample;
                    monoBuffer[index + 1] = (byte)(shortSample >> 8);
                }
            }
        }

        return monoBuffer;
    }

    public void Update()
    {
        while (_soundEffectInstance.PendingBufferCount < 10)
        {
            SubmitBuffer();
        }
    }

    public void Play() =>
        _soundEffectInstance.Play();

    public void Pause() =>
        _soundEffectInstance.Pause();

    public void Toggle()
    {
        switch (_soundEffectInstance.State)
        {
            case SoundState.Playing:
                _soundEffectInstance.Pause();
                break;
            case SoundState.Paused:
                _soundEffectInstance.Play();
                break;
        }
    }
}