using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAnimationController : MonoBehaviour
{
    public AnimalSprites sprites;
    [SerializeField] Gender gender;
    [SerializeField] Direction dir;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        AnimalPack tmp;
        switch (gender)
        {
            case Gender.male:
                tmp = sprites.male;
                break;
            case Gender.female:
                tmp = sprites.female;
                break;
            default:
                tmp = sprites.child;
                break;
        }
        switch(dir)
        {
            case Direction.up:
                SetUpSprites(tmp.up);
                break;
            case Direction.down:
                SetUpSprites(tmp.down);
                break;
            case Direction.side:
                SetUpSprites(tmp.side);
                break;
        }

        
    }


    #region Sprites SetUp
    [SerializeField] SpriteRenderer body;
    [SerializeField] SpriteRenderer face;
    [SerializeField] SpriteRenderer eyes;
    [SerializeField] SpriteRenderer right_leg;
    [SerializeField] SpriteRenderer left_leg;
    [SerializeField] SpriteRenderer right_hand;
    [SerializeField] SpriteRenderer left_hand;
    [SerializeField] SpriteRenderer tail;
    void SetUpSprites(DirectionPack pack)
    {
        body.sprite = pack.body;
        face.sprite = pack.face;
        eyes.sprite = pack.eyes;
        right_leg.sprite = pack.right_leg;
        left_leg.sprite = pack.left_leg;
        right_hand.sprite = pack.right_hand;
        left_hand.sprite = pack.left_hand;
        tail.sprite = pack.tail;
    }
    #endregion
}
public enum Gender
{
    male,
    female,
    child
}
public enum Direction
{
    up,
    down,
    side
}
