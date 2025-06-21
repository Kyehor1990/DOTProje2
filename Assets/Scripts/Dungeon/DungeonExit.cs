using UnityEngine;

public class DungeonExit : MonoBehaviour
{
    private SceneChange sceneChange;
    private SpriteRenderer spriteRenderer;
    [SerializeField] Sprite openSprite;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sceneChange = FindObjectOfType<SceneChange>(); 
        if (spriteRenderer != null && openSprite != null)
        spriteRenderer.sprite = openSprite; 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {   
            sceneChange.BeforeCustomerSceneChange();
        }
    }

}
