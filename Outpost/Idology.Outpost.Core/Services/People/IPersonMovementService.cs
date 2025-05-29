namespace Idology.Outpost.Core.Services.People;

public interface IPersonMovementService
{
    void HandleSunrise();
    void HandleSunset();

    void Update(float delta);
}
