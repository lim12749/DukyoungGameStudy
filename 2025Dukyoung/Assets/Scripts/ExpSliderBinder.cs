using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ExpSliderBinder : MonoBehaviour
{
    [SerializeField] private PlayerExperience xp;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text tmpText;      // TextMeshProUGUI
    void Awake()
    {
        if (!xp)
            xp = PlayerExperience.Instance ?? FindFirstObjectByType<PlayerExperience>();
        if (!slider)
            slider = GetComponent<Slider>();
        if (!tmpText)
            tmpText = GetComponentInChildren<TMP_Text>(true);
    }

    void OnEnable()
    {
        if (!xp || !slider) return;
        slider.minValue = 0;
        slider.maxValue = xp.ExpToNext;
        slider.SetValueWithoutNotify(xp.CurrentExp);
        UpdateText(xp.CurrentExp, xp.ExpToNext); // 초기 동기화
        xp.onExpChanged.AddListener(OnExpChanged);
    }

    void OnDisable()
    {
        if (xp) xp.onExpChanged.RemoveListener(OnExpChanged);
    }

    void OnExpChanged(int cur, int toNext)
    {
        slider.maxValue = toNext;
        slider.SetValueWithoutNotify(cur);
         UpdateText(cur, toNext); // 경험치 변경 시 텍스트 갱신
    }
    void UpdateText(int cur, int toNext)
    {
        string s = $"{cur}/{toNext}";

        if (tmpText)
            tmpText.text = s;

    }
}
