using droeftoeters_api.Controllers;
using droeftoeters_api.Interfaces;
using droeftoeters_api.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace droeftoeters_api.Tests;

[TestClass]
public class ProcedureTests
{
    [TestMethod]
    public void ReadAll_Success()
    {
        //Arrange
        Mock<IProcedureData> procedureData = new();
        List<Procedure> procedures = new();
        procedureData.Setup(x => x.ReadAll()).Returns(procedures);
        
        ProcedureController procedureController = GenerateProcedureController(procedureData: procedureData);
        
        //Act
        var response = procedureController.ReadAll();

        //Assert
        Assert.IsInstanceOfType(response, typeof(OkObjectResult));
    }   
    
    [TestMethod]
    public void ReadAll_Fail()
    {
        //Arrange
        Mock<IProcedureData> procedureData = new();
        List<Procedure>? procedures = null;
        procedureData.Setup(x => x.ReadAll()).Returns(procedures);
        
        ProcedureController procedureController = GenerateProcedureController(procedureData: procedureData);
        
        //Act
        var response = procedureController.ReadAll();

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }
    
    

    /// <summary>
    /// Generates a procedure controller, override the default parameters for customization
    /// </summary>
    /// <param name="procedureData"></param>
    /// <param name="procedureItemData"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    private ProcedureController GenerateProcedureController(
        Mock<IProcedureData>? procedureData = null,
        Mock<IProcedureItemData>? procedureItemData = null,
        Mock<ILogger<ProcedureController>>? logger = null
    )
    {
        procedureData ??= new();
        procedureItemData ??= new();
        logger ??= new();

        return new ProcedureController(procedureData.Object, procedureItemData.Object, logger.Object);
    }
}