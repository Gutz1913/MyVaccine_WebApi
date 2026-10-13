namespace MyVaccine.WebApi.Models;

public class DependentAllergy : BaseModel
{
    public int DependentId { get; set; }
    public int AllergyId { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    public Allergy Allergy { get; set; } = null!;
    public Dependent Dependent { get; set; } = null!;
}

