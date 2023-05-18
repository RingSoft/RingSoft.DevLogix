namespace RingSoft.DevLogix.DataAccess.LookupModel.QualityAssurance
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
}
