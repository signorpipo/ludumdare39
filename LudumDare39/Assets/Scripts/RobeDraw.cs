using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RobeDraw : MonoBehaviour {

    [SerializeField]
    private Transform[] m_joints;
    [SerializeField]
    private float m_smoothness;
	LineRenderer m_myRenderer;

    public void RemoveLastJount()
    {
        Transform[] newJoints = new Transform[m_joints.Length - 1];
        for(int i=0; i<m_joints.Length-1; i++)
        {
            newJoints[i] = m_joints[i];
        }
        m_joints = newJoints;
    }

	void Start () {
		m_myRenderer = GetComponent<LineRenderer>();
		m_myRenderer.startWidth = 0.1f;
		m_myRenderer.endWidth = 0.1f;
	}

	void Update () {

        List<Vector3> positions = new List<Vector3>();
        foreach(Transform pos in m_joints)
        {
            positions.Add(pos.position);
        }

		Vector3[] points = Curver.MakeSmoothCurve(positions.ToArray(), m_smoothness);
		m_myRenderer.numPositions = points.Length;
		for (int i = 0; i < m_myRenderer.numPositions; ++i)
        {
            m_myRenderer.SetPosition(i, points[i]);
		}
	}
}
