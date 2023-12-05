using UnityEngine;

public interface ConsumablesInterface {
    void consume(Transform player);
    void consume(Transform player, int value, string name);
}
