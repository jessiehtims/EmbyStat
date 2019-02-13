﻿using System;

namespace EmbyStat.Controllers.Show
{
    public class ShowCollectionRowViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int Seasons { get; set; }
        public int Episodes { get; set; }
        public int Specials { get; set; }
        public int MissingEpisodes { get; set; }
        public DateTime? PremiereDate { get; set; }
        public bool Status { get; set; }
        public string SortName { get; set; }
    }
}