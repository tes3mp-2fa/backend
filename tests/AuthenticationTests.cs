using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using tes3mp_verifier.API.Controllers;
using tes3mp_verifier.Data;
using tes3mp_verifier.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace tests
{
  [TestClass]
  public class AuthenticationTests
  {
    [TestMethod]
    public async Task Register()
    {
      var options = new DbContextOptionsBuilder<VerifierContext>()
            .UseInMemoryDatabase(databaseName: "Verifier")
            .Options;
      var context = new VerifierContext(options);

      var hasher = new Mock<PasswordHasher>();
      hasher.Setup(x => x.Hash(It.IsAny<string>())).Returns("");

      var controller = new AuthenticationController(hasher.Object, context);

      var input = new AuthenticationController.RegisterInput()
      {
        Nickname = "Test",
        Email = "test@abc.com",
        Password = "test"
      };
      var result = await controller.Register(input);
      Assert.IsTrue(result is OkResult);
    }
  }
}
