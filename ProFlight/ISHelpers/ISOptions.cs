using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;

namespace attackGame
{
    class ISOptions
    {
        /// <summary>
        /// Metoda za spremanje podataka u isolatedStorage
        /// </summary>
        /// <param name="fileName">Ime file-a</param>
        /// <param name="obj">Lista opcija koje se spremaju u isolated storage</param>
        public void SaveOptions(string fileName, List<bool> obj)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream stream = storage.CreateFile(fileName);

            XmlSerializer xml = new XmlSerializer(typeof(List<bool>));
            xml.Serialize(stream, obj);
            GameplayHelper.optionChanged = true;
            stream.Close();
            stream.Dispose();
        }

        /// <summary>
        /// Metoda za uèitavanje korisnièkih postavki iz isolatedStorage-a
        /// </summary>
        /// <param name="fileName">Ime file-a</param>
        /// <returns>Lista korisnikèik opcija</returns>
        public List<bool> LoadOptions(string fileName)
        {
            List<bool> tmp = new List<bool>();
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists(fileName))
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                    {
                        if (stream != null)
                        {
                            XmlSerializer xml = new XmlSerializer(typeof(List<bool>));
                            tmp = xml.Deserialize(stream) as List<bool>;
                        }
                        stream.Close();
                        stream.Dispose();
                    }
                    //_music = tmp[0];
                    //_vibration = tmp[1];
                }
            }

            return tmp;
        }
    }
}
