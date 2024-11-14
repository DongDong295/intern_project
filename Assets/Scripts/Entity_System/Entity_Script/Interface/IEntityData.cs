using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityData
{
    public int ID {  get; set; }
    public string Name { get; set; }
    public EntityType Type { get; set; }
}
