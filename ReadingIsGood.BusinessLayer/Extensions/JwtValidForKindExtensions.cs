using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingIsGood.EntityLayer.Enum;

namespace ReadingIsGood.BusinessLayer.Extensions
{
    public static class JwtValidForKindExtensions
    {
        public static List<string> ToAudience(this JwtValidForKind kind)
        {
            var list = new List<string>();
            switch (kind)
            {
                case JwtValidForKind.Admin:
                    list.Add(nameof(Audience.System));
                    list.Add(nameof(Audience.Auth));
                    break;

                case JwtValidForKind.Customer:
                    list.Add(nameof(Audience.Shopping));
                    list.Add(nameof(Audience.Auth));
                    break;

                default:
                    list.Add("none");
                    break;
            }

            return list;
        }
    }
}
