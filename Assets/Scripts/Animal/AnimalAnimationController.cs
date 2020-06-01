using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAnimationController : MonoBehaviour
{
    public AnimalSprites sprites;
    [SerializeField, Range(0,1)] private float verticalThreshhold;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Sprite[] foods;
    [SerializeField] private SpriteRenderer foodSprite;
    [SerializeField] private Shader normal, highlighted;
    private Animator anim;
    private AnimalData data;
    private AnimalPack GenderPack
    {
        get
        {
            if (data == null) data = GetComponent<AnimalDataHolder>().data;
            return data.male ? sprites.male : sprites.female;
        }
    }

    private Material currentMat;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        data = GetComponent<AnimalDataHolder>().data;
        Apply();
        currentMat = new Material(normal);
        body.material = currentMat;
        face.material = currentMat;
        eyes.material = currentMat;
        right_leg.material = currentMat;
        left_leg.material = currentMat;
        right_hand.material = currentMat;
        left_hand.material = currentMat;
        tail.material = currentMat;
    }
    public void SetHighlight()
    {
        currentMat.shader = highlighted;

    }
    public void SetBase()
    {
        currentMat.shader = normal;
    }
    public void Apply()
    {
        SetUp(GenderPack.down);
    }
	public void FlipX()
	{
		bodyTransform.localScale = new Vector3(bodyTransform.localScale.x*-1f,bodyTransform.localScale.y,bodyTransform.localScale.z);
	}
    public void Walk(Vector2 dir)
    {
        dir = dir.normalized;
        string animation;
        if (Mathf.Abs(dir.y) > verticalThreshhold)
        {
            animation = dir.y > 0 ? "Walk_up" : "Walk_down";
            SetUp(dir.y > 0 ? GenderPack.up : GenderPack.down);
        }
        else
        {
            animation = dir.x > 0 ? "Walk_right" : "Walk_left";
            SetUp(GenderPack.side);
        }
        anim.Play(animation);

    }
    public void Idle()
    {
        SetUp(GenderPack.down);
        anim.Play("idle");
    }
    public void Eat(Food type, bool right)
    {
        SetUp(GenderPack.side);
        foodSprite.sprite = foods[(int)type];
        anim.Play(right ? "Eat_right" : "Eat_left");
    }
    public float DoSpecial(SpecialItem item)
    {
        var animator = item.GetComponent<Animator>();
        if (animator != null)
            animator.Play("Action");
        switch(item.type)
        {
            case Special.jump:
                SetUp(GenderPack.side);
                anim.Play("Jump");
                return 6f;
			case Special.swim:
				SetUp(GenderPack.side);
				anim.Play("Swimming");
                return 9.67f;
			case Special.run:
				SetUp(GenderPack.side);
				anim.Play("Run");
                return 5f;
			case Special.sleep:
				SetUp(GenderPack.down);
				anim.Play("Sleep");
                return 6f;
            default:
                return 0;
        }
    }
    public void Mate()
    {
        SetUp(GenderPack.side);
        if (data.male)
            anim.Play("Sex");
        else
            anim.Play("Sex_female");
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
    void SetUp(DirectionPack pack)
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
public enum Direction
{
    up,
    down,
    left,
    right

}
