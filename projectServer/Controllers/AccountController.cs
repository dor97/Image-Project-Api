using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projectServer.DTOs.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectServer.Interfaces;
using projectServer.Models.Account;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserModel> m_userManager;
        private readonly ITokenService m_tokenService;
        private readonly SignInManager<UserModel> m_signInManager;
        public AccountController(UserManager<UserModel> userManager, ITokenService tokenService, SignInManager<UserModel> signInManager)
        {
            m_userManager = userManager;
            m_tokenService = tokenService;
            m_signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await m_userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

            if (user == null)
            {
                return Unauthorized("Username not found and/or password incorrect");
            }

            var result = await m_signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized("Username not found and/or password incorrect");
            }

            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = m_tokenService.CreateToken(user)
                }
            );
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userModel = new UserModel
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email
                };

                var createdUser = await m_userManager.CreateAsync(userModel, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await m_userManager.AddToRoleAsync(userModel, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                UserName = userModel.UserName,
                                Email = userModel.Email,
                                Token = m_tokenService.CreateToken(userModel)
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}