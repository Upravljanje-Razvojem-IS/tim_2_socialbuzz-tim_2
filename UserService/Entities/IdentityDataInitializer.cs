using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    public class IdentityDataInitializer
    {
        public static void SeedData (UserManager<AccountInfo> userManager, RoleManager<AccountRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }
        public static void SeedUsers(UserManager<AccountInfo> userManager)
        {
            Guid firstPersonalUserAdminId = Guid.Parse("CE593D02-C615-4AF6-A794-C450B79E9B4D");
            Guid secondPersonalUserRegularUserId = Guid.Parse("FF0C9396-7C4C-4BF5-A12E-6AA79C272413");
            Guid thirdPersonalUserRegularUserId = Guid.Parse("8C349E7B-1C97-486D-AA2E-E58205D11577");
            Guid firstCorporationUserRegularUserId = Guid.Parse("33253633-10E4-45C8-9B8E-84020A5C8C58");
            Guid secondCorporationUserRegularUserId = Guid.Parse("987268E5-F880-4F81-B1BF-5B9704604E26");

            if (userManager.FindByNameAsync("NatalijaG").Result == null)
            {
                AccountInfo acc = new AccountInfo("NatalijaG", "nata@gmail.com", firstPersonalUserAdminId);
                IdentityResult result = userManager.CreateAsync(acc, "pass123").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(acc,"Admin").Wait();
                }
                 
            }
            if (userManager.FindByNameAsync("VladikaF").Result == null)
            {
                AccountInfo acc = new AccountInfo("VladikaF", "vladika@gmail.com", secondPersonalUserRegularUserId);
                IdentityResult result = userManager.CreateAsync(acc, "pass123").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(acc, "Regular user").Wait();
                }
            }
            if (userManager.FindByNameAsync("StefanO").Result == null)
            {
                AccountInfo acc = new AccountInfo("StefanO", "stefke@gmail.com", thirdPersonalUserRegularUserId);
                IdentityResult result = userManager.CreateAsync(acc, "pass123").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(acc, "Regular user").Wait();
                }
            }
            //TODO: UserName in AspNetUsers table can't contain any special characters, including space
            if (userManager.FindByNameAsync("FinancialCorporation").Result == null)
            {
                AccountInfo acc = new AccountInfo("FinancialCorporation", "financial_corpo@gmail.com", firstCorporationUserRegularUserId);
                IdentityResult result = userManager.CreateAsync(acc, "pass123").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(acc, "Regular user").Wait();
                }
            }
            if (userManager.FindByNameAsync("BillingCorporation").Result == null)
            {
                AccountInfo acc = new AccountInfo("BillingCorporation", "billing_corpo@gmail.com", secondCorporationUserRegularUserId);
                IdentityResult result = userManager.CreateAsync(acc, "pass123").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(acc, "Regular user").Wait();
                }
            }

        }

        public static void SeedRoles(RoleManager<AccountRole> roleManager)
        {
            Guid adminRoleId = Guid.Parse("194DF880-D4CE-4997-96C9-878102EB6E0E");
            Guid regularUserRoleId = Guid.Parse("728569AA-7A1F-45C9-B9D4-94BCC176BD0C");
          
            if (!roleManager.RoleExistsAsync("Regular user").Result)
            {
                AccountRole role = new AccountRole(regularUserRoleId, "Role that basic level privileges", "Regular user");
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                AccountRole role = new AccountRole(adminRoleId, "Role that enables root level privileges", "Admin");
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
