namespace WebRepo.Models
{
    public class FileSearchViewModel
    {
       
        public FileSearchViewModel() { }

        public int Offset { get; set; }
        public int? Limit { get; set; }
        public string UserEmail { get; set; }
        public IList<BootstrapTableFilter> SearchParameter { get; set; }
    }

    public class BootstrapTableFilter
    {
        public BootstrapTableFilter() { }

        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }
}
