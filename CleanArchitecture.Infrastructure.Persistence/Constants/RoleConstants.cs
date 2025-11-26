namespace CleanArchitecture.Infrastructure.Persistence.Constants
{
    public class RoleConstants
    {
        public static RoleConstantDTO? AdministratorRole { get; private set; } = new RoleConstantDTO()
        {
            Name = "Administrator",
            NormalizedName = "ADMINISTRATOR"
        };
    }

    public class RoleConstantDTO
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}
