using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 拖放脚本，用于拖拽ui，实现发射到显示屏上
/// </summary>
public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private RectTransform targetRect;//中心位置
    private Vector3 oldPosition;//初始位置
    private string buttonName;//按钮名字
    private Image img;//图片

    private void Awake()
    {
        img = transform.GetComponent<Image>();
        buttonName = gameObject.name;
        oldPosition = transform.position;
    }
    //按下
    public void OnBeginDrag()
    {

    }
    //拖拽
    public void OnDrag()
    {
        transform.position = Input.mousePosition;//位置跟随鼠标
    }
    //松开
    public void OnEndDrag()
    {
        Rect rec = targetRect.rect;
        rec.position += new Vector2( targetRect.position.x,targetRect.position.y);

        if (rec.Contains(transform.position))
        {
            //位置Y移动600，动画结束后淡入回到初始位置
            transform.DOLocalMoveY(600, 1f).OnComplete(() =>
            {
                Debug.Log(buttonName);//udp输出

                img.color = new Color(1, 1, 1, 0);
                transform.position = oldPosition;
                img.DOFade(1f, 1f);
            });
        }
        else
        {
            //平移回到初始位置
            transform.DOMove(oldPosition,1f);
        }
    }
}
