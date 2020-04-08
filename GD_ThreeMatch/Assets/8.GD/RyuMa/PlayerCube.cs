using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCube : MonoBehaviour
{

    public Direction direction;

    public Sprite[] PlayerSprites;

    private SpriteRenderer PlayerSprite;
    private void Start()
    {
        PlayerSprite = GetComponent<SpriteRenderer>();
    }

    public void ChangeSprite(Direction _direction)
    {
        PlayerSprite.sprite = PlayerSprites[(int)_direction];
        direction = _direction;
    }



}
