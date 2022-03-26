using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager
{
    private static Dictionary<int, BaseGameEntity> entityMap = new Dictionary<int, BaseGameEntity>();

    public static void RegisterEntity(BaseGameEntity entity)
    {
        entityMap.Add(entity.ID, entity);
    }

    public static BaseGameEntity GetEntityByID(int ID)
    {
        return entityMap[ID];
    }
}
