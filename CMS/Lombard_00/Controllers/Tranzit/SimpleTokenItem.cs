using System.Collections.Generic;

namespace Lombard_00.Controllers.Tranzit
{
    public class SimpleTokenItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string FinallizationDateTime { get; set; }
        public double FinallizationDateTimeDouble { get; set; }

        //tag system
        public IEnumerable<string> Tags { get; set; }
        //optional - rating
        public string RatingAvarage { get; set; }
        public string NumberOfRatings { get; set; }
    }
}
