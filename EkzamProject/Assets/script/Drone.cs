using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] Transform _base;
    [SerializeField] Transform[] _wayPoints;
    [SerializeField] Transform cargoPlace;

    private int currentWayPont = 0;
    private bool _isHaveCommand = false;
    private bool _isHaveResource = false;
    private Transform _target;
    private Resource tempResource;


    private void Update()
    {
        if (_isHaveCommand)
        {
            if (_target != null)
            {
                moveTarget(_target);
            }
            else
            {
                _isHaveCommand = false;
            }
        }
        else if (_isHaveResource)
        {
            if (_base != null)
            {
                moveTarget(_base);
            }
            else
            {
                Debug.LogError("База не назначена!");
                _isHaveResource = false;
            }
        }
        else
        {
            if (_wayPoints != null && _wayPoints.Length > 0)
            {
                freeMove();
            }
            else
            {
                Debug.LogError("WayPoints не назначены!");
            }
        }
    }

    private void freeMove()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            _wayPoints[currentWayPont].position,
            _speed * Time.deltaTime
        );
        transform.LookAt(_wayPoints[currentWayPont].position);
    }

    private void moveTarget(Transform target)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            _speed * Time.deltaTime
        );
        transform.LookAt(target.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Point point))
        {
            currentWayPont = ++currentWayPont % _wayPoints.Length;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out CommandCenter commandCenter))
        {
            // реализорвать разгрузку ресурса, сбрасывать параметр _isHaveResource
            // и возвращаемся в очередь
            UnloadCargo();
            commandCenter.addCountResource();
            commandCenter.AddDrone(this);
        }
        else if (collision.gameObject.TryGetComponent(out Resource resource))
        {
            // загружаем ресурс, устанавливаем _isshaveres == true
            if (_target != null && resource.transform.position == _target.position)
            {
                LoadCargo(resource);
            }
        }
    }


    private void UnloadCargo()
    {
        tempResource = null;

        foreach (Transform child in cargoPlace) // ищем среди детей cargoPlace!
        {
            if (child.gameObject.TryGetComponent(out Resource resource))
            {
                tempResource = resource;
                break;
            }
        }

        if (tempResource != null)
        {
            // Вернуть физику, если нужно
            Rigidbody rb = tempResource.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.detectCollisions = true;
            }

            tempResource.transform.parent = null;

            Destroy(tempResource.gameObject);
            tempResource = null;
        }

        _isHaveResource = false;
        _target = null;
    }


    private void LoadCargo(Resource resource)
    {
        // Отключаем физику перед прикреплением
        Rigidbody rb = resource.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        // Прикрепляем к cargoPlace
        resource.transform.SetParent(cargoPlace);
        resource.transform.localPosition = Vector3.zero;
        resource.transform.localRotation = Quaternion.identity;

        _isHaveResource = true;
        _isHaveCommand = false;

        // Отключаем коллайдер ресурса
        Collider col = resource.GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }

    public void takeCommand(Transform resource)
    {
        _target = resource;
        _isHaveCommand = true;
    }

    public void Initialize(Transform baseTransform, Transform[] wayPoints)
    {
        _base = baseTransform;
        _wayPoints = wayPoints;
    }
}