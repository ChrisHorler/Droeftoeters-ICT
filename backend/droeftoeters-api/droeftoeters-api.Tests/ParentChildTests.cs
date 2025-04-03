using droeftoeters_api.Controllers;
using droeftoeters_api.Interfaces;
using droeftoeters_api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace droeftoeters_api.Tests;

[TestClass]
public class ParentChildTests
{
     //Read all
    [TestMethod]
    public void Read_ReadAll_Success()
    {
        //Arrange
        ParentChildController parentChildItemController = GenerateParentChildController(out _, out _, outputParentChildren: new());
        
        //Act
        var response = parentChildItemController.ReadAll();

        //Assert
        Assert.IsInstanceOfType(response, typeof(OkObjectResult));
    }   
    
    [TestMethod]
    public void Read_ReadAll_Failed()
    {
        //Arrange
        ParentChildController parentChildItemController = GenerateParentChildController(out _, out _, outputParentChildren: null);
        
        //Act
        var response = parentChildItemController.ReadAll();

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    //Read at id
    [TestMethod]
    public void Read_ReadAtId_Succes()
    {
        //Arrange
        ParentChildController parentChildItemController = GenerateParentChildController(out string id, out _, outputParentChild: new ());
        
        //Act
        var response = parentChildItemController.Read(id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(OkObjectResult));
    }
    
    [TestMethod]
    public void Read_ReadAtId_Failed()
    {
        //Arrange
        ParentChildController parentChildItemController = GenerateParentChildController(out string id, out _, outputParentChild: null);
        
        //Act
        var response = parentChildItemController.Read(id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }
    
    [TestMethod]
    public void Read_ReatAtId_InvalidGuid()
    {
        //Arrange
        ParentChildController parentChildItemController = GenerateParentChildController(out string id, out _, inputId: "invalid guid", outputParentChild: new ());
        
        //Act
        var response = parentChildItemController.Read(id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }
    
    [TestMethod]
    public void Read_ReatAtId_NotFound()
    {
        //Arrange
        ParentChildController parentChildItemController = GenerateParentChildController(out string id, out _, outputParentChild: null);
        
        //Act
        var response = parentChildItemController.Read(id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }    

    //Write parentChild
    [TestMethod]
    public void Write_WriteProcedure_Success()
    {
        //Arrange
        ParentChildController parentChildItemController = GenerateParentChildController(out _, out var parentChild, outputBoolean:true);
        
        //Act
        var response = parentChildItemController.Write(parentChild);

        //Assert
        Assert.IsInstanceOfType(response, typeof(OkObjectResult));
    }

    [TestMethod]
    public void Write_WriteProcedure_Failed()
    {
        //Arrange
        ParentChildController parentChildItemController = GenerateParentChildController(out _, out var parentChild, outputBoolean:false);
        
        //Act
        var response = parentChildItemController.Write(parentChild);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Write_WriteProcedure_InvalidGuid()
    {
        //Arrange
        ParentChildController parentChildItemController = GenerateParentChildController(out _,  out var parentChild, inputId: "hldfllghdf", outputBoolean:true);
        
        //Act
        var response = parentChildItemController.Write(parentChild);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Write_WriteProcedure_AlreadyExists()
    {
        string id = Guid.NewGuid().ToString();
        ParentChild parentChild = GenerateParentChild(id);
        
        //Arrange
        
        //For the existance check to complete
        //The output parentChild will have to be the same as the input id, since the existance check runs read(id)
        //which returns a parentChild
        ParentChildController parentChildItemController = GenerateParentChildController(out _, out _, inputId:id, outputParentChild:parentChild, outputBoolean:true);
        
        //Act
        var response = parentChildItemController.Write(parentChild);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }
    
    //Delete parentChild
    [TestMethod]
    public void Delete_DeleteProcedure_Success()
    {
        //Arrange
        ParentChild parentChild = GenerateParentChild();
        
        //For the existance check to complete
        //The output parentChild will have to be the same as the input parentChild, since the existance check runs read(id)
        //which returns a parentChild
        ParentChildController parentChildItemController = GenerateParentChildController(out _, out _, inputId:parentChild.Id, inputParentChild:parentChild, outputParentChild:parentChild, outputBoolean:true);
        
        //Act
        var response = parentChildItemController.Delete(parentChild.Id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(OkObjectResult));
    }

    [TestMethod]
    public void Delete_DeleteProcedure_Failed()
    {
        //Arrange
        ParentChild parentChild = GenerateParentChild();
        
        //For the existance check to complete
        //The output parentChild will have to be the same as the input parentChild, since the existance check runs read(id)
        //which returns a parentChild
        ParentChildController parentChildItemController = GenerateParentChildController(out _, out _, inputId:parentChild.Id, inputParentChild:parentChild, outputParentChild:parentChild, outputBoolean:false);
        
        //Act
        var response = parentChildItemController.Delete(parentChild.Id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Delete_DeleteProcedure_InvalidGuid()
    {
        //Arrange
        ParentChildController parentChildItemController = GenerateParentChildController(out _, out _);
        
        //Act
        var response = parentChildItemController.Delete("invalid guid");

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Delete_DeleteProcedure_NotFound()
    {
        //Arrange
        ParentChildController parentChildItemController = GenerateParentChildController(out _, out var parentChild, outputBoolean:false);
        
        //Act
        var response = parentChildItemController.Delete(parentChild.Id);

        //Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestResult));
    }
    
    private ParentChild GenerateParentChild(string? id = null, string? parentId = null) => new()
    {
        Id = id ?? Guid.NewGuid().ToString(),
        ParentId = parentId ?? Guid.NewGuid().ToString(),
        ChildId =  Guid.NewGuid().ToString()
    };
    
    /// <summary>
    /// Generates a post setup parentChild controller,
    /// use GenerateEmptyProcedureController If you want to tweak the constructor data.
    /// Use the output parentChild if you want to be able to get true back from a mocked method using the parentChild as parameter
    /// </summary>
    /// <param name="outId">Use this guid if you dont want to generate one for the controller method</param>
    /// <param name="inputId">The id being used in the data call</param>
    /// <param name="outParentChild">The parentChild being used in the data call, same as inputparentChild, generates a new one if that one is left null</param>
    /// <param name="inputParentChild">The parentChild being used in the data call, same as outProcedure</param>
    /// <param name="outputParentChild">The parentChild being returned from the data call.
    /// Used by Read and thus the exists checks. Will succeed if proceedure is not null and the id is the same as inputId</param>
    /// <param name="outputParentChildren">The parentChilds list being returned from the data call. Used by ReadAll</param>
    /// <param name="outputBoolean">The boolean being returned from the data call. Used by Write, Update, Delete, AddItem and RemoveItem</param>
    /// <returns></returns>
    private ParentChildController GenerateParentChildController(out string outId, out ParentChild outParentChild,
        string? inputId = null, ParentChild? inputParentChild = null, ParentChild? outputParentChild = null,
        List<ParentChild>? outputParentChildren = null, bool outputBoolean = false)
    {
        //set up input output data
        Mock<IParentChildData> parentChildData = new();
        inputId ??= Guid.NewGuid().ToString();
        outId = inputId; //Lambdas do not accept out variables
        inputParentChild ??= GenerateParentChild(inputId);
        outParentChild = inputParentChild;
        
        //Setup mock data layer
        parentChildData.Setup(x => x.ReadAll()).Returns(outputParentChildren!);
        parentChildData.Setup(x => x.Read(inputId)).Returns(outputParentChild);
        parentChildData.Setup(x => x.Write(inputParentChild)).Returns(outputBoolean);
        parentChildData.Setup(x => x.Update(inputParentChild)).Returns(outputBoolean);
        parentChildData.Setup(x => x.Delete(inputId)).Returns(outputBoolean);
        
        //Create controller
        return GenerateEmptyParentChildController(parentChildData);
    }    
    
    
    /// <summary>
    /// Generates an empty parentChild item controller, override the default parameters for customization
    /// </summary>
    /// <param name="parentChildData"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    private ParentChildController GenerateEmptyParentChildController(
        Mock<IParentChildData>? parentChildData = null,
        Mock<ILogger<ParentChildController>>? logger = null
    )
    {
        parentChildData ??= new();
        logger ??= new();
        
        return new (parentChildData.Object, logger.Object);
    }
}