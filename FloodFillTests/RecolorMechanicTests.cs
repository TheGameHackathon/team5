namespace FloodFillTests
{
    public class RecolorMechanicTests
    {
        private GameDto game;
        private GamesRepository gamesRepository;
        private FieldGenerator fieldGenerator;
        private FloodFillGame floodFillGame;

        [SetUp]
        public void Setup()
        {
            var width = 4;
            var height = 4;
            var testCells = new[]
            {
            new CellDto("1", new VectorDto {X = 0, Y = 0}, "color1", "", 0),
            new CellDto("2", new VectorDto {X = 0, Y = 1}, "color1", "", 0),
            new CellDto("3", new VectorDto {X = 1, Y = 0}, "color2", "", 20),
            new CellDto("4", new VectorDto {X = 1, Y = 1 }, "color4", "☺", 10),
            };
            game = new GameDto(testCells, false, true, width, height, Guid.NewGuid(), false, 0);
            fieldGenerator = new FieldGenerator();
            gamesRepository = new GamesRepository(fieldGenerator);
            gamesRepository.AddNewGame(game);
            floodFillGame = gamesRepository.GetGame(game.Id);
        }

        [TestCase(3, 0, ExpectedResult = "color4", TestName = "Diagonal click")]
        [TestCase(2, 0, ExpectedResult = "color2", TestName = "Click in a row X")]
        public string ChangeColor_OnCorrectClick(int x, int y)
        {
            var userInput = new UserInputDto();
            userInput.ClickedPos = new VectorDto()
            {
                X = x,
                Y = y
            };
            floodFillGame.Move(userInput);
            //floodFillGame.Field.Where(cell => cell.Pos.X == 0 && cell.Pos.Y == 0).First().Type.Should().Be("color4");
            return floodFillGame.Field.Where(cell => cell.Pos.X == 0 && cell.Pos.Y == 0).First().Type;
        }

        [Test]
        public void FindAndRecolor_Neighborhoods_OnClick()
        {
            var userInput = new UserInputDto();
            userInput.ClickedPos = new VectorDto()
            {
                X = 3,
                Y = 0
            };
            floodFillGame.Move(userInput);
            floodFillGame.Field
                .Where(cell => cell.Pos.X == 0 && cell.Pos.Y == 0)
                .First().Type.Should().Be("color4");
            floodFillGame.Field
                .Where(cell => cell.Pos.X == 0 && cell.Pos.Y == 1)
                .First().Type.Should().Be("color4");
        }

    }
}