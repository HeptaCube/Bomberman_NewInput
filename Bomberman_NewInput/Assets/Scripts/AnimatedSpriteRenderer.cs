using UnityEngine;


public class AnimatedSpriteRenderer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // 여기서 Sprite 형이 의미하는 것은 Sprite 라는 클래스를 의미한다. // 상속이 아님.
    public Sprite idleSprite;
    public Sprite[] animationSprites;

    // 애니메이션 시간, 애니메이션 프레임 변수
    public float animationTime = 0.25f;
    private int animationFrame;


    // 루프, idle 여부
    public bool loop = true;
    public bool idle = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }

    private void Start()
    {
        InvokeRepeating(nameof(NextFrame), animationTime, animationTime);
    }

    // 유니티와 관계없이 생성한 메서드.
    private void NextFrame()
    {
        animationFrame++;

        // 루프 = true 이고 animationFrame이 스프라이트 열 길이보다 작으면 실행.
        if (loop && animationFrame >= animationSprites.Length)
        {
            // 애니메이션 멈춤.
            animationFrame = 0;
        }


        // idle 상태일 경우 idle 스프라이트 출력.
        if (idle)
        {
            spriteRenderer.sprite = idleSprite;
        }
        // 버그 방지를 위한 else if 문이 포함됨.
        // 그렇지 않다면 스프라이트 열에 맞는 스프라이트를 출력.
        else if (animationFrame >= 0 && animationFrame < animationSprites.Length)
        {
            spriteRenderer.sprite = animationSprites[animationFrame];
        }
    }
}
