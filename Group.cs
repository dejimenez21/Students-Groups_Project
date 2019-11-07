using System;
using System.Collections.Generic;

namespace Students_Groups
{
    class Group
    {
        public string Name;
        public List<string> estudiantes = new List<string>();
        public List<string> temas = new List<string>();

        public Group(string name)
        {
            this.Name=name;
        }
    }
}