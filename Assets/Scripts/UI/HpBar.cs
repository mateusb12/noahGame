using Combat;
using UnityEngine;

namespace UI
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField] private Color colorFull;
        [SerializeField] private Color colorLow;
        private GameObject _player;
        public Color _currentColor;
        public float hpRatio;
        private Health _healthComponent;
        private Material _currentMaterial;
        private float _initialBarSize;
        private static readonly int Color1 = Shader.PropertyToID("_Color");
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _healthComponent = _player.GetComponent<Health>();
            _currentMaterial = GetComponent<Renderer>().material;
            _initialBarSize = transform.localScale.x;
            _currentMaterial.SetColor(EmissionColor, colorFull);
            // var currentMaterialEmissionColorIntensity = _currentMaterial.GetColor(EmissionColor).grayscale;
            // Debug.Log(currentMaterialEmissionColorIntensity);
        }
        
        // Update is called once per frame
        private void Update()
        {
            UpdateBar();
        }

        private void UpdateBar()
        {
            hpRatio = GetHpRatio();
            UpdateColor(hpRatio);
            UpdateSize(hpRatio);
        }
        
        private float GetHpRatio()
        {
            hpRatio = _healthComponent.GetPercentage();
            return hpRatio;
        }

        private void UpdateColor(float inputRatio)
        {
            var interpolatedColor = Color.Lerp(colorLow, colorFull, inputRatio);
            _currentColor = interpolatedColor;
            _currentMaterial.SetColor(Color1, interpolatedColor);
            var interpolatedEmissionIntensity = Mathf.Lerp(0f, 1f, inputRatio);
            var newEmissionColor = interpolatedColor * interpolatedEmissionIntensity;
            _currentMaterial.SetColor(EmissionColor, newEmissionColor);
            // _currentMaterial.SetColor(EmissionColor, interpolatedColor);
        }

        private void UpdateSize(float inputRatio)
        {
            var newBarSize = _initialBarSize * inputRatio;
            var scaleChange = new Vector3(newBarSize, transform.localScale.y, transform.localScale.z);
            transform.localScale = scaleChange;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }
    }
}
