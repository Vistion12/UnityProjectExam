using System.Collections.Generic;
using UnityEngine;

public class CommandCenter : MonoBehaviour
{
    [SerializeField] Scaner scaner;
    [SerializeField] Drone[] drones;
    [SerializeField] Counter counter;
    [SerializeField] Drone dronePrefab; // ������ �����
    [SerializeField] Transform dronesParent; // �������� ��� ������
    [SerializeField] Transform[] _wayPoints;


    private const KeyCode scanKey = KeyCode.UpArrow;
    private Queue<Drone> queueDrones = new Queue<Drone>();
    private Queue<Resource> queueResorces = new Queue<Resource>();
    private Transform target;
    private int maxDrones = 3;
    private int currentDrones = 0;
    private bool droneJustCreated = false;
    private bool blockNextCount = false;

    private void Awake()
    {
        for (int i = 0; i < drones.Length; i++)
        {
            queueDrones.Enqueue(drones[i]);
        }
        if (counter != null)
            counter.MaxReached += OnCounterMaxReached;
        currentDrones = drones.Length;
    }

    private void Update()
    {
        if (droneJustCreated)
        {
            droneJustCreated = false;
            return; // ���������� ���� ���� ����� �������� �����
        }

        // 1. ��������� ������� (��������, �� ������)
        if (Input.GetKeyDown(scanKey))
        {
            queueResorces = scaner.Scan(queueResorces);
        }

        // 2. ����� ������� �����
        if (queueDrones.Count > 0 && queueResorces.Count > 0)
        {
            target = queueResorces.Dequeue().transform;
            if (target != null)
            {
                sendDrone(queueDrones.Dequeue(), target);
            }
        }
    }

    private void sendDrone(Drone drone, Transform resource)
    {
        Debug.Log($"����� ������� ����� {drone.name} �� ������ {resource.name}");
        drone.takeCommand(resource);
    }

    public void AddDrone(Drone drone)
    {
        Debug.Log($"���� {drone.name} �������� � �������. � �������: {queueDrones.Count + 1}");
        queueDrones.Enqueue(drone);
    }

    public void addCountResource()
    {
        if (blockNextCount)
        {
            blockNextCount = false;
            return;
        }
        counter.CounterAdd();
    }

    private void OnCounterMaxReached()
    {
        Debug.Log("[CommandCenter] �������� OnCounterMaxReached!");
        if (currentDrones < maxDrones)
        {
            // ���������� ������� ������ ������� ����� ��� ����������� �����
            Vector3 spawnPos = drones.Length > 0 ? drones[0].transform.position : transform.position;

            Drone newDrone = Instantiate(dronePrefab, spawnPos, Quaternion.identity, dronesParent);

            // �������� ���������� ���� (�� dronesParent, � ���� ����)
            newDrone.Initialize(
                transform, // ���������� transform CommandCenter ��� ����
                _wayPoints
            );

            // ��������� � ����� ������ � �������
            queueDrones.Enqueue(newDrone);
            currentDrones++;
            droneJustCreated = true;
            blockNextCount = true;

            Debug.Log($"������ ����� ����. ����� ������: {currentDrones}");
        }
        else
        {
            Debug.Log("����� ������ ���������!");
        }
    }
}