using System;

namespace Assets.Scripts.BattleSystem
{
    public interface ISpawnable
    {
        event EventHandler OnSpawnableDestroyed;
    }
}
