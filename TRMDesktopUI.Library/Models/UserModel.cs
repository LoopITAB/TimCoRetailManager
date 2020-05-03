using System.Collections.Generic;
using System.Linq;

namespace TRMDesktopUI.Library.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public Dictionary<string, string> Roles { get; set; } = new Dictionary<string, string>();

        // This part should be added to the UserDisplayModel in TRMDesktopUI.Models
        public string RoleList
        {
            get { return string.Join(", ", Roles.Select(x => x.Value)); }
        }

    }
}