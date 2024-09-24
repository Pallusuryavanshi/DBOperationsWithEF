namespace DBOperationsWithEF.Data
{
    public class BookPrice
    {
        public int ID { get; set; }
        public int BookId { get; set; }
        public int CurrancyId { get; set; }
        public int Amount { get; set; }

        public Book Book { get; set; }

        public Currency Currency { get; set; }
    }
}
