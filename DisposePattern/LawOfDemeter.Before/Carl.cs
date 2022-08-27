namespace LawOfDemeter.Before;

public class Carl : Person
{
    private readonly List<ISkill> skills = new();
    public DateTime Dob { get; set; }
    public List<Membership> Memberships { get; set; }
    public Job? Job { get; set; }
    public List<Asset> Assets { get; set; }
    public List<Tattoo> Tattoos { get; set; }
    public Home Home { get; set; }
    public Hairstyle Hairstyle { get; set; }
    public List<Friend> Friends { get; set; }
    public bool Alive { get; set; }
    public bool IsSleeping { get; set; }

    public void Quit(Job job)
    {
        if (job == Job)
        {
            Job = null;
        }
    }

    public void Learn(ISkill skill)
    {
        skills.Add(skill);
    }
}