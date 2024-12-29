namespace MonoApp.Audio;

public interface IAudioSource
{
    public void Trigger();
    public void Release();
    public double Sample(double t);
}