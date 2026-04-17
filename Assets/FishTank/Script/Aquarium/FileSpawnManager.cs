using PimDeWitte.UnityMainThreadDispatcher;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class FileSpawnManager : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;

    [Header("Folder Configuration")]
    [Tooltip("Folder name inside Application.dataPath (Editor) or Game_Data (Build)")]
    public string folderName = "ExternalAssets";

    [Header("Spawn Settings")]
    public Vector3 spawnAreaSize = new Vector3(20, 0, 20);

    private FileSystemWatcher watcher;
    private Queue<string> fileQueue = new Queue<string>();
    private Dictionary<string, ScriptableObject> activeAssets = new Dictionary<string, ScriptableObject>();

    private string targetPath;

    void Start()
    {
        targetPath = Path.Combine(Application.dataPath, folderName);

        if (!Directory.Exists(targetPath))
        {
            Directory.CreateDirectory(targetPath);
        }

        InitWatcher();
        Debug.Log($"Monitoring folder: {targetPath}");

        LoadExistingFiles();
    }

    void LoadExistingFiles()
    {
        if (Directory.Exists(targetPath))
        {
            string[] existingFiles = Directory.GetFiles(targetPath, "*.png");
            lock (fileQueue)
            {
                foreach (string filePath in existingFiles)
                {
                    fileQueue.Enqueue(Path.GetFullPath(filePath));
                    //fileQueue.Enqueue(filePath);
                }
            }
            Debug.Log($"Found {existingFiles.Length} existing files in folder.");
        }
    }

    void InitWatcher()
    {
        watcher = new FileSystemWatcher();
        watcher.Path = targetPath;
        watcher.Filter = "*.png";

        watcher.Created += OnFileCreated;
        watcher.Deleted += OnFileDeleted;
        watcher.EnableRaisingEvents = true;
    }

    private void OnFileCreated(object sender, FileSystemEventArgs e)
    {
        lock (fileQueue)
        {
            fileQueue.Enqueue(Path.GetFullPath(e.FullPath));
            //fileQueue.Enqueue(e.FullPath);
        }
    }

    private void OnFileDeleted(object sender, FileSystemEventArgs e)
    {
        string deletedPath = Path.GetFullPath(e.FullPath);
        Debug.Log($"File deleted event triggered for: {Path.GetFileName(deletedPath)}");
        UnityMainThreadDispatcher.Instance().Enqueue(() => RemoveAsset(deletedPath));
    }

    void Update()
    {
        lock (fileQueue)
        {
            while (fileQueue.Count > 0)
            {
                string path = fileQueue.Dequeue();
                if (!activeAssets.ContainsKey(path))
                {
                    StartCoroutine(ProcessImageFile(path));
                }
            }
        }
    }

    IEnumerator ProcessImageFile(string path)
    {
        // Tunggu sebentar untuk memastikan file sudah selesai ditulis oleh OS
        yield return new WaitForSeconds(0.5f);

        if (!File.Exists(path)) yield break;

        string fileName = Path.GetFileNameWithoutExtension(path);
        string[] parts = fileName.Split('_');

        if (parts.Length < 2)
        {
            Debug.LogWarning($"Invalid naming format: {fileName}. Use CATEGORY_TYPE_TIMESTAMP");
            yield break;
        }

        string category = parts[0].ToUpper();
        string type = parts[1];

        // 1. Load Image as Sprite
        byte[] fileData = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(fileData);
        Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 300);

        // 3. Create SO and send to specific Manager
        if (category == "FISH")
        {
            FishData data = ScriptableObject.CreateInstance<FishData>();
            data.fishName = fileName;
            data.fishType = type;
            data.fishSprite = newSprite;

            activeAssets.Add(path, data);
            gameEvent.AddFish(data);
        }
        else if (category == "TRASH")
        {
            TrashData data = ScriptableObject.CreateInstance<TrashData>();
            data.trashName = fileName;
            data.trashType = type;
            data.trashSprite = newSprite;

            activeAssets.Add(path, data);
            gameEvent.AddTrash(data);
        }
    }

    private void RemoveAsset(string path)
    {
        if (activeAssets.ContainsKey(path))
        {
            ScriptableObject dataToRemove = activeAssets[path];

            if (dataToRemove != null) ///// Cek null safety
            {
                if (dataToRemove is FishData fish)
                {
                    Debug.Log($"RemoveFishData {fish.fishName}");
                    gameEvent.RemoveFishData(fish);
                }
                else if (dataToRemove is TrashData trash)
                {
                    Debug.Log($"RemoveTrashData {trash.trashName}"); ///// Perbaikan typo name
                    gameEvent.RemoveTrashData(trash);
                }

                activeAssets.Remove(path);
                Destroy(dataToRemove);
                Debug.Log($"File dihapus, mencopot asset: {Path.GetFileName(path)}");
            }
        }
        else
        {
            ///// Jika masih tidak jalan, log ini akan membantu memberitahu path mana yang miss
            Debug.LogWarning($"Path not found in activeAssets: {path}");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }

    private void OnDestroy()
    {
        if (watcher != null)
        {
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
        }
    }
}