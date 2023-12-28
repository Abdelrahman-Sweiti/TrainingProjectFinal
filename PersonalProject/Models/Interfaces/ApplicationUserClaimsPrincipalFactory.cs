using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonalProject.Models;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace PersonalProject.Models.Interfaces
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {

        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        { }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("UserFirstName", user.FirstName ?? ""));
            identity.AddClaim(new Claim("UserLastName", user.LastName ?? ""));
            identity.AddClaim(new Claim("Gender", user.Gender ?? ""));
            identity.AddClaim(new Claim("Email", user.Email ?? ""));
            identity.AddClaim(new Claim("Image", user.Image ?? ""));


            return identity;

        }
    }
}
