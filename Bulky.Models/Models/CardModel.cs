namespace PersonalProject.Models
{
    public class CardModel
    {
        public class CardSet
        {
            public string SetName { get; set; }
            public string SetCode { get; set; }
            public string SetRarity { get; set; }
            public string SetRarityCode { get; set; }
            public string SetPrice { get; set; }
        }

        public class CardImage
        {
            public int Id { get; set; }
            public string ImageUrl { get; set; }
            public string ImageUrlSmall { get; set; }
            public string ImageUrlCropped { get; set; }
        }

        public class CardPrice
        {
            public string CardmarketPrice { get; set; }
            public string TcgplayerPrice { get; set; }
            public string EbayPrice { get; set; }
            public string AmazonPrice { get; set; }
            public string CoolstuffincPrice { get; set; }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string FrameType { get; set; }
        public string Desc { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int Level { get; set; }
        public string Race { get; set; }
        public string Attribute { get; set; }
        public List<CardSet> CardSets { get; set; }
        public List<CardImage> CardImages { get; set; }
        public List<CardPrice> CardPrices { get; set; }
    }

}
