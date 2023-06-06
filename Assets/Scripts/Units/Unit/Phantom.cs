using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phantom : UnitBase
{
    int bonus_damage;
    int total_bonus_damage;
    protected override void Update()
    {
        base.Update();
    }

    public override void UnitInit(int _num)
    {
        GameManager.Instance.Synergy_manager.Increase_Unitcount(UnitType.Ghost);
        base.UnitInit(_num);
        AttackEvent.AddListener(AttackMonster);
        GameManager.Instance.Ability_manager.Add_ghost_list(SpecialAbility);
    }
    public override void UnitDelete()
    {
        GameManager.Instance.Ability_manager.Remove_ghost_list(SpecialAbility);
        base.UnitDelete();
    }

    protected override void Apply_ArtifactOption()
    {
        int add_type_damage = ArtifactManager.Instance.have_Artifact[1] ? 5 : 0;
        bonus_damage = ArtifactManager.Instance.have_Artifact[9] ? 2 : 1;

        damage = BaseData.base_attackpower + add_type_damage;
        temp_cooltime = BaseData.base_attackcooltime;
        Debug.Log("Phantom | " + add_type_damage + " | " + bonus_damage + " | " + temp_cooltime);
    }
    protected override void SpecialAbility()
    {
        total_bonus_damage += bonus_damage;
        damage += bonus_damage;
    }
}
