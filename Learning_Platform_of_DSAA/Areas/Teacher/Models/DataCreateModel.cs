using System.Collections.Generic;

namespace Learning_Platform_of_DSAA.Areas.Teacher.Models
{
    public class DataCreateModel
    {
        public string Id { get; set; }
        public List<Example> Examples { get; set; }


    }

    public class Example
    {
        public string Input { get; set; }
        public string Output { get; set; }
    }
}
