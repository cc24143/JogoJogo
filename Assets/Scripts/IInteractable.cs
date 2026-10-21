using UnityEngine;

public interface IInteractable
{
    Transform transform { get; } 
    void Interagir(Vector3 direction);
}