using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExNovo.Example
{
    public class ArmRotator : MonoBehaviour
    {
        [SerializeField] private float rotationMultiplier = 1f;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(
                new Vector3(
                    Input.GetAxis("Mouse Y") * Time.deltaTime * rotationMultiplier,
                    Input.GetAxis("Mouse X") * Time.deltaTime * -rotationMultiplier,
                    0f
                ),
                Space.World
            );
        }
    }
}
