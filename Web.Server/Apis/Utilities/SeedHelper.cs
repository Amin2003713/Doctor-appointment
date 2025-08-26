using Api.Endpoints.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Api.Endpoints.Utilities;

public static class SeedHelper
{
    public static async Task SeedUserAsync(
        UserManager<AppUser> userManager ,
        string               phone ,
        string               fullName ,
        string?              email ,
        string               password ,
        string               role)
    {
        // Normalize phone like your controller does
        var e164     = PhoneHelper.NormalizeToE164Guess(phone);
        var username = PhoneHelper.NormalizeUsername(e164);

        var existing = await userManager.FindByNameAsync(username);

        if (existing != null)
        {
            // Ensure role assignment (in case the user exists but lacks role)
            var rolesForUser = await userManager.GetRolesAsync(existing);

            if (!rolesForUser.Contains(role))
                await userManager.AddToRoleAsync(existing , role);

            // Make sure it’s active
            if (!existing.IsActive)
            {
                existing.IsActive = true;
                await userManager.UpdateAsync(existing);
            }

            return;
        }

        var user = new AppUser
        {
            UserName = username ,
            PhoneNumber = e164 ,
            Email            = string.IsNullOrWhiteSpace(email) ? null : email ,
            FullName = fullName ,
            IsActive = true ,
            Address         = "somewhere" ,
            FirstName = "doctor" ,
            LastName                                        = "appointment"
        };

        var create = await userManager.CreateAsync(user , password);

        if (!create.Succeeded)
            throw new Exception("Failed to seed user " +
                                fullName               +
                                ": "                   +
                                string.Join("; " , create.Errors.Select(e => e.Description)));

        await userManager.AddToRoleAsync(user , role);
    }
}