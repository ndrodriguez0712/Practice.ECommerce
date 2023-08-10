namespace Identity.Service.Queries.DTOs
{
    public class EncryptionOptionsDto
    {
        public int Iterations { get; set; }
        public int KeySize { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }
        public string Vector { get; set; }
    }
}
