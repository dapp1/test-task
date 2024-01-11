using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public Action attack;
    public Action doubleAttack;
    [SerializeField] private Button _singleAttackButton;
    [SerializeField] private Button _doubleAttackButton;
    [SerializeField] private Image _doubleAttackImage;

    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private TextMeshProUGUI _waveCounter;

    private int minutes;
    private float seconds;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        _singleAttackButton.onClick.AddListener(() => attack?.Invoke());
        _doubleAttackButton.onClick.AddListener(() => doubleAttack?.Invoke());
    }

    private void FixedUpdate()
    {
        if (_timer != null)
        {
            seconds += Time.fixedDeltaTime;

            if(seconds >= 59)
            {
                seconds = 0;
                minutes++;
            }

            _timer.text = "Time - " + minutes.ToString("F0") + ":" + seconds.ToString("F0");
        }
    }

    public void SetWaveText(int waveNumber)
    {
        _waveCounter.text = "Current wave: " + waveNumber.ToString();
    }

    public void SetActiveButton(bool state)
    {
        if(state && _doubleAttackImage.fillAmount == 1)
        {
            _doubleAttackImage.color = Color.green;
            _doubleAttackButton.enabled = true;
        }
        else
        {
            _doubleAttackButton.enabled = false;
            _doubleAttackImage.color = Color.grey;
        }
    }

    public IEnumerator FillImage(float duration)
    {
        _doubleAttackImage.fillAmount = 0f;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            _doubleAttackImage.fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / duration);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _doubleAttackImage.fillAmount = 1f;
    }
}
