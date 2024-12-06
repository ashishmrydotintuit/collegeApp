using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Repository;

public class StudentRepository : IStudentRepository
{
    private readonly CollegeDBContext _context;
  
    public StudentRepository( CollegeDBContext context)
    {
        _context = context;
       
    }
    public async Task<List<Student>> GetAllAsync()
    {
        return await _context.Students.ToListAsync();
    }

    public async Task<Student> GetByIdAsync(int Id)
    {
        return await _context.Students.Where(n => n.Id == Id).FirstOrDefaultAsync();
        
    }

    public async Task<Student> GetByNameAsync(string name)
    {
        return await _context.Students.Where(n => n.StudentName.ToLower().Contains(name.ToLower())).FirstOrDefaultAsync();
    }

    public async Task<int> CreateAsync(Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
        return student.Id;
    }

    public async Task<int> UpdateAsync(Student student)
    {
        _context.Update(student);
        await _context.SaveChangesAsync();
        return student.Id;
    }

    public async Task<bool> DeleteAsync(Student StudentToDelete)
    {
        _context.Students.Remove(StudentToDelete);
        await _context.SaveChangesAsync();
        return true;
    }
}