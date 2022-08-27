namespace LawOfDemeter.Before;

public interface IOrganization
{
    public Membership Join(Person person);
}