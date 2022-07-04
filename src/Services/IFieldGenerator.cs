using thegame.Models;
using static thegame.Services.FieldGenerator;

namespace thegame.Services
{
    public interface IFieldGenerator
    {
        public GameDto Create(Difficult dif);
    }
}