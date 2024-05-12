using UnityEngine;

public class clickCamera : MonoBehaviour
{
    public GameObject shutter;
    private Animator shutterAnimator;

    private void Start()
    {
        // 获取快门对象的Animator组件
        shutterAnimator = shutter.GetComponent<Animator>();
    }

    private void Update()
    {
        // 检测鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            // 发射一条射线，检测是否点击到了男孩
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // 检查点击到的对象是否是男孩对象
                if (hit.collider.gameObject == gameObject)
                {
                    shutterAnimator.SetTrigger("click");
                }
            }
        }
    }
}