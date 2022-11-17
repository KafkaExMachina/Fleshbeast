using System;
using System.Collections.Generic;
using XRL.Rules;
using XRL.UI;
using XRL.World;
using XRL.World.Parts.Mutation;

namespace XRL.World.Parts.Mutation
{
	[Serializable]
    class KF_Omnisight : BaseMutation
	{
		public KF_Omnisight() => this.DisplayName = "Omnivision";
	
		public override bool CanLevel()
	    {
		    return true;
	    }
		
        public override string GetDescription()
        {
            return "";
        }

        public override string GetLevelText(int Level)
        {
            return "Your {{psychalflesh|unearthly senory organs}} can see beyond mortal limits into the {{extradimensional|truth of things}}.\n";
        }
		
		public override bool WantEvent(int ID, int cascade) => base.WantEvent(ID, cascade) || ID == BeforeRenderEvent.ID;
		
		public override bool HandleEvent(BeforeRenderEvent E)
		{
		  if (this.ParentObject.IsPlayer())
		  {
			this.AddLight(this.Level + 4, LightLevel.Omniscient);
		  }
		  return base.HandleEvent(E);
		}

        public override bool ChangeLevel(int NewLevel)
        {
			if (NewLevel == 3 )
				this.ParentObject.ModIntProperty("BioScannerEquipped",1,true);
			else if (NewLevel == 6)
				this.ParentObject.ModIntProperty("TechScannerEquipped",1,true);
			else if (NewLevel == 9)
				this.ParentObject.ModIntProperty("StructureScannerEquipped",1,true);
				
            return base.ChangeLevel(NewLevel);
        }

        public override bool Mutate(GameObject GO, int Level)
        {
			return base.Mutate(GO, Level);
        }
 
        public override bool Unmutate(GameObject GO)
        {
			if (this.Level == 3 )
				this.ParentObject.ModIntProperty("BioScannerEquipped",-1,true);
			else if (this.Level == 6)
				this.ParentObject.ModIntProperty("TechScannerEquipped",-1,true);
			else if (this.Level == 9)
				this.ParentObject.ModIntProperty("StructureScannerEquipped",-1,true);
			return base.Unmutate(GO);
        }		
	}
}