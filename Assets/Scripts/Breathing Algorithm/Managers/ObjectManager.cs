using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class ObjectManager : MonoBehaviour {
    public Image feedbackImage;
    public TextMeshProUGUI instructionsText;

    public GraphDraw inputGraph;
    public float amplitudeChangeSpeed = 1f;

    int breathingMode; //0 for inhale, 1 for exhale, 2 for hold 
    public int[] breathingPattern, breathingPatternTime;
    [HideInInspector]
    public float[] breath, features, envelope;
    float peak, duration, mean, updatePattern;

    float currentTime = 0, breathingTime = 0;
    int i, i1;
    float updateTime = 0, updateInstructionsTime = 0, numSeconds = 0.25f; // Update every interval seconds


    
    AudioManager audioManager;
    AudioProcessor audioProcessor;
    MicrophoneCheck MicCheck;
    public CorrelationFromGraphs correlationFromGraphs;


    bool flag = true;
    bool correctTime = true;
    bool flag1 = true;
    private float updatePatternInstructions;
    private bool correctTimeInstructions = true;
    private int breathingModeInstructions;
    private int previousModeInstructions = -1;
    private float breathingTimeInstructions = 0;
    private int CountCycleChanges = 0;
    
    public bool calibrateMic;
    [Tooltip("Used and needed only for calibration")]
    public TextMeshProUGUI micAlertText;
    [Tooltip("Used and needed only for calibration")]
    public TextMeshProUGUI micCalText;
    [Tooltip("Used and needed only for calibration")]
    public Button micResetButton;
    private float calibrateTime; // for how long will it calibrate the mic

    //ADDED FOR GAME

    public CalibrationMenu calibrationMenu;


    void Start () {
        audioManager = GetComponent<AudioManager> ();
        audioProcessor = new AudioProcessor ();
        MicCheck = GetComponent<MicrophoneCheck> ();
    }

    void OnEnable () {
        if (flag) {
            // Initialize breathingPattern and Inhale, Exhale and Hold times
            if (breathingPattern.Length == 0)
                breathingPattern = new int[] { 0, 1 };
            if (breathingPatternTime.Length == 0)
                breathingPatternTime = new int[] { 3, 3 };
            flag = false;
        }
        
        if (calibrateMic) {
            micResetButton.gameObject.SetActive(false);
            inputGraph.enableMicTest();
            calibrateTime = 18f; //time to calibrate the mic
            micResetButton.gameObject.SetActive(false);
        }else{
            inputGraph.disableMicTest();
            calibrateTime = -1f;
        }


        // Variables to Iterate over the breathing patterns
        i = 0;
        i1 = 0;
        updatePattern = 0;
        updatePatternInstructions = 0;
        breathingMode = breathingPattern[0];
        breathingModeInstructions = breathingMode;
        breathingTime = breathingPatternTime[0];
        breathingTimeInstructions = breathingTime;
        inputGraph.show = true;
        inputGraph.seconds = 0;
        foreach (int bt in breathingPatternTime)
        {
            inputGraph.seconds += bt;
        }
    }

     void Update () {       
        currentTime += Time.deltaTime;
        
        if (flag1)
        {
            if (currentTime <= 0.5)
                return;
            else
                flag1 = false;
        }
        
        // Update Instructions
        if (currentTime >= 0.5 + updateInstructionsTime)
        {
            updateInstructionsTime += numSeconds;
            updatePatternInstructions += numSeconds;

            breath = audioManager.GetSamples();
            features = audioManager.GetFeatures();         

            //features = audioProcessor.ExtractFeatures (breath);
            //peak = features[0];
            //duration = features[1];
            //mean = features[2];
            Task.Run(() => {
                lock(envelope){
                    envelope = audioProcessor.ExtractFeatures2(breath, features);
                }
                lock (inputGraph.envelope){
                    inputGraph.envelope = envelope;
                }
                MicCheck.UpdateData(envelope,features[0]);// Update Microphone Check data
            });
            if (correctTimeInstructions)
            {
                updatePatternInstructions = 0;
                correctTimeInstructions = false;
            }

            // Get breathing mode 
            // use % to perform cycles ove the breathingPattern vector
            if (updatePatternInstructions >= breathingTimeInstructions)
            {
                i += 1;
                breathingModeInstructions = breathingPattern[i % breathingPattern.Length];
                breathingTimeInstructions = breathingPatternTime[i % breathingPattern.Length];
                updatePatternInstructions = 0;
            }

            switch (breathingModeInstructions)
            {
                case 0: // inhale
                    instructionsText.text = "Inhale";
                    break;
                case 2: // hold
                    instructionsText.text = "Hold";
                    break;
                case 1: //exhale
                    instructionsText.text = "Exhale";
                    break;
            }
            //run once per breath mode
            if (previousModeInstructions == -1 || previousModeInstructions != breathingModeInstructions)
            {
                CountCycleChanges++;
                switch (previousModeInstructions){
                    case 0: // inhale
                        this.onEndInhale();
                        break;
                    case 2: // hold
                        this.onEndHold();
                        break;
                    case 1: //exhale
                        this.onEndExhale();
                        break;
                }
                previousModeInstructions = breathingModeInstructions;
                switch (breathingModeInstructions)
                {
                    case 0: // on inhale run once
                        this.onInhale();
                        break;
                    case 2: // hold
                        this.onHold();
                        break;
                    case 1: //exhale
                        this.onExhale();
                        break;
                }
                //run once per breath cycle
                if(CountCycleChanges%breathingPattern.Length == 1)
                {
                    this.onCycleChange();
                }
            }

        }

        if (calibrateMic){
            if (calibrateTime>=0){ // calibrate the mic
                calibrateTime -= Time.deltaTime;
                calibrateMic = true;
                micCalText.text = "Try and follow the instuction bellow and the white line for a few seconds while we are calibrating the microphone";
                MicCheck.UpdateNormalization();
            }else
            {       
                if (MicCheck.checkIfHotWasDetected()){
                    micAlertText.text = "Microphone is too loud, please move it away and try again";
                    micResetButton.GetComponentInChildren<Text>().text = "Reset";
                    micResetButton.GetComponent<SceneLoader>().SetSceneName(SceneManager.GetActiveScene().name);
                }
                else if (MicCheck.checkIfColdWasDetected()){
                    micAlertText.text = "Microphone is too quiet, please move closer and try again";
                    micResetButton.GetComponentInChildren<Text>().text = "Reset";
                    micResetButton.GetComponent<SceneLoader>().SetSceneName(SceneManager.GetActiveScene().name);
                }
                else{
                    micAlertText.text = "";
                    PlayerPrefs.SetInt("calibrateMic",0);
                    PlayerPrefs.SetFloat("calibrateGraphValue",MicCheck.getNormalization());
                }
                inputGraph.gameObject.SetActive(false);
                micResetButton.gameObject.SetActive(true);
                micCalText.text = "";
                feedbackImage.gameObject.SetActive(false);
                instructionsText.gameObject.SetActive(false);
                this.enabled = false;
                if (micAlertText.text.Equals(""))
                {
                    if (calibrationMenu != null)
                    {
                        calibrationMenu.StartAnimation();
                    }
                }
            }
        }
        //Update every numSeconds
        if (currentTime >= 1 + updateTime)
        {
            updateTime += numSeconds;
            updatePattern += numSeconds;

            if (correctTime)
            {
                updatePattern = 0;
                correctTime = false;
            }

            // Get breathing mode 
            // use % to perform cycles ove the breathingPattern vector
            if (updatePattern >= breathingTime)
            {
                i1 += 1;
                breathingMode = breathingPattern[i1 % breathingPattern.Length];
                breathingTime = breathingPatternTime[i1 % breathingPattern.Length];
                updatePattern = 0;
            }

            switch (breathingMode)
            {
                case 0: // inhale
                    //instructionsText.text = "Inhale";

                    inputGraph.SetFrequency(breathingTime);
                    inputGraph.ReturnToSnapshot();

                    inputGraph.breathAmplitude = MicCheck.GetNormalizationForGraphInhale();
                    //Debug.Log("inputGraph.breathAmplitude: " + inputGraph.breathAmplitude);
                    if (peak < 0.1 || duration < 0.5)
                    {
                        feedbackImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                        inputGraph.userAmplitude = Mathf.Lerp(inputGraph.userAmplitude, 0f, amplitudeChangeSpeed);

                    }
                    else
                    {
                        feedbackImage.GetComponent<Image>().color = new Color32(236, 224, 0, 255);
                        inputGraph.userAmplitude = Mathf.Lerp(inputGraph.userAmplitude, 4f, amplitudeChangeSpeed);

                    }
                    break;
                case 2: // hold
                    //instructionsText.text = "Hold";
                    inputGraph.CreateSnapshot();
                    inputGraph.SetFrequency(breathingTime * .5f);
                    if (peak > 0.1 && duration > 0.5)
                    {
                        feedbackImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                        inputGraph.userAmplitude = Mathf.Lerp(inputGraph.userAmplitude, 4f, amplitudeChangeSpeed);
                    }
                    else
                    {
                        feedbackImage.GetComponent<Image>().color = new Color32(236, 224, 0, 255);
                        inputGraph.userAmplitude = Mathf.Lerp(inputGraph.userAmplitude, 0f, amplitudeChangeSpeed);
                    }
                    break;
                case 1: //exhale
                    //instructionsText.text = "Exhale";

                    inputGraph.SetFrequency(breathingTime);
                    inputGraph.ReturnToSnapshot();

                    inputGraph.breathAmplitude = MicCheck.GetNormalizationForGraphExhale();
                    //Debug.Log("inputGraph.breathAmplitude: " + inputGraph.breathAmplitude);
                    if (peak < 0.1 || duration < 0.5)
                    {
                        feedbackImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                        inputGraph.userAmplitude = Mathf.Lerp(inputGraph.userAmplitude, 0f, amplitudeChangeSpeed);
                    }
                    else
                    {
                        feedbackImage.GetComponent<Image>().color = new Color32(236, 224, 0, 255);
                        inputGraph.userAmplitude = Mathf.Lerp(inputGraph.userAmplitude, 4f, amplitudeChangeSpeed);
                    }
                    break;
            }
        }
    }

    //those run once per breath mode when the mode is changed
    //for performance reasons, we don't want to run them every frame or run them without asyncronous calls
    //If someone someday gets too bored, pls refactor the object manager to use some kind of an event system (main consern is envelop calculations)
    private async void onInhale(){
    }
    private async void onHold(){
    }
    private async void onExhale(){
    }
    //those run once per breath mode when the mode is ended
    private async void onEndInhale(){
        if(!calibrateMic){
        //var delay = inputGraph.GetDelay();
        //int delayInt = (int)(delay*1000);
        //Debug.Log(delayInt);
        await correlationFromGraphs.EndInhaleUpdate(0);
        }
    }
    private async void onEndHold(){
        if (!calibrateMic){
        //var delay = inputGraph.GetDelay();
        //int delayInt = (int)(delay*1000);
        await correlationFromGraphs.EndHoldUpdate(0);
        }
    }
    private async void onEndExhale(){
        if (!calibrateMic){
        //var delay = inputGraph.GetDelay();
        //int delayInt = (int)(delay*1000);
        await correlationFromGraphs.EndExhaleUpdate(0);
        }
    }

    private async void onCycleChange(){
        await Task.Run( () =>{
            if(!calibrateMic){
                Debug.Log("Cycle : " + correlationFromGraphs.getCorrelationCycle());
            }
        });
    }
    



    //private void OnApplicationQuit()
    //{
    //    audioProcessor.smEnvFile.Close();
    //}
}