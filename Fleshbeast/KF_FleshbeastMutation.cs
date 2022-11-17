using Qud.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRL.Core;
using XRL.Language;
using XRL.Rules;
using XRL.UI;
using XRL.World.Capabilities;
using XRL.World.Parts.Mutation;

namespace XRL.World.Parts.Mutation
{
  [Serializable]
  public class KF_FleshbeastMutation : BaseMutation
  {
    public KF_FleshbeastMutation() => this.DisplayName = "Fleshbeast Evolution";

    public override bool CanLevel() => false;

    public override bool ShouldShowLevel() => false;

    public override bool SameAs(IPart p) => true;

    public override void Register(GameObject Object)
    {
      Object.RegisterPartEvent((IPart) this, "AfterLevelGainedEarly");
      base.Register(Object);
    }

    public override bool Unmutate(GameObject GO)
    {
      GO.UnregisterPartEvent((IPart) this, "AfterLevelGainedEarly");
      return true;
    }

    public override string GetDescription() => "Your {{rusty|fleshy body}} evolves under pressure.\n\nWith time who knows what you will become.";

    public override string GetLevelText(int Level) => "";

	private void RapidAdvancementHelper(int Amount)
    {
      if (Amount <= 0)
        return;
      string str = GetMutationTermEvent.GetFor(this.ParentObject);
      bool flag1 = this.ParentObject.IsPlayer() && this.ParentObject.Stat("MP") >= 4;
      bool flag2 = false;
      if (flag1 && Popup.ShowYesNo("Your genome enters an excited state! Would you like to spend {{rules|4}} mutation points to buy " + Grammar.A(str) + " before rapidly mutating?", false) == DialogResult.Yes)
        flag2 = MutationsAPI.BuyRandomMutation(this.ParentObject, Confirm: false, MutationTerm: str);
      List<BaseMutation> list = this.ParentObject.GetPhysicalMutations().Where<BaseMutation>((Func<BaseMutation, bool>) (m => m.CanLevel())).ToList<BaseMutation>();
      if (list.Count > 0)
      {
        if (!flag1 && this.ParentObject.IsPlayer())
          Popup.Show("Your genome enters an excited state!");
        if (this.ParentObject.IsPlayer())
        {
          string[] array = list.Select<BaseMutation, string>((Func<BaseMutation, string>) (m => m.DisplayName + " ({{C|" + m.Level.ToString() + "}})")).ToArray<string>();
          int index = Popup.ShowOptionList("Choose a physical " + str + " to rapidly advance.", array);
          Popup.Show("You have rapidly advanced " + list[index].DisplayName + " by " + Grammar.Cardinal(Amount) + " ranks to rank {{C|" + (list[index].Level + Amount).ToString() + "}}!");
          list[index].RapidLevel(Amount);
        }
        else
          list.GetRandomElement<BaseMutation>((Random) null).RapidLevel(Amount);
      }
      else
      {
        if (!flag2)
          return;
        Popup.Show("You have no physical " + Grammar.Pluralize(str) + " to rapidly advance!");
      }
    }
	
    public override bool FireEvent(Event E)
    {
      if (E.ID == "AfterLevelGainedEarly")
      {
		if(this.ParentObject != null  && this.ParentObject.IsPlayer() 
			&& this.ParentObject.GetGenotype() == "KF_Fleshbeast")
		{
			this.ParentObject.GetStat("Strength").BaseValue += 1;
			//this.ParentObject.GetStat("Intelligence").BaseValue += 1;
			//this.ParentObject.GetStat("Willpower").BaseValue += 1;
			this.ParentObject.GetStat("Agility").BaseValue += 1;
			this.ParentObject.GetStat("Toughness").BaseValue += 1;
			//this.ParentObject.GetStat("Ego").BaseValue += 1;
			
			int _level = this.ParentObject.Stat("Level");
			if( _level == 2)
			{
				this.ParentObject.RequirePart<Mutations>().AddChimericBodyPart();
				this.ParentObject.RequirePart<Mutations>().AddChimericBodyPart();
				
				StatusScreen.BuyRandomMutation(this.ParentObject);
				StatusScreen.BuyRandomMutation(this.ParentObject);
				RapidAdvancementHelper(3);
			}
			else if( _level % 2 == 0)
			{
				StatusScreen.BuyRandomMutation(this.ParentObject);
			}
			if ( _level % 2 == 1  && _level != 1)
			{
				this.ParentObject.RequirePart<Mutations>().AddChimericBodyPart();
				RapidAdvancementHelper(3);
			}	
		}
	  }
	  return base.FireEvent(E);
    }
  }
}