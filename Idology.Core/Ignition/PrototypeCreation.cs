namespace Idology.Core.Ignition;

public class PrototypeCreation
{
    private readonly IPrototypeService<BuildingPrototype, BuildingComponent> _buildingPrototypeService;
    private readonly IPrototypeService<WorkerPrototype, WorkerComponent> _workerPrototypeService;

    public PrototypeCreation(
        IPrototypeService<BuildingPrototype, BuildingComponent> buildingPrototypeService,
        IPrototypeService<WorkerPrototype, WorkerComponent> workerPrototypeService)
    {
        _buildingPrototypeService = buildingPrototypeService;
        _workerPrototypeService = workerPrototypeService;
    }

    public void Init()
    {
        Buildings();
        Workers();
    }

    private void Buildings()
    {
        _buildingPrototypeService.RegisterPrototype("BasicTent", new BuildingPrototype
        {
            Id = StringHash.Hash("BasicTent"),
            Name = "BasicTent",
            SheetName = ResourceConstants.CustomSpriteSheet,
            SpriteName = CustomSpriteNames.BasicTent,
            ProvidedAccomodation =
            {
                { StringHash.Hash("Basic"), 3 }
            }
        });
        _buildingPrototypeService.RegisterPrototype("BasicField", new BuildingPrototype
        {
            Id = StringHash.Hash("BasicField"),
            Name = "BasicField",
            SheetName = ResourceConstants.CustomSpriteSheet,
            SpriteName = CustomSpriteNames.BasicField,
            ProvidedJobs =
            {
                { StringHash.Hash("Farmer"), 2 }
            },
            Production = new()
            {
                Inputs = [],
                Outputs =
                {
                    { "Wheat", new ProductionRange(4, 8) }
                },
                Time = 16.0f
            }
        });
        _buildingPrototypeService.RegisterPrototype("Bakery", new BuildingPrototype
        {
            Id = StringHash.Hash("Bakery"),
            Name = "Bakery",
            SheetName = ResourceConstants.CustomSpriteSheet,
            SpriteName = CustomSpriteNames.Bakery,
            ProvidedJobs =
            {
                { StringHash.Hash("Baker"), 1 }
            },
            Production = new()
            {
                Inputs =
                {
                    { "Wheat", 12 }
                },
                Outputs =
                {
                    { "Bread", new ProductionRange(4, 8) }
                },
                Time = 24.0f
            }
        });
    }

    private void Workers()
    {
        _workerPrototypeService.RegisterPrototype("Unemployed", new WorkerPrototype
        {
            Id = StringHash.Hash("Unemployed"),
            Name = "Unemployed"
        });
        _workerPrototypeService.RegisterPrototype("BasicLabourer", new WorkerPrototype
        {
            Id = StringHash.Hash("BasicLabourer"),
            Name = "BasicLabourer",
            JobTypes =
            {
                StringHash.Hash("Farmer"),
                StringHash.Hash("Builder"),
                StringHash.Hash("Baker"),
                StringHash.Hash("Hauler")
            }
        });
    }
}
