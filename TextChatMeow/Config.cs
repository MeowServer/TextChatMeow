using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextChatMeow
{
    internal class Config
    {
        public Dictionary<RoleTypeId, string> RoleName { get; set; } = new Dictionary<RoleTypeId, string>()
        {
            { RoleTypeId.None, "无角色"},
            { RoleTypeId.Scp173, "Scp173"},
            { RoleTypeId.ClassD, "D级人员"},
            { RoleTypeId.Spectator, "观察者"},
            { RoleTypeId.Scp106, "Scp106"},
            { RoleTypeId.NtfSpecialist, "九尾狐收容专家"},
            { RoleTypeId.Scp049, "Scp049"},
            { RoleTypeId.Scientist, "科学家"},
            { RoleTypeId.Scp079, "Scp079"},
            { RoleTypeId.ChaosConscript, "混沌征召兵"},
            { RoleTypeId.Scp096, "Scp096"},
            { RoleTypeId.Scp0492, "Scp049-2"},
            { RoleTypeId.NtfSergeant, "九尾狐中士"},
            { RoleTypeId.NtfCaptain, "九尾狐队长"},
            { RoleTypeId.NtfPrivate, "九尾狐列兵"},
            { RoleTypeId.Tutorial, "教程人员"},
            { RoleTypeId.FacilityGuard, "设施警卫"},
            { RoleTypeId.Scp939, "Scp939"},
            { RoleTypeId.CustomRole, "自定义角色"},
            { RoleTypeId.ChaosRifleman, "混沌步枪兵"},
            { RoleTypeId.ChaosMarauder, "混沌掠夺者"},
            { RoleTypeId.ChaosRepressor, "混沌压制者"},
            { RoleTypeId.Overwatch, "角色Overwatch"},
            { RoleTypeId.Filmmaker, "摄像机"},
            { RoleTypeId.Scp3114, "Scp3114"},
        };
    }
}
