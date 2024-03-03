using System;
using System.IO;

namespace Omino.Infra.Master
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class TableNameAttribute : Attribute
    {
        private string _tableName;

        public string TableName 
        { 
            get { return Path.ChangeExtension(_tableName, "yaml"); }
        }
        
        public TableNameAttribute(string tableName)
        {
            _tableName = tableName;
        }
    }
}