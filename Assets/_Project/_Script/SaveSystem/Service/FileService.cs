using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public static class FileService
{
    private const string KEY = "rLmiasAMPNLCQ21TqeBd/BtkmsokhVy/3YCHpCXthTM=";
    private const string IV = "4/BnWPU2Xcv7yHiBiHtgCA==";
    
    public static bool SaveLocal<T>(string savePath, T Data, bool isEncrypted)
    {
        var relativePath = Application.persistentDataPath + savePath;  //relative path for different machine systems
        var backupPath = Path.ChangeExtension(relativePath, ".bak");
        try
        {
            if(File.Exists(relativePath))
            {
                Debug.Log("Save file existed, creating a backup...");
                if (File.Exists(backupPath)) //delete previous backup
                {
                    File.Delete(backupPath);
                }
                File.Copy(relativePath, backupPath); // Backup the existing save
            } else {
                Debug.Log("Creating save file...");
            }
            using FileStream stream = File.Create(relativePath);
            if(isEncrypted)
            {
                WriteEncryptedData(Data, stream);
            }
            else
            {
                stream.Close();
                File.WriteAllText(relativePath, JsonConvert.SerializeObject(Data, Formatting.Indented));
            }
            
            Debug.Log(relativePath);
            return true;
        } catch (Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}\n{e.StackTrace}");
            return false;
        }
    }

    private static void WriteEncryptedData<T>(T data, FileStream stream)
    {
        using Aes aesProvider = Aes.Create();

        Debug.Log("Mode: " + aesProvider.Mode);
        Debug.Log("Padding: " + aesProvider.Padding);
        // Debug.Log($"Initialization Vector: {Convert.ToBase64String(aesProvider.IV)}");
        // Debug.Log($"Key: {Convert.ToBase64String(aesProvider.Key)}");

        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);
        using ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor(aesProvider.Key, aesProvider.IV);
        using CryptoStream cryptoStream = new(stream, cryptoTransform, CryptoStreamMode.Write);

        cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));
    }

    public static T LoadLocal<T>(string loadPath, bool isEncrypted)
    {
        var relativePath = Application.persistentDataPath + loadPath;
        try
        {
            if (!File.Exists(relativePath))
            {
                Debug.LogWarning($"No save file at {relativePath}");
                return default;
            }

            T data;
            if(isEncrypted)
            {
                data = ReadEncryptedData<T>(relativePath);
            } else {
                data = JsonConvert.DeserializeObject<T>(File.ReadAllText(relativePath));
            }
            if (data == null)
            {
                Debug.LogError($"data null, maybe failed to deserialize data from {relativePath}");
                return default;
            }
            Debug.Log("Load success");
            return data;
        }
        catch (Exception e)
        {
            File.Delete(relativePath); //delete the corrupted file
            SaveSystem.isCorrupted = true;
            Application.Quit(); 
            throw new Exception($"Failed to load game data: {e.Message}, \n {e.StackTrace}", e);   
        }
    }

    private static T ReadEncryptedData<T>(string relativePath)
    {
        byte[] bytes = File.ReadAllBytes(relativePath);
        using Aes aesProvider = Aes.Create();

        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);

        using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(aesProvider.Key, aesProvider.IV);
        using MemoryStream memoryStream = new(bytes);
        using CryptoStream cryptoStream = new(memoryStream, cryptoTransform, CryptoStreamMode.Read);
        using StreamReader streamReader = new(cryptoStream);

        string result = streamReader.ReadToEnd();
        Debug.Log(result);
        return JsonConvert.DeserializeObject<T>(result);
    }

}
