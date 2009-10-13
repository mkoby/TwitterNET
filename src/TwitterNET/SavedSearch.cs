using System;

namespace TwitterNET
{
    public class SavedSearch
    {
        private long _id;
        private string _name;
        private string _query;
        private string _position;
        private DateTime _createdAt;

        public SavedSearch(long id, string name, string query, string position, DateTime createdAt)
        {
            _id = id;
            _name = name;
            _query = query;
            _position = position;
            _createdAt = createdAt;
        }


        public DateTime CreatedAt
        {
            get { return _createdAt; }
        }

        public string Position
        {
            get { return _position; }
        }

        public string Query
        {
            get { return _query; }
        }

        public string Name
        {
            get { return _name; }
        }

        public long Id
        {
            get { return _id; }
        }
    }
}