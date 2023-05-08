using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneSoundList", menuName = "Sound/SceneSound")]
public class SceneSoundList : ScriptableObject
{
    public List<SceneSound> sceneSoundsList;
    public SceneSound FindSceneSound(string scenename)
    {
        return sceneSoundsList.Find(s => s.sceneName == scenename);
    }
}
[System.Serializable]
public class SceneSound
{
    public string sceneName;
    public soundName ambient;
    public soundName music;
}