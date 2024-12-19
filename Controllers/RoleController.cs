using System.Net;
using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICollegeRepository<Role> _roleRepository;
    private readonly ApiResponse _apiResponse;
    
    public RoleController(IMapper mapper,ICollegeRepository<Role> roleRepository)
    {
        _mapper = mapper;
        _roleRepository = roleRepository;
        _apiResponse = new ApiResponse();
    }
    [HttpPost]
    [Route("Create")]
    public async Task<ActionResult<ApiResponse>> CreateRole(RoleDto roleDto)
    {
        if (roleDto == null)
        {
            return BadRequest();
        }
        Role role = _mapper.Map<Role>(roleDto);
        role.IsDeleted = false;
        role.CreatedDate = DateTime.Now;
        role.ModifiedDate = DateTime.Now;

        var result = await _roleRepository.CreateAsync(role);
        roleDto.Id = result.Id;
        _apiResponse.Data = roleDto;
        _apiResponse.Status = true;
        _apiResponse.StatusCode = HttpStatusCode.OK;
        //return CreatedAtRoute("GetRoleById", new { id = roleDto.Id }, _apiResponse);
        return Ok(_apiResponse);

    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllRoles()
    {
        var roles = await _roleRepository.GetAllAsync();
        _apiResponse.Data = _mapper.Map<List<RoleDto>>(roles);
        _apiResponse.Status = true;
        _apiResponse.StatusCode = HttpStatusCode.OK;
        return Ok(_apiResponse);
    }
}