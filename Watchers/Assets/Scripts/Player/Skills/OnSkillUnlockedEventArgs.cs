using System;

namespace Assets.Scripts.Player.Skills
{
    public class OnSkillUnlockedEventArgs : EventArgs
    {
        private readonly SkillType _skillType;

        SkillType GetSkillType()
        {
            return _skillType;
        }

        public OnSkillUnlockedEventArgs(SkillType skillType)
        {
            _skillType = skillType;
        }
    }
}
