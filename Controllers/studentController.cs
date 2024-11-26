﻿using CollegeApp.Dto;
using CollegeApp.Models;
using CollegeApp.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class studentController : ControllerBase
    {
        [HttpGet("All")]
        public ActionResult<IEnumerable<studentDto>> GetStudents()
        {
            var student = collegeRepository.Students.Select(n => new studentDto()
            {
                Id = n.Id,
                StudentName= n.StudentName,
                Address= n.Address,
                Email= n.Email,
            });
            return Ok(student);
        }

        [HttpGet("{Id:int}", Name = "GetStudentById")]
        public ActionResult<studentDto> GetStudentById(int Id)
        {
            if(Id<=0)
            {
                return BadRequest("Id is less than zero");
            }
            var student = collegeRepository.Students.Where(n => n.Id == Id).FirstOrDefault();
            if (student == null)
            {
                return NotFound();
            }
            var studentDTO = new studentDto
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Address = student.Address,
                Email = student.Email,
            };
            return Ok(studentDTO);
        }

        [HttpPost]
        [Route("create")]
        public ActionResult<studentDto> CreateStudent([FromBody]studentDto model)
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

            int newId = collegeRepository.Students.LastOrDefault().Id + 1;
            Student student = new Student
            {
                Id = newId,
                StudentName = model.StudentName,
                Address = model.Address,
                Email = model.Email,
            };
            collegeRepository.Students.Add(student);
            model.Id = student.Id;
            
            return CreatedAtRoute("GetStudentById", new {id = model.Id},model);
            
        }

        [HttpGet("by-name/{name:alpha}", Name = "GetStudentByName")]
        public ActionResult<studentDto> GetStudentByName(string name)
        {
            var student = collegeRepository.Students.Where(n => n.StudentName == name).FirstOrDefault();
            if (student == null) { return NotFound("Name of student is not available"); }
            var studentDTO = new studentDto
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Address = student.Address,
                Email = student.Email,
            };
            return Ok(studentDTO);
        }

        [HttpPut]
        [Route("Update")]
        public ActionResult UpdateStudent([FromBody] studentDto model)
        {
            if(model == null || model.Id <=0)
            {
                return NotFound();
            }

            var  existingStudent = collegeRepository.Students.Where(s => s.Id == model.Id).FirstOrDefault();
            if(existingStudent == null){
                return NotFound();
            }
            existingStudent.StudentName = model.StudentName;
            existingStudent.Email = model.Email;
            existingStudent.Address = model.Address;
            
            return Ok();
        }
       [HttpPatch]
        [Route("{Id:int}/UpdatePartial")]
        public ActionResult UpdateStudentPartial(int Id, [FromBody] JsonPatchDocument<studentDto> patchDocument)
        {
            if(patchDocument == null || Id <=0)
            {
                return BadRequest("Not found");
            }

            var  existingStudent = collegeRepository.Students.Where(s => s.Id == Id).FirstOrDefault();
            if(existingStudent == null){
                return NotFound();
            }
            var studentDTO= new studentDto
            { 
                Id = existingStudent.Id,
                StudentName = existingStudent.StudentName,
                Email = existingStudent.Email,
                Address = existingStudent.Address
            };
            patchDocument.ApplyTo(studentDTO,ModelState); //If there is any error the error is store in the model state.
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            existingStudent.StudentName = studentDTO.StudentName;
            existingStudent.Email = studentDTO.Email;
            existingStudent.Address = studentDTO.Address;
            
            return Ok();
        }

        [HttpDelete("{Id:int}")]
        public ActionResult DeleteStudentById(int Id)
        {
            var student = collegeRepository.Students.FirstOrDefault(n => n.Id == Id);
            if(student == null)
            {
                return NotFound();
            }
            collegeRepository.Students.Remove(student);

            return NoContent();
        }
    }
}