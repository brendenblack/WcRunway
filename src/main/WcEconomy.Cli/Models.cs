﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcEconomy.Cli
{
    public class Genre
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Movie
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class Person
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class MovieInformation
    {
        [JsonProperty("movie")]
        public Movie Movie { get; set; }

        [JsonProperty("director")]
        public Person Director { get; set; }

        [JsonProperty("genres")]
        public IList<Genre> Genres { get; set; }

        [JsonProperty("cast")]
        public IList<Person> Cast { get; set; }
    }
}
