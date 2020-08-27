using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player.Skills
{
    public class SkillManager : MonoBehaviour
    {
        private PlayerSkills _playerSkills;
        private int _skillPoints;

        public int GetSkillPoints()
        {
            return _skillPoints;
        }

        public void SetSkillPoints(int value)
        {
            _skillPoints = value;
        }

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

        // This is just for testing purposes
        IEnumerator UnlockSkills()
        {
            yield return new WaitForSeconds(0.1f);
            _playerSkills.UnlockSkill(SkillType.Dash);
            Debug.Log("You can dash now!");

            yield return new WaitForSeconds(0.1f);
            _playerSkills.UnlockSkill(SkillType.WallJump);
            Debug.Log("You can wall jump now!");

            yield return new WaitForSeconds(0.1f);
            _playerSkills.UnlockSkill(SkillType.Heal);
            Debug.Log("You can cast heal now!");

            yield return new WaitForSeconds(0.1f);
            _playerSkills.UnlockSkill(SkillType.AbyssBolt);
            Debug.Log("You can cast abyss bolt now!");
        }
    }
}
