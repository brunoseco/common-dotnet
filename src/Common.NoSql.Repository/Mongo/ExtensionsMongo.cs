using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Common.NoSql.Repository.Mongo
{
    public static class ExtensionsMongo
    {
        public static IEnumerable<dynamic> MongoCursorToDynamic(this MongoCursor<BsonDocument> result)
        {
            var newResultTemp = new List<dynamic>();
            foreach (var document in result)
            {
                var rowDictonary = BsonDocumentToDyctonary(document);
                var rowDynamic = DictionaryToObject(rowDictonary);
                newResultTemp.Add(rowDynamic);
            }

            return newResultTemp;
        }

        public static Dictionary<string, object> BsonDocumentToDyctonary(BsonDocument document)
        {
            return MakeDictonary(document);
        }

        public static dynamic DictionaryToObject(this IDictionary<string, object> dictionary)
        {
            var eo = new ExpandoObject() as IDictionary<string, object>;
            foreach (var kvp in dictionary)
            {
                var subDictionary = kvp.Value as IDictionary<string, object>;
                if (subDictionary == null)
                    eo.Add(kvp);
                else
                {
                    var subEo = subDictionary.DictionaryToObject();
                    eo.Add(new KeyValuePair<string, object>(kvp.Key, subEo));
                }
            }
            return eo;
        }

        private static Dictionary<string, object> MakeDictonary(BsonDocument document)
        {
            var propertys = new Dictionary<string, object>();
            foreach (var fields in document)
            {
                if (!fields.Value.IsBsonDocument)
                {
                    if (fields.Value.IsBsonDateTime)
                    {
                        DateTime value = Convert.ToDateTime(fields.Value);
                        propertys.Add(fields.Name, value.ToShortDateString());
                    }
                    else
                        propertys.Add(fields.Name, fields.Value);
                }

                if (fields.Value.IsBsonDocument)
                {
                    var subDocument = MakeDictonary(fields.Value.AsBsonDocument);
                    propertys.Add(fields.Name, subDocument);
                }
            }
            return propertys;
        }

    }
}
