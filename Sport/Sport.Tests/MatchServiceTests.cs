namespace Sport.Tests
{
    using Moq;
    using Services;
    using ViewModels.Match;

    using System.Threading.Tasks;
    using Xunit;


    public class MatchServiceTests
    {
        [Fact]
        public async Task GetMatchShouldReturnTrue()
        {
            var matchServiceMock = new Mock<IMatchService>();

            matchServiceMock.Setup(p => p.GetMatch(1)).Returns(new LiveResultViewModel());
        }
    }
}
