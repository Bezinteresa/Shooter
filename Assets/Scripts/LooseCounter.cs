
using TMPro;
using UnityEngine;

public class LooseCounter : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI _text;
    private int _enemyLoss;
    private int _playerLoss;

    public void SetEnemyLoss(int val) {
        _enemyLoss = val;
        UpdateText();
    }

    public void SetPlayerLoss(int val) {
        _playerLoss = val;
        UpdateText();
    }

    private void UpdateText() {
        _text.text = $"{_playerLoss} : {_enemyLoss}";
    }

}
