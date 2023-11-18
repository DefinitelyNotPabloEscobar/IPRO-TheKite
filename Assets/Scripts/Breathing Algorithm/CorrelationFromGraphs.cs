using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using MathNet.Numerics.Statistics;
using System.IO;

public class CorrelationFromGraphs : MonoBehaviour
{
    public GraphDraw graphDraw;
    private List<double> patternControlledInput;
    private List<double> userControlledInput;

    private double[] patternControlledInhale;
    private double[] userControlledInhale;
    private double[] patternControlledHold;
    private double[] userControlledHold;
    private double[] patternControlledExhale;
    private double[] userControlledExhale;

    private double[] patternControlledCycle;
    private double[] userControlledCycle;


    void Start(){// initialize lists and arrays
        resetDataAll();
    }
    void Update(){ //sample every update the transforms of the objects
        patternControlledInput.Add(graphDraw.patternControlled.position.y);
        userControlledInput.Add(graphDraw.breath.position.y);
    }

    public async Task userControlledEndInhaleUpdate(int delay){
        await Task.Delay(delay);
        lock (userControlledInhale){
        userControlledInhale = new double[userControlledInput.Count];
        userControlledInhale = userControlledInput.ToArray();
        }
        //Debug.Log("userControlledInhale: " + userControlledInhale.Length);
        deleteUserControlledInput();
    }
    public async Task userControlledEndHoldUpdate(int delay){
        await Task.Delay(delay);
        lock (userControlledHold){
        userControlledHold = new double[userControlledInput.Count];
        userControlledHold = userControlledInput.ToArray();
        }
        //Debug.Log("userControlledHold: " + userControlledHold.Length);
        deleteUserControlledInput();
    }
    public async Task userControlledEndExhaleUpdate(int delay){
        await Task.Delay(delay);
        lock (userControlledExhale){
        userControlledExhale = new double[userControlledInput.Count];
        userControlledExhale = userControlledInput.ToArray();
        }
        //Debug.Log("userControlledExhale: " + userControlledExhale.Length);
        deleteUserControlledInput();
    }
    public async Task patternControlledEndInhaleUpdate(int delay){
        await Task.Delay(delay);
        lock (patternControlledInhale){
        patternControlledInhale = new double[patternControlledInput.Count];
        patternControlledInhale = patternControlledInput.ToArray();
        }
        //Debug.Log("patternControlledInhale: " + patternControlledInhale.Length);
        deletePatternControlledInput();
    }
    public async Task patternControlledEndHoldUpdate(int delay){
        await Task.Delay(delay);
        lock (patternControlledHold){
        patternControlledHold = new double[patternControlledInput.Count];
        patternControlledHold = patternControlledInput.ToArray();
        }
        //Debug.Log("patternControlledHold: " + patternControlledHold.Length);
        deletePatternControlledInput();
    }
    public async Task patternControlledEndExhaleUpdate(int delay){
        await Task.Delay(delay);
        lock (patternControlledExhale){
        patternControlledExhale = new double[patternControlledInput.Count];
        patternControlledExhale = patternControlledInput.ToArray();
        }
        //Debug.Log("patternControlledExhale: " + patternControlledExhale.Length);
        deletePatternControlledInput();
    }
    public async Task EndInhaleUpdate(int delay){
        //call the update functions for the graphs and await them simultaneously
        Task[] tasks = new Task[2];
        tasks[0] = userControlledEndInhaleUpdate(delay);
        tasks[1] = patternControlledEndInhaleUpdate(delay);
        await Task.WhenAll(tasks);
        AddUpdatesToCycle();
    }
    public async Task EndHoldUpdate(int delay){
        //call the update functions for the graphs and await them simultaneously
        Task[] tasks = new Task[2];
        tasks[0] = userControlledEndHoldUpdate(delay);
        tasks[1] = patternControlledEndHoldUpdate(delay);
        await Task.WhenAll(tasks);
        AddUpdatesToCycle();
    }
    public async Task EndExhaleUpdate(int delay){
        //call the update functions for the graphs and await them simultaneously
        Task[] tasks = new Task[2];
        tasks[0] = userControlledEndExhaleUpdate(delay);
        tasks[1] = patternControlledEndExhaleUpdate(delay);
        await Task.WhenAll(tasks);
        AddUpdatesToCycle();
    }
    private void AddUpdatesToCycle(){
        lock (patternControlledCycle){
            lock(userControlledCycle){
                patternControlledCycle = patternControlledInhale.Concat(patternControlledHold).Concat(patternControlledExhale).ToArray();
                userControlledCycle = userControlledInhale.Concat(userControlledHold).Concat(userControlledExhale).ToArray();
            }
        }
    }
    public void deletePatternControlledInput(){
        lock (patternControlledInput){
            patternControlledInput.Clear();
        }
    }
    public void deleteUserControlledInput(){
        lock (userControlledInput){
            userControlledInput.Clear();
        }
    }

    //get correlation between pattern and user controlled for each phase of the cycle
    public double getCorrelationInhale(){
        return computeCorrelation(patternControlledInhale,userControlledInhale);
    }
    public double getCorrelationHold(){
        return computeCorrelation(patternControlledHold, userControlledHold);
    }
    public double getCorrelationExhale(){
        return computeCorrelation(patternControlledExhale, userControlledExhale);
    }
    public double getCorrelationCycle(){
        return computeCorrelation(patternControlledCycle, userControlledCycle);
    }
    public double computeCorrelation(double[] a, double[] b){
        
        //sometimes when we have too much fps, the arrays are not the same size, so we need to make them the same size
        //from testing they only differ by 1 or 2 elements

        //check lenghs of arrays and if they differ make them the same length
        if (a.Length!=b.Length){
            int min = System.Math.Min(a.Length, b.Length);
            if (a.Length>b.Length){
                double[] temp = new double[min];
                for (int i = 0; i < min; i++){
                    temp[i] = a[i];
                }
                a = temp;
            }
            else{
                double[] temp = new double[min];
                for (int i = 0; i < min; i++){
                    temp[i] = b[i];
                }
                b = temp;
            }
        }
        return Correlation.Pearson(a,b);
    }
    public double ComputeCoeff(double[] values1, double[] values2)
    {
        if(values1.Length != values2.Length){
            Debug.Log("Error: values1 and values2 must have the same length");  // error handling
            return (double.NaN);
        }
        else{
            double avg1 = values1.Average();
            var avg2 = values2.Average();
            var sum1 = values1.Zip(values2, (x1, y1) => (x1 - avg1) * (y1 - avg2)).Sum();
            var sumSqr1 = values1.Sum(x => System.Math.Pow((x - avg1), 2.0));
            var sumSqr2 = values2.Sum(y => System.Math.Pow((y - avg2), 2.0));
            var result = sum1 / System.Math.Sqrt(sumSqr1 * sumSqr2);
            return result;
        }
    }
    private void PrintArray(double[] array){ //for debugging
        string s = "";
        foreach(double d in array){
            s += d + " ";
        }
        Debug.Log(s);
    }

    public void resetDataAll(){
        patternControlledInput = new List<double>();
        userControlledInput = new List<double>();
        patternControlledInhale = new double[1];
        userControlledInhale = new double[1];
        patternControlledHold = new double[1];
        userControlledHold = new double[1];
        patternControlledExhale = new double[1];
        userControlledExhale = new double[1];
        patternControlledCycle = new double[1];
        userControlledCycle = new double[1];
        patternControlledInhale[0] = 0;
        userControlledInhale[0] = 0;
        patternControlledHold[0] = 0;
        userControlledHold[0] = 0;
        patternControlledExhale[0] = 0;
        userControlledExhale[0] = 0;
        patternControlledCycle[0] = 0;
        userControlledCycle[0] = 0;
    }

    

    
}
