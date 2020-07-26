using System;
using System.Collections.Generic;

namespace Assets.Scripts.Player.Skills
{
    public class PlayerSkills
    {
        public event EventHandler OnSkillUnlocked;

        private List<SkillType> _unlockedSkillTypes;

        public PlayerSkills()
        {
            _unlockedSkillTypes = new List<SkillType>();
        }

        public void UnlockSkill(SkillType skillType)
        {
            if (!IsSkillUnlocked(skillType))
            {
                _unlockedSkillTypes.Add(skillType);
                OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedEventArgs(skillType));
            }
        }

        public bool IsSkillUnlocked(SkillType skillType)
        {
            return _unlockedSkillTypes.Contains(skillType);
        }
    }
}
