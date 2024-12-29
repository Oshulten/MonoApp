using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Input;

namespace MonoApp.Input;

public enum KeyTrigger
{
    Up,
    Pressed,
    Down,
    Released
}

public class InputManager : IInputManager
{
    private ILogger<InputManager> _logger;
    private KeyboardState _lastKeyboardState = Keyboard.GetState();
    public Dictionary<Keys, KeyTrigger> KeyStates { get; set; } = [];
    private Dictionary<Keys, Dictionary<KeyTrigger, List<Action>>> KeyEvents { get; set; } = [];

    public InputManager(ILogger<InputManager> logger)
    {
        _logger = logger;

        foreach (var key in Enum.GetValues<Keys>())
        {
            KeyStates[key] = KeyTrigger.Up;

            KeyEvents[key] = new() {
                {
                    KeyTrigger.Up, []
                },
                {
                    KeyTrigger.Pressed,
                    [() => _logger.LogInformation($"{key}: {KeyTrigger.Pressed}")]
                },
                {
                    KeyTrigger.Down, []
                },
                {
                    KeyTrigger.Released,
                    [() => _logger.LogInformation($"{key}: {KeyTrigger.Released}")]
                },
            };
        }
    }

    public IInputManager RegisterEvent(Keys key, KeyTrigger state, Action action) 
    {
        KeyEvents[key][state].Add(action);
        return this;
    }

    public void Update()
    {
        var keyboardState = Keyboard.GetState();

        foreach (var key in Enum.GetValues<Keys>())
        {
            KeyStates[key] = (_lastKeyboardState[key], keyboardState[key]) switch
            {
                (KeyState.Up, KeyState.Down) => KeyTrigger.Pressed,
                (KeyState.Down, KeyState.Down) => KeyTrigger.Down,
                (KeyState.Down, KeyState.Up) => KeyTrigger.Released,
                (_, _) => KeyTrigger.Up,
            };

            var state = KeyStates[key];

            foreach (var action in KeyEvents[key][state])
                action();
        }

        _lastKeyboardState = keyboardState;
    }
}