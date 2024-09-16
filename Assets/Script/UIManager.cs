using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public TimeManager timeManager;
    public GoalManager goalManager;
    public GameObject panelMonth;
    public GameObject panelStart;
    public GameObject panelGoal;
    public GameObject panelMesurement;

    public Button nextButtonMonth;          // スタートパネルへの遷移ボタン
    public Button nextButtonStart;          // ゴールパネルへの遷移ボタン
    public Button resetButtonStart;         // スタート位置へ戻すボタン
    public Button startButtonMesurement;    // 計測開始ボタン
    public Button initializeButton;         // 初めに戻るボタン
    public Button poseButton;               // 一時停止ボタン
    public Button resumeButton;             // 再開ボタン
    public Button goalButtonBalcony;        // ベッドにゴールを設定
    public Button goalButtonBed;            // ベランダにゴールを設定
    public Button goalButtonChair;          // 椅子にゴールを設定
    public Button goalButtonSofa;           // ソファにゴールを設定

    void Start()
    {
        // シーンがロードされたときに月齢パネルを表示
        ShowMonthPanel();

        // nextButtonMonthがクリックされたときにスタートパネルを表示
        nextButtonMonth.onClick.AddListener(ShowStartPanel);

        // resetButtonStartがクリックされたときに原点に戻るように設定
        resetButtonStart.onClick.AddListener(() =>
        {
            playerMovement.ResetPosition();
        });

        // nextButtonStartがクリックされたときにゴールパネルを表示
        nextButtonStart.onClick.AddListener(ShowGoalPanel);

        // startButtonMesurementがクリックされたときにゴールをベランダに設定
        goalButtonBalcony.onClick.AddListener(() =>
        {
            goalManager.SetGoal(goalManager.goalPositionBalcony);
            goalButtonBalcony.interactable = true;
            goalButtonBed.interactable = false;
            goalButtonChair.interactable = false;
            goalButtonSofa.interactable = false;
        });

        // goalButtonBalconyがクリックされたときにゴールをベッドに設定
        goalButtonBed.onClick.AddListener(() =>
        {
            goalManager.SetGoal(goalManager.goalPositionBed);
            goalButtonBalcony.interactable = false;
            goalButtonBed.interactable = true;
            goalButtonChair.interactable = false;
            goalButtonSofa.interactable = false;
        });

        // goalButtonChairがクリックされたときにゴールをソファに設定
        goalButtonChair.onClick.AddListener(() =>
        {
            goalManager.SetGoal(goalManager.goalPositionChair);
            goalButtonBalcony.interactable = false;
            goalButtonBed.interactable = false;
            goalButtonChair.interactable = true;
            goalButtonSofa.interactable = false;
        });

        // goalButtonSofaがクリックされたときにゴールをソファに設定
        goalButtonSofa.onClick.AddListener(() =>
        {
            goalManager.SetGoal(goalManager.goalPositionSofa);
            goalButtonBalcony.interactable = false;
            goalButtonBed.interactable = false;
            goalButtonChair.interactable = false;
            goalButtonSofa.interactable = true;
        });

        // startButtonMesurementがクリックされたときに計測パネルを表示、PlayerMovementのStartMovingメソッドの呼び出し
        startButtonMesurement.onClick.AddListener(() =>
        {
            ShowMesurementPanel();
            playerMovement.StartMoving();
        });

        // 一時停止ボタンがクリックされたときの処理の追加
        poseButton.onClick.AddListener(() =>
        {
            playerMovement.PauseMovement();
            timeManager.PauseTimer();
        });

        // 再開ボタンがクリックされたときの処理の追加
        resumeButton.onClick.AddListener(() =>
        {
            playerMovement.ResumeMovement();
            timeManager.ResumeTimer();
        });

        // initializeButtonがクリックされたとき、月齢パネルを表示
        initializeButton.onClick.AddListener(() =>
        {
            ShowMonthPanel();
            playerMovement.ResetPosition();
            timeManager.ResetTimer();
        });
    }

    void Update()
    {
        // ゴールに到着しているかどうかで再開ボタンの状態を更新
        if (playerMovement.HasReachedGoal)
        {
            resumeButton.interactable = false;
        }
        else
        {
            resumeButton.interactable = true;
        }

        // ゴールに到着しているかどうかで再開ボタンの状態を更新
        if (playerMovement.HasReachedGoal)
        {
            poseButton.interactable = false;
        }
        else
        {
            poseButton.interactable = true;
        }
    }

    // 月齢パネルを表示する
    public void ShowMonthPanel()
    {
        panelMonth.SetActive(true);
        panelStart.SetActive(false);
        panelGoal.SetActive(false);
        panelMesurement.SetActive(false);
    }

    // スタートパネルを表示する
    public void ShowStartPanel()
    {
        panelMonth.SetActive(false);
        panelStart.SetActive(true);
        panelGoal.SetActive(false);
        panelMesurement.SetActive(false);
    }

    // ゴールパネルを表示する
    public void ShowGoalPanel()
    {
        panelMonth.SetActive(false);
        panelStart.SetActive(false);
        panelGoal.SetActive(true);
        panelMesurement.SetActive(false);
    }

    // 計測パネルを表示する
    public void ShowMesurementPanel()
    {
        panelMonth.SetActive(false);
        panelStart.SetActive(false);
        panelGoal.SetActive(false);
        panelMesurement.SetActive(true);
    }
}
