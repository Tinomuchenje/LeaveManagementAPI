using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LeaveManagementAPI.Services;
using LeaveManagementAPI.Models.DTOs;

namespace LeaveManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ILoginService _loginService;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        public AccountController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _loginService.LoginAsync(loginDto.Username, loginDto.Password);
            if (!result) return Unauthorized("Invalid login attempt");

            return Ok("Login successful");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _loginService.LogoutAsync();
            return Ok("Logged out successfully");
        }

        // Endpoint to get the current user
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _loginService.GetCurrentUserAsync();
            if (user == null) return NotFound("User not found");

            return Ok(user);
        }
    }
}