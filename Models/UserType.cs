namespace CollegeApp.Data;

public class UserType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    //student can assign to many people one usertype can have multile user
    public virtual ICollection<User> Users { get; set; }
}