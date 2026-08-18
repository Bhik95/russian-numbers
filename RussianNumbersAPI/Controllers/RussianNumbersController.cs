using Microsoft.AspNetCore.Mvc;
using RussianNumbers;

namespace RussianNumbersAPI.Controllers
{
    [ApiController]
    [Route("api/russianNumbers/")]
    public class RussianNumbersController : ControllerBase
    {

        [HttpGet("{number}")]
        public string Get(
            [FromRoute] int number,
            [FromQuery] bool stressMarks = false,
            [FromQuery] GenderNumber gender = GenderNumber.Masculine)
        {
            RussianNumberWriter russianNumberWriter = new()
            {
                GenderNumber = gender,
                IncludeStressMarks = stressMarks
            };

            return russianNumberWriter.Write(number);
        }
    }
}
