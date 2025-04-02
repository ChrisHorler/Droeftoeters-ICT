using droeftoeters_api.Controllers;
using droeftoeters_api.Interfaces;
using droeftoeters_api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace droeftoeters_api.Tests;

[TestClass]
public class ProcedureTests
{
    
    //Read all
    [TestMethod]
    public void Read_ReadAll_Success()
    {
        //Arrange
        ProcedureController procedureController = GenerateProcedureController(out _, out _, outputProcedures: new());
        
        //Act
        var response = procedureController.ReadAll();

        //Assert
        Assert.IsInstanceOfType(response, typeof(OkObjectResult));
    }   
    
    [TestMethod]
    public void Read_ReadAll_Failed()
    {
        //Arrange
        ProcedureController procedureController = GenerateProcedureController(out _, out _, outputProcedures: null);
        
        //Act
        var response = procedureController.ReadAll();

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    //Read at id
    [TestMethod]
    public void Read_ReadAtId_Succes()
    {
        //Arrange
        ProcedureController procedureController = GenerateProcedureController(out string id, out _, outputProcedure: new ());
        
        //Act
        var response = procedureController.Read(id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(OkObjectResult));
    }
    
    [TestMethod]
    public void Read_ReadAtId_Failed()
    {
        //Arrange
        ProcedureController procedureController = GenerateProcedureController(out string id, out _, outputProcedure: null);
        
        //Act
        var response = procedureController.Read(id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }
    
    [TestMethod]
    public void Read_ReatAtId_InvalidGuid()
    {
        //Arrange
        ProcedureController procedureController = GenerateProcedureController(out string id, out _, inputId: "invalid guid", outputProcedure: new ());
        
        //Act
        var response = procedureController.Read(id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }
    
    [TestMethod]
    public void Read_ReatAtId_NotFound()
    {
        //Arrange
        ProcedureController procedureController = GenerateProcedureController(out string id, out _, outputProcedure: null);
        
        //Act
        var response = procedureController.Read(id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));;
    }    

    //Write procedure
    [TestMethod]
    public void Write_WriteProcedure_Success()
    {
        //Arrange
        ProcedureController procedureController = GenerateProcedureController(out string id, out var procedure, outputBoolean:true);
        
        //Act
        var response = procedureController.Write(procedure);

        //Assert
        Assert.IsInstanceOfType(response, typeof(OkObjectResult));
    }

    [TestMethod]
    public void Write_WriteProcedure_Failed()
    {
        //Arrange
        ProcedureController procedureController = GenerateProcedureController(out string id, out var procedure, outputBoolean:false);
        
        //Act
        var response = procedureController.Write(procedure);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Write_WriteProcedure_InvalidGuid()
    {
        //Arrange
        ProcedureController procedureController = GenerateProcedureController(out string id,  out var procedure, inputId: "hldfllghdf", outputBoolean:true);
        
        //Act
        var response = procedureController.Write(procedure);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Write_WriteProcedure_AlreadyExists()
    {
        string id = Guid.NewGuid().ToString();
        Procedure procedure = GenerateProcedure(id);
        
        //Arrange
        
        //For the existance check to complete
        //The output procedure will have to be the same as the input id, since the existance check runs read(id)
        //which returns a procedure
        ProcedureController procedureController = GenerateProcedureController(out _, out _, inputId:id, outputProcedure:procedure, outputBoolean:true);
        
        //Act
        var response = procedureController.Write(procedure);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }
    
    //Update procedure
    [TestMethod]
    public void Update_UpdateProcedure_Success()
    {
        //Arrange
        Procedure procedure = GenerateProcedure();
        
        //For the existance check to complete
        //The output procedure will have to be the same as the input procedure, since the existance check runs read(id)
        //which returns a procedure
        ProcedureController procedureController = GenerateProcedureController(out _, out _, inputId:procedure.Id, inputProcedure:procedure, outputProcedure:procedure, outputBoolean:true);
        
        //Act
        var response = procedureController.Update(procedure);

        //Assert
        Assert.IsInstanceOfType(response, typeof(OkObjectResult));

    }

    [TestMethod]
    public void Update_UpdateProcedure_Failed()
    {
        //Arrange
        Procedure procedure = GenerateProcedure();
        
        //For the existance check to complete
        //The output procedure will have to be the same as the input procedure, since the existance check runs read(id)
        //which returns a procedure
        ProcedureController procedureController = GenerateProcedureController(out _, out _, inputId:procedure.Id, inputProcedure:procedure, outputProcedure:procedure, outputBoolean:false);
        
        //Act
        var response = procedureController.Update(procedure);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Delete_Procedure_InvalidGuid()
    {
        //Arrange
        ProcedureController procedureController = GenerateProcedureController(out _, out var procedure, inputId:"invalid guid");
        
        //Act
        var response = procedureController.Update(procedure);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Update_UpdateProcedure_NotFound()
    {
        //Arrange
        ProcedureController procedureController = GenerateProcedureController(out _, out var procedure);
        
        //Act
        var response = procedureController.Update(procedure);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }
    
    //Delete procedure
    [TestMethod]
    public void Delete_DeleteProcedure_Success()
    {
        //Arrange
        Procedure procedure = GenerateProcedure();
        
        //For the existance check to complete
        //The output procedure will have to be the same as the input procedure, since the existance check runs read(id)
        //which returns a procedure
        ProcedureController procedureController = GenerateProcedureController(out _, out _, inputId:procedure.Id, inputProcedure:procedure, outputProcedure:procedure, outputBoolean:true);
        
        //Act
        var response = procedureController.Delete(procedure.Id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(OkObjectResult));
    }

    [TestMethod]
    public void Delete_DeleteProcedure_Failed()
    {
        //Arrange
        Procedure procedure = GenerateProcedure();
        
        //For the existance check to complete
        //The output procedure will have to be the same as the input procedure, since the existance check runs read(id)
        //which returns a procedure
        ProcedureController procedureController = GenerateProcedureController(out _, out _, inputId:procedure.Id, inputProcedure:procedure, outputProcedure:procedure, outputBoolean:false);
        
        //Act
        var response = procedureController.Delete(procedure.Id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Delete_DeleteProcedure_InvalidGuid()
    {
        //Arrange
        ProcedureController procedureController = GenerateProcedureController(out _, out _);
        
        //Act
        var response = procedureController.Delete("invalid guid");

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Delete_DeleteProcedure_NotFound()
    {
        //Arrange
        ProcedureController procedureController = GenerateProcedureController(out _, out var procedure, outputBoolean:false);
        
        //Act
        var response = procedureController.Delete(procedure.Id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }
    
    //Add procedure item
    [TestMethod]
    public void Write_AddItem_Success()
    {
        //Arrange
        Mock<IProcedureData> procedureData = new();
        Mock<IProcedureItemData> procedureItemData = new();
        var id = Guid.NewGuid().ToString();
        var procedure = GenerateProcedure(id);
        
        procedureData.Setup(x => x.AddProcedureItem(It.IsAny<ProcedureItem>())).Returns(true);
        procedureItemData.Setup(x => x.Read(id)).Returns(GenerateItem(id));
        
        var procedureController = GenerateEmptyProcedureController(procedureData: procedureData, procedureItemData: procedureItemData);

        //Act
        var response = procedureController.AddProcedureItem(GenerateItem());

        //Assert
        Assert.IsInstanceOfType(response, typeof(OkObjectResult));
    }

    [TestMethod]
    public void Write_AddItem_Failed()
    {
        //Arrange
        
        //Act
        
        //Assert

    }

    [TestMethod]
    public void Write_AddItem_InvalidProcedureGuid()
    {
        //Arrange
        
        //Act
        
        //Assert

    }

    [TestMethod]
    public void Write_AddItem_InvalidItemGuid()
    {
        //Arrange
        
        //Act
        
        //Assert

    }

    [TestMethod]
    public void Write_AddItem_ProcedureNotFound()
    {
        //Arrange
        
        //Act
        
        //Assert

    }

    [TestMethod]
    public void Write_AddItem_ItemAlreadyExists()
    {
        //Arrange
        
        //Act
        
        //Assert

    }
    
    //Delete procedure item
    [TestMethod]
    public void Delete_RemoveItem_Succes()
    {
        //Arrange
        
        //Act
        
        //Assert

    }

    [TestMethod]
    public void Delete_RemoveItem_Failed()
    {
        //Arrange
        
        //Act
        
        //Assert

    }

    [TestMethod]
    public void Delete_RemoveItem_InvalidProcedureGuid()
    {
        //Arrange
        
        //Act
        
        //Assert

    }

    [TestMethod]
    public void Delete_RemoveItem_InvalidItemGuid()
    {
        //Arrange
        
        //Act
        
        //Assert

    }

    [TestMethod]
    public void Delete_RemoveItem_ProcedureNotFound()
    {
        //Arrange
        
        //Act
        
        //Assert

    }

    [TestMethod]
    public void Delete_RemoveItem_ItemNotFound()
    {
        //Arrange
        
        //Act
        
        //Assert

    }

    private Procedure GenerateProcedure(string? id = null) => new()
    {
        Id = id ?? Guid.NewGuid().ToString(),
        Title = "Title",
        Description = "Description",
        ProcedureItems = []
    };    
    private ProcedureItem GenerateItem(string? id = null) => new()
    {
        Id = id ?? Guid.NewGuid().ToString(),
        Title = "Title",
        Description = "Description",
        ProcedureId = Guid.NewGuid().ToString(),
        NextItemId =  Guid.NewGuid().ToString(),
        PreviousItemId =  Guid.NewGuid().ToString()
    };

    /// <summary>
    /// Generates a post setup procedure controller,
    /// use GenerateEmptyProcedureController If you want to tweak the constructor data.
    /// Use the output procedure if you want to be able to get true back from a mocked method using the procedure as parameter
    /// </summary>
    /// <param name="outId">Use this guid if you dont want to generate one for the controller method</param>
    /// <param name="inputId">The id being used in the data call</param>
    /// <param name="outProcedure">The procedure being used in the data call, same as inputProcedure, generates a new one if that one is left null</param>
    /// <param name="inputProcedure">The procedure being used in the data call, same as outProcedure</param>
    /// <param name="outputProcedure">The procedure being returned from the data call.
    /// Used by Read and thus the exists checks. Will succeed if proceedure is not null and the id is the same as inputId</param>
    /// <param name="outputProcedures">The procedures list being returned from the data call. Used by ReadAll</param>
    /// <param name="outputBoolean">The boolean being returned from the data call. Used by Write, Update, Delete, AddItem and RemoveItem</param>
    /// <returns></returns>
    private ProcedureController GenerateProcedureController(out string outId, out Procedure outProcedure,
        string? inputId = null, Procedure? inputProcedure = null, Procedure? outputProcedure = null,
        List<Procedure>? outputProcedures = null, bool outputBoolean = false)
    {
        Mock<IProcedureData> procedureData = new();
        inputId ??= Guid.NewGuid().ToString();
        outId = inputId; //Lambdas do not accept out variables
        inputProcedure ??= GenerateProcedure(inputId);
        outProcedure = inputProcedure;
        
        procedureData.Setup(x => x.ReadAll()).Returns(outputProcedures);
        procedureData.Setup(x => x.Read(inputId)).Returns(outputProcedure);
        procedureData.Setup(x => x.Write(inputProcedure)).Returns(outputBoolean);
        procedureData.Setup(x => x.Update(inputProcedure)).Returns(outputBoolean);
        procedureData.Setup(x => x.Delete(inputId)).Returns(outputBoolean);
        procedureData.Setup(x => x.AddProcedureItem(It.IsAny<ProcedureItem>())).Returns(outputBoolean);
        procedureData.Setup(x => x.RemoveProcedureItem(inputId)).Returns(outputBoolean);
        
        return GenerateEmptyProcedureController(procedureData: procedureData);
    }    
    
    
    /// <summary>
    /// Generates an empty procedure controller, override the default parameters for customization
    /// </summary>
    /// <param name="procedureData"></param>
    /// <param name="procedureItemData"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    private ProcedureController GenerateEmptyProcedureController(
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