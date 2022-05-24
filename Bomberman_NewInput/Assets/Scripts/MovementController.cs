using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    private Vector2 inputDirection = Vector2.zero;
    public float speed = 5f;


    // 위에 4개중 하나 대입 가능.
    private AnimatedSpriteRenderer activeSpriteRenderer;


    // 오브젝트 접속을 위한 public 변수 생성.
    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;

    public AnimatedSpriteRenderer spriteRendererDeath;


    // dictionary 콜렉션을 생성해줌.
    Dictionary<Vector2, AnimatedSpriteRenderer> spriteDictionary = new Dictionary<Vector2, AnimatedSpriteRenderer>();

    private void Awake()
    {
        // dictionary 콜렉션에 값을 더해줌.
        spriteDictionary.Add(Vector2.up, spriteRendererUp);
        spriteDictionary.Add(Vector2.down, spriteRendererDown);
        spriteDictionary.Add(Vector2.left, spriteRendererLeft);
        spriteDictionary.Add(Vector2.right, spriteRendererRight);

        // Rigidbody2D 컴포넌트를 찾음.
        rigidbody = GetComponent<Rigidbody2D>();

        // 시작할 때 스프라이트 상태를 정함.
        SetDirection(spriteRendererDown);
    }


    // 플레이어가 실제로 이동하는 스크립트.
    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = inputDirection * speed * Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);
    }


    // inputDirection에 값을 유니티가 알아서 넣어줌.
    public void SetDirection(InputAction.CallbackContext input)
    {
        char[] horizontals = { 'a', 'd' };
        char[] verticals = { 'w', 's' };


        char getKey = input.control.name[0];
        // string a = "hello";
        // print($"a[0] == {a[4]}");

        // string형변수[몇번째 글자(0부터)] 를 입력하면 그 번째의 글자를 얻을수 있음.


        // Original Version
        inputDirection = input.ReadValue<Vector2>();
        if (input.performed)
        {
            if (spriteDictionary.ContainsKey(inputDirection))
            {
                SetDirection(spriteDictionary[inputDirection]);
            }
        }

        //// New Version
        //     if (input.performed)
        //     {
        //         inputDirection = input.ReadValue<Vector2>();

        //         // 가로
        //         // Recognize horizontal inputs.
        //         if (getKey.Equals(horizontals[0]) || getKey.Equals(horizontals[1]))
        //         {
        //             inputDirection.x = Mathf.Round(inputDirection.x);
        //             inputDirection.y = 0f;
        //         }

        //         // 세로
        //         // Recognize vertical inputs.
        //         if (getKey.Equals(verticals[0]) || getKey.Equals(verticals[1]))
        //         {
        //             inputDirection.x = 0f;
        //             inputDirection.y = Mathf.Round(inputDirection.y);
        //         }

        //         if (spriteDictionary.ContainsKey(inputDirection))
        //         {
        //             SetDirection(spriteDictionary[inputDirection]);
        //         }
        //     }


        if (input.canceled || inputDirection == Vector2.zero)
        {
            inputDirection = Vector2.zero;
            activeSpriteRenderer.idle = true;
        }
    }


    private void SetDirection(AnimatedSpriteRenderer spriteRenderer)
    {
        // 받은 값과 일치하는 스프라이트를 켜줌.
        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        // 활성화된 스프라이트를 5번에서 찾은 스프라이트로 정함.
        activeSpriteRenderer = spriteRenderer;

        // 가만히 있는지 확인함.
        activeSpriteRenderer.idle = inputDirection == Vector2.zero;
    }

    // 폭발에 닿였을 때 사망하는 트리거.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeathSequence();
        }
    }

    // 조작을 막는 사망 트리거.
    private void DeathSequence()
    {
        enabled = false;
        GetComponent<BombController>().enabled = false;

        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererDeath.enabled = true;

        Invoke(nameof(OnDeathSequenceEnabled), 1.25f);
    }

    private void OnDeathSequenceEnabled()
    {
        // 이 스크립트를 끔.
        gameObject.SetActive(false);

        // GameManager.cs 의 CheckWinState 메서드를 찾아 실행함.
        FindObjectOfType<GameManager>().CheckWinState();
    }

}