namespace projectServer.Models
{
    public class SampleImageModel
    {
        public int Id { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public float Score { get; set; }
        public DateTime date { get; set; }
    }
}
