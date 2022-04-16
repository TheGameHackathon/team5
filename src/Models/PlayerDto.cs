namespace thegame.Models
{
    public class PlayerDto
    {
        public VectorDto Position { get; set; }

        public PlayerDto(VectorDto position)
        {
            Position = position;
        }
    }
}