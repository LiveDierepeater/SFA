using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrashSpawner : MonoBehaviour
{
    [Header("Spawning")] [SerializeField] private SOAllComponents m_AllComponentsList;
    [SerializeField] private GameObject m_TrashPrefab;
    [SerializeField] private Material m_ComponentMaterial;
    [SerializeField] private float m_SpawnDelay = 0.3f;
    [SerializeField] private int m_SpawnCount = 15;
    [SerializeField] private float m_SpawnRadius = 1.0f;
    
    private bool m_SpawnerIsReady = false;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        m_SpawnerIsReady = true;
    }

    public void SpawnTrashPile(int componentAmount)
    {
        if (m_SpawnerIsReady)
            StartCoroutine(SpawnTrashPile_Coroutine(componentAmount));
    }
    private IEnumerator SpawnTrashPile_Coroutine(int componentAmount = 0)
    {
        var componentPool = m_AllComponentsList.GetCarComponentsList().ToList();
        componentPool.Shuffle();
        
        StartCoroutine(SpawnTrash(componentAmount, componentPool));
        yield return new WaitForSeconds(m_SpawnDelay * componentAmount);
        StartCoroutine(SpawnTrash(m_SpawnCount));
    }

    private IEnumerator SpawnTrash(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(m_SpawnDelay);
            Instantiate(m_TrashPrefab, GetRandomPointInSphere(), GetRandomRotation(), transform);
        }
    }
    private IEnumerator SpawnTrash(int count, List<CarComponent> componentPool)
    {
        if (componentPool is null || componentPool.Count == 0) yield break;
        
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(m_SpawnDelay);
            var tmp = Instantiate(m_TrashPrefab, transform.position, Quaternion.identity).GetComponent<ITrashComponent>();
            tmp.Initialize(componentPool.FirstOrDefault(carComponent => !ComponentSpawnerManager.Instance.GetRegisteredComponents().Contains(carComponent)), m_ComponentMaterial);
            componentPool.Remove(tmp.GetCarComponent());
        }
    }

    public void DeleteTrash()
    {
        if (transform.childCount <= 0) return;

        var trashList = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++) trashList.Add(transform.GetChild(i));
        foreach (var trash in trashList) Destroy(trash.gameObject);
    }

    private Vector3 GetRandomPointInSphere() => Random.insideUnitSphere * m_SpawnRadius + transform.position;
    private static Quaternion GetRandomRotation() => Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

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