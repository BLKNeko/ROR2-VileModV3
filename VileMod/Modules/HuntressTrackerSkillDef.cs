using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;

namespace VileMod.Modules
{
    public class HuntressTrackerSkillDef : SkillDef
    {
        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new HuntressTrackerSkillDef.InstanceData
            {
                huntressTracker = skillSlot.GetComponent<HuntressTracker>()
            };
        }
        private static bool HasTarget([NotNull] GenericSkill skillSlot)
        {
            HuntressTracker huntressTracker = ((HuntressTrackerSkillDef.InstanceData)skillSlot.skillInstanceData).huntressTracker;
            return (huntressTracker != null) ? huntressTracker.GetTrackingTarget() : null;
        }
        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return HuntressTrackerSkillDef.HasTarget(skillSlot) && base.CanExecute(skillSlot);
        }
        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && HuntressTrackerSkillDef.HasTarget(skillSlot);
        }
        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public HuntressTracker huntressTracker;
        }

    }
}
