using AutoFixture;
using AutoFixture.AutoFakeItEasy;
using AutoFixture.Xunit2;

namespace bramvandenbussche.readwiser.api.tests;

public class UnitTestAttribute : AutoDataAttribute
{
    public UnitTestAttribute() : base(CreateBasicFixture)
    {
        /* Leave empty */
    }

    /// <summary>
    ///     Creates the fixture for a basic in-memory unit test, to be re-used by other TestAttributes
    /// </summary>
    /// <returns>Configured fixture</returns>
    public static IFixture CreateBasicFixture()
    {
        var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization() { ConfigureMembers = true });

        //fixture.Customizations.Add(new EmptyUdbUserSpecimenBuilder(fixture));

        //fixture.Customize<IHttpContextAccessor>(c => c.FromFactory(() =>
        //{
        //    var hca = A.Fake<IHttpContextAccessor>();
        //    A.CallTo(() => hca.HttpContext).Returns(fixture.Create<HttpContext>());
        //    return hca;
        //}));

        return fixture;
    }
}

/// <summary>
/// Ensures that test params with for type UdbUser with a name containing "empty" get generated with an empty UdbUser
/// https://blog.ploeh.dk/2010/10/19/Convention-basedCustomizationswithAutoFixture/
/// </summary>
//public class EmptyUdbUserSpecimenBuilder : ISpecimenBuilder
//{
//    private readonly IFixture _fixture;

//    public EmptyUdbUserSpecimenBuilder(IFixture fixture)
//    {
//        _fixture = fixture;
//    }

//    public object Create(object request, ISpecimenContext context)
//    {
//        var pi = request as ParameterInfo;
//        if (pi == null)
//        {
//            return new NoSpecimen();
//        }
//        if (pi.ParameterType != typeof(UdbUser)
//            || !pi.Name.Contains("empty"))
//        {
//            return new NoSpecimen();
//        }

//        return _fixture.Build<UdbUser>().With(x => x.UdbUserId, string.Empty).Create(); ;
//    }
//}