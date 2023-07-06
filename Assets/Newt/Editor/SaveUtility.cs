using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Newt
{
    public static class SaveUtility
    {
        public static bool TryCreateFolder(string path, string folderName, out string guid)
        {
            if (AssetDatabase.IsValidFolder($"{path}/{folderName}"))
            {
                guid = null;
                return false;
            }

            guid = AssetDatabase.CreateFolder(path, folderName);

            return true;
        }

        public static bool TryCreateFolder(string path, string folderName)
        {
            return TryCreateFolder(path, folderName, out _);
        }
    }

}