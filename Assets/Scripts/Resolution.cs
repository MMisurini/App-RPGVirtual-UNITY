using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolution : MonoBehaviour
{
    private static int[] resX = new int[] { 3840, 2560, 1920, 1280, 854, 640, 426};
    private static int[] resY = new int[] { 2160, 1440, 1080, 720, 480, 360, 240 };
    private static int[] index = new int[7];
    private static int menor = 0, maior = 0, posicao_menor = 0, posicao_maior = 0;

    public static float camValueX = 0;
    public static float camValueY = 0;

    public static void CheckResolution (int value){
        for (int i = 0; i < resY.Length; i++) {
            index[i] = resY[i] - value;

            if (i == 0) {
                menor = resY[0];
                maior = resY[0];
            }

            if (Mathf.Abs(index[i]) < menor) {  
                menor = Mathf.Abs(index[i]);
                posicao_menor = i;
            }
        }

        camValueX = resX[posicao_menor];
        camValueY = resY[posicao_menor];
    }

}
