using droeftoeters_api.Controllers;
using droeftoeters_api.Interfaces;
using droeftoeters_api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace droeftoeters_api.Tests;

[TestClass]
public class ProcedureItemTests
{
     //Read all
    [TestMethod]
    public void Read_ReadAll_Success()
    {
        //Arrange
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out _, out _, outputProcedureItems: new());
        
        //Act
        var response = procedureItemController.ReadAll();

        //Assert
        Assert.IsInstanceOfType(response, typeof(OkObjectResult));
    }   
    
    [TestMethod]
    public void Read_ReadAll_Failed()
    {
        //Arrange
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out _, out _, outputProcedureItems: null);
        
        //Act
        var response = procedureItemController.ReadAll();

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    //Read at id
    [TestMethod]
    public void Read_ReadAtId_Succes()
    {
        //Arrange
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out string id, out _, outputProcedureItem: new ());
        
        //Act
        var response = procedureItemController.Read(id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(OkObjectResult));
    }
    
    [TestMethod]
    public void Read_ReadAtId_Failed()
    {
        //Arrange
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out string id, out _, outputProcedureItem: null);
        
        //Act
        var response = procedureItemController.Read(id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }
    
    [TestMethod]
    public void Read_ReatAtId_InvalidGuid()
    {
        //Arrange
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out string id, out _, inputId: "invalid guid", outputProcedureItem: new ());
        
        //Act
        var response = procedureItemController.Read(id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }
    
    [TestMethod]
    public void Read_ReatAtId_NotFound()
    {
        //Arrange
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out string id, out _, outputProcedureItem: null);
        
        //Act
        var response = procedureItemController.Read(id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }    

    //Write procedure
    [TestMethod]
    public void Write_WriteProcedure_Success()
    {
        //Arrange
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out _, out var procedure, outputBoolean:true);
        
        //Act
        var response = procedureItemController.Write(procedure);

        //Assert
        Assert.IsInstanceOfType(response, typeof(OkObjectResult));
    }

    [TestMethod]
    public void Write_WriteProcedure_Failed()
    {
        //Arrange
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out _, out var procedure, outputBoolean:false);
        
        //Act
        var response = procedureItemController.Write(procedure);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Write_WriteProcedure_InvalidGuid()
    {
        //Arrange
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out _,  out var procedure, inputId: "hldfllghdf", outputBoolean:true);
        
        //Act
        var response = procedureItemController.Write(procedure);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Write_WriteProcedure_AlreadyExists()
    {
        string id = Guid.NewGuid().ToString();
        ProcedureItem procedure = GenerateProcedureItem(id);
        
        //Arrange
        
        //For the existance check to complete
        //The output procedure will have to be the same as the input id, since the existance check runs read(id)
        //which returns a procedure
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out _, out _, inputId:id, outputProcedureItem:procedure, outputBoolean:true);
        
        //Act
        var response = procedureItemController.Write(procedure);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }
    
    //Update procedure
    [TestMethod]
    public void Update_UpdateProcedure_Success()
    {
        //Arrange
        ProcedureItem procedure = GenerateProcedureItem();
        
        //For the existance check to complete
        //The output procedure will have to be the same as the input procedure, since the existance check runs read(id)
        //which returns a procedure
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out _, out _, inputId:procedure.Id, inputProcedureItem:procedure, outputProcedureItem:procedure, outputBoolean:true);
        
        //Act
        var response = procedureItemController.Update(procedure);

        //Assert
        Assert.IsInstanceOfType(response, typeof(OkObjectResult));

    }

    [TestMethod]
    public void Update_UpdateProcedure_Failed()
    {
        //Arrange
        ProcedureItem procedure = GenerateProcedureItem();
        
        //For the existance check to complete
        //The output procedure will have to be the same as the input procedure, since the existance check runs read(id)
        //which returns a procedure
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out _, out _, inputId:procedure.Id, inputProcedureItem:procedure, outputProcedureItem:procedure, outputBoolean:false);
        
        //Act
        var response = procedureItemController.Update(procedure);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Delete_Procedure_InvalidGuid()
    {
        //Arrange
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out _, out var procedure, inputId:"invalid guid");
        
        //Act
        var response = procedureItemController.Update(procedure);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Update_UpdateProcedure_NotFound()
    {
        //Arrange
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out _, out var procedure);
        
        //Act
        var response = procedureItemController.Update(procedure);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }
    
    //Delete procedure
    [TestMethod]
    public void Delete_DeleteProcedure_Success()
    {
        //Arrange
        ProcedureItem procedure = GenerateProcedureItem();
        
        //For the existance check to complete
        //The output procedure will have to be the same as the input procedure, since the existance check runs read(id)
        //which returns a procedure
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out _, out _, inputId:procedure.Id, inputProcedureItem:procedure, outputProcedureItem:procedure, outputBoolean:true);
        
        //Act
        var response = procedureItemController.Delete(procedure.Id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(OkObjectResult));
    }

    [TestMethod]
    public void Delete_DeleteProcedure_Failed()
    {
        //Arrange
        ProcedureItem procedure = GenerateProcedureItem();
        
        //For the existance check to complete
        //The output procedure will have to be the same as the input procedure, since the existance check runs read(id)
        //which returns a procedure
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out _, out _, inputId:procedure.Id, inputProcedureItem:procedure, outputProcedureItem:procedure, outputBoolean:false);
        
        //Act
        var response = procedureItemController.Delete(procedure.Id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Delete_DeleteProcedure_InvalidGuid()
    {
        //Arrange
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out _, out _);
        
        //Act
        var response = procedureItemController.Delete("invalid guid");

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Delete_DeleteProcedure_NotFound()
    {
        //Arrange
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out _, out var procedure, outputBoolean:false);
        
        //Act
        var response = procedureItemController.Delete(procedure.Id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }
    
    //Parent
    [TestMethod]
    public void Read_Parent_Success()
    {
        //Arrange
        Mock<IProcedureItemData> procedureItemData = new();
        Mock<IProcedureData> procedureData = new();
        var id = Guid.NewGuid().ToString();
        var procedure = GenerateProcedure(id);
        var procedureItem = GenerateProcedureItem(id, id);
        
        procedureItemData.Setup(x => x.Read(id)).Returns(procedureItem);
        procedureItemData.Setup(x => x.Parent(id)).Returns(new List<ProcedureItem>());
        procedureData.Setup(x => x.Read(id)).Returns(procedure);
        var procedureItemController =
            GenerateEmptyProcedureItemController(procedureItemData: procedureItemData, procedureData: procedureData);
        
        //Act
        var response = procedureItemController.Parent(id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(OkObjectResult));
    }

    [TestMethod]
    public void Read_Parent_Failure()
    {
        //Arrange
        Mock<IProcedureItemData> procedureItemData = new();
        Mock<IProcedureData> procedureData = new();
        var id = Guid.NewGuid().ToString();
        Procedure? procedure = null;
        var procedureItem = GenerateProcedureItem(id, id);
        
        procedureItemData.Setup(x => x.Read(id)).Returns(procedureItem);
        procedureItemData.Setup(x => x.Parent(id)).Returns(new List<ProcedureItem>());
        procedureData.Setup(x => x.Read(id)).Returns(procedure);
        var procedureItemController =
            GenerateEmptyProcedureItemController(procedureItemData: procedureItemData, procedureData: procedureData);
        
        //Act
        var response = procedureItemController.Parent(id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Read_Parent_InvalidGuid()
    {
        //Arrange
        ProcedureItemController procedureItemController = GenerateProcedureItemController(out _, out _);
        
        //Act
        var response = procedureItemController.Parent("invalid guid");

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Read_Parent_IdNotFound()
    {
        
    }

    [TestMethod]
    public void Read_Parent_ParentNotFound()
    {
        
    }
    
    private Procedure GenerateProcedure(string? id = null) => new()
    {
        Id = id ?? Guid.NewGuid().ToString(),
        Title = "Title",
        Description = "Description",
        ProcedureItems = []
    };    
    private ProcedureItem GenerateProcedureItem(string? id = null, string? parentId = null) => new()
    {
        Id = id ?? Guid.NewGuid().ToString(),
        Title = "Title",
        Description = "Description",
        ProcedureId = parentId ?? Guid.NewGuid().ToString(),
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
    /// <param name="outProcedureItem">The procedure being used in the data call, same as inputProcedureItem, generates a new one if that one is left null</param>
    /// <param name="inputProcedureItem">The procedure being used in the data call, same as outProcedure</param>
    /// <param name="outputProcedureItem">The procedure being returned from the data call.
    /// Used by Read and thus the exists checks. Will succeed if proceedure is not null and the id is the same as inputId</param>
    /// <param name="outputProcedureItems">The procedures list being returned from the data call. Used by ReadAll</param>
    /// <param name="outputBoolean">The boolean being returned from the data call. Used by Write, Update, Delete, AddItem and RemoveItem</param>
    /// <returns></returns>
    private ProcedureItemController GenerateProcedureItemController(out string outId, out ProcedureItem outProcedureItem,
        string? inputId = null, ProcedureItem? inputProcedureItem = null, ProcedureItem? outputProcedureItem = null,
        List<ProcedureItem>? outputProcedureItems = null, bool outputBoolean = false)
    {
        //set up input output data
        Mock<IProcedureItemData> procedureItemData = new();
        inputId ??= Guid.NewGuid().ToString();
        outId = inputId; //Lambdas do not accept out variables
        inputProcedureItem ??= GenerateProcedureItem(inputId);
        outProcedureItem = inputProcedureItem;
        
        //Setup mock data layer
        procedureItemData.Setup(x => x.ReadAll()).Returns(outputProcedureItems!);
        procedureItemData.Setup(x => x.Read(inputId)).Returns(outputProcedureItem);
        procedureItemData.Setup(x => x.Write(inputProcedureItem)).Returns(outputBoolean);
        procedureItemData.Setup(x => x.Update(inputProcedureItem)).Returns(outputBoolean);
        procedureItemData.Setup(x => x.Delete(inputId)).Returns(outputBoolean);
        procedureItemData.Setup(x => x.Parent(inputId)).Returns(new List<ProcedureItem>());
        
        //Create controller
        return GenerateEmptyProcedureItemController(procedureItemData: procedureItemData);
    }


    /// <summary>
    /// Generates an empty procedure item controller, override the default parameters for customization
    /// </summary>
    /// <param name="procedureItemData"></param>
    /// <param name="logger"></param>
    /// <param name="procedureData"></param>
    /// <returns></returns>
    private ProcedureItemController GenerateEmptyProcedureItemController(
        Mock<IProcedureItemData>? procedureItemData = null,
        Mock<ILogger<ProcedureItemController>>? logger = null,
        Mock<IProcedureData>? procedureData = null
    )
    {
        procedureItemData ??= new();
        logger ??= new();
        procedureData ??= new();

        return new ProcedureItemController(procedureItemData.Object, logger.Object, procedureData.Object);
    }
}