using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemasSalgados.Controllers.Base;
using SistemasSalgados.Dtos;
using SistemasSalgados.Models.Auth;
using SistemasSalgados.Models.Base;
using SistemasSalgados.Services.Interfaces.Auth;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SistemasSalgados.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController<User>
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        public UsersController(
            IUserService userService,
            IUserRoleService userRoleService,
            IAuthService authService,
            IHttpContextAccessor contextAccessor) : base(userService, contextAccessor)
        {
            _authService = authService;
            _userService = userService;
            _userRoleService = userRoleService;
        }

        protected override Task<PagedResult<User>> ApplyPaginationParams(QueryParams queryParams, IQueryable<User> query)
        {
            if (!string.IsNullOrEmpty(queryParams.Search))
                query = query.Where(p => p.Name.Contains(queryParams.Search));

            return base.ApplyPaginationParams(queryParams, query);
        }

        [HttpPost("register"), AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var user = await _baseService
                .Select()
                .Where(w => w.Email == registerDto.Email)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                return BadRequest("Usuário ja existe.");
            }

            CreatePasswordHash(registerDto.Password,
                 out byte[] passwordHash,
                 out byte[] passwordSalt);

            user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                IsActive = true,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            await _baseService.Insert(user);

            var result = _userRoleService.Insert(user.Id);

            if (result) 
                return Ok("Usuario criado com sucesso!");

            return BadRequest("Erro ao realizar cadastro!");
        }

        [HttpPost("authenticate"), AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto loginDto)
        {
            AuthenticatedUser authenticatedUser;

            var user = await _baseService
                .Select()
                .Where(w => w.Email == loginDto.Email)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return BadRequest("Usuário não encontrado.");
            }

            switch (loginDto.GrantType)
            {
                case "refresh_token":
                    authenticatedUser = await _authService.AuthenticateWithRefreshToken(loginDto.RefreshToken);
                    break;
                case "password":
                    authenticatedUser =
                        await _authService.AuthenticateWithPassword(loginDto.Password, user);
                    break;
                default: return BadRequest("Invalid grant type");
            }

            if (authenticatedUser == null)
            {
                return Unauthorized();
            }

            return Ok(authenticatedUser);

        }

        [HttpPost("refresh"), AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] LoginDto loginDto)
        {
            AuthenticatedUser authenticatedUser = await _authService.AuthenticateWithRefreshToken(loginDto.RefreshToken);
            if (authenticatedUser == null)
                return Unauthorized();

            return Ok(authenticatedUser);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetUserDetails()
        {
            var user = await _baseService.Select()
                .FirstOrDefaultAsync(x => x.Id == _userId);

            return Ok(user);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
