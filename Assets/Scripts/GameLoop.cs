using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;

public class GameLoop : MonoBehaviour
{
    private static GameLoop instance=null;
    public static GameLoop Instance { get { return instance; } }
    private ISceneState m_state;
    public string SceneState =>m_state.Name;
    //讀取進度條
    private float progreeBar = 0;
    private void Awake()
    {
        instance = this;
        //保留此script作為唯一的GameLoop
        GameObject.DontDestroyOnLoad(this.gameObject);

    }
    private void Start()
    {
        Addressables.LoadSceneAsync("Scenes/BattleScene", LoadSceneMode.Single);
        //ChangeScene(new MainMenuState(this), "Scenes/MainMenuScene");
    }

    private void Update()
    {
        m_state?.SceneUpdate();

    }

    private void ChangeScene(ISceneState sceneState,string sceneName)
    {
        StartCoroutine(SetState(sceneState,sceneName));
    }
    #region ChangeSceneFunction
    public IEnumerator SetState(ISceneState sceneState, string sceneName)
    {
        //重置讀取進度
        progreeBar = 0f;
        //讀取Scene
        AsyncOperation AO = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        if (AO == null)
        {
            Debug.LogError("The scene is not exist.");
            yield break;
        }
        yield return LoadScene(AO);
        //完成Scene的全部讀取
        yield return ChangeScene(AO);
        //切換m_state
        ChangeState(sceneState);

    }






    IEnumerator LoadScene(AsyncOperation AO)
    {
        AO.allowSceneActivation = false;
        while (progreeBar < 0.89f)
        {
            progreeBar = AO.progress;
            yield return 0;
        }

    }
    IEnumerator ChangeScene(AsyncOperation AO)
    {
        AO.allowSceneActivation = true;
        while (!AO.isDone)
        {
            progreeBar = AO.progress;
            yield return null;
        }
    }
    void ChangeState(ISceneState sceneState)
    {
        if (m_state != null)
        {
            m_state.SceneEnd();
        }
        m_state = sceneState;
        m_state.SceneBegin();
    }
    #endregion
    public float GetProgress
    {
        get { return progreeBar; }
    }

    
    


}
