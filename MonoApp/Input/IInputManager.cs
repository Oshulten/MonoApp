using Microsoft.Xna.Framework.Input;

namespace MonoApp.Input;

public interface IInputManager
{
    public Dictionary<Keys, KeyTrigger> KeyStates { get; set; }
    public IInputManager RegisterEvent(Keys key, KeyTrigger state, Action action);
    public void Update();
}