using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using DataAccess.Interface;
using System.IO;

namespace DataAccess.Base
{
    public class Repository<T> : IRepository<T>
    {
        protected readonly DataContractJsonSerializer Serializer;
        protected readonly string DataText;
        protected readonly IList<T> Data;
        protected readonly string FilePath;
                
        public Repository(string filePath, bool forceFileCreation = false)
        {
            try
            {
                var fileMode = forceFileCreation ? FileMode.Create : FileMode.OpenOrCreate;
                FilePath = filePath;
                Serializer = new DataContractJsonSerializer(typeof(List<T>));

                //Read file and set internal content DataText property
                using (var stream = File.Open(filePath, fileMode))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        DataText = reader.ReadToEnd();
                    }
                }
                //Deserialize DataText into Data
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(DataText)))
                {
                    stream.Position = 0;
                    Data = string.IsNullOrEmpty(DataText) ? new List<T>() : (List<T>)Serializer.ReadObject(stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public virtual IList<T> GetAll()
        {
            return Data;
        }

        public virtual void Save(T item)
        {
            Data.Add(item);
            using (var fs = new FileStream(FilePath, FileMode.Create))
            {
                Serializer.WriteObject(fs, Data);
            }
        }
    }
}
