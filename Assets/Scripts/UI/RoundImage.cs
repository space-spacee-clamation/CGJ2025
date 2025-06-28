using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Renderer))]
public class RoundImage : MonoBehaviour
{
    [SerializeField, Range(0, 1)] 
    private float _progress = 0;
    [SerializeField, Range(0, 360)] 
    private float _startAngle = 0;
    [SerializeField] 
    private float _animSpeed = 2f;
    [SerializeField] 
    private float _antialiasing = 0.05f;

    private Material _material;

    public float Progress
    {
        get => _progress;
        set => SetProgress(value);
    }

    public float StartAngle
    {
        get => _startAngle;
        set
        {
            _startAngle = Mathf.Repeat(value, 360);
            UpdateShaderProperties();
        }
    }

    private void Start()
    {
        InitializeMaterial();
    }

    private void InitializeMaterial()
    {
        var renderer = GetComponent<Image>();
        _material = renderer.material ;
    }

    public void SetProgress(float value, bool instant = false)
    {
        if (_material == null) InitializeMaterial();
        
        _progress = Mathf.Clamp01(value);
        if (instant)
        {
            UpdateShaderProperties();
        }
        else
        {
            AnimateProgress(value);
        }
    }

    private void AnimateProgress(float target)
    {
        var current = _progress;
        _material.SetFloat("_Progress", current);
    }

    private void UpdateShaderProperties()
    {
        if (_material == null) return;
        
        _material.SetFloat("_Progress", _progress);
        _material.SetFloat("_StartAngle", _startAngle);
        _material.SetFloat("_Antialiasing", _antialiasing);
    }

    private void OnValidate()
    {
        UpdateShaderProperties();
    }

    // 调试时自动添加纹理（可选）
    #if UNITY_EDITOR
    private void Reset()
    {
        if (GetComponent<Renderer>().sharedMaterial.mainTexture == null)
        {
            var debugTexture = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Texture2D>("UI/Skin/UISprite.psd");
            GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", debugTexture);
        }
    }
    #endif
}