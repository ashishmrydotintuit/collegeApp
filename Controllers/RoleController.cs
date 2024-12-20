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
    public async Task<ActionResult<ApiResponse>> CreateRoleAsync(RoleDto roleDto)
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
    public async Task<ActionResult<ApiResponse>> GetAllRolesAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        _apiResponse.Data = _mapper.Map<List<RoleDto>>(roles);
        _apiResponse.Status = true;
        _apiResponse.StatusCode = HttpStatusCode.OK;
        return Ok(_apiResponse);
    }

    [HttpGet]
    [Route("{id:int}", Name = "GetRoleById")]
    public async Task<ActionResult<ApiResponse>> GetRoleByIdAsync(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var role = await _roleRepository.GetAsync(n => n.Id == id);
            if (role == null)
            {
                return NotFound($"Role not found with Id : {id} ");
            }
            _apiResponse.Data = _mapper.Map<RoleDto>(role);  
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.Status = false;
            _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
            _apiResponse.Errors.Add(e.Message);
            return _apiResponse;
            
        }
    }

    [HttpPut]
    [Route("Update")]
    public async Task<ActionResult<ApiResponse>> UpdateRoleAsunc(RoleDto roleDto)
    {
        try
        {
            if (roleDto == null || roleDto.Id <= 0)
            {
                return BadRequest("Role is null");
            }

            var existingRole = await _roleRepository.GetAsync(n => n.Id == roleDto.Id,true);
            if (existingRole == null)
            {
                return NotFound($"Role not found with Id : {roleDto.Id} ");
            }
            var role = _mapper.Map<Role>(roleDto);
            await _roleRepository.UpdateAsync(role);
            _apiResponse.Data = role;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.Status = false;
            _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
            _apiResponse.Errors.Add(e.Message);
            return _apiResponse;
        }
    }

    [HttpDelete]
    [Route("Delete/{id:int}")]
    public async Task<ActionResult<ApiResponse>> DeleteRoleAsync(int id)
    {
        try
        {
            var existingRole = await _roleRepository.GetAsync(n => n.Id == id, false);
            await _roleRepository.DeleteAsync(existingRole);
            _apiResponse.Data = _mapper.Map<RoleDto>(existingRole);
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.Status = false;
            _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
            _apiResponse.Errors.Add(e.Message);
            return _apiResponse;
        }
    }
}