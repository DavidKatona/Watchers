using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player.Skills
{
    public class SkillManager : MonoBehaviour
    {
        private PlayerSkills _playerSkills;

        private void Start()
        {
            _playerSkills = new PlayerSkills();
            StartCoroutine(UnlockSkills());
        }

        public bool CanDash()
        {
            return _playerSkills.IsSkillUnlocked(SkillType.Dash);
        }

        public bool CanWallJump()
        {
            return _playerSkills.IsSkillUnlocked(SkillType.WallJump);
        }

        public bool CanHeal()
        {
            return _playerSkills.IsSkillUnlocked(SkillType.Heal);
        }

        public bool CanAbyssBolt()
        {
            return _playerSkills.IsSkillUnlocked(SkillType.AbyssBolt);
        }

        IEnumerator UnlockSkills()
        {
            yield return new WaitForSeconds(0.1f);
            _playerSkills.UnlockSkill(SkillType.Dash);

            yield return new WaitForSeconds(0.1f);
            _playerSkills.UnlockSkill(SkillType.WallJump);

            yield return new WaitForSeconds(0.1f);
            _playerSkills.UnlockSkill(SkillType.AbyssBolt);
        }
    }
}
