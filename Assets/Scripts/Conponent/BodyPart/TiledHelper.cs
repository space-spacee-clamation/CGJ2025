using UnityEngine;
/// <summary>
///     辅助拉长sprite的工具
///     而不是简单的拉伸
/// </summary>
public class TiledHelper : MonoBehaviour
{
    [SerializeField] private SpriteRenderer tragetRenderer;
    private void Update()
    {
        tragetRenderer.size = transform.localScale;
        tragetRenderer.gameObject.transform.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1);
    }
}