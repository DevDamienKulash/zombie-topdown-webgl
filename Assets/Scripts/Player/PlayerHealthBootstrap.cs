using UnityEngine;


public class PlayerHealthBootstrap : MonoBehaviour
{
[SerializeField] int startHP = 100;


void Start()
{
var p = GameObject.FindGameObjectWithTag("Player");
if (!p) return;
var h = p.GetComponent<Health>();
if (h) h.SetMaxAndFill(startHP);
}
}