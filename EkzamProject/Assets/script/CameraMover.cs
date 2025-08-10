using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";

    private float directionX;
    private float directionZ;

    private void Update()
    {
        directionX = Input.GetAxis(horizontal);
        directionZ = Input.GetAxis(vertical);

        transform.Translate(new Vector3(directionX * speed * Time.deltaTime, 0, directionZ * speed * Time.deltaTime));


        if (transform.position.x < startPoint.position.x)
        {
            transform.position = new Vector3(
               startPoint.position.x,
                transform.position.y,
                transform.position.z
            );
        }
        else if (transform.position.x > endPoint.position.x)
        {
            transform.position = new Vector3(
               endPoint.position.x,
               transform.position.y,
               transform.position.z
            );
        }

        // Ограничение по Z
        if (transform.position.z < startPoint.position.z)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                startPoint.position.z
            );
        }
        else if (transform.position.z > endPoint.position.z)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                endPoint.position.z
            );
        }       
    }
}


//if (transform.position.x < startPoint.position.x)
//{
//    transform.position = new Vector3(
//        startPoint.position.x,
//        transform.position.y,
//        transform.position.z
//    );
//}
//else if (transform.position.x > endPoint.position.x)
//{
//    transform.position = new Vector3(
//        endPoint.position.x,
//        transform.position.y,
//        transform.position.z
//    );
//}

//// Ограничение по Z
//if (transform.position.z < startPoint.position.z)
//{
//    transform.position = new Vector3(
//        transform.position.x,
//        transform.position.y,
//        startPoint.position.z
//    );
//}
//else if (transform.position.z > endPoint.position.z)
//{
//    transform.position = new Vector3(
//        transform.position.x,
//        transform.position.y,
//        endPoint.position.z
//    );
//}