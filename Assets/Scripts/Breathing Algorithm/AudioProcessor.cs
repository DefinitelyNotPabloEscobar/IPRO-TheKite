using ScottPlot.Statistics.Interpolation;
using System.Linq;
using Accord.Math;
using System;
using System.Numerics;
using UnityEngine;
using System.IO;
using IIR_Butterworth_CS_Library;
using Filtering;

public class AudioProcessor
{
    float[] breath, breath_hilbert, features;
    double[] breathDouble;
    Complex[] breath_analytic = new Complex[Convert.ToInt32(Math.Pow(10, 4))];
    int numSamples = 2500;

    public StreamWriter smEnvFile, breathFile, envFile, filtBreathFile, normBreathFile;// file writers
    private IIR_Butterworth butterworth ;
    private SavitzkyGolayFilter savgolFilter;

    private double[][] filt_Coefficients;

    public AudioProcessor()
    {
        butterworth = new IIR_Butterworth();
        filt_Coefficients = butterworth.Lp2bp(0.1, 0.98, 5);
        savgolFilter = new SavitzkyGolayFilter(1000, 1);
        //// Initialize file writed
        //smEnvFile = new StreamWriter("sm_envelope.csv");
        //var directory = Directory.CreateDirectory(Application.dataPath + "/Breaths");
        //var ID = Guid.NewGuid().ToString("N");
        //breathFile = new StreamWriter(Application.dataPath + "/Breaths/breath" + ID + ".csv");
        //envFile = new StreamWriter("envelope.csv");
        //filtBreathFile = new StreamWriter("filt_breath.csv");
        //normBreathFile = new StreamWriter("norm_breath.csv");
    }

    public float[] ExtractFeatures(float[] breath)
    {
        //Save breathing signal
        foreach (float b in breath)
        {
            breathFile.Write(b + "\n");
        }



        // Compute Hilbert transform 
        breathDouble = ZeroPad(ConvertToDouble(breath), Convert.ToInt32(Math.Pow(2, 12))); // zero-padding
        HilbertTransform.FHT(breathDouble, FourierTransform.Direction.Forward);
        breathDouble = RemoveValues(breathDouble, 3* Convert.ToInt32(Math.Pow(10, 3)));// remove zero padding
        breath_hilbert = ConvertToSingle(breathDouble);

        for (int i = 0; i < breath.Length; i++)
            breath_analytic[i] = new Complex(breath[i], breath_hilbert[i]);

        // Calculate complex magnitude of Hilbert transform
        double[] envelope = new double[breath.Length];
        for (int i = 0; i < envelope.Length; i++)
            envelope[i] = Complex.Abs(Complex.Abs(breath_analytic[i]));

        ////Save envelope
        //foreach (float e in envelope)
        //{
        //    envFile.Write(e + "\n");
        //}

        // Perform Spline interpolation over local maxima separated by at least f_s/5 samples 
        double[] t = new double[5];
        double[] envMaxima = new double[5];
        for (int i = 0; i < t.Length; i++)
        {
            t[i] = (float)i / t.Length;

            // Get the max of 2000 samples of the envelope at each iteration 
            envMaxima[i] = envelope.Skip(i * numSamples/5).Take(numSamples/5).ToArray().Max();
        }

        PeriodicSpline nsi = new PeriodicSpline(t, envMaxima, resolution: 20);

        //Save smoothedenvelope
        foreach (float y in nsi.interpolatedYs)
        {
            smEnvFile.Write(y + "\n");
        }

        // Obtain the features
        double[] smoothedEnvelope = nsi.interpolatedYs;
        double[] time = nsi.interpolatedXs;
        double peak = smoothedEnvelope.Max();
        double mean = smoothedEnvelope.Average();
        double duration = 0.8, t1 = 0, t2 = 0.8; // max duration = 0.8 due to the nsi.interpolatedXs is from 0 to 0.8
        int idx = 0;

        // Calculate the duration that the signal is above 0.01 its' maximum value
        for (int j = 0; j < smoothedEnvelope.Length; j++)
        {
            if (smoothedEnvelope[j] >= 0.5 * peak)
            {
                t1 = time[j];
                idx = j;
                break;
            }
        }
        for (int j = smoothedEnvelope.Length - 1; j >= idx; j--)
        {
            if (smoothedEnvelope[j] <= 0.5 * peak)
            {
                t2 = time[j];
                duration = t2 - t1;
                break;
            }
        }

        double[] f = new double[] { peak, duration, mean};
        return ConvertToSingle(smoothedEnvelope);
    }

    public float[] ExtractFeatures2(float[] breath, float[] features)
    {
        ////Save breathing signal
        //foreach (float b in breath)
        //{
        //    breathFile.Write(b + "\n");
        //}

        //// Normalize
        //breath = breath.Select(x => x - features[1]).ToArray();
        //breath = breath.Select(x => x / features[0]).ToArray();

        //foreach(float b in breath)
        //{
        //    normBreathFile.WriteLine(b);
        //}

        // Filter
        var filtered_breath = butterworth.Filter_Data(filt_Coefficients, ConvertToDouble(breath));

        //foreach (float fb in filtered_breath)
        //{
        //    filtBreathFile.WriteLine(fb);
        //}

        // Compute Hilbert transform 
        breathDouble = ZeroPad(filtered_breath, Convert.ToInt32(Math.Pow(2, 12))); // zero-padding
        HilbertTransform.FHT(breathDouble, FourierTransform.Direction.Forward);
        breathDouble = RemoveValues(breathDouble, 3 * Convert.ToInt32(Math.Pow(10, 3)));// remove zero padding
        breath_hilbert = ConvertToSingle(breathDouble);

        for (int i = 0; i < filtered_breath.Length; i++)
            breath_analytic[i] = new Complex(filtered_breath[i], breath_hilbert[i]);

        // Calculate complex magnitude of Hilbert transform
        double[] envelope = new double[filtered_breath.Length];
        for (int i = 0; i < envelope.Length; i++)
            envelope[i] = Complex.Abs(breath_analytic[i]);

        ////Save envelope
        //foreach (float e in envelope)
        //{
        //    envFile.Write(e + "\n");
        //}

        // Savgol smoothing
        double[] smEnvelope = savgolFilter.Process(envelope);

        //Save smoothedenvelope
        //foreach (float y in smEnvelope)
        //{
        //    smEnvFile.Write(y + "\n");
        //}
        return ConvertToSingle(smEnvelope);
    }

    // Convert to Double because Hilbert Transform Implementation works with double 
    public static double[] ConvertToDouble(float[] inputArray)
    {
        if (inputArray == null)
            return null;

        double[] output = new double[inputArray.Length];
        for (int i = 0; i < inputArray.Length; i++)
            output[i] = inputArray[i];

        return output;
    }

    // Convert to single after applying the Hilbert Transform
    public static float[] ConvertToSingle(double[] inputArray)
    {
        if (inputArray == null)
            return null;

        float[] output = new float[inputArray.Length];
        for (int i = 0; i < inputArray.Length; i++)
            output[i] = Convert.ToSingle(inputArray[i]);

        return output;
    }

    // Zero-pad array to perform the hilbert transform
    public static double[] ZeroPad(double[] inputArray, int N)
    {
        double[] output = new double[N];
        for (int i = 0; i < inputArray.Length; i++)
            output[i] = inputArray[i];

        return output;
    }

    // Remove zero-padded values
    public static double[] RemoveValues(double[] inputArray, int N)
    {
        double[] output = new double[N];
        for (int i = 0; i < N; i++)
            output[i] = inputArray[i];

        return output;
    }
}
