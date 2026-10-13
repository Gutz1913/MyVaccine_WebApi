namespace MyVaccine.WebApi.Models;

public class User : BaseModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string AspNetUserId { get; set; } = string.Empty;
    public ApplicationUser AspNetUser { get; set; } = null!;
    public int FamilyGroupId { get; set; }
    public FamilyGroup FamilyGroup { get; set; } = null!;
    public ICollection<Dependent> Dependents { get; set; } = new List<Dependent>();
    public ICollection<UserAllergy> UserAllergies { get; set; } = new List<UserAllergy>();
    public ICollection<VaccineRecord> VaccineRecords { get; set; } = new List<VaccineRecord>();
}

