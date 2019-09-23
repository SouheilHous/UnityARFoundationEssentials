﻿using System;
using System.Text;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using static HumanBoneController;

public class BuildRig : MonoBehaviour
{

    int[] parentIndex = new int[] {-1, 0, 1, 2, 3, 4, 5, 1, 7, 8, 9, 10, 1, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 23, 28, 29, 30, 31, 23, 33, 34, 35, 36, 23, 38, 39, 40, 41, 23, 43, 44, 45, 46, 18, 48, 49, 50, 51, 52, 53, 54, 55, 52, 57, 58, 59, 60, 52, 62, 63, 64, 65, 52, 67, 68, 69, 70, 52, 72, 73, 74, 75, 18, 77, 78, 79, 80, 81, 82, 81, 81, 85, 85, 85, 81, 89, 89, 89};

    Vector4 GetColumn(int joint, int i) {
        int idx = joint * 16 + 4 * i;
        return new Vector4(
            (float)xforms[idx], 
            (float)xforms[idx + 1], 
            (float)xforms[idx + 2], 
            (float)xforms[idx + 3]);
    }

    Matrix4x4[] arkitNeutralPos;
    [SerializeField]
    GameObject prefab;
    Dictionary <int, Transform> fakeRig = new Dictionary<int, Transform>();
    void Build() 
    {
        GameObject fakeRoot = new GameObject();
        fakeRoot.name = "FakeRoot";
        fakeRig.Add(-1, fakeRoot.transform);
        for (int i = 0; i < 93; i++) {
            GameObject g = Instantiate(prefab);
            g.name = System.Enum.GetName(typeof(JointIndices), i);
            g.transform.parent = fakeRig[parentIndex[i]];
            FromMatrix(g.transform,arkitNeutralPos[i]);
            fakeRig.Add(i, g.transform);
        }
    }

    void Start()
    {
        arkitNeutralPos = new Matrix4x4[93];
        for (int i = 0; i < 93; i++) {
            Matrix4x4 m4 = new Matrix4x4(
                GetColumn(i, 0),
                GetColumn(i, 1),
                GetColumn(i, 2),
                GetColumn(i, 3)
            );
            arkitNeutralPos[i] = m4;
        }
        Build();

    }


       public static Quaternion ExtractRotation(Matrix4x4 matrix)
    {
        Vector3 forward;
        forward.x = matrix.m02;
        forward.y = matrix.m12;
        forward.z = matrix.m22;
 
        Vector3 upwards;
        upwards.x = matrix.m01;
        upwards.y = matrix.m11;
        upwards.z = matrix.m21;
 
        return Quaternion.LookRotation(forward, upwards);
    }
 
    public static Vector3 ExtractPosition(Matrix4x4 matrix)
    {
        Vector3 position;
        position.x = matrix.m03;
        position.y = matrix.m13;
        position.z = matrix.m23;
        return position;
    }
 
    public static Vector3 ExtractScale(Matrix4x4 matrix)
    {
        Vector3 scale;
        scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
        scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
        scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
        return scale;
    }
    public static void FromMatrix(Transform transform, Matrix4x4 matrix)
    {
        transform.localScale = ExtractScale(matrix);
        transform.rotation = ExtractRotation(matrix);
        transform.position = ExtractPosition(matrix);
    }
    public static double[] xforms = new double[] {1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 1.0, -6e-06, -3e-06, 0.0, 6e-06, 1.0, 6e-06, 0.0, 3e-06, -6e-06, 1.0, 0.0, -0.0, 0.0, 0.0, 1.0, -0.00015, -0.994522, 0.104523, 0.0, 0.001046, -0.104523, -0.994522, 0.0, 0.999999, -4e-05, 0.001056, 0.0, 0.10054, -0.024979, 0.000958, 1.0, 0.000122, -0.987686, -0.156447, 0.0, 0.001051, 0.156447, -0.987686, 0.0, 0.999999, -4.4e-05, 0.001057, 0.0, 0.100477, -0.443773, 0.044973, 1.0, -0.000993, -0.388347, 0.921512, 0.0, 0.00037, -0.921513, -0.388347, 0.0, 0.999999, -4.5e-05, 0.001059, 0.0, 0.100532, -0.895449, -0.026572, 1.0, -0.00106, -0.017458, 0.999847, 0.0, -2.7e-05, -0.999848, -0.017458, 0.0, 0.999999, -4.5e-05, 0.001059, 0.0, 0.100384, -0.953325, 0.110763, 1.0, -0.001059, -0.017458, 0.999847, 0.0, -2.6e-05, -0.999848, -0.017458, 0.0, 0.999999, -4.5e-05, 0.001059, 0.0, 0.100321, -0.954384, 0.171414, 1.0, 6e-06, 0.994522, -0.104523, 0.0, 3e-06, 0.104523, 0.994522, 0.0, 1.0, -6e-06, -2e-06, 0.0, -0.10054, -0.024973, 0.000959, 1.0, 7e-06, 0.987686, 0.156448, 0.0, 1e-06, -0.156447, 0.987686, 0.0, 1.0, -6e-06, -2e-06, 0.0, -0.100543, -0.44376, 0.044973, 1.0, 2e-06, 0.388348, -0.921513, 0.0, 6e-06, 0.921513, 0.388348, 0.0, 1.0, -6e-06, -1e-06, 0.0, -0.100545, -0.895435, -0.026572, 1.0, -0.0, 0.017459, -0.999848, 0.0, 7e-06, 0.999848, 0.017459, 0.0, 1.0, -7e-06, -0.0, 0.0, -0.100545, -0.953311, 0.110763, 1.0, -0.0, 0.017459, -0.999848, 0.0, 9e-06, 0.999848, 0.017459, 0.0, 1.0, -9e-06, -0.0, 0.0, -0.100545, -0.95437, 0.171414, 1.0, 0.0, 1.0, 0.0, 0.0, 2e-06, 0.0, 1.0, 0.0, 1.0, -0.0, -2e-06, 0.0, -0.0, 0.0, 0.0, 1.0, 0.0, 1.0, 0.0, 0.0, 2e-06, 0.0, 1.0, 0.0, 1.0, -0.0, -2e-06, 0.0, -0.0, 0.083, 0.0, 1.0, 0.0, 1.0, 0.0, 0.0, 2e-06, 0.0, 1.0, 0.0, 1.0, -0.0, -2e-06, 0.0, -0.0, 0.166, 0.0, 1.0, 0.0, 1.0, 0.0, 0.0, 2e-06, 0.0, 1.0, 0.0, 1.0, -0.0, -2e-06, 0.0, -0.0, 0.249, 0.0, 1.0, 3e-06, 0.999544, -0.030188, 0.0, 2e-06, 0.030188, 0.999544, 0.0, 1.0, -3e-06, -2e-06, 0.0, -0.0, 0.332436, 0.0, 1.0, 3e-06, 0.999544, -0.030188, 0.0, 2e-06, 0.030188, 0.999544, 0.0, 1.0, -3e-06, -2e-06, 0.0, 0.0, 0.415398, -0.002506, 1.0, 3e-06, 0.999544, -0.030188, 0.0, 2e-06, 0.030188, 0.999544, 0.0, 1.0, -3e-06, -2e-06, 0.0, 0.0, 0.49836, -0.005011, 1.0, 0.9664, 0.256672, 0.0138, 0.0, -0.014416, 0.000522, 0.999896, 0.0, 0.256638, -0.966498, 0.004204, 0.0, -0.012148, 0.572, -0.012559, 1.0, 0.966532, 0.256206, 0.013204, 0.0, -0.01303, -0.002377, 0.999912, 0.0, 0.256215, -0.966619, 0.001041, 0.0, -0.089365, 0.551491, -0.013662, 1.0, 0.999999, -0.000955, -0.000442, 0.0, 0.000442, -0.000142, 1.0, 0.0, -0.000955, -1.0, -0.000141, 0.0, -0.22956, 0.514329, -0.015577, 1.0, 0.996154, -0.000942, -0.087617, 0.0, 0.087617, -0.000221, 0.996154, 0.0, -0.000957, -1.0, -0.000137, 0.0, -0.495546, 0.514583, -0.015459, 1.0, 0.996154, -0.000942, -0.087617, 0.0, 0.000957, 1.0, 0.000137, 0.0, 0.087617, -0.000221, 0.996154, 0.0, -0.762486, 0.514835, 0.00802, 1.0, 0.539265, 0.28491, -0.792476, 0.0, 0.729853, 0.311348, 0.608586, 0.0, 0.420128, -0.906581, -0.040043, 0.0, -0.790163, 0.500577, 0.034369, 1.0, 0.772194, 0.380332, -0.508984, 0.0, 0.476665, 0.182917, 0.859844, 0.0, 0.420128, -0.906581, -0.040043, 0.0, -0.81321, 0.488398, 0.068235, 1.0, 0.849355, 0.408379, -0.334399, 0.0, 0.319513, 0.106479, 0.94158, 0.0, 0.420128, -0.906581, -0.040043, 0.0, -0.837025, 0.476673, 0.083931, 1.0, 0.849355, 0.408379, -0.334399, 0.0, 0.319513, 0.106479, 0.94158, 0.0, 0.420128, -0.906581, -0.040044, 0.0, -0.862715, 0.464319, 0.094046, 1.0, 0.978297, -0.000245, -0.207208, 0.0, 0.000972, 0.999994, 0.003405, 0.0, 0.207206, -0.003533, 0.978291, 0.0, -0.806019, 0.515274, 0.033395, 1.0, 0.948007, 0.308728, -0.077265, 0.0, -0.274286, 0.915729, 0.293611, 0.0, 0.1614, -0.257153, 0.952797, 0.0, -0.869749, 0.51529, 0.046894, 1.0, 0.843053, 0.537825, 0.002346, 0.0, -0.513042, 0.80288, 0.303599, 0.0, 0.1614, -0.257153, 0.952797, 0.0, -0.90374, 0.50422, 0.049664, 1.0, 0.79677, 0.603636, 0.027948, 0.0, -0.58233, 0.754649, 0.302318, 0.0, 0.1614, -0.257153, 0.952797, 0.0, -0.928207, 0.488612, 0.049596, 1.0, 0.79677, 0.603636, 0.027948, 0.0, -0.58233, 0.754649, 0.302319, 0.0, 0.1614, -0.257153, 0.952797, 0.0, -0.944725, 0.476097, 0.049017, 1.0, 0.996154, -0.000662, -0.087616, 0.0, 0.000957, 0.999994, 0.003326, 0.0, 0.087614, -0.003397, 0.996149, 0.0, -0.808923, 0.515568, 0.015199, 1.0, 0.984287, 0.173046, -0.035143, 0.0, -0.172921, 0.984914, 0.006598, 0.0, 0.035754, -0.000417, 0.99936, 0.0, -0.876028, 0.513487, 0.021094, 1.0, 0.862358, 0.505371, -0.030642, 0.0, -0.505035, 0.862902, 0.018429, 0.0, 0.035754, -0.000417, 0.99936, 0.0, -0.912908, 0.507003, 0.022411, 1.0, 0.787946, 0.615111, -0.027934, 0.0, -0.614706, 0.78844, 0.022321, 0.0, 0.035754, -0.000417, 0.99936, 0.0, -0.937934, 0.492337, 0.0233, 1.0, 0.787946, 0.615111, -0.027934, 0.0, -0.614706, 0.78844, 0.022321, 0.0, 0.035754, -0.000417, 0.99936, 0.0, -0.957966, 0.476699, 0.02401, 1.0, 0.999923, -0.000994, 0.012347, 0.0, 0.000953, 0.999994, 0.00333, 0.0, -0.012351, -0.003318, 0.999918, 0.0, -0.807519, 0.512609, 7.9e-05, 1.0, 0.977071, 0.201514, 0.06873, 0.0, -0.186566, 0.965873, -0.179673, 0.0, -0.102591, 0.162731, 0.981322, 0.0, -0.869362, 0.508209, -0.000699, 1.0, 0.802106, 0.59699, -0.015143, 0.0, -0.588304, 0.785571, -0.191773, 0.0, -0.102591, 0.162731, 0.981322, 0.0, -0.907362, 0.500371, -0.003372, 1.0, 0.718454, 0.69442, -0.040045, 0.0, -0.687967, 0.700927, -0.188156, 0.0, -0.102591, 0.162731, 0.981322, 0.0, -0.926663, 0.486006, -0.003008, 1.0, 0.718454, 0.69442, -0.040045, 0.0, -0.687967, 0.700927, -0.188156, 0.0, -0.102591, 0.162731, 0.981322, 0.0, -0.945679, 0.467626, -0.001948, 1.0, 0.993528, -0.001333, 0.113576, 0.0, 0.000953, 0.999994, 0.0034, 0.0, -0.113579, -0.00327, 0.993523, 0.0, -0.805697, 0.508161, -0.016606, 1.0, 0.955086, 0.263785, 0.135011, 0.0, -0.220262, 0.936738, -0.272042, 0.0, -0.19823, 0.230086, 0.952767, 0.0, -0.859693, 0.499039, -0.022809, 1.0, 0.893489, 0.442055, 0.079144, 0.0, -0.402966, 0.866976, -0.293208, 0.0, -0.19823, 0.230086, 0.952767, 0.0, -0.884472, 0.492195, -0.026311, 1.0, 0.832581, 0.552472, 0.039807, 0.0, -0.517218, 0.801146, -0.301082, 0.0, -0.19823, 0.230086, 0.952767, 0.0, -0.900939, 0.484048, -0.02777, 1.0, 0.832581, 0.552472, 0.039807, 0.0, -0.517218, 0.801146, -0.301082, 0.0, -0.19823, 0.230086, 0.952767, 0.0, -0.919764, 0.471556, -0.02867, 1.0, 0.966399, -0.256678, -0.013772, 0.0, -0.014388, -0.000522, -0.999896, 0.0, 0.256644, 0.966497, -0.004197, 0.0, 0.012185, 0.571991, -0.012562, 1.0, 0.966533, -0.256204, -0.013158, 0.0, -0.01299, 0.002345, -0.999913, 0.0, 0.256213, 0.96662, -0.001061, 0.0, 0.089366, 0.551491, -0.013662, 1.0, 0.999999, 0.00095, 0.000457, 0.0, 0.000457, 0.000132, -1.0, 0.0, -0.00095, 1.0, 0.000132, 0.0, 0.229562, 0.514329, -0.015571, 1.0, 0.996153, 0.000937, 0.087632, 0.0, 0.087632, 0.000215, -0.996153, 0.0, -0.000952, 1.0, 0.000132, 0.0, 0.495548, 0.514582, -0.015449, 1.0, 0.996153, 0.000937, 0.087632, 0.0, 0.000952, -0.999999, -0.000128, 0.0, 0.087632, 0.000211, -0.996153, 0.0, 0.762487, 0.514833, 0.008034, 1.0, 0.539242, -0.284927, 0.792486, 0.0, 0.72985, -0.31138, -0.608574, 0.0, 0.420164, 0.906564, 0.040044, 0.0, 0.790163, 0.500571, 0.034384, 1.0, 0.772171, -0.38036, 0.508997, 0.0, 0.47667, -0.182941, -0.859836, 0.0, 0.420164, 0.906564, 0.040044, 0.0, 0.813211, 0.488394, 0.06825, 1.0, 0.849333, -0.408411, 0.334415, 0.0, 0.319523, -0.106498, -0.941575, 0.0, 0.420164, 0.906564, 0.040044, 0.0, 0.837024, 0.476664, 0.083947, 1.0, 0.849333, -0.408411, 0.334415, 0.0, 0.319523, -0.106498, -0.941575, 0.0, 0.420164, 0.906564, 0.040044, 0.0, 0.862713, 0.464311, 0.094062, 1.0, 0.978295, 0.000207, 0.207216, 0.0, 0.000932, -0.999994, -0.003403, 0.0, 0.207214, 0.003523, -0.978289, 0.0, 0.80602, 0.515265, 0.03341, 1.0, 0.947994, -0.308765, 0.077273, 0.0, -0.27432, -0.915719, -0.293612, 0.0, 0.161417, 0.257145, -0.952796, 0.0, 0.86975, 0.515279, 0.046909, 1.0, 0.843032, -0.537858, -0.002338, 0.0, -0.513071, -0.80286, -0.303601, 0.0, 0.161417, 0.257145, -0.952796, 0.0, 0.903741, 0.504208, 0.04968, 1.0, 0.796197, -0.604378, -0.028225, 0.0, -0.583108, -0.754058, -0.302295, 0.0, 0.161417, 0.257145, -0.952796, 0.0, 0.928206, 0.488598, 0.049612, 1.0, 0.796197, -0.604378, -0.028225, 0.0, -0.583108, -0.754058, -0.302296, 0.0, 0.161417, 0.257145, -0.952796, 0.0, 0.944714, 0.476068, 0.049027, 1.0, 0.996152, 0.001251, 0.087633, 0.0, 0.001543, -0.999993, -0.003265, 0.0, 0.087629, 0.003388, -0.996147, 0.0, 0.808925, 0.515564, 0.015214, 1.0, 0.984387, -0.172467, 0.03517, 0.0, -0.172344, -0.985015, -0.00654, 0.0, 0.035771, 0.000377, -0.99936, 0.0, 0.876031, 0.513524, 0.021111, 1.0, 0.862653, -0.504864, 0.030687, 0.0, -0.504529, -0.863199, -0.018385, 0.0, 0.035771, 0.000377, -0.99936, 0.0, 0.912915, 0.507062, 0.022428, 1.0, 0.788305, -0.614647, 0.027985, 0.0, -0.614244, -0.788802, -0.022284, 0.0, 0.035771, 0.000377, -0.99936, 0.0, 0.93795, 0.49241, 0.023319, 1.0, 0.788305, -0.614647, 0.027985, 0.0, -0.614244, -0.788802, -0.022284, 0.0, 0.035771, 0.000377, -0.99936, 0.0, 0.95799, 0.476785, 0.02403, 1.0, 0.999924, 0.000954, -0.012333, 0.0, 0.000913, -0.999994, -0.003321, 0.0, -0.012336, 0.003309, -0.999918, 0.0, 0.80752, 0.5126, 9.4e-05, 1.0, 0.977064, -0.201554, -0.068713, 0.0, -0.186608, -0.965864, 0.17968, 0.0, -0.102583, -0.162736, -0.981322, 0.0, 0.869364, 0.508199, -0.000684, 1.0, 0.802081, -0.597022, 0.01516, 0.0, -0.588338, -0.785545, 0.191772, 0.0, -0.102583, -0.162736, -0.981322, 0.0, 0.907363, 0.50036, -0.003356, 1.0, 0.802081, -0.597022, 0.01516, 0.0, -0.588338, -0.785545, 0.191772, 0.0, -0.102583, -0.162736, -0.981322, 0.0, 0.926664, 0.485994, -0.002991, 1.0, 0.802081, -0.597022, 0.01516, 0.0, -0.588338, -0.785545, 0.191772, 0.0, -0.102583, -0.162736, -0.981322, 0.0, 0.947893, 0.470192, -0.00259, 1.0, 0.99353, 0.001292, -0.113561, 0.0, 0.000913, -0.999994, -0.003387, 0.0, -0.113565, 0.003261, -0.993525, 0.0, 0.805699, 0.50816, -0.016591, 1.0, 0.955077, -0.263825, -0.134993, 0.0, -0.220303, -0.936725, 0.272051, 0.0, -0.198225, -0.23009, -0.952767, 0.0, 0.859695, 0.49903, -0.022793, 1.0, 0.893473, -0.442092, -0.079125, 0.0, -0.403005, -0.866956, 0.293213, 0.0, -0.198225, -0.23009, -0.952767, 0.0, 0.884473, 0.492186, -0.026295, 1.0, 0.832559, -0.552505, -0.039787, 0.0, -0.517255, -0.801122, 0.301084, 0.0, -0.198225, -0.23009, -0.952767, 0.0, 0.90094, 0.484037, -0.027753, 1.0, 0.832559, -0.552505, -0.039787, 0.0, -0.517255, -0.801122, 0.301084, 0.0, -0.198225, -0.23009, -0.952767, 0.0, 0.919765, 0.471545, -0.028653, 1.0, 3e-06, 0.970422, 0.241416, 0.0, 1e-06, -0.241416, 0.970422, 0.0, 1.0, -3e-06, -2e-06, 0.0, 1e-06, 0.573254, -0.015749, 1.0, 0.0, 0.970169, 0.242431, 0.0, 2e-06, -0.242431, 0.970169, 0.0, 1.0, 0.0, -2e-06, 0.0, 1e-06, 0.602367, -0.008507, 1.0, 0.0, 0.970169, 0.242431, 0.0, 2e-06, -0.242431, 0.970169, 0.0, 1.0, 0.0, -2e-06, 0.0, 1e-06, 0.641173, 0.001191, 1.0, 0.0, 0.970169, 0.242431, 0.0, 2e-06, -0.242431, 0.970169, 0.0, 1.0, 0.0, -2e-06, 0.0, 1e-06, 0.67998, 0.010888, 1.0, -1e-06, 0.999857, -0.016928, 0.0, 2e-06, 0.016928, 0.999857, 0.0, 1.0, 1e-06, -2e-06, 0.0, 3.3e-05, 0.724817, 0.022647, 1.0, -1e-06, 0.999857, -0.016928, 0.0, 2e-06, 0.016928, 0.999857, 0.0, 1.0, 1e-06, -2e-06, 0.0, 3.3e-05, 0.699905, 0.02807, 1.0, -1e-06, 0.999857, -0.016928, 0.0, 2e-06, 0.016928, 0.999857, 0.0, 1.0, 1e-06, -2e-06, 0.0, 3.3e-05, 0.651605, 0.128902, 1.0, -1e-06, 1.0, 0.000524, 0.0, 2e-06, -0.000524, 1.0, 0.0, 1.0, 1e-06, -2e-06, 0.0, 3.3e-05, 0.726679, 0.132632, 1.0, -1e-06, 1.0, 0.000514, 0.0, 2e-06, -0.000514, 1.0, 0.0, 1.0, 1e-06, -2e-06, 0.0, -0.032314, 0.758888, 0.108689, 1.0, -1e-06, 1.0, 0.000514, 0.0, 2e-06, -0.000514, 1.0, 0.0, 1.0, 1e-06, -2e-06, 0.0, -0.032314, 0.758888, 0.108689, 1.0, -1e-06, 1.0, 0.000514, 0.0, 2e-06, -0.000514, 1.0, 0.0, 1.0, 1e-06, -2e-06, 0.0, -0.032314, 0.758888, 0.108689, 1.0, -1e-06, 1.0, 0.000514, 0.0, 2e-06, -0.000514, 1.0, 0.0, 1.0, 1e-06, -2e-06, 0.0, -0.032314, 0.758888, 0.108689, 1.0, -1e-06, 1.0, 0.000514, 0.0, 2e-06, -0.000514, 1.0, 0.0, 1.0, 1e-06, -2e-06, 0.0, 0.032321, 0.758888, 0.108689, 1.0, -1e-06, 1.0, 0.000514, 0.0, 2e-06, -0.000514, 1.0, 0.0, 1.0, 1e-06, -2e-06, 0.0, 0.032321, 0.758888, 0.108689, 1.0, -1e-06, 1.0, 0.000514, 0.0, 2e-06, -0.000514, 1.0, 0.0, 1.0, 1e-06, -2e-06, 0.0, 0.032321, 0.758888, 0.108689, 1.0, -1e-06, 1.0, 0.000514, 0.0, 2e-06, -0.000514, 1.0, 0.0, 1.0, 1e-06, -2e-06, 0.0, 0.032321, 0.758888, 0.108689, 1.0};
}