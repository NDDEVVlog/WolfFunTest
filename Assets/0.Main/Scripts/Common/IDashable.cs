using UnityEngine;

public interface IDashable
{
    void BeginDash(Vector3 velocity);
    void EndDash();
}