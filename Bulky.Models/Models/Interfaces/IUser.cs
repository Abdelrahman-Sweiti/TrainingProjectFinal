using PersonalProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace PersonalProject.Models.Interfaces
{
    public interface IUser
    {

         Task<UserDTO> Register(RegisterUserDTO registerDto, ModelStateDictionary modelstate, IFormFile file);
        //login Method

         Task<UserDTO> Authenticate(string username, string password);
        // Get All users method
         Task<UserDTO> GetUser(ClaimsPrincipal principal);
        // logout method
         Task LogOut();
         Task<List<ApplicationUser>> getAll();
        Task<ApplicationUser> GetFile(IFormFile file, ApplicationUser registerUserDTO);

        //Task<UpdataInfoModel> GetFile2(IFormFile file, UpdataInfoModel registerUserDTO);
    }
}
