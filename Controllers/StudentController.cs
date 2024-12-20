
using System.Net;
using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Dto;
using CollegeApp.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    public class StudentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;
        private readonly ApiResponse _apiResponse;
        public StudentController(CollegeDBContext dbContext, IMapper mapper, IStudentRepository studentRepository) 
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _apiResponse = new ApiResponse();
        }
        
        [HttpGet("All")]
        //[AllowAnonymous] //This indicates that this api is accessible by everyone.
        public async Task<ActionResult<ApiResponse>> GetStudentsAsync()
        {
            try
            {
                var students = await _studentRepository.GetAllAsync();
                _apiResponse.Data = _mapper.Map<List<studentDto>>(students);//In map always pass the destination object
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception e)
            {
                _apiResponse.Errors.Add(e.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }
            
        }

        
        [HttpGet("{Id:int}", Name = "GetStudentById")]
        public async Task<ActionResult<studentDto>> GetStudentById(int Id)
        {
            if(Id<=0)
                return BadRequest("Id is less than zero");
            var student = await _studentRepository.GetAsync(student => student.Id == Id);
            if (student == null)
            {
                return NotFound($"The student with id {Id} was not found.");
            }
            _apiResponse.Data = _mapper.Map<studentDto>(student);
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<studentDto>> CreateStudent([FromBody]studentDto model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            //This I have done by using custom validator.
  //          if (model.AdmissionDate < DateTime.Now)
  //          {
  //              ModelState.AddModelError("AdmissionDate Error", "Admission Date Must be greater than or equal to the current date");
  //              return BadRequest(ModelState);
   //         }
            Student student = _mapper.Map<Student>(model);
            var newStudent = await _studentRepository.CreateAsync(student);
            model.Id = newStudent.Id;
            _apiResponse.Data= model;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return CreatedAtRoute("GetStudentById", new {id = model.Id},_apiResponse);
            
        }

        
        [HttpGet("by-name/{name:alpha}", Name = "GetStudentByName")]
        public async Task<ActionResult<studentDto>> GetStudentByName(string name)
        {
            var student = await _studentRepository.GetByNameAsync(student => student.StudentName.ToLower().Contains(name.ToLower()));
            if (student == null) { return NotFound("Name of student is not available"); }
            _apiResponse.Data= _mapper.Map<studentDto>(student);
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        [HttpPut]
        [Route("Update")]
        //api/student/update
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> UpdateStudentAsync([FromBody] studentDto dto)
        {
            try
            {
                if (dto == null || dto.Id <= 0)
                    BadRequest();

                var existingStudent = await _studentRepository.GetAsync(student => student.Id == dto.Id, true);

                if (existingStudent == null)
                    return NotFound();

                var newRecord = _mapper.Map<Student>(dto);

                await _studentRepository.UpdateAsync(newRecord);

                return NoContent();
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }
        }


        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> DeleteStudentById(int Id)
        {
            var student =await _studentRepository.GetAsync(student => student.Id == Id);
            if(student == null)
            {
                return NotFound();
            }
            await _studentRepository.DeleteAsync(student);
    
            return NoContent();
        }
    }
}