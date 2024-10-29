using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile : IEntityData
{
    public float Speed { get; set; }
    public float Range { get; set; }
    public Vector3 Direction { get; set; }
    public bool CanPierce {  get; set; }
    public bool CanSearch {  get; set; }
    public float SearchRange {  get; set; }
}
