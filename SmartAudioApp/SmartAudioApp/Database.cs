using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SQLiteClient;
using System.Collections.Generic;
using System.Linq;
using System.IO.IsolatedStorage;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SmartAudioApp
{
    public class Database
    {

        private string fileName;
        private IsolatedStorageFile storedFile;

        public Database()
        {
            fileName = "Data\\db.txt";
            storedFile = IsolatedStorageFile.GetUserStoreForApplication();
            storedFile.CreateDirectory("Data");
        }



        public void createTableForLatLongAndSound()
        {
            FileStream stream = storedFile.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            stream.Close();
        }

        public int addItemAndReturnId(coordinatesAndSound item)
        {
            List<coordinatesAndSound> objectStored = getListItems();
            FileStream stream = storedFile.OpenFile(fileName, FileMode.Open);
            string fileString;

            if (objectStored != null)
            {
                coordinatesAndSound maxId = objectStored.OrderBy(m => m.id).LastOrDefault();
                if (maxId != null)
                    item.id = maxId.id + 1;
                else
                    item.id = 1;
            }
            else
            {
                item.id = 1;
                objectStored = new List<coordinatesAndSound>();
            }
            
            objectStored.Add(item);
            fileString = serialize(objectStored);

            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(fileString);
            writer.Close();            
            stream.Close();

            return item.id;
        }

        public void addList(List<coordinatesAndSound> listDatabase)
        {
            FileStream stream = storedFile.OpenFile(fileName, FileMode.Open);
            string fileString = serialize(listDatabase);
            BinaryWriter writer = new BinaryWriter(stream);
            
            writer.Write(fileString);
            writer.Close();
            stream.Close();
        }
        
        public List<coordinatesAndSound> getListItems()
        {
            //storedFile.DeleteFile(fileName);
            string fileString = getStringStored();
            FileStream stream = storedFile.OpenFile(fileName, FileMode.Open);            
            MemoryStream streamText = new MemoryStream(UTF8Encoding.UTF8.GetBytes(fileString));
            DataContractJsonSerializer serializerAndDeserializer = new DataContractJsonSerializer(typeof(List<coordinatesAndSound>));
            List<coordinatesAndSound> objectStored = (List<coordinatesAndSound>)serializerAndDeserializer.ReadObject(streamText);
            stream.Close();

            return objectStored;
        }

        public string getStringStored()
        {
            if (storedFile.FileExists(fileName))
            {
                IsolatedStorageFileStream file = new IsolatedStorageFileStream(fileName, FileMode.Open, storedFile);
                BinaryReader reader = new BinaryReader(file);
                string theStringStored;
                try
                {
                     theStringStored = reader.ReadString();
                }
                catch
                {
                    theStringStored = "";
                }
                
                reader.Close();
                file.Close();
                return theStringStored;
            }

            createTableForLatLongAndSound();
            return "";
        }

        public coordinatesAndSound findLocalDataBaseForLatLongAndSoundById(int id)
        {
            coordinatesAndSound item = new coordinatesAndSound();
            List<coordinatesAndSound> storedItens = getListItems();

            item = (from stored in storedItens
                    where stored.id == id
                    select stored).FirstOrDefault();

            return item;
        }

        public void removeLocalDataBaseForLatLongAndSound(int id)
        {
            List<coordinatesAndSound> listLocalDataBaseForLatLongAndSound = getListItems();

            coordinatesAndSound item = (from stored in listLocalDataBaseForLatLongAndSound
                                                    where stored.id == id
                                                    select stored).FirstOrDefault();

            int idItem = listLocalDataBaseForLatLongAndSound.IndexOf(item);

            listLocalDataBaseForLatLongAndSound.RemoveAt(idItem);

            addList(listLocalDataBaseForLatLongAndSound);

        }


        public string serialize(List<coordinatesAndSound> fileStored)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<coordinatesAndSound>));
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, fileStored);

            byte[] jsonBytes = ms.ToArray();

            ms.Close();

            string json = UTF8Encoding.UTF8.GetString(jsonBytes, 0, jsonBytes.Length);
            
            return json;
        }
        
       
    }



    public class coordinatesAndSound
    {
        public int id;
        public double lat;
        public double lon;
        public string sound;

        public coordinatesAndSound()
        {
        }

        public coordinatesAndSound(double lat,double lon,string sound)
        {
            this.lat = lat;
            this.lon = lon;
            this.sound = sound;
        }
    }


}
