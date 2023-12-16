using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill {

  [Header("Dodge")]
  [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
  [SerializeField] private int evasionAmount;
  public bool dodgeUnloked { get; private set; }

  [Header("Parry")]
  [SerializeField] private UI_SkillTreeSlot unlockMirageDodge;
  public bool dodgeMirageUnlocked { get; private set; }

  protected override void Start() {
    base.Start();

    unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
    unlockMirageDodge.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
  }

  private void UnlockDodge() {

    if (unlockDodgeButton.unlocked && !dodgeUnloked) {

      player.stats.evasion.AddModifier(evasionAmount);
      Inventory.instance.UpdateStatsUI();
      dodgeUnloked = true;
    }
  }

  private void UnlockMirageDodge() {

    if (unlockMirageDodge.unlocked)
      dodgeMirageUnlocked = true;
  }

  public void CreateMirageOnDodge() {

    if (dodgeMirageUnlocked)
      SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2 * player.facingDir, 0));
  }
}
