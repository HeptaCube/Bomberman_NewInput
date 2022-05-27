using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    // 연결과 값 수정을 위한 public 변수.
    public GameObject bombPrefab;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    public int bombsRemaining = 0;

    // 스페이스가 눌렸는지 확인하는 bool 형 변수.
    bool isSpaceDown = false;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    // 넣어준 프리팹 오브젝트(explosionPrefab)가 Explosion 스크립트를 가질 때만 작동한다.

    public float explosionDuration = 1f;
    public LayerMask explosionLayerMask;
    public int explosionRadius = 1;

    [Header("Destructible")]
    public Tilemap destructibleTiles;
    public Destructibles destructiblePrefab;


    // 스페이스바가 눌렸는지 확인해주는 메서드.
    public void DetectSpace(InputAction.CallbackContext bombinput)
    {
        Debug.Log(bombinput.control.name);
        if (bombinput.control.name == "leftShift" && bombinput.canceled || bombinput.control.name == "space" && bombinput.canceled)
        {
            isSpaceDown = true;
        }
        else
        {
            isSpaceDown = false;
        }
    }

    // 오브젝트가 활성화 된 시점에 실행.
    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }


    // 매 프레임마다 업데이트됨.
    private void Update()
    {
        if (bombsRemaining > 0 && isSpaceDown == true)
        {
            // Run IEnumerator-kind method.
            StartCoroutine(PlaceBomb());
            isSpaceDown = false;
        }
    }


    private IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;
        isSpaceDown = false;

        yield return new WaitForSeconds(bombFuseTime);

        // position 변수를 만들고 반올림을 함.
        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        // explosionPrefab 프리팹을 실행하고 스프라이트 상태를 "start"로 바꿈.
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);

        // 폭발 후 폭발 이펙트 오브젝트를 지움.
        explosion.DestroyAfter(explosionDuration);

        // Explode 메서드를 지정해준 값과 함께 실행함.
        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        // 폭탄 지움.
        Destroy(bomb);
        isSpaceDown = false;
        bombsRemaining++;
    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0)
        {
            return;
        }

        // "Vector2 변수" += "Vector2 변수"
        position += direction;

        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearDestructible(position);
            return;
        }

        // PlaceBomb() 메서드의 explosion 변수와는 다른 변수임.
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);

        // "middle"과 "end" 중 어느 스프라이트를 사용할지 결정함.
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);

        // 폭발 이펙트가 번지는 방향을 설정함.
        explosion.SetDirection(direction);

        explosion.DestroyAfter(explosionDuration);

        // 번짐을 멈춤.
        Explode(position, direction, length - 1);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }

    private void ClearDestructible(Vector2 position)
    {
        // 타일 맵은 모든 것들을 Vector3Int 형으로 저장한다.

        // World position에서 cell position으로 전환함.
        Vector3Int cell = destructibleTiles.WorldToCell(position);

        // 받은 cell XYZ 값에 있는 타일을 골라줌.
        TileBase tile = destructibleTiles.GetTile(cell);

        // 해당 타일이 null이 아니라면 프리팹을 Instantiate 하고 해당 타일 오브젝트를 삭제한다.
        if (tile != null)
        {
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
    }

    public void AddBomb()
    {
        bombAmount++;
        bombsRemaining++;
    }
}