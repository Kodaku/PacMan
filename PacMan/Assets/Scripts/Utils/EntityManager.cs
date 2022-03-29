using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager
{
    private static Dictionary<int, List<BaseGameEntity>> entityMap = new Dictionary<int, List<BaseGameEntity>>();

    public static void RegisterEntity(BaseGameEntity entity)
    {
        if(!entityMap.ContainsKey(entity.ID))
            entityMap.Add(entity.ID, new List<BaseGameEntity>());
        entityMap[entity.ID].Add(entity);
    }

    public static List<BaseGameEntity> GetEntityByID(int ID)
    {
        if(entityMap.ContainsKey(ID))
            return entityMap[ID];
        return new List<BaseGameEntity>();
    }

    public static void DeleteEntity(BaseGameEntity entity)
    {
        entityMap[entity.ID].Remove(entity);
    }

    public static void ClearEntities(int ID)
    {
        if(entityMap.ContainsKey(ID))
            entityMap[ID].Clear();
    }
}
