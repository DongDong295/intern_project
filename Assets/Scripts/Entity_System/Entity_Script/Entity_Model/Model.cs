using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EntityModel : IEntityData {
    protected int id;
    protected string name;
    protected EntityType type;

    public int ID { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }

    public EntityType Type { get => type; set => type = value; }
    
    public void InitModelCoreData()
    {

    }
}

public class CharacterModel : EntityModel { }

public class EnemyModel : EntityModel { } 
