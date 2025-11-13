using UnityEngine;

public class DamageArea :MonoBehaviour {

    [SerializeField] private int damageAmount = 10;

    public int GetDamageAmount() {
        return damageAmount;
    }

}
