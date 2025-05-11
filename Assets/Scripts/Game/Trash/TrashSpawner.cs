using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] private SOAllComponents m_AllComponentsList;
    [SerializeField] private GameObject m_TrashPrefab;
    [SerializeField] private Material m_ComponentMaterial;
    [SerializeField] private float m_SpawnDelay = 0.3f;
    [SerializeField] private int m_SpawnCount = 15;
    [SerializeField] private float m_SpawnRadius = 1.0f;
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        SpawnTrashPile();
    }
    
    private void SpawnTrashPile()
    {
        var componentPool = m_AllComponentsList.GetCarComponentsList().ToList();
        componentPool.Shuffle();
        
        Instantiate(m_TrashPrefab, transform.position, Quaternion.identity)
            .GetComponent<Trash>().Initialize(componentPool.FirstOrDefault(carComponent => !ComponentSpawnerManager.Instance.GetRegisteredComponents().Contains(carComponent)), m_ComponentMaterial);

        StartCoroutine(SpawnTrash(m_SpawnCount));
    }

    private IEnumerator SpawnTrash(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(m_SpawnDelay);
            Instantiate(m_TrashPrefab,
                new Vector3(
                    Random.Range(0f, m_SpawnRadius) + transform.position.x,
                    Random.Range(0f, m_SpawnRadius) + transform.position.y,
                    Random.Range(0f, m_SpawnRadius) + transform.position.z),
                new Quaternion(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)),
                transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, m_SpawnRadius);
    }
}

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1); // UnityEngine.Random!
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}
