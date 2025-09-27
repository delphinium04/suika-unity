using System;

public class Managers : Singleton<Managers>
{
    public static GameManager Game { get; private set; }

    public static InputManager Input { get; private set; }
    public static SuikaManager Suika { get; private set; }
    public static UIManager UI { get; private set; }

    public override void Awake()
    {
        base.Awake();

        Game = GetComponent<GameManager>();
        Input = GetComponent<InputManager>();
        Suika = GetComponent<SuikaManager>();
        UI = GetComponent<UIManager>();
    }
}