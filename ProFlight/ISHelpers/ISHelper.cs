using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;

namespace attackGame
{
    class ISHelper
    {
        /// <summary>
        /// Metoda za spremanje najboljih rezultata u IsolatedStorage
        /// </summary>
        /// <param name="fileName">Ime file-a u koji spremamo najbolje rezultate</param>
        /// <param name="obj">Lista najboljih rezultata koje spremamo</param>
        public void SaveHighScores(string fileName, List<HighScore> obj)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream stream = storage.CreateFile(fileName);

            XmlSerializer xml = new XmlSerializer(typeof(List<HighScore>));
            xml.Serialize(stream, obj);

            stream.Close();
            stream.Dispose();
        }

        /// <summary>
        /// Metoda za dohvaæanje najboljih rezultata iz IsolatedStorage-a
        /// </summary>
        /// <param name="fileName">Ime file-a koji sadržava najbolje rezultate</param>
        /// <returns>Lista najboljih rezultata</returns>
        public List<HighScore> LoadHighScores(string fileName)
        {
            List<HighScore> tmp = new List<HighScore>();
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                {
                    if (stream != null)
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(List<HighScore>));
                        tmp = xml.Deserialize(stream) as List<HighScore>;
                    }
                    stream.Close();
                    stream.Dispose();
                }
            }
            return tmp;
        }
    }
}
