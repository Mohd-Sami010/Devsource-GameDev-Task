using TMPro;
using UnityEngine;

public class Coin :MonoBehaviour {

    [SerializeField] private int coinValue = 1;
    [SerializeField] private TextMeshPro coinValueTextMesh;

    private void Start()
    {
        coinValueTextMesh.text = coinValue.ToString();
    }
    public int GetCoinValue()
    {
        return coinValue;
    }
    public void DestroyCoin()
    {
        Destroy(gameObject);
    }

}
