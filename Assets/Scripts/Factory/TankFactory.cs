using System;
using Basic.Armor;
using Basic.Engine;
using Basic.Weapon;
using Item;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Factory
{
    public static class TankFactory
    {
        public static ITank CreateLightTank(MonoBehaviour tank)
        {
            return new TankItem(new LightArmor(), new LightWeapon(tank), new LightEngine());
        }

        public static ITank CreateTankByParam(TankType armorType, TankType weaponType, TankType engineType,
            MonoBehaviour tank)
        {
            IArmor armor = armorType switch
            {
                TankType.Light => new LightArmor(),
                TankType.Medium => new MediumArmor(),
                _ => new HeavyArmor()
            };

            IWeapon weapon = weaponType switch
            {
                TankType.Light => new LightWeapon(tank),
                TankType.Medium => new MediumWeapon(tank),
                _ => new HeavyWeapon(tank)
            };

            IEngine engine = engineType switch
            {
                TankType.Light => new LightEngine(),
                TankType.Medium => new MediumEngine(),
                _ => new HeavyEngine()
            };

            return new TankItem(armor, weapon, engine);
        }

        public static ITank CreatRandomTank(MonoBehaviour tank)
        {
            var armorIndex = Random.Range(0, 9) % 3;
            var weaponIndex = Random.Range(0, 9) % 3;
            var engineIndex = Random.Range(0, 9) % 3;
            var armor = GetTankType(armorIndex);
            var weapon = GetTankType(weaponIndex);
            var engine = GetTankType(engineIndex);
            return CreateTankByParam(armor, weapon, engine, tank);
        }

        private static TankType GetTankType(int index)
        {
            return index switch
            {
                1 => TankType.Medium,
                2 => TankType.Heavy,
                _ => TankType.Light
            };
        }
    }

    public enum TankType
    {
        Light,
        Medium,
        Heavy,
    }
}