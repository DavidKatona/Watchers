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

        // This is just for testing purposes
        IEnumerator UnlockSkills()
        {
            yield return new WaitForSeconds(1);
            _playerSkills.UnlockSkill(SkillType.Dash);
            Debug.Log("You can dash now!");

            yield return new WaitForSeconds(1);
            _playerSkills.UnlockSkill(SkillType.WallJump);
            Debug.Log("You can wall jump now!");
        }
    }
}
