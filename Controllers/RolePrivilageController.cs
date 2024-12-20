using System.Net;
using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RolePrivilageController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICollegeRepository<RolePrivilage> _rolePrivilageRepository;
    private readonly ApiResponse _apiResponse;
    public RolePrivilageController(IMapper mapper, ICollegeRepository<RolePrivilage> rolePrivilageRepository)
    {
        _mapper = mapper;
        _rolePrivilageRepository = rolePrivilageRepository;
        _apiResponse = new ApiResponse();
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllRolePrivilagesAsync()
    {
        try
        {
            var rolePrivilage = await _rolePrivilageRepository.GetAllAsync();
            _apiResponse.Data = _mapper.Map<List<RolePrivilageDto>>(rolePrivilage);
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.Status = true;
            return Ok(_apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
            _apiResponse.Status = false;
            _apiResponse.Errors.Add(e.Message);
            return _apiResponse;
        }
    }

    [HttpPost]
    [Route("Create")]
    public async Task<ActionResult<ApiResponse>> CreateRolePrivilageAsync(RolePrivilageDto rolePrivilageDto)
    {
        try
        {
            if (rolePrivilageDto == null)
            {
                return BadRequest();
            }

            RolePrivilage rolePrivilage = _mapper.Map<RolePrivilage>(rolePrivilageDto);
            rolePrivilage.IsDeleted = false;
            rolePrivilage.CreatedDate = DateTime.Now;
            rolePrivilage.ModifiedDate = DateTime.Now;

            var result = await _rolePrivilageRepository.CreateAsync(rolePrivilage);
            rolePrivilageDto.Id = result.Id;
            
            _apiResponse.Data = rolePrivilageDto;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.Status = true;
            return Ok(_apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
            _apiResponse.Status = false;
            _apiResponse.Errors.Add(e.Message);
            return _apiResponse;
        }
    }

    [HttpPut]
    [Route("Update")]
    public async Task<ActionResult<ApiResponse>> UpdateRolePrivilageAsync(RolePrivilageDto rolePrivilageDto)
    {
        try
        {
            if (rolePrivilageDto == null || rolePrivilageDto.Id <= 0)
            {
                return BadRequest("Not valid request");
            }
            var existingRolePrivilage = await _rolePrivilageRepository.GetAsync(n => n.Id == rolePrivilageDto.Id);
            
            var rolePrivilage = _mapper.Map<RolePrivilage>(rolePrivilageDto);
            await _rolePrivilageRepository.UpdateAsync(rolePrivilage);
            _apiResponse.Data = rolePrivilage;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.Status = true;
            return Ok(_apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
            _apiResponse.Status = false;
            _apiResponse.Errors.Add(e.Message);
            return _apiResponse;
        }
    }

    [HttpDelete]
    [Route("Delete/{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteRolePrivilageAsync(int id)
    {
        try
        {
            var ExistingRolePrivilage = await _rolePrivilageRepository.GetAsync(n => n.Id == id);
            if (ExistingRolePrivilage == null)
            {
                return BadRequest("Role Privilage Not Found");
            }
            await _rolePrivilageRepository.DeleteAsync(ExistingRolePrivilage);
            _apiResponse.Data = _mapper.Map<RolePrivilageDto>(ExistingRolePrivilage);
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.Status = true;
            return Ok(_apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
            _apiResponse.Status = false;
            _apiResponse.Errors.Add(e.Message);
            return _apiResponse;
        }
    }
}