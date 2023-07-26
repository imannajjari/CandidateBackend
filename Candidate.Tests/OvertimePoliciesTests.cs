using Candidate.OvertimePolicies.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Candidate.Tests;

[TestClass]
public class OvertimePoliciesTests
{
    private IServiceProvider _serviceProvider;
    private  IOvertimeCalculatorFactory _factory;

    [TestInitialize]
    public void Initialize()
    {
        var services = new ServiceCollection();
        Api.IoC.Injector.RegisterServices(services);
        _serviceProvider = services.BuildServiceProvider();
       
        _factory = _serviceProvider.GetService<IOvertimeCalculatorFactory>();
    }

    [TestMethod]
    [DataRow("CalculatorA", 1000, 10, 45, 337.5)]
    [DataRow("CalculatorB", 1000, 10, 45, 450)]
    [DataRow("CalculatorC", 1000, 10, 45, 337.5)]
    public void TestCalculator(string type,long basicSalary,long allowance,int hoursWorked,double expectedResult)
    {
            
        var calculator = _factory.CreateOvertimeCalculator(type);
        var result=calculator.CalculateOvertime(basicSalary, allowance, hoursWorked);
        
        // Assert
        // Check if the result matches the expected value
         Assert.AreEqual(expectedResult, result);
    }

        
}