namespace projectServer.DTOs
{
    public class ImageUpdateDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; } = "All";
        public DateTime date { get; set; } = DateTime.Now;
        public float Score { get; set; } = -1;
    }
}
