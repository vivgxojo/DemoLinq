using DemoLinq.Controllers;
using DemoLinq.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Net.Http;

namespace TestProject1
{
    public class UnitTest1
    {
        private LieuDBContext context;
        private LieuxController controller;

        private void Setup()
        {
            //simuler la BD
            var options = new DbContextOptionsBuilder<LieuDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            context = new LieuDBContext(options);

            context.Lieux.AddRange(
                new Lieu() { Nom = "Lieu 1" },
                new Lieu() { Nom = "Lieu 2" }
                );

            context.SaveChanges();

            controller = new LieuxController(context);

            //simuler la session
            var httpContext = new DefaultHttpContext();
            var session = new Mock<ISession>();
            var sessionStorage = new Dictionary<string, byte[]>();

            // Mock du comportement de Get / Set pour ISession
            session.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                   .Callback<string, byte[]>((key, value) => sessionStorage[key] = value);

            session.Setup(s => s.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]>.IsAny))
                   .Returns((string key, out byte[] value) =>
                   {
                       return sessionStorage.TryGetValue(key, out value);
                   });

            httpContext.Session = session.Object;

            // Injecter le HttpContext simulé
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task Test1()
        {
            Setup();

            var result = await controller.Index(1);

            var vr = Assert.IsType<ViewResult>(result);
            var m = Assert.IsType < PaginatedList<Lieu>>(vr.Model);
            Assert.Equal(2, m.Count);
        }
    }
}
