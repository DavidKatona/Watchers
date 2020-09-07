using System;

namespace Assets.Scripts.Collectibles
{
    public interface ICollectible
    {
        void Collect(StatManager statManager);
    }
}
