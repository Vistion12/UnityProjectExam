using System.Collections.Generic;
using UnityEngine;

public class CommandCenter : MonoBehaviour
{
    [SerializeField] Scaner scaner;
    [SerializeField] Drone[] drones;
    [SerializeField] Counter counter;
    [SerializeField] Drone dronePrefab; // префаб дрона
    [SerializeField] Transform dronesParent; // родитель для дронов
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
            return; // Пропустить один кадр после создания дрона
        }

        // 1. Сканируем ресурсы (например, по кнопке)
        if (Input.GetKeyDown(scanKey))
        {
            queueResorces = scaner.Scan(queueResorces);
        }

        // 2. Выдаём задание дрону
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
        Debug.Log($"Выдаю команду дрону {drone.name} на ресурс {resource.name}");
        drone.takeCommand(resource);
    }

    public void AddDrone(Drone drone)
    {
        Debug.Log($"Дрон {drone.name} добавлен в очередь. В очереди: {queueDrones.Count + 1}");
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
        Debug.Log("[CommandCenter] Сработал OnCounterMaxReached!");
        if (currentDrones < maxDrones)
        {
            // Используем позицию спавна первого дрона или специальную точку
            Vector3 spawnPos = drones.Length > 0 ? drones[0].transform.position : transform.position;

            Drone newDrone = Instantiate(dronePrefab, spawnPos, Quaternion.identity, dronesParent);

            // Передаем правильную базу (не dronesParent, а саму базу)
            newDrone.Initialize(
                transform, // Используем transform CommandCenter как базу
                _wayPoints
            );

            // Добавляем в общий список и очередь
            queueDrones.Enqueue(newDrone);
            currentDrones++;
            droneJustCreated = true;
            blockNextCount = true;

            Debug.Log($"Создан новый дрон. Всего дронов: {currentDrones}");
        }
        else
        {
            Debug.Log("Лимит дронов достигнут!");
        }
    }
}