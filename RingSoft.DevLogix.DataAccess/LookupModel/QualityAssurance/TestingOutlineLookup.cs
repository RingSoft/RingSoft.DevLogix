namespace RingSoft.DevLogix.DataAccess.LookupModel
{
    public class TestingOutlineLookup
    {
        public string Name { get; set; }

        public string Product { get; set; }

        public string AssignedTo { get; set; }
    }

    public class TestingOutlineDetailsLookup
    {
        public string TestingOutline { get; set; }

        public string Step { get; set; }
    }

    public class TestingOutlineTemplateLookup
    {
        public string TestingOutline { get; set; }

        public string TestingTemplate { get; set; }
    }

    public class TestingOutlineCostLookup
    {
        public string TestingOutline { get; set; }

        public string UserName { get; set; }

        public decimal MinutesSpent { get; set; }
    }
}
