using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Suika : MonoBehaviour
{
    public bool IsMoving { get; private set; }
    int Level { get; set; }
    int _maxLevel = 5;

    public Color[] colors = new Color[5] { Color.white, Color.magenta, Color.cyan, Color.red, Color.yellow };

    // Components
    Rigidbody2D _rigidbody2D;
    readonly float _velocityThreshold = 1f;
    readonly float _levelScaleIncrease = 0.5f; // 1 -> 1.5 -> 2 -> ...

    void Awake()
    {
        IsMoving = false;
        Level = 1;
    }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        IsMoving = _rigidbody2D.linearVelocity.magnitude > _velocityThreshold;

        if (transform.position.y < -15)
        {
            GameManager.Instance.EndGame();
        }
    }

    void Upgrade(Suika other)
    {
        if (GetInstanceID() < other.GetInstanceID()) return;
        Vector3 otherPosition = other.transform.position;
        Destroy(other.gameObject);

        Level++;
        if (Level > _maxLevel)
        {
            GameManager.Instance.AddScore(Level * 2);
            Destroy(gameObject);
        }
        
        transform.position = (otherPosition + transform.position) / 2;
        float scale = 1f + (Level - 1) * _levelScaleIncrease;
        transform.localScale = Vector3.one * scale;
        gameObject.GetComponent<SpriteRenderer>().color = colors[Level-1];

        GameManager.Instance.AddScore(Level * 2);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag(tag))
            return;

        var otherSuika = collision.gameObject.GetComponent<Suika>();
        if (otherSuika.Level == Level) Upgrade(otherSuika);
    }
}