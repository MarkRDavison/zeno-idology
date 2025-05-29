namespace Idology.Outpost.Core.Tests.Services.Resources;

[TestClass]
public sealed class ResourceServiceTests
{
    private readonly GameData _gameData;
    private readonly ResourceService _service;

    public ResourceServiceTests()
    {
        _gameData = new GameData();
        _service = new ResourceService(_gameData);
    }

    [TestMethod]
    public void GetResources_DoesNotCreateResources()
    {
        var resources = _service.GetResources();

        Assert.IsFalse(resources.Any());
    }

    [TestMethod]
    public void GetResource_CreatesResourceIfNotPresent()
    {
        const string ResourceName = "GOLD";

        _ = _service.GetResource(ResourceName);

        var resources = _service.GetResources();

        Assert.IsTrue(resources.Any());
        Assert.IsTrue(resources.ContainsKey(ResourceName));
    }

    [TestMethod]
    public void GetResource_ReturnsClonedInstance()
    {
        const string ResourceName = "GOLD";
        const int NewResourceAmount = 100;

        var resource = _service.GetResource(ResourceName);

        var amount = resource.Current;

        resource.Current = NewResourceAmount;

        resource = _service.GetResource(ResourceName);

        Assert.AreNotEqual(NewResourceAmount, resource.Current);
        Assert.AreEqual(amount, resource.Current);
    }

    [TestMethod]
    public void GetResources_ReturnsClonedInstances()
    {
        const string ResourceName = "GOLD";
        const int NewResourceAmount = 100;

        _ = _service.GetResource(ResourceName);
        var resources = _service.GetResources();

        var resource = resources[ResourceName];
        var amount = resource.Current;

        resource.Current = NewResourceAmount;

        resource = _service.GetResource(ResourceName);

        Assert.AreNotEqual(NewResourceAmount, resource.Current);
        Assert.AreEqual(amount, resource.Current);
    }

    [TestMethod]
    public void IncreaseResources_CapsAtMaximum()
    {
        const string ResourceName1 = "GOLD";
        const string ResourceName2 = "WOOD";
        const int Min = 10;
        const int Max = 100;

        _gameData.Resources.Add(ResourceName1, new()
        {
            Min = Min,
            Current = Max / 2,
            Max = Max
        });
        _gameData.Resources.Add(ResourceName2, new()
        {
            Min = Min,
            Current = Max / 2,
            Max = Max
        });

        _service.IncreaseResources(new Dictionary<string, int>
        {
            { ResourceName1, Max },
            { ResourceName2, Max },
        });

        var resources = _service.GetResources();

        Assert.IsTrue(resources.All(_ => _.Value.Max == Max && _.Value.Current == Max));
    }

    [TestMethod]
    public void IncreaseResources_ForNegativeValuesDoesNothing()
    {
        const string ResourceName1 = "GOLD";
        const string ResourceName2 = "WOOD";
        const int Min = 10;
        const int Max = 100;
        const int Amount = 50;

        _gameData.Resources.Add(ResourceName1, new()
        {
            Min = Min,
            Current = Amount,
            Max = Max
        });
        _gameData.Resources.Add(ResourceName2, new()
        {
            Min = Min,
            Current = Amount,
            Max = Max
        });

        _service.IncreaseResources(new Dictionary<string, int>
        {
            { ResourceName1, -Max },
            { ResourceName2, -Max },
        });

        var resources = _service.GetResources();

        Assert.IsTrue(resources.All(_ => _.Value.Current == Amount));
    }

    [TestMethod]
    public void IncreaseResources_IncreasesWhenAboveMaximum()
    {
        const string ResourceName1 = "GOLD";
        const string ResourceName2 = "WOOD";
        const int Min = 10;
        const int Max = 100;
        const int Amount = 50;

        _gameData.Resources.Add(ResourceName1, new()
        {
            Min = Min,
            Current = Amount,
            Max = Max
        });
        _gameData.Resources.Add(ResourceName2, new()
        {
            Min = Min,
            Current = Amount,
            Max = Max
        });

        _service.IncreaseResources(new Dictionary<string, int>
        {
            { ResourceName1, Max },
            { ResourceName2, Max },
        });

        var resources = _service.GetResources();

        Assert.IsTrue(resources.All(_ => _.Value.Current == Max));
    }

    [TestMethod]
    public void TryIncreaseResources_DoesNotAllowAboveMaximum()
    {
        const string ResourceName1 = "GOLD";
        const string ResourceName2 = "WOOD";
        const int Min = 10;
        const int Max = 100;
        const int Amount = 50;

        _gameData.Resources.Add(ResourceName1, new()
        {
            Min = Min,
            Current = Amount,
            Max = Max
        });
        _gameData.Resources.Add(ResourceName2, new()
        {
            Min = Min,
            Current = Amount,
            Max = Max
        });

        var result = _service.TryIncreaseResources(new Dictionary<string, int>
        {
            { ResourceName1, Max },
            { ResourceName2, Max },
        });

        var resources = _service.GetResources();

        Assert.IsFalse(result);
        Assert.IsTrue(resources.All(_ => _.Value.Current == Amount));
    }

    [TestMethod]
    public void TryIncreaseResources_IncreasesWhenBelowMaximum()
    {
        const string ResourceName1 = "GOLD";
        const string ResourceName2 = "WOOD";
        const int Min = 10;
        const int Max = 100;
        const int Amount = 20;

        _gameData.Resources.Add(ResourceName1, new()
        {
            Min = Min,
            Current = Amount,
            Max = Max
        });
        _gameData.Resources.Add(ResourceName2, new()
        {
            Min = Min,
            Current = Amount,
            Max = Max
        });

        var result = _service.TryIncreaseResources(new Dictionary<string, int>
        {
            { ResourceName1, Amount },
            { ResourceName2, Amount },
        });

        var resources = _service.GetResources();

        Assert.IsTrue(result);
        Assert.IsTrue(resources.All(_ => _.Value.Current == Amount + Amount));
    }

    [DataRow(0, 10, 100, 10, true)]
    [DataRow(0, 50, 100, 10, true)]
    [DataRow(0, 50, 100, 50, true)]
    [DataRow(0, 50, 100, 100, false)]
    [DataRow(0, 50, 100, -100, true)]
    [DataTestMethod]
    public void CanIncreaseResources_ReturnsAsExpected(
        int min,
        int current,
        int max,
        int amount,
        bool expectedResult)
    {
        const string ResourceName = "GOLD";

        _gameData.Resources.Add(ResourceName, new()
        {
            Min = min,
            Current = current,
            Max = max
        });

        var result = _service.CanIncreaseResources(new Dictionary<string, int>
        {
            { ResourceName, amount }
        });

        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public void ReduceResources_CapsAtMinimum()
    {
        const string ResourceName1 = "GOLD";
        const string ResourceName2 = "WOOD";
        const int Min = 10;
        const int Max = 100;

        _gameData.Resources.Add(ResourceName1, new()
        {
            Min = Min,
            Current = Max / 2,
            Max = Max
        });
        _gameData.Resources.Add(ResourceName2, new()
        {
            Min = Min,
            Current = Max / 2,
            Max = Max
        });

        _service.ReduceResources(new Dictionary<string, int>
        {
            { ResourceName1, Max / 2 - Min },
            { ResourceName2, Max / 2 - Min },
        });

        var resources = _service.GetResources();

        Assert.IsTrue(resources.All(_ => _.Value.Min == Min && _.Value.Current == Min));
    }

    [TestMethod]
    public void ReduceResources_ForNegativeValuesDoesNothing()
    {
        const string ResourceName1 = "GOLD";
        const string ResourceName2 = "WOOD";
        const int Min = 10;
        const int Max = 100;
        const int Amount = 50;

        _gameData.Resources.Add(ResourceName1, new()
        {
            Min = Min,
            Current = Amount,
            Max = Max
        });
        _gameData.Resources.Add(ResourceName2, new()
        {
            Min = Min,
            Current = Amount,
            Max = Max
        });

        _service.ReduceResources(new Dictionary<string, int>
        {
            { ResourceName1, -Max },
            { ResourceName2, -Max },
        });

        var resources = _service.GetResources();

        Assert.IsTrue(resources.All(_ => _.Value.Current == Amount));
    }

    [TestMethod]
    public void ReduceResources_ReducesWhenBelowMinimum()
    {
        const string ResourceName1 = "GOLD";
        const string ResourceName2 = "WOOD";
        const int Min = 10;
        const int Max = 100;

        _gameData.Resources.Add(ResourceName1, new()
        {
            Min = Min,
            Current = Max / 2,
            Max = Max
        });
        _gameData.Resources.Add(ResourceName2, new()
        {
            Min = Min,
            Current = Max / 2,
            Max = Max
        });

        _service.ReduceResources(new Dictionary<string, int>
        {
            { ResourceName1, Max },
            { ResourceName2, Max },
        });

        var resources = _service.GetResources();

        Assert.IsTrue(resources.All(_ => _.Value.Min == Min && _.Value.Current == Min));
    }

    [TestMethod]
    public void TryReduceResources_DoesNotAllowBelowMinimum()
    {
        const string ResourceName1 = "GOLD";
        const string ResourceName2 = "WOOD";
        const int Min = 10;
        const int Max = 100;
        const int Amount = 50;

        _gameData.Resources.Add(ResourceName1, new()
        {
            Min = Min,
            Current = Amount,
            Max = Max
        });
        _gameData.Resources.Add(ResourceName2, new()
        {
            Min = Min,
            Current = Amount,
            Max = Max
        });

        var success = _service.TryReduceResources(new Dictionary<string, int>
        {
            { ResourceName1, Max },
            { ResourceName2, Max },
        });

        Assert.IsFalse(success);

        var resources = _service.GetResources();

        Assert.IsTrue(resources.All(_ => _.Value.Current == Amount));
    }

    [TestMethod]
    public void TryReduceResources_ReducesWhenAboveMinimum()
    {
        const string ResourceName1 = "GOLD";
        const string ResourceName2 = "WOOD";
        const int Min = 0;
        const int Max = 100;
        const int Amount = 50;

        _gameData.Resources.Add(ResourceName1, new()
        {
            Min = Min,
            Current = Amount,
            Max = Max
        });
        _gameData.Resources.Add(ResourceName2, new()
        {
            Min = Min,
            Current = Amount,
            Max = Max
        });

        var success = _service.TryReduceResources(new Dictionary<string, int>
        {
            { ResourceName1, Amount / 2 },
            { ResourceName2, Amount / 2 },
        });

        Assert.IsTrue(success);

        var resources = _service.GetResources();

        Assert.IsTrue(resources.All(_ => _.Value.Current == Amount / 2));
    }
}
