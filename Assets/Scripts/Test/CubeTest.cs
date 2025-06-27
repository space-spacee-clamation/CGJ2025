using UnityEngine;
public class CubeTest : MonoBehaviour
{
    [SerializeField] private Transform center;
    [SerializeField] private LineRenderer _renderer;
    [SerializeField] private Vector3 speed;
    [SerializeField] private float a;
    private float renderTick = 0.01f;
    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < _renderer.positionCount; i++)
        {
            _renderer.SetPosition(i, transform.position);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        MoveAround();
        LineRend();
        RandomRotate();
    }
    private void RandomRotate()
    {
        transform.Rotate(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * Time.deltaTime);
    }
    private void MoveAround()
    {
        speed = (center.transform.position - transform.position).normalized * a + speed;
        transform.position += speed * Time.deltaTime;
    }
    private void LineRend()
    {
        renderTick -= Time.deltaTime;
        if (renderTick < 0)
        {
            for (int i = _renderer.positionCount - 1; i >= 1; i--)
            {
                _renderer.SetPosition(i, _renderer.GetPosition(i - 1));
            }
            renderTick =  0.01f;
        }
        _renderer.SetPosition(0, transform.position);
    }
}