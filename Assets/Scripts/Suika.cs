using UnityEngine;

public class Suika : MonoBehaviour
{
    // Components
    Rigidbody2D _rigidbody2D;

    public bool IsMoving { get; private set; }
    public Color[] colors = { Color.white, Color.red, Color.cyan, Color.magenta, Color.yellow, Color.gray };

    readonly float _velocityThreshold = 1f;
    readonly float _levelScaleIncrease = 0.5f; // 1 -> 1.5 -> 2 -> ...
    int _maxLevel = 6;
    int _level;

    void Awake()
    {
        IsMoving = false;

        _level = Random.Range(1, 4);
    }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        SettingLevel();
    }

    void FixedUpdate()
    {
        IsMoving = _rigidbody2D.linearVelocity.magnitude > _velocityThreshold;
    }

    void SettingLevel()
    {
        float scale = 1f + (_level - 1) * _levelScaleIncrease;
        transform.localScale = Vector3.one * scale;
        gameObject.GetComponent<SpriteRenderer>().color = colors[_level - 1];
    }

    void Upgrade(Suika other)
    {
        if (GetInstanceID() < other.GetInstanceID()) return;
        Vector3 otherPosition = other.transform.position;
        Destroy(other.gameObject);

        _level++;
        GameManager.Instance.AddScore(_level * 2);
        if (_level > _maxLevel)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = (otherPosition + transform.position) / 2;
        SettingLevel();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag(tag))
            return;

        var otherSuika = collision.gameObject.GetComponent<Suika>();
        if (otherSuika._level == _level) Upgrade(otherSuika);
    }
}