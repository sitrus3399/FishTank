using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Collections.Generic;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    [SerializeField] private SaveData saveData;
    [SerializeField] private SaveData defaultData;

    [Header("Events")]
    [SerializeField] private UIEvent uiEvent;
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private SaveEvent saveEvent;

    [Header("Settings")]
    [SerializeField] private float defaultSFXVolume = 0.5f;

    private string savePath;
    // PENTING: Jangan ubah kunci ini setelah game dirilis atau save file lama tidak bisa dibuka
    private readonly string encryptionKey = "ReHunterPlayground99!*++";
    private readonly string encryptionIV = "KucingOren113399";

    void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "rehunter_save.dat");
    }

    private void OnEnable()
    {
        saveEvent.OnSaveSFXVolume += ChangeSFXVolume;
    }

    private void OnDisable()
    {
        saveEvent.OnSaveSFXVolume -= ChangeSFXVolume;
    }

    void Start()
    {
        LoadGame();
    }

    public void ChangeSFXVolume(float value)
    {
        saveData.sfxVolume = value;
        SaveGame();
    }


    public void SaveGame()
    {
        try
        {
            string json = JsonUtility.ToJson(saveData, true);
            string encryptedData = Encrypt(json);
            File.WriteAllText(savePath, encryptedData);

            //Debug.Log("<color=green>✓ Game Berhasil Disimpan</color>");
        }
        catch (Exception e)
        {
            Debug.LogError("Gagal Save: " + e.Message);
        }
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            try
            {
                string encryptedData = File.ReadAllText(savePath);
                string json = Decrypt(encryptedData);

                // Menggunakan FromJsonOverwrite agar referensi objek saveData di Inspector tetap sama
                JsonUtility.FromJsonOverwrite(json, saveData);
                Debug.Log("<color=cyan>✓ Game Berhasil Dimuat</color>");
            }
            catch (Exception e)
            {
                Debug.LogError("Gagal Load (Data mungkin korup): " + e.Message);
                ResetToDefault();
            }
        }
        else
        {
            ResetToDefault();
        }

        // Jalankan event setelah data dimuat
        gameEvent.ChangeSFXVolume(saveData.sfxVolume);

        saveEvent.LoadSFXVolume(saveData.sfxVolume);
        saveEvent.LoadMoney(saveData.money);
        //saveEvent.LoadTargetAttack(saveData.targetAttack); //Nanti diisi saat script target sudah jadi
    }

    private void ResetToDefault()
    {
        Debug.Log("Menginisialisasi data baru...");

        // Pastikan List tidak null agar tidak error saat diakses script lain
        //saveData.targetAttack = new List<bool>(); //Nanti diisi saat script target sudah jadi

        saveData = defaultData;

        SaveGame(); // Simpan data default sebagai file pertama
    }

    public void UpdateUnlockStatus(List<bool> targetList, int index, bool status)
    {
        if (index < targetList.Count)
        {
            targetList[index] = status;
            SaveGame();
        }
    }

    #region Encryption Engine (AES)

    private string Encrypt(string plainText)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
        byte[] ivBytes = Encoding.UTF8.GetBytes(encryptionIV);

        using (Aes aes = Aes.Create())
        {
            using (ICryptoTransform encryptor = aes.CreateEncryptor(keyBytes, ivBytes))
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                return Convert.ToBase64String(encryptedBytes);
            }
        }
    }

    private string Decrypt(string encryptedText)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
        byte[] ivBytes = Encoding.UTF8.GetBytes(encryptionIV);

        using (Aes aes = Aes.Create())
        {
            using (ICryptoTransform decryptor = aes.CreateDecryptor(keyBytes, ivBytes))
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }

    #endregion
}

[System.Serializable]
public class SaveData
{
    public float sfxVolume;
    public float money;
    //public List<TargetAttack> targetAttack; //Nanti diisi saat script target sudah jadi
}