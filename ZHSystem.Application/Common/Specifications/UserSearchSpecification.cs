using ZHSystem.Application.Common.Extensions;
using ZHSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Application.Common.Specifications
{
    public class UserSearchSpecification : BaseSpecification<User>
    {

        public UserSearchSpecification( string? sortBy = null, bool descending = false, string? searchUserName = null,
            string? searchEmail = null,
            string? searchRole = null)
        {
            
            if (!string.IsNullOrWhiteSpace(searchUserName))
                Criteria = u => u.UserName.Contains(searchUserName);

            if (!string.IsNullOrWhiteSpace(searchEmail))
                Criteria =
                    Criteria == null ?
                     u => u.Email.Contains(searchEmail)
                    : Criteria.And(u => u.Email.Contains(searchEmail));

            if (!string.IsNullOrWhiteSpace(searchRole))
                Criteria = Criteria == null
                    ? u => u.UserRoles.Any(ur => ur.Role.Name.Contains(searchRole))
                    : Criteria.And(u => u.UserRoles.Any(ur => ur.Role.Name.Contains(searchRole)));

            Includes.Add("UserRoles");
            Includes.Add("UserRoles.Role");
            //Includes.Add(u => u.UserRoles.Select(ur => ur.Role));
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "username":
                        if (descending)
                            OrderByDescending = u => u.UserName;
                        else
                            OrderBy = u => u.UserName;
                        break;
                    case "email":
                        if (descending)
                            OrderByDescending = u => u.Email;
                        else
                            OrderBy = u => u.Email;
                        break;
                    default:
                        OrderBy = u => u.Id; // Default sorting
                        break;
                }
            }
            else
            {
                OrderBy = u => u.Id; // Default sorting
            }
        }

       
    }
}
