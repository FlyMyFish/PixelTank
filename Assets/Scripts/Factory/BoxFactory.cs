using System;
using Basic;
using Basic.Armor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Factory
{
    public static class BoxFactory
    {
        private static readonly Type[,] TypeList =
        {
            {
                Type.GetType("Basic.Armor.HeavyArmor"),
                Type.GetType("Basic.Armor.MediumArmor"),
                Type.GetType("Basic.Armor.LightArmor")
            },
            {
                Type.GetType("Basic.Weapon.HeavyWeapon"),
                Type.GetType("Basic.Weapon.MediumWeapon"),
                Type.GetType("Basic.Weapon.LightWeapon")
            },
            {
                Type.GetType("Basic.Engine.HeavyEngine"),
                Type.GetType("Basic.Engine.MediumEngine"),
                Type.GetType("Basic.Engine.LightEngine")
            }
        };

        public static IConsumable CreateBoxPrefab((int typeIndex, int weightIndex) type, MonoBehaviour mono)
        {
            var typeObj = type.typeIndex == 1
                ? Activator.CreateInstance(TypeList[type.typeIndex, type.weightIndex], new object[] {mono})
                : Activator.CreateInstance(TypeList[type.typeIndex, type.weightIndex]);

            return typeObj switch
            {
                IConsumable iConsumable => iConsumable,
                _ => new LightArmor()
            };
        }

        public static (int typeIndex, int weightIndex) CreateRandomBox()
        {
            var typeIndex = Random.Range(0, 3);
            var weightIndex = Random.Range(0, typeIndex == 0 ? 3 : 2);
            return (typeIndex, weightIndex);
        }
    }
}