using AutoMapper;
using FluentValidation;
using Malmasp.Contexts;
using Malmasp.Dtos;
using Malmasp.Models;
using Malmasp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Malmasp.Controllers;

[ApiController]
[Route("users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public ActionResult GetAll(
        [FromServices] IMapper mapper)
    {
        Console.WriteLine(HttpContext.User.Claims.First().Value);
        
        var users = _context.Users
            .Select(u => mapper.Map<UserResponseDto>(u))
            .ToList();

        return Ok(
            new Payload()
            {
                Data = users,
            });
    }

    [HttpGet("{id:required}")]
    public ActionResult GetById(
        [FromServices] IMapper mapper,
        ulong id
        )
    {
        var user = _context.Users
            .Select(u => mapper.Map<UserResponseDto>(u))
            .SingleOrDefault(u => u.Id == id);

        return Ok(new Payload()
        {
            Data = user
        });
    }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult Create(
        [FromServices] IValidator<UserRequestDto> validator,
        [FromServices] IMapper mapper,
        [FromBody] UserRequestDto dto)
    {
        var result = validator.Validate(dto);
        if (!result.IsValid)
        {
            return BadRequest(Payload.FromValidationResult(result));
        }

        var user = mapper.Map<User>(dto);
        _context.Add(user);
        _context.SaveChanges();

        return Created(nameof(GetById), new Payload()
        {
            Data = mapper.Map<UserResponseDto>(user)
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult Login(
        [FromServices] IValidator<UserRequestDto> validator,
        [FromServices] HasherService hasherService,
        [FromBody] UserRequestDto dto
        )
    {
        var result = validator.Validate(dto);
        if (!result.IsValid)
        {
            return BadRequest(Payload.FromValidationResult(result));
        }
        
        var user = _context.Users.SingleOrDefault(u => u.Name == dto.Name);
        if (user == null || !hasherService.VerifyPassword(user.Password, dto.Password))
        {
            return Unauthorized(new Payload()
            {
                Error = "Wrong name or password"
            });
        }

        return Ok(new Payload()
        {
            Data = hasherService.CreateJwt(user.Id.ToString())
        });
    }
}