using System;

namespace XRL.World.Parts.Mutation
{
  [Serializable]
  public class KF_LevitationFieldMutation : BaseDefaultEquipmentMutation
  {
    public string BodyPartType = "Feet";
    public GameObject LevitatorObject;

    public KF_LevitationFieldMutation() => this.DisplayName = "{{extradimentional|Levitation Field}}";

    public override bool CanLevel() => false;

    public override bool GeneratesEquipment() => true;

    public override string GetDescription() => "You use a {{extradimentional|Levitation Field}} to get around.\n\nShoes are for mortals.";

    public override string GetLevelText(int Level) => "";

    public override void OnRegenerateDefaultEquipment(Body Body)
    { 
      if (!GameObject.validate(ref this.LevitatorObject))
        this.LevitatorObject = GameObjectFactory.Factory.CreateObject("Levitator");
      BodyPart Part = this.RequireRegisteredSlot(Body, this.BodyPartType);
      if (Part != null && Part.Equipped != this.LevitatorObject && Part.ForceUnequip(true))
      {
        Armor part2 = this.LevitatorObject.GetPart<Armor>();
        part2.WornOn = Part.Type;
        part2.AV = 0;
		part2.DV = 2;
		part2.SpeedBonus = 25;
        this.ParentObject.ForceEquipObject(this.LevitatorObject, Part, true, new int?(0));
      }
      base.OnRegenerateDefaultEquipment(Body);
    }

    public override bool Unmutate(GameObject GO)
    {
      this.CleanUpMutationEquipment(GO, ref this.LevitatorObject);
      return base.Unmutate(GO);
    }
  }
}