using System.Collections.Generic;
using UnityEngine;


public class ObjectPool<T> : MonoBehaviour where T : Component
{
[SerializeField] T prefab;
[SerializeField] int initialSize = 8;
[SerializeField] int maxSize = 128;
readonly Queue<T> q = new Queue<T>();


void Awake() { Prewarm(); }


void Prewarm()
{
for (int i = 0; i < initialSize; i++)
{
var x = Instantiate(prefab, transform);
x.gameObject.SetActive(false);
q.Enqueue(x);
}
}


public T Get()
{
T x = q.Count > 0 ? q.Dequeue() : Instantiate(prefab, transform);
x.gameObject.SetActive(true);
(x as IPoolable)?.OnSpawned();
return x;
}


public void Release(T x)
{
if (!x) return;
(x as IPoolable)?.OnDespawned();
x.gameObject.SetActive(false);
if (q.Count + 1 <= maxSize) q.Enqueue(x); else Destroy(x.gameObject);
}
}