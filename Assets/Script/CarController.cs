using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public float speed = 15f;
    public float turnSpeed = 50f;

    void Update()
    {
        float verticalInput = 0f;
        float horizontalInput = 0f;

        // ตรวจสอบว่ามีคีย์บอร์ดเชื่อมต่ออยู่หรือไม่
        if (Keyboard.current != null)
        {
            // เช็คการกดปุ่มเดินหน้า-ถอยหลัง (W/S หรือ ลูกศรขึ้น/ลง)
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) verticalInput = 1f;
            else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) verticalInput = -1f;

            // เช็คการกดปุ่มเลี้ยวซ้าย-ขวา (A/D หรือ ลูกศรซ้าย/ขวา)
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontalInput = -1f;
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontalInput = 1f;
        }

        // เคลื่อนที่ไปข้างหน้าและหลัง
        transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);

        // หมุนเลี้ยวซ้ายขวา
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);
    }
}