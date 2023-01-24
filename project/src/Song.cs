using Nest;
using System;
using System.Collections;
using System.ComponentModel;

namespace project.src
{
    [ElasticsearchType(RelationName = "song")]
    internal class Song
    {
        public static int idGlobal { get; set; } = 1;
        [Number()] public int Id { get; set; }
        [Text(Name = "name", Norms = false)] public string name { get; set; }
        [Text(Name = "author")] public string author { get; set; }
        [Description()] public string description { get; set; }
        [Category()] public ArrayList tagsList { get; set; }

        public Song(string name, string author, string description, ArrayList tagsList)
        {
            Id = idGlobal++;
            this.name = name;
            this.author = author;
            this.description = description;
            this.tagsList = tagsList;
        }
        public Song(string name, string author, string description, string[] tagsList)
        {
            Id = idGlobal++;
            this.name = name;
            this.author = author;
            this.description = description;
            this.tagsList = new ArrayList();
            foreach (string tag in tagsList) { this.tagsList.Add(tag); }
        }

        public void addTag(string tag)
        {
            tagsList.Add(tag);
        }

        public void removeTag(string tag) { }

        override public string ToString()
        {
            string result = "";
            result += Id;
            result += " " + name + " by " + author + ": {";
            foreach (string tag in tagsList) { result += tag + ", "; }
            result += "}: " + description;
            return result;
        }
    }
}
